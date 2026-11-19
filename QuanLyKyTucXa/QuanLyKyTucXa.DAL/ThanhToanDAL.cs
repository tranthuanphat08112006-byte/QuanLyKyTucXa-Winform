using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.DAL
{
    public class ThanhToanDAL
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        private string ConnectionString => dbHelper.ConnectionString;

        /// <summary>
        /// Thực hiện thanh toán hóa đơn (toàn bộ hoặc một phần) trong 1 SqlTransaction.
        /// Đảm bảo tính toàn vẹn dữ liệu: Cập nhật DaThanhToan và TrangThai trong HoaDon, ghi nhận lịch sử vào LichSuThanhToan.
        /// </summary>
        public bool ThanhToanHoaDon(int maHoaDon, decimal soTienThanhToan, string phuongThuc, string nguoiNop, string ghiChu)
        {
            if (maHoaDon <= 0)
                throw new ArgumentException("Mã hóa đơn không hợp lệ.");

            if (soTienThanhToan <= 0)
                throw new ArgumentException("Số tiền thanh toán phải lớn hơn 0 VNĐ.");

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Khóa và lấy thông tin hóa đơn hiện tại
                        const string sqlSelect = @"
SELECT TongTien, ISNULL(DaThanhToan, 0) AS DaThanhToan, TrangThai 
FROM HoaDon WITH (UPDLOCK, ROWLOCK)
WHERE MaHoaDon = @MaHoaDon";

                        decimal tongTien = 0;
                        decimal daThanhToanHienTai = 0;
                        string trangThaiHienTai = "";

                        using (SqlCommand cmdSelect = new SqlCommand(sqlSelect, conn, trans))
                        {
                            cmdSelect.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                            using (SqlDataReader reader = cmdSelect.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    throw new Exception($"Không tìm thấy hóa đơn mã #{maHoaDon} trong hệ thống.");
                                }

                                tongTien = reader["TongTien"] != DBNull.Value ? Convert.ToDecimal(reader["TongTien"]) : 0;
                                daThanhToanHienTai = reader["DaThanhToan"] != DBNull.Value ? Convert.ToDecimal(reader["DaThanhToan"]) : 0;
                                trangThaiHienTai = reader["TrangThai"]?.ToString() ?? "";
                            }
                        }

                        decimal conNo = tongTien - daThanhToanHienTai;
                        if (conNo <= 0)
                        {
                            throw new Exception($"Hóa đơn #{maHoaDon} đã được thanh toán đầy đủ, không thể thu thêm tiền.");
                        }

                        if (soTienThanhToan > conNo)
                        {
                            throw new Exception($"Số tiền thanh toán ({soTienThanhToan:N0} đ) vượt quá số tiền còn nợ ({conNo:N0} đ) của hóa đơn.");
                        }

                        decimal daThanhToanMoi = daThanhToanHienTai + soTienThanhToan;
                        string trangThaiMoi = (daThanhToanMoi >= tongTien) ? "Đã thanh toán" : "Thanh toán một phần";

                        // 2. Cập nhật hóa đơn
                        const string sqlUpdateHD = @"
UPDATE HoaDon
SET DaThanhToan = @DaThanhToan,
    TrangThai = @TrangThai,
    NgayThanhToan = GETDATE()
WHERE MaHoaDon = @MaHoaDon";

                        using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdateHD, conn, trans))
                        {
                            cmdUpdate.Parameters.AddWithValue("@DaThanhToan", daThanhToanMoi);
                            cmdUpdate.Parameters.AddWithValue("@TrangThai", trangThaiMoi);
                            cmdUpdate.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        // 3. Ghi nhận phiếu thu vào LichSuThanhToan
                        const string sqlInsertLS = @"
INSERT INTO LichSuThanhToan (MaHoaDon, NgayThanhToan, SoTienThanhToan, PhuongThuc, NguoiNop, GhiChu)
VALUES (@MaHoaDon, GETDATE(), @SoTienThanhToan, @PhuongThuc, @NguoiNop, @GhiChu)";

                        using (SqlCommand cmdInsert = new SqlCommand(sqlInsertLS, conn, trans))
                        {
                            cmdInsert.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                            cmdInsert.Parameters.AddWithValue("@SoTienThanhToan", soTienThanhToan);
                            cmdInsert.Parameters.AddWithValue("@PhuongThuc", string.IsNullOrWhiteSpace(phuongThuc) ? "Tiền mặt" : phuongThuc);
                            cmdInsert.Parameters.AddWithValue("@NguoiNop", (object)nguoiNop ?? DBNull.Value);
                            cmdInsert.Parameters.AddWithValue("@GhiChu", (object)ghiChu ?? DBNull.Value);
                            cmdInsert.ExecuteNonQuery();
                        }

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

        public DataTable GetLichSuThanhToanByHoaDon(int maHoaDon)
        {
            string query = @"
SELECT 
    ls.MaThanhToan,
    ls.MaHoaDon,
    ls.NgayThanhToan,
    ls.SoTienThanhToan,
    ls.PhuongThuc,
    ISNULL(ls.NguoiNop, N'') AS NguoiNop,
    ISNULL(ls.GhiChu, N'') AS GhiChu
FROM LichSuThanhToan ls
WHERE ls.MaHoaDon = @MaHoaDon
ORDER BY ls.NgayThanhToan DESC";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaHoaDon", maHoaDon)
            };

            return dbHelper.ExecuteQuery(query, parameters);
        }

        public DataTable GetAllLichSuThanhToan(int? maHoaDon = null, string keyword = "")
        {
            List<string> conditions = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (maHoaDon.HasValue && maHoaDon.Value > 0)
            {
                conditions.Add("ls.MaHoaDon = @MaHoaDon");
                parameters.Add(new SqlParameter("@MaHoaDon", maHoaDon.Value));
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                conditions.Add(@"(
                    CAST(ls.MaThanhToan AS NVARCHAR(20)) LIKE @Keyword
                    OR CAST(ls.MaHoaDon AS NVARCHAR(20)) LIKE @Keyword
                    OR hd.MaPhong LIKE @Keyword
                    OR ISNULL(p.TenPhong, N'') LIKE @Keyword
                    OR ISNULL(ls.NguoiNop, N'') LIKE @Keyword
                    OR ISNULL(ls.PhuongThuc, N'') LIKE @Keyword
                )");
                parameters.Add(new SqlParameter("@Keyword", "%" + keyword.Trim() + "%"));
            }

            string whereClause = conditions.Count > 0 ? (" WHERE " + string.Join(" AND ", conditions)) : "";

            string query = @"
SELECT 
    ls.MaThanhToan,
    ls.MaHoaDon,
    hd.MaPhong,
    ISNULL(p.TenPhong, N'Phòng ' + hd.MaPhong) AS TenPhong,
    hd.Thang,
    hd.Nam,
    ls.NgayThanhToan,
    ls.SoTienThanhToan,
    ls.PhuongThuc,
    ISNULL(ls.NguoiNop, N'') AS NguoiNop,
    ISNULL(ls.GhiChu, N'') AS GhiChu
FROM LichSuThanhToan ls
INNER JOIN HoaDon hd ON ls.MaHoaDon = hd.MaHoaDon
LEFT JOIN Phong p ON hd.MaPhong = p.MaPhong"
                + whereClause + @"
ORDER BY ls.NgayThanhToan DESC";

            return dbHelper.ExecuteQuery(query, parameters.ToArray());
        }
    }
}

