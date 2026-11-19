using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.DAL
{
    public class HoaDonDAL
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();
        private static bool isSchemaEnsured = false;

        public HoaDonDAL()
        {
            EnsureSchema();
        }

        public void EnsureSchema()
        {
            if (isSchemaEnsured) return;
            try
            {
                string[] schemas = new string[]
                {
                    @"IF OBJECT_ID('CauHinhDichVu', 'U') IS NULL
                    BEGIN
                        CREATE TABLE CauHinhDichVu (
                            MaDichVu VARCHAR(20) PRIMARY KEY,
                            TenDichVu NVARCHAR(100) NOT NULL,
                            DonGia DECIMAL(18, 0) NOT NULL,
                            DonViTinh NVARCHAR(20) NOT NULL,
                            GhiChu NVARCHAR(255) NULL
                        );
                        INSERT INTO CauHinhDichVu (MaDichVu, TenDichVu, DonGia, DonViTinh, GhiChu) VALUES
                        ('DIEN', N'Điện sinh hoạt', 3500, N'kWh', N'Đơn giá điện ký túc xá'),
                        ('NUOC', N'Nước sinh hoạt', 15000, N'm³', N'Đơn giá nước ký túc xá'),
                        ('RAC', N'Vệ sinh & Rác', 30000, N'Phòng/tháng', N'Phí vệ sinh định kỳ');
                    END",

                    "IF COL_LENGTH('HoaDon', 'DaThanhToan') IS NULL ALTER TABLE HoaDon ADD DaThanhToan DECIMAL(18, 0) NOT NULL DEFAULT 0",
                    "IF COL_LENGTH('HoaDon', 'TienDichVuKhac') IS NULL ALTER TABLE HoaDon ADD TienDichVuKhac DECIMAL(18, 0) NOT NULL DEFAULT 0",
                    "IF COL_LENGTH('HoaDon', 'NgayLap') IS NULL ALTER TABLE HoaDon ADD NgayLap DATETIME NOT NULL DEFAULT GETDATE()",
                    "IF COL_LENGTH('HoaDon', 'NgayThanhToan') IS NULL ALTER TABLE HoaDon ADD NgayThanhToan DATETIME NULL",
                    "IF COL_LENGTH('HoaDon', 'GhiChu') IS NULL ALTER TABLE HoaDon ADD GhiChu NVARCHAR(255) NULL",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UC_HoaDon_PhongThangNam' AND object_id = OBJECT_ID('HoaDon'))
                    BEGIN
                        ALTER TABLE HoaDon ADD CONSTRAINT UC_HoaDon_PhongThangNam UNIQUE (MaPhong, Thang, Nam);
                    END",

                    @"IF OBJECT_ID('LichSuThanhToan', 'U') IS NULL
                    BEGIN
                        CREATE TABLE LichSuThanhToan (
                            MaThanhToan INT IDENTITY(1,1) PRIMARY KEY,
                            MaHoaDon INT NOT NULL CONSTRAINT FK_LichSuThanhToan_HoaDon REFERENCES HoaDon(MaHoaDon) ON DELETE CASCADE,
                            NgayThanhToan DATETIME NOT NULL DEFAULT GETDATE(),
                            SoTienThanhToan DECIMAL(18, 0) NOT NULL,
                            PhuongThuc NVARCHAR(50) DEFAULT N'Tiền mặt',
                            NguoiNop NVARCHAR(100) NULL,
                            GhiChu NVARCHAR(255) NULL
                        );
                    END"
                };

                foreach (string sql in schemas)
                {
                    try
                    {
                        dbHelper.ExecuteNonQuery(sql);
                    }
                    catch
                    {
                    }
                }

                isSchemaEnsured = true;
            }
            catch
            {
            }
        }

        public DataTable GetAllHoaDon()
        {
            return FilterHoaDon(null, null, null, null, null);
        }

        public DataTable FilterHoaDon(string maPhong, int? thang, int? nam, string trangThai, string keyword)
        {
            List<string> conditions = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(maPhong) && maPhong != "Tất cả phòng")
            {
                conditions.Add("hd.MaPhong = @MaPhong");
                parameters.Add(new SqlParameter("@MaPhong", maPhong.Trim()));
            }

            if (thang.HasValue && thang.Value > 0)
            {
                conditions.Add("hd.Thang = @Thang");
                parameters.Add(new SqlParameter("@Thang", thang.Value));
            }

            if (nam.HasValue && nam.Value > 0)
            {
                conditions.Add("hd.Nam = @Nam");
                parameters.Add(new SqlParameter("@Nam", nam.Value));
            }

            if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "Tất cả" && trangThai != "Tất cả trạng thái")
            {
                if (trangThai == "Chưa thanh toán")
                {
                    conditions.Add("(ISNULL(hd.DaThanhToan, 0) = 0)");
                }
                else if (trangThai == "Thanh toán một phần")
                {
                    conditions.Add("(ISNULL(hd.DaThanhToan, 0) > 0 AND ISNULL(hd.DaThanhToan, 0) < ISNULL(hd.TongTien, 0))");
                }
                else if (trangThai == "Đã thanh toán")
                {
                    conditions.Add("(ISNULL(hd.DaThanhToan, 0) >= ISNULL(hd.TongTien, 0) OR hd.TrangThai LIKE N'%Đã thanh toán%')");
                }
                else
                {
                    conditions.Add("hd.TrangThai = @TrangThai");
                    parameters.Add(new SqlParameter("@TrangThai", trangThai.Trim()));
                }
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                conditions.Add(@"(
                    CAST(hd.MaHoaDon AS NVARCHAR(20)) LIKE @Keyword
                    OR hd.MaPhong LIKE @Keyword
                    OR ISNULL(p.TenPhong, N'Phòng ' + hd.MaPhong) LIKE @Keyword
                    OR ISNULL(p.TenKhu, N'') LIKE @Keyword
                    OR ISNULL(hd.GhiChu, N'') LIKE @Keyword
                )");
                parameters.Add(new SqlParameter("@Keyword", "%" + keyword.Trim() + "%"));
            }

            string whereClause = conditions.Count > 0 ? (" WHERE " + string.Join(" AND ", conditions)) : "";

            string query = @"
