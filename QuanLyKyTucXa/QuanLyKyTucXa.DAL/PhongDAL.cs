using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.DAL
{
    public class PhongDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private static bool isSchemaEnsured = false;

        public PhongDAL()
        {
            EnsureSchema();
        }

        public void EnsureSchema()
        {
            if (isSchemaEnsured) return;
            try
            {
                string sqlDayNha = @"
IF OBJECT_ID('DayNha', 'U') IS NULL
BEGIN
    CREATE TABLE DayNha (
        MaKhu VARCHAR(20) PRIMARY KEY,
        TenKhu NVARCHAR(50) NOT NULL UNIQUE,
        LoaiKhu NVARCHAR(30) DEFAULT N'Nam',
        MoTa NVARCHAR(255) DEFAULT N'',
        TrangThai NVARCHAR(30) DEFAULT N'Hoạt động'
    );
END";
                dbHelper.ExecuteNonQuery(sqlDayNha);

                string sqlAlterPhong = @"
IF COL_LENGTH('Phong', 'TenPhong') IS NULL
BEGIN
    ALTER TABLE Phong ADD TenPhong NVARCHAR(100) NULL;
    EXEC('UPDATE Phong SET TenPhong = CONCAT(N''Phòng '', MaPhong) WHERE TenPhong IS NULL');
END

IF COL_LENGTH('Phong', 'TrangThaiPhong') IS NULL
BEGIN
    ALTER TABLE Phong ADD TrangThaiPhong NVARCHAR(50) NULL;
    EXEC('UPDATE Phong SET TrangThaiPhong = CASE WHEN SoGiuongTrong <= 0 THEN N''Đã đầy'' ELSE N''Còn trống'' END WHERE TrangThaiPhong IS NULL');
END

IF COL_LENGTH('Phong', 'LoaiPhong') IS NOT NULL AND COL_LENGTH('Phong', 'LoaiPhong') < 200
BEGIN
    ALTER TABLE Phong ALTER COLUMN LoaiPhong NVARCHAR(100) NULL;
END";
                dbHelper.ExecuteNonQuery(sqlAlterPhong);

                string sqlSyncDayNha = @"
INSERT INTO DayNha (MaKhu, TenKhu, LoaiKhu, MoTa, TrangThai)
SELECT DISTINCT 
    'KHU_' + UPPER(REPLACE(REPLACE(TenKhu, N' ', ''), N'Đ', 'D')),
    TenKhu,
    CASE WHEN TenKhu LIKE N'%Nữ%' OR TenKhu LIKE N'%B%' THEN N'Nữ' ELSE N'Nam' END,
    N'Dãy nhà lưu trú sinh viên ' + TenKhu,
    N'Hoạt động'
FROM Phong p
WHERE p.TenKhu IS NOT NULL AND LTRIM(RTRIM(p.TenKhu)) <> ''
  AND NOT EXISTS (SELECT 1 FROM DayNha d WHERE d.TenKhu = p.TenKhu);

IF NOT EXISTS (SELECT 1 FROM DayNha)
BEGIN
    INSERT INTO DayNha (MaKhu, TenKhu, LoaiKhu, MoTa, TrangThai) VALUES
    ('KHU_A', N'Khu A', N'Nam', N'Dãy nhà A dành cho sinh viên Nam', N'Hoạt động'),
    ('KHU_B', N'Khu B', N'Nữ', N'Dãy nhà B dành cho sinh viên Nữ', N'Hoạt động'),
    ('KHU_C', N'Khu C', N'Nam', N'Dãy nhà C tiêu chuẩn dịch vụ', N'Hoạt động');
END";
                dbHelper.ExecuteNonQuery(sqlSyncDayNha);

                isSchemaEnsured = true;
            }
            catch
            {
            }
        }

        public DataTable GetAllPhong()
        {
            string query = @"
SELECT 
    MaPhong,
    ISNULL(TenPhong, N'Phòng ' + MaPhong) AS TenPhong,
    TenKhu,
    LoaiPhong,
    SoGiuongToiDa,
    (SoGiuongToiDa - SoGiuongTrong) AS SoNguoiHienTai,
    SoGiuongTrong,
    DonGia,
    CASE 
        WHEN TrangThaiPhong = N'Bảo trì' THEN N'Bảo trì'
        WHEN SoGiuongTrong <= 0 THEN N'Đã đầy'
        ELSE N'Còn trống'
    END AS TrangThai
FROM Phong
ORDER BY TenKhu, MaPhong";
            return dbHelper.ExecuteQuery(query);
        }

        public DataTable FilterPhong(string keyword, string tenKhu, string trangThai)
        {
            List<string> conditions = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                conditions.Add("(MaPhong LIKE @Keyword OR ISNULL(TenPhong, N'Phòng ' + MaPhong) LIKE @Keyword OR TenKhu LIKE @Keyword OR LoaiPhong LIKE @Keyword)");
                parameters.Add(new SqlParameter("@Keyword", "%" + keyword.Trim() + "%"));
            }

            if (!string.IsNullOrWhiteSpace(tenKhu) && tenKhu != "Tất cả dãy nhà")
            {
                conditions.Add("TenKhu = @TenKhu");
                parameters.Add(new SqlParameter("@TenKhu", tenKhu));
            }

            if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "Tất cả trạng thái")
            {
                if (trangThai == "Bảo trì")
                {
                    conditions.Add("TrangThaiPhong = N'Bảo trì'");
                }
                else if (trangThai == "Đã đầy")
                {
                    conditions.Add("(ISNULL(TrangThaiPhong, N'') <> N'Bảo trì' AND SoGiuongTrong <= 0)");
                }
                else if (trangThai == "Còn trống")
                {
                    conditions.Add("(ISNULL(TrangThaiPhong, N'') <> N'Bảo trì' AND SoGiuongTrong > 0)");
                }
            }

            string whereClause = conditions.Count > 0 ? (" WHERE " + string.Join(" AND ", conditions)) : "";

            string query = @"
