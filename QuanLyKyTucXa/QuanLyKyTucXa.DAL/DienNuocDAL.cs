using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.DAL
{
    public class DienNuocDAL
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();
        private static bool isSchemaEnsured = false;

        public DienNuocDAL()
        {
            EnsureSchema();
        }

        public void EnsureSchema()
        {
            if (isSchemaEnsured) return;
            try
            {
                string sqlCheck = @"
IF OBJECT_ID('DienNuoc', 'U') IS NULL
BEGIN
    CREATE TABLE DienNuoc (
        MaDienNuoc INT IDENTITY(1,1) PRIMARY KEY,
        MaPhong VARCHAR(10) NOT NULL CONSTRAINT FK_DienNuoc_Phong REFERENCES Phong(MaPhong),
        Thang INT NOT NULL,
        Nam INT NOT NULL,
        ChiSoDienCu INT DEFAULT 0,
        ChiSoDienMoi INT DEFAULT 0,
        ChiSoNuocCu INT DEFAULT 0,
        ChiSoNuocMoi INT DEFAULT 0,
        CONSTRAINT UC_DienNuoc_PhongThangNam UNIQUE (MaPhong, Thang, Nam)
    );
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UC_DienNuoc_PhongThangNam' AND object_id = OBJECT_ID('DienNuoc'))
    BEGIN
        ALTER TABLE DienNuoc ADD CONSTRAINT UC_DienNuoc_PhongThangNam UNIQUE (MaPhong, Thang, Nam);
    END
END";
                dbHelper.ExecuteNonQuery(sqlCheck);
                isSchemaEnsured = true;
            }
            catch
            {
            }
        }

        public DataTable GetAllDienNuoc()
        {
            string query = @"
SELECT 
    dn.MaDienNuoc,
    dn.MaPhong,
    ISNULL(p.TenPhong, N'Phòng ' + dn.MaPhong) AS TenPhong,
    ISNULL(p.TenKhu, N'') AS TenKhu,
    dn.Thang,
    dn.Nam,
    dn.ChiSoDienCu,
    dn.ChiSoDienMoi,
    CASE 
        WHEN dn.ChiSoDienMoi >= dn.ChiSoDienCu THEN (dn.ChiSoDienMoi - dn.ChiSoDienCu)
        ELSE 0 
    END AS DienTieuThu,
    dn.ChiSoNuocCu,
    dn.ChiSoNuocMoi,
    CASE 
        WHEN dn.ChiSoNuocMoi >= dn.ChiSoNuocCu THEN (dn.ChiSoNuocMoi - dn.ChiSoNuocCu)
        ELSE 0 
    END AS NuocTieuThu
FROM DienNuoc dn
LEFT JOIN Phong p ON dn.MaPhong = p.MaPhong
ORDER BY dn.Nam DESC, dn.Thang DESC, dn.MaPhong ASC";

            return dbHelper.ExecuteQuery(query);
        }

        public DataTable FilterDienNuoc(string maPhong, int? thang, int? nam, string keyword)
        {
            List<string> conditions = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(maPhong) && maPhong != "Tất cả phòng")
            {
                conditions.Add("dn.MaPhong = @MaPhong");
                parameters.Add(new SqlParameter("@MaPhong", maPhong.Trim()));
            }

            if (thang.HasValue && thang.Value > 0)
            {
                conditions.Add("dn.Thang = @Thang");
                parameters.Add(new SqlParameter("@Thang", thang.Value));
            }

            if (nam.HasValue && nam.Value > 0)
            {
                conditions.Add("dn.Nam = @Nam");
                parameters.Add(new SqlParameter("@Nam", nam.Value));
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                conditions.Add("(dn.MaPhong LIKE @Keyword OR ISNULL(p.TenPhong, N'Phòng ' + dn.MaPhong) LIKE @Keyword OR ISNULL(p.TenKhu, N'') LIKE @Keyword)");
                parameters.Add(new SqlParameter("@Keyword", "%" + keyword.Trim() + "%"));
            }

            string whereClause = conditions.Count > 0 ? (" WHERE " + string.Join(" AND ", conditions)) : "";

            string query = @"
SELECT 
    dn.MaDienNuoc,
    dn.MaPhong,
    ISNULL(p.TenPhong, N'Phòng ' + dn.MaPhong) AS TenPhong,
    ISNULL(p.TenKhu, N'') AS TenKhu,
    dn.Thang,
    dn.Nam,
    dn.ChiSoDienCu,
    dn.ChiSoDienMoi,
    CASE 
        WHEN dn.ChiSoDienMoi >= dn.ChiSoDienCu THEN (dn.ChiSoDienMoi - dn.ChiSoDienCu)
        ELSE 0 
    END AS DienTieuThu,
    dn.ChiSoNuocCu,
    dn.ChiSoNuocMoi,
    CASE 
        WHEN dn.ChiSoNuocMoi >= dn.ChiSoNuocCu THEN (dn.ChiSoNuocMoi - dn.ChiSoNuocCu)
        ELSE 0 
    END AS NuocTieuThu
FROM DienNuoc dn
LEFT JOIN Phong p ON dn.MaPhong = p.MaPhong"
                + whereClause + @"
ORDER BY dn.Nam DESC, dn.Thang DESC, dn.MaPhong ASC";

            return dbHelper.ExecuteQuery(query, parameters.ToArray());
        }

        public DataRow GetDienNuocById(int maDienNuoc)
        {
            string query = @"
SELECT 
    dn.MaDienNuoc,
    dn.MaPhong,
    ISNULL(p.TenPhong, N'Phòng ' + dn.MaPhong) AS TenPhong,
    ISNULL(p.TenKhu, N'') AS TenKhu,
    dn.Thang,
    dn.Nam,
    dn.ChiSoDienCu,
    dn.ChiSoDienMoi,
    (dn.ChiSoDienMoi - dn.ChiSoDienCu) AS DienTieuThu,
    dn.ChiSoNuocCu,
    dn.ChiSoNuocMoi,
    (dn.ChiSoNuocMoi - dn.ChiSoNuocCu) AS NuocTieuThu
FROM DienNuoc dn
LEFT JOIN Phong p ON dn.MaPhong = p.MaPhong
WHERE dn.MaDienNuoc = @MaDienNuoc";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaDienNuoc", maDienNuoc)
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public DataRow GetDienNuocByPhongThangNam(string maPhong, int thang, int nam)
        {
            string query = @"
SELECT 
    dn.MaDienNuoc,
    dn.MaPhong,
    ISNULL(p.TenPhong, N'Phòng ' + dn.MaPhong) AS TenPhong,
    ISNULL(p.TenKhu, N'') AS TenKhu,
    dn.Thang,
    dn.Nam,
    dn.ChiSoDienCu,
    dn.ChiSoDienMoi,
    (dn.ChiSoDienMoi - dn.ChiSoDienCu) AS DienTieuThu,
    dn.ChiSoNuocCu,
    dn.ChiSoNuocMoi,
    (dn.ChiSoNuocMoi - dn.ChiSoNuocCu) AS NuocTieuThu
FROM DienNuoc dn
LEFT JOIN Phong p ON dn.MaPhong = p.MaPhong
WHERE dn.MaPhong = @MaPhong AND dn.Thang = @Thang AND dn.Nam = @Nam";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam)
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// Lấy chỉ số mới nhất trước tháng/năm chỉ định của phòng để tự động đề xuất làm chỉ số cũ.
        /// </summary>
        public DataRow GetChiSoThangTruoc(string maPhong, int thangHienTai, int namHienTai)
        {
            string query = @"
SELECT TOP 1 ChiSoDienMoi, ChiSoNuocMoi, Thang, Nam
FROM DienNuoc
WHERE MaPhong = @MaPhong 
  AND ((Nam < @Nam) OR (Nam = @Nam AND Thang < @Thang))
ORDER BY Nam DESC, Thang DESC";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@Thang", thangHienTai),
                new SqlParameter("@Nam", namHienTai)
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public bool CheckDienNuocExists(string maPhong, int thang, int nam, int? excludeMaDienNuoc = null)
        {
            string query = "SELECT COUNT(1) FROM DienNuoc WHERE MaPhong = @MaPhong AND Thang = @Thang AND Nam = @Nam";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam)
            };

            if (excludeMaDienNuoc.HasValue && excludeMaDienNuoc.Value > 0)
            {
                query += " AND MaDienNuoc <> @ExcludeMaDienNuoc";
                parameters.Add(new SqlParameter("@ExcludeMaDienNuoc", excludeMaDienNuoc.Value));
            }

            DataTable dt = dbHelper.ExecuteQuery(query, parameters.ToArray());
            return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool CheckDienNuocDaCoHoaDon(string maPhong, int thang, int nam)
        {
            string query = "SELECT COUNT(1) FROM HoaDon WHERE MaPhong = @MaPhong AND Thang = @Thang AND Nam = @Nam";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam)
            };

            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool InsertDienNuoc(DienNuocDTO dn)
        {
            string query = @"
INSERT INTO DienNuoc (MaPhong, Thang, Nam, ChiSoDienCu, ChiSoDienMoi, ChiSoNuocCu, ChiSoNuocMoi)
VALUES (@MaPhong, @Thang, @Nam, @ChiSoDienCu, @ChiSoDienMoi, @ChiSoNuocCu, @ChiSoNuocMoi)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", dn.MaPhong),
                new SqlParameter("@Thang", dn.Thang),
                new SqlParameter("@Nam", dn.Nam),
                new SqlParameter("@ChiSoDienCu", dn.ChiSoDienCu),
                new SqlParameter("@ChiSoDienMoi", dn.ChiSoDienMoi),
                new SqlParameter("@ChiSoNuocCu", dn.ChiSoNuocCu),
                new SqlParameter("@ChiSoNuocMoi", dn.ChiSoNuocMoi)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateDienNuoc(DienNuocDTO dn)
        {
            string query = @"
UPDATE DienNuoc 
SET MaPhong = @MaPhong,
    Thang = @Thang,
    Nam = @Nam,
    ChiSoDienCu = @ChiSoDienCu,
    ChiSoDienMoi = @ChiSoDienMoi,
    ChiSoNuocCu = @ChiSoNuocCu,
    ChiSoNuocMoi = @ChiSoNuocMoi
WHERE MaDienNuoc = @MaDienNuoc";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaDienNuoc", dn.MaDienNuoc),
                new SqlParameter("@MaPhong", dn.MaPhong),
                new SqlParameter("@Thang", dn.Thang),
                new SqlParameter("@Nam", dn.Nam),
                new SqlParameter("@ChiSoDienCu", dn.ChiSoDienCu),
                new SqlParameter("@ChiSoDienMoi", dn.ChiSoDienMoi),
                new SqlParameter("@ChiSoNuocCu", dn.ChiSoNuocCu),
                new SqlParameter("@ChiSoNuocMoi", dn.ChiSoNuocMoi)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteDienNuoc(int maDienNuoc)
        {
            string query = "DELETE FROM DienNuoc WHERE MaDienNuoc = @MaDienNuoc";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaDienNuoc", maDienNuoc)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}

