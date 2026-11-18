using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.DAL
{
    public class HopDongDAL
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        private string ConnectionString => dbHelper.ConnectionString;

        /// <summary>
        /// Lấy toàn bộ danh sách hợp đồng kèm thông tin Sinh viên và Phòng.
        /// </summary>
        public DataTable GetAllHopDong()
        {
            return FilterHopDong(string.Empty, string.Empty);
        }

        /// <summary>
        /// Tìm kiếm và lọc danh sách hợp đồng theo từ khóa (mã HĐ, MSSV, họ tên, tên phòng) và trạng thái.
        /// </summary>
        public DataTable FilterHopDong(string keyword, string trangThai)
        {
            DataTable dt = new DataTable();
            List<string> conditions = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                conditions.Add(@"(
                    CAST(hd.MaHopDong AS NVARCHAR(20)) LIKE @Keyword
                    OR hd.MaSV LIKE @Keyword
                    OR sv.HoTen LIKE @Keyword
                    OR hd.MaPhong LIKE @Keyword
                    OR ISNULL(p.TenPhong, N'Phòng ' + hd.MaPhong) LIKE @Keyword
                )");
                parameters.Add(new SqlParameter("@Keyword", "%" + keyword.Trim() + "%"));
            }

            if (!string.IsNullOrWhiteSpace(trangThai)
                && !trangThai.Equals("Tất cả", StringComparison.OrdinalIgnoreCase)
                && !trangThai.Equals("Tất cả trạng thái", StringComparison.OrdinalIgnoreCase))
            {
                conditions.Add("hd.TrangThai = @TrangThai");
                parameters.Add(new SqlParameter("@TrangThai", trangThai.Trim()));
            }

            string whereClause = conditions.Count > 0
                ? " WHERE " + string.Join(" AND ", conditions)
                : string.Empty;

            string query = @"
                SELECT 
                    hd.MaHopDong,
                    hd.MaSV,
                    sv.HoTen,
                    hd.MaPhong,
                    ISNULL(p.TenPhong, N'Phòng ' + hd.MaPhong) AS TenPhong,
                    hd.NgayBatDau,
                    hd.NgayKetThuc,
                    hd.TrangThai
                FROM HopDong hd
                INNER JOIN SinhVien sv ON hd.MaSV = sv.MaSV
                INNER JOIN Phong p ON hd.MaPhong = p.MaPhong"
                + whereClause
                + " ORDER BY hd.MaHopDong DESC";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        /// <summary>
        /// Kiểm tra sinh viên đã có hợp đồng ở trạng thái 'Đang hiệu lực' hay chưa.
        /// </summary>
        public bool KiemTraSinhVienCoHopDongHieuLuc(string maSV)
        {
            const string query = "SELECT COUNT(1) FROM HopDong WHERE MaSV = @MaSV AND TrangThai = N'Đang hiệu lực'";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaSV", maSV);
                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        /// <summary>
        /// Lấy giới tính của sinh viên theo MaSV (trả về null nếu không tồn tại sinh viên).
        /// </summary>
        public string GetGioiTinhSinhVien(string maSV)
        {
            const string query = "SELECT GioiTinh FROM SinhVien WHERE MaSV = @MaSV";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaSV", maSV);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result == null) return null;
                return result == DBNull.Value ? string.Empty : result.ToString().Trim();
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của phòng phục vụ kiểm tra nghiệp vụ hợp đồng.
        /// </summary>
        public DataRow GetThongTinPhong(string maPhong)
        {
            const string query = @"
                SELECT MaPhong, TenPhong, LoaiPhong, SoGiuongToiDa, SoGiuongTrong, TrangThaiPhong
                FROM Phong
                WHERE MaPhong = @MaPhong";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// Lấy thông tin hợp đồng theo MaHopDong.
        /// </summary>
        public DataRow GetThongTinHopDong(string maHopDong)
        {
            if (!int.TryParse(maHopDong, out int maHopDongInt))
            {
                return null;
            }

            const string query = @"
                SELECT MaHopDong, MaSV, MaPhong, NgayBatDau, NgayKetThuc, TrangThai
                FROM HopDong
                WHERE MaHopDong = @MaHopDong";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@MaHopDong", SqlDbType.Int).Value = maHopDongInt;
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// Lập hợp đồng mới và trừ số giường trống của phòng trong cùng 1 SqlTransaction.
        /// </summary>
        public bool LapHopDongMoi(HopDongDTO hd)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        InsertHopDongCore(conn, trans, hd);
                        GiamGiuongTrongCore(conn, trans, hd.MaPhong);

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        try { trans.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Thanh lý hợp đồng và cộng trả 1 giường trống cho phòng trong cùng 1 SqlTransaction.
        /// </summary>
        public bool ThanhLyHopDong(string maHopDong, string maPhong)
        {
            if (!int.TryParse(maHopDong, out int maHopDongInt))
            {
                throw new ArgumentException("Mã hợp đồng không hợp lệ.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        const string sqlUpdateHD = @"
                            UPDATE HopDong
                            SET TrangThai = N'Đã thanh lý',
                                NgayKetThuc = GETDATE()
                            WHERE MaHopDong = @MaHopDong
                              AND TrangThai = N'Đang hiệu lực'";

                        using (SqlCommand cmdHD = new SqlCommand(sqlUpdateHD, conn, trans))
                        {
                            cmdHD.Parameters.Add("@MaHopDong", SqlDbType.Int).Value = maHopDongInt;
                            int affected = cmdHD.ExecuteNonQuery();
                            if (affected == 0)
                            {
                                throw new Exception("Hợp đồng không tồn tại hoặc không ở trạng thái 'Đang hiệu lực'.");
                            }
                        }

                        TangGiuongTrongCore(conn, trans, maPhong, 1);

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        try { trans.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Chuyển phòng theo luồng: Chuyển trạng thái hợp đồng cũ + trả giường phòng cũ -> Tạo hợp đồng mới + trừ giường phòng mới.
        /// Toàn bộ nằm trong 1 SqlTransaction để giữ nguyên lịch sử và toàn vẹn dữ liệu.
        /// </summary>
        public bool ChuyenPhong(string maHopDongCu, string maPhongCu, HopDongDTO hdMoi)
        {
            if (!int.TryParse(maHopDongCu, out int maHopDongCuInt))
            {
                throw new ArgumentException("Mã hợp đồng cũ không hợp lệ.");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Cập nhật hợp đồng cũ thành 'Đã chuyển phòng'
                        const string sqlUpdateOldHD = @"
                            UPDATE HopDong
                            SET TrangThai = N'Đã chuyển phòng',
                                NgayKetThuc = GETDATE()
                            WHERE MaHopDong = @MaHopDongCu
                              AND TrangThai = N'Đang hiệu lực'";

                        using (SqlCommand cmdOldHD = new SqlCommand(sqlUpdateOldHD, conn, trans))
                        {
                            cmdOldHD.Parameters.Add("@MaHopDongCu", SqlDbType.Int).Value = maHopDongCuInt;
                            int affected = cmdOldHD.ExecuteNonQuery();
                            if (affected == 0)
                            {
                                throw new Exception("Hợp đồng cũ không tồn tại hoặc không còn hiệu lực để chuyển phòng.");
                            }
                        }

                        // 2. Cộng +1 giường trống cho phòng cũ và đặt TrangThaiPhong = N'Còn trống'
                        TangGiuongTrongCore(conn, trans, maPhongCu, 1);

                        // 3. Tạo hợp đồng mới ở trạng thái 'Đang hiệu lực'
                        InsertHopDongCore(conn, trans, hdMoi);

                        // 4. Trừ -1 giường trống cho phòng mới (nếu về 0 thì cập nhật N'Đã đầy')
                        GiamGiuongTrongCore(conn, trans, hdMoi.MaPhong);

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        try { trans.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Quét các hợp đồng quá hạn (NgayKetThuc < ngày hiện tại) vẫn đang hiệu lực,
        /// chuyển sang N'Hết hạn' và cộng trả số giường trống cho từng phòng tương ứng trong 1 SqlTransaction.
        /// </summary>
        public int CapNhatHopDongHetHan()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Thống kê số lượng hợp đồng hết hạn theo từng phòng trước khi cập nhật
                        const string sqlGetExpired = @"
                            SELECT MaPhong, COUNT(*) AS SoLuong
                            FROM HopDong
                            WHERE TrangThai = N'Đang hiệu lực'
                              AND NgayKetThuc < CAST(GETDATE() AS DATE)
                            GROUP BY MaPhong";

                        List<KeyValuePair<string, int>> expiredByRoom = new List<KeyValuePair<string, int>>();
                        using (SqlCommand cmdSelect = new SqlCommand(sqlGetExpired, conn, trans))
                        using (SqlDataReader reader = cmdSelect.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string maPhong = reader["MaPhong"].ToString();
                                int soLuong = Convert.ToInt32(reader["SoLuong"]);
                                expiredByRoom.Add(new KeyValuePair<string, int>(maPhong, soLuong));
                            }
                        }

                        if (expiredByRoom.Count == 0)
                        {
                            trans.Commit();
                            return 0;
                        }

                        // 2. Chuyển trạng thái các hợp đồng hết hạn sang N'Hết hạn'
                        const string sqlUpdateExpired = @"
                            UPDATE HopDong
                            SET TrangThai = N'Hết hạn'
                            WHERE TrangThai = N'Đang hiệu lực'
                              AND NgayKetThuc < CAST(GETDATE() AS DATE)";

                        int totalUpdated;
                        using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdateExpired, conn, trans))
                        {
                            totalUpdated = cmdUpdate.ExecuteNonQuery();
                        }

                        // 3. Cộng trả giường trống cho từng phòng tương ứng
                        foreach (var item in expiredByRoom)
                        {
                            TangGiuongTrongCore(conn, trans, item.Key, item.Value);
                        }

                        trans.Commit();
                        return totalUpdated;
                    }
                    catch
                    {
                        try { trans.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        private void InsertHopDongCore(SqlConnection conn, SqlTransaction trans, HopDongDTO hd)
        {
            const string sqlInsert = @"
                INSERT INTO HopDong (MaSV, MaPhong, NgayBatDau, NgayKetThuc, TrangThai)
                VALUES (@MaSV, @MaPhong, @NgayBatDau, @NgayKetThuc, N'Đang hiệu lực')";

            using (SqlCommand cmd = new SqlCommand(sqlInsert, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaSV", hd.MaSV);
                cmd.Parameters.AddWithValue("@MaPhong", hd.MaPhong);
                cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = hd.NgayBatDau.Date;
                cmd.Parameters.Add("@NgayKetThuc", SqlDbType.Date).Value = hd.NgayKetThuc.Date;
                cmd.ExecuteNonQuery();
            }
        }

        private void GiamGiuongTrongCore(SqlConnection conn, SqlTransaction trans, string maPhong)
        {
            const string sqlUpdatePhong = @"
                UPDATE Phong
                SET SoGiuongTrong = SoGiuongTrong - 1,
                    TrangThaiPhong = CASE 
                        WHEN (SoGiuongTrong - 1) <= 0 THEN N'Đã đầy' 
                        ELSE ISNULL(TrangThaiPhong, N'Còn trống') 
                    END
                WHERE MaPhong = @MaPhong
                  AND SoGiuongTrong > 0
                  AND ISNULL(TrangThaiPhong, N'') <> N'Đang sửa chữa'";

            using (SqlCommand cmd = new SqlCommand(sqlUpdatePhong, conn, trans))
            {
                cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                {
                    throw new Exception($"Không thể trừ giường trống của phòng '{maPhong}' (phòng đã đầy, đang sửa chữa hoặc không tồn tại).");
                }
            }
        }

        private void TangGiuongTrongCore(SqlConnection conn, SqlTransaction trans, string maPhong, int soLuong)
        {
            const string sqlUpdatePhong = @"
                UPDATE Phong
                SET SoGiuongTrong = CASE 
                        WHEN (SoGiuongTrong + @SoLuong) > SoGiuongToiDa THEN SoGiuongToiDa 
                        ELSE (SoGiuongTrong + @SoLuong) 
                    END,
                    TrangThaiPhong = CASE 
                        WHEN ISNULL(TrangThaiPhong, N'') = N'Đang sửa chữa' THEN TrangThaiPhong 
                        ELSE N'Còn trống' 
                    END
                WHERE MaPhong = @MaPhong";

            using (SqlCommand cmd = new SqlCommand(sqlUpdatePhong, conn, trans))
            {
                cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = soLuong;
                cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                {
                    throw new Exception($"Không tìm thấy phòng '{maPhong}' để cập nhật lại số giường trống.");
                }
            }
        }
    }
}