SELECT 
    MaPhong,
    ISNULL(TenPhong, N'Phòng ' + MaPhong) AS TenPhong,
    TenKhu,
    LoaiPhong,
    SoGiuongToiDa,
    (SoGiuongToiDa - SoGiuongTrong) AS SoNguoiHienTai,
    SoGiuongTrong,
    DonGia,
    CASE 
        WHEN TrangThaiPhong = N'Bảo trì' THEN N'Bảo trì'
        WHEN SoGiuongTrong <= 0 THEN N'Đã đầy'
        ELSE N'Còn trống'
    END AS TrangThai
FROM Phong" + whereClause + " ORDER BY TenKhu, MaPhong";

            return dbHelper.ExecuteQuery(query, parameters.ToArray());
        }

        public bool InsertPhong(Phong p)
        {
            string query = @"
INSERT INTO Phong (MaPhong, TenPhong, TenKhu, LoaiPhong, SoGiuongToiDa, SoGiuongTrong, DonGia, TrangThaiPhong)
VALUES (@MaPhong, @TenPhong, @TenKhu, @LoaiPhong, @SoGiuongToiDa, @SoGiuongTrong, @DonGia, @TrangThaiPhong)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", p.MaPhong),
                new SqlParameter("@TenPhong", string.IsNullOrWhiteSpace(p.TenPhong) ? ("Phòng " + p.MaPhong) : p.TenPhong),
                new SqlParameter("@TenKhu", p.TenKhu),
                new SqlParameter("@LoaiPhong", p.LoaiPhong),
                new SqlParameter("@SoGiuongToiDa", p.SoGiuongToiDa),
                new SqlParameter("@SoGiuongTrong", p.SoGiuongTrong),
                new SqlParameter("@DonGia", p.DonGia),
                new SqlParameter("@TrangThaiPhong", string.IsNullOrWhiteSpace(p.TrangThai) ? (p.SoGiuongTrong <= 0 ? "Đã đầy" : "Còn trống") : p.TrangThai)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CheckMaPhongExists(string maPhong)
        {
            string query = "SELECT COUNT(*) FROM Phong WHERE MaPhong = @MaPhong";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaPhong", maPhong) };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool CheckPhongHasActiveStudentsOrContracts(string maPhong)
        {
            string query = "SELECT (SoGiuongToiDa - SoGiuongTrong) FROM Phong WHERE MaPhong = @MaPhong";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaPhong", maPhong) };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                return true;
            }

            try
            {
                string queryHD = "IF OBJECT_ID('HopDong', 'U') IS NOT NULL SELECT COUNT(*) FROM HopDong WHERE MaPhong = @MaPhong ELSE SELECT 0";
                SqlParameter[] paramHD = new SqlParameter[] { new SqlParameter("@MaPhong", maPhong) };
                DataTable dtHD = dbHelper.ExecuteQuery(queryHD, paramHD);
                if (dtHD.Rows.Count > 0 && Convert.ToInt32(dtHD.Rows[0][0]) > 0)
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public bool UpdatePhong(Phong p)
        {
            string query = @"
UPDATE Phong 
SET TenPhong = @TenPhong,
    TenKhu = @TenKhu,
    LoaiPhong = @LoaiPhong,
    SoGiuongToiDa = @SoGiuongToiDa,
    SoGiuongTrong = @SoGiuongTrong,
    DonGia = @DonGia,
    TrangThaiPhong = @TrangThaiPhong
WHERE MaPhong = @MaPhong";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", p.MaPhong),
                new SqlParameter("@TenPhong", string.IsNullOrWhiteSpace(p.TenPhong) ? ("Phòng " + p.MaPhong) : p.TenPhong),
                new SqlParameter("@TenKhu", p.TenKhu),
                new SqlParameter("@LoaiPhong", p.LoaiPhong),
                new SqlParameter("@SoGiuongToiDa", p.SoGiuongToiDa),
                new SqlParameter("@SoGiuongTrong", p.SoGiuongTrong),
                new SqlParameter("@DonGia", p.DonGia),
                new SqlParameter("@TrangThaiPhong", string.IsNullOrWhiteSpace(p.TrangThai) ? (p.SoGiuongTrong <= 0 ? "Đã đầy" : "Còn trống") : p.TrangThai)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeletePhong(string maPhong)
        {
            string query = "DELETE FROM Phong WHERE MaPhong = @MaPhong";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", maPhong)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public DataTable SearchPhong(string keyword)
        {
            return FilterPhong(keyword, "Tất cả dãy nhà", "Tất cả trạng thái");
        }

        // ========================================================
        // QUẢN LÝ DÃY NHÀ (KHU NHÀ)
        // ========================================================

        public DataTable GetAllDayNha(string keyword = "")
        {
            string where = "";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                where = " WHERE d.MaKhu LIKE @Keyword OR d.TenKhu LIKE @Keyword OR d.LoaiKhu LIKE @Keyword OR d.MoTa LIKE @Keyword ";
                parameters.Add(new SqlParameter("@Keyword", "%" + keyword.Trim() + "%"));
            }

            string query = @"
SELECT 
    d.MaKhu,
    d.TenKhu,
    d.LoaiKhu,
    COUNT(p.MaPhong) AS SoPhong,
    ISNULL(SUM(p.SoGiuongToiDa), 0) AS TongSucChua,
    ISNULL(SUM(p.SoGiuongToiDa - p.SoGiuongTrong), 0) AS DangO,
    d.MoTa,
    d.TrangThai
FROM DayNha d
LEFT JOIN Phong p ON d.TenKhu = p.TenKhu" + where + @"
GROUP BY d.MaKhu, d.TenKhu, d.LoaiKhu, d.MoTa, d.TrangThai
ORDER BY d.MaKhu";

            return dbHelper.ExecuteQuery(query, parameters.ToArray());
        }

        public bool CheckMaKhuExists(string maKhu)
        {
            string query = "SELECT COUNT(*) FROM DayNha WHERE MaKhu = @MaKhu";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaKhu", maKhu) };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool CheckTenKhuExists(string tenKhu, string excludeMaKhu = null)
        {
            string query = "SELECT COUNT(*) FROM DayNha WHERE TenKhu = @TenKhu";
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@TenKhu", tenKhu) };
            if (!string.IsNullOrWhiteSpace(excludeMaKhu))
            {
                query += " AND MaKhu <> @ExcludeMaKhu";
                parameters.Add(new SqlParameter("@ExcludeMaKhu", excludeMaKhu));
            }
            DataTable dt = dbHelper.ExecuteQuery(query, parameters.ToArray());
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public int CountPhongInDayNha(string tenKhu)
        {
            string query = "SELECT COUNT(*) FROM Phong WHERE TenKhu = @TenKhu";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@TenKhu", tenKhu) };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
        }

        public bool InsertDayNha(DayNha d)
        {
            string query = @"
INSERT INTO DayNha (MaKhu, TenKhu, LoaiKhu, MoTa, TrangThai)
VALUES (@MaKhu, @TenKhu, @LoaiKhu, @MoTa, @TrangThai)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaKhu", d.MaKhu),
                new SqlParameter("@TenKhu", d.TenKhu),
                new SqlParameter("@LoaiKhu", d.LoaiKhu),
                new SqlParameter("@MoTa", d.MoTa ?? ""),
                new SqlParameter("@TrangThai", string.IsNullOrWhiteSpace(d.TrangThai) ? "Hoạt động" : d.TrangThai)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateDayNha(DayNha d, string oldTenKhu = null)
        {
            string query = @"
UPDATE DayNha
SET TenKhu = @TenKhu,
    LoaiKhu = @LoaiKhu,
    MoTa = @MoTa,
    TrangThai = @TrangThai
WHERE MaKhu = @MaKhu";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaKhu", d.MaKhu),
                new SqlParameter("@TenKhu", d.TenKhu),
                new SqlParameter("@LoaiKhu", d.LoaiKhu),
                new SqlParameter("@MoTa", d.MoTa ?? ""),
                new SqlParameter("@TrangThai", string.IsNullOrWhiteSpace(d.TrangThai) ? "Hoạt động" : d.TrangThai)
            };

            bool updated = dbHelper.ExecuteNonQuery(query, parameters) > 0;

            if (updated && !string.IsNullOrWhiteSpace(oldTenKhu) && oldTenKhu != d.TenKhu)
            {
                string syncPhong = "UPDATE Phong SET TenKhu = @NewTenKhu WHERE TenKhu = @OldTenKhu";
                SqlParameter[] syncParams = new SqlParameter[]
                {
                    new SqlParameter("@NewTenKhu", d.TenKhu),
                    new SqlParameter("@OldTenKhu", oldTenKhu)
                };
                dbHelper.ExecuteNonQuery(syncPhong, syncParams);
            }

            return updated;
        }

        public bool DeleteDayNha(string maKhu)
        {
            string query = "DELETE FROM DayNha WHERE MaKhu = @MaKhu";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaKhu", maKhu)
            };
            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