SELECT 
    hd.MaHoaDon,
    hd.MaPhong,
    ISNULL(p.TenPhong, N'Phòng ' + hd.MaPhong) AS TenPhong,
    ISNULL(p.TenKhu, N'') AS TenKhu,
    hd.Thang,
    hd.Nam,
    ISNULL(hd.TienPhong, 0) AS TienPhong,
    ISNULL(hd.TienDien, 0) AS TienDien,
    ISNULL(hd.TienNuoc, 0) AS TienNuoc,
    ISNULL(hd.TienDichVuKhac, 0) AS TienDichVuKhac,
    ISNULL(hd.TongTien, 0) AS TongTien,
    ISNULL(hd.DaThanhToan, 0) AS DaThanhToan,
    CASE 
        WHEN (ISNULL(hd.TongTien, 0) - ISNULL(hd.DaThanhToan, 0)) > 0 THEN (ISNULL(hd.TongTien, 0) - ISNULL(hd.DaThanhToan, 0))
        ELSE 0 
    END AS ConNo,
    CASE 
        WHEN ISNULL(hd.DaThanhToan, 0) >= ISNULL(hd.TongTien, 0) THEN N'Đã thanh toán'
        WHEN ISNULL(hd.DaThanhToan, 0) > 0 THEN N'Thanh toán một phần'
        ELSE N'Chưa thanh toán'
    END AS TrangThai,
    hd.NgayLap,
    hd.NgayThanhToan,
    ISNULL(hd.GhiChu, N'') AS GhiChu,
    ISNULL(dn.ChiSoDienMoi - dn.ChiSoDienCu, 0) AS SoDienTieuThu,
    ISNULL(dn.ChiSoNuocMoi - dn.ChiSoNuocCu, 0) AS SoNuocTieuThu
FROM HoaDon hd
LEFT JOIN Phong p ON hd.MaPhong = p.MaPhong
LEFT JOIN DienNuoc dn ON hd.MaPhong = dn.MaPhong AND hd.Thang = dn.Thang AND hd.Nam = dn.Nam"
                + whereClause + @"
ORDER BY hd.Nam DESC, hd.Thang DESC, hd.MaHoaDon DESC";

            return dbHelper.ExecuteQuery(query, parameters.ToArray());
        }

        public DataRow GetHoaDonById(int maHoaDon)
        {
            string query = @"
SELECT 
    hd.MaHoaDon,
    hd.MaPhong,
    ISNULL(p.TenPhong, N'Phòng ' + hd.MaPhong) AS TenPhong,
    ISNULL(p.TenKhu, N'') AS TenKhu,
    hd.Thang,
    hd.Nam,
    ISNULL(hd.TienPhong, 0) AS TienPhong,
    ISNULL(hd.TienDien, 0) AS TienDien,
    ISNULL(hd.TienNuoc, 0) AS TienNuoc,
    ISNULL(hd.TienDichVuKhac, 0) AS TienDichVuKhac,
    ISNULL(hd.TongTien, 0) AS TongTien,
    ISNULL(hd.DaThanhToan, 0) AS DaThanhToan,
    CASE 
        WHEN (ISNULL(hd.TongTien, 0) - ISNULL(hd.DaThanhToan, 0)) > 0 THEN (ISNULL(hd.TongTien, 0) - ISNULL(hd.DaThanhToan, 0))
        ELSE 0 
    END AS ConNo,
    CASE 
        WHEN ISNULL(hd.DaThanhToan, 0) >= ISNULL(hd.TongTien, 0) THEN N'Đã thanh toán'
        WHEN ISNULL(hd.DaThanhToan, 0) > 0 THEN N'Thanh toán một phần'
        ELSE N'Chưa thanh toán'
    END AS TrangThai,
    hd.NgayLap,
    hd.NgayThanhToan,
    ISNULL(hd.GhiChu, N'') AS GhiChu,
    ISNULL(dn.ChiSoDienMoi - dn.ChiSoDienCu, 0) AS SoDienTieuThu,
    ISNULL(dn.ChiSoNuocMoi - dn.ChiSoNuocCu, 0) AS SoNuocTieuThu
FROM HoaDon hd
LEFT JOIN Phong p ON hd.MaPhong = p.MaPhong
LEFT JOIN DienNuoc dn ON hd.MaPhong = dn.MaPhong AND hd.Thang = dn.Thang AND hd.Nam = dn.Nam
WHERE hd.MaHoaDon = @MaHoaDon";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaHoaDon", maHoaDon)
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public bool CheckHoaDonExists(string maPhong, int thang, int nam, int? excludeMaHoaDon = null)
        {
            string query = "SELECT COUNT(1) FROM HoaDon WHERE MaPhong = @MaPhong AND Thang = @Thang AND Nam = @Nam";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam)
            };

            if (excludeMaHoaDon.HasValue && excludeMaHoaDon.Value > 0)
            {
                query += " AND MaHoaDon <> @ExcludeMaHoaDon";
                parameters.Add(new SqlParameter("@ExcludeMaHoaDon", excludeMaHoaDon.Value));
            }

            DataTable dt = dbHelper.ExecuteQuery(query, parameters.ToArray());
            return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool InsertHoaDon(HoaDonDTO hd)
        {
            string query = @"
INSERT INTO HoaDon (MaPhong, Thang, Nam, TienPhong, TienDien, TienNuoc, TienDichVuKhac, TongTien, TrangThai, DaThanhToan, NgayLap, GhiChu)
VALUES (@MaPhong, @Thang, @Nam, @TienPhong, @TienDien, @TienNuoc, @TienDichVuKhac, @TongTien, @TrangThai, @DaThanhToan, @NgayLap, @GhiChu)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", hd.MaPhong),
                new SqlParameter("@Thang", hd.Thang),
                new SqlParameter("@Nam", hd.Nam),
                new SqlParameter("@TienPhong", hd.TienPhong),
                new SqlParameter("@TienDien", hd.TienDien),
                new SqlParameter("@TienNuoc", hd.TienNuoc),
                new SqlParameter("@TienDichVuKhac", hd.TienDichVuKhac),
                new SqlParameter("@TongTien", hd.TongTien),
                new SqlParameter("@TrangThai", string.IsNullOrWhiteSpace(hd.TrangThai) ? "Chưa thanh toán" : hd.TrangThai),
                new SqlParameter("@DaThanhToan", hd.DaThanhToan),
                new SqlParameter("@NgayLap", hd.NgayLap == DateTime.MinValue ? DateTime.Now : hd.NgayLap),
                new SqlParameter("@GhiChu", (object)hd.GhiChu ?? DBNull.Value)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateHoaDon(HoaDonDTO hd)
        {
            string query = @"
UPDATE HoaDon
SET TienPhong = @TienPhong,
    TienDien = @TienDien,
    TienNuoc = @TienNuoc,
    TienDichVuKhac = @TienDichVuKhac,
    TongTien = @TongTien,
    TrangThai = @TrangThai,
    DaThanhToan = @DaThanhToan,
    GhiChu = @GhiChu
WHERE MaHoaDon = @MaHoaDon";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaHoaDon", hd.MaHoaDon),
                new SqlParameter("@TienPhong", hd.TienPhong),
                new SqlParameter("@TienDien", hd.TienDien),
                new SqlParameter("@TienNuoc", hd.TienNuoc),
                new SqlParameter("@TienDichVuKhac", hd.TienDichVuKhac),
                new SqlParameter("@TongTien", hd.TongTien),
                new SqlParameter("@TrangThai", hd.TrangThai),
                new SqlParameter("@DaThanhToan", hd.DaThanhToan),
                new SqlParameter("@GhiChu", (object)hd.GhiChu ?? DBNull.Value)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteHoaDon(int maHoaDon)
        {
            string query = "DELETE FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaHoaDon", maHoaDon)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        // ========================================================
        // DỊCH VỤ VÀ ĐƠN GIÁ CẤU HÌNH
        // ========================================================

        public DataTable GetAllCauHinhDichVu()
        {
            string query = "SELECT MaDichVu, TenDichVu, DonGia, DonViTinh, ISNULL(GhiChu, N'') AS GhiChu FROM CauHinhDichVu ORDER BY MaDichVu";
            return dbHelper.ExecuteQuery(query);
        }

        public decimal GetDonGiaDichVu(string maDichVu, decimal fallback = 0)
        {
            string query = "SELECT DonGia FROM CauHinhDichVu WHERE MaDichVu = @MaDichVu";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaDichVu", maDichVu) };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0 && dt.Rows[0]["DonGia"] != DBNull.Value)
            {
                return Convert.ToDecimal(dt.Rows[0]["DonGia"]);
            }
            return fallback;
        }

        public bool UpdateCauHinhDichVu(string maDichVu, decimal donGia, string ghiChu = null)
        {
            string query = "UPDATE CauHinhDichVu SET DonGia = @DonGia WHERE MaDichVu = @MaDichVu";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaDichVu", maDichVu),
                new SqlParameter("@DonGia", donGia)
            };

            if (ghiChu != null)
            {
                query = "UPDATE CauHinhDichVu SET DonGia = @DonGia, GhiChu = @GhiChu WHERE MaDichVu = @MaDichVu";
                parameters.Add(new SqlParameter("@GhiChu", ghiChu));
            }

            return dbHelper.ExecuteNonQuery(query, parameters.ToArray()) > 0;
        }

        // ========================================================
        // HỖ TRỢ TRUY VẤN DỮ LIỆU TỪ MODULE PHÒNG VÀ ĐIỆN NƯỚC
        // ========================================================

        public DataRow GetThongTinPhongChoHoaDon(string maPhong)
        {
            string query = @"
SELECT MaPhong, ISNULL(TenPhong, N'Phòng ' + MaPhong) AS TenPhong, TenKhu, LoaiPhong, DonGia
FROM Phong
WHERE MaPhong = @MaPhong";

            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaPhong", maPhong) };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public DataRow GetThongTinDienNuocChoHoaDon(string maPhong, int thang, int nam)
        {
            string query = @"
SELECT 
    MaDienNuoc, MaPhong, Thang, Nam,
    ChiSoDienCu, ChiSoDienMoi,
    CASE WHEN ChiSoDienMoi >= ChiSoDienCu THEN (ChiSoDienMoi - ChiSoDienCu) ELSE 0 END AS DienTieuThu,
    ChiSoNuocCu, ChiSoNuocMoi,
    CASE WHEN ChiSoNuocMoi >= ChiSoNuocCu THEN (ChiSoNuocMoi - ChiSoNuocCu) ELSE 0 END AS NuocTieuThu
FROM DienNuoc
WHERE MaPhong = @MaPhong AND Thang = @Thang AND Nam = @Nam";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam)
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        // ========================================================
        // BÁO CÁO THỐNG KÊ TÀI CHÍNH
        // ========================================================

        public DataTable GetThongKeTaiChinh()
        {
            string query = @"
SELECT 
    COUNT(MaHoaDon) AS TongHoaDon,
    ISNULL(SUM(TongTien), 0) AS TongTienPhaiThu,
    ISNULL(SUM(DaThanhToan), 0) AS TongTienDaThu,
    ISNULL(SUM(CASE WHEN (TongTien - DaThanhToan) > 0 THEN (TongTien - DaThanhToan) ELSE 0 END), 0) AS TongTienConNo,
    ISNULL(SUM(CASE WHEN ISNULL(DaThanhToan, 0) = 0 THEN 1 ELSE 0 END), 0) AS SoChuaThanhToan,
    ISNULL(SUM(CASE WHEN ISNULL(DaThanhToan, 0) > 0 AND ISNULL(DaThanhToan, 0) < ISNULL(TongTien, 0) THEN 1 ELSE 0 END), 0) AS SoThanhToanMotPhan,
    ISNULL(SUM(CASE WHEN ISNULL(DaThanhToan, 0) >= ISNULL(TongTien, 0) THEN 1 ELSE 0 END), 0) AS SoDaThanhToan
FROM HoaDon";

            return dbHelper.ExecuteQuery(query);
        }
    }
}

