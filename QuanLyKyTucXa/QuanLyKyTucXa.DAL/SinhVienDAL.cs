using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.DAL
{
    public class SinhVienDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public SinhVienDAL()
        {
            try
            {
                new PhongDAL();
            }
            catch { }
        }

        public DataTable GetAllSinhVien()
        {
            string query = @"
                        SELECT 
                            sv.MaSV, 
                            sv.HoTen, 
                            sv.NgaySinh, 
                            sv.CCCD, 
                            sv.SDT, 
                            sv.GioiTinh,
                            ISNULL(p.TenPhong, N'Chưa xếp') AS TenPhong,
                            CASE 
                                WHEN hd.MaHopDong IS NOT NULL THEN N'Đang lưu trú'
                                ELSE N'Chưa xếp phòng'
                            END AS TrangThai
                        FROM SinhVien sv
                        LEFT JOIN HopDong hd ON sv.MaSV = hd.MaSV AND hd.TrangThai = N'Đang hiệu lực'
                        LEFT JOIN Phong p ON hd.MaPhong = p.MaPhong
                        ORDER BY sv.MaSV";
            return dbHelper.ExecuteQuery(query);
        }

        public DataTable FilterSinhVien(string keyword, string gioiTinh)
        {
            List<string> conditions = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                conditions.Add("(sv.MaSV LIKE @Keyword OR sv.HoTen LIKE @Keyword OR sv.CCCD LIKE @Keyword OR sv.SDT LIKE @Keyword)");
                parameters.Add(new SqlParameter("@Keyword", "%" + keyword.Trim() + "%"));
            }

            if (!string.IsNullOrWhiteSpace(gioiTinh) && gioiTinh != "Tất cả giới tính")
            {
                conditions.Add("sv.GioiTinh = @GioiTinh");
                parameters.Add(new SqlParameter("@GioiTinh", gioiTinh.Trim()));
            }

            string whereClause = conditions.Count > 0 ? (" WHERE " + string.Join(" AND ", conditions)) : "";
            string query = @"
                SELECT 
                    sv.MaSV, 
                    sv.HoTen, 
                    sv.NgaySinh, 
                    sv.CCCD, 
                    sv.SDT, 
                    sv.GioiTinh,
                    ISNULL(p.TenPhong, N'Chưa xếp') AS TenPhong,
                    CASE 
                        WHEN hd.MaHopDong IS NOT NULL THEN N'Đang lưu trú'
                        ELSE N'Chưa xếp phòng'
                    END AS TrangThai
                FROM SinhVien sv
                LEFT JOIN HopDong hd ON sv.MaSV = hd.MaSV AND hd.TrangThai = N'Đang hiệu lực'
                LEFT JOIN Phong p ON hd.MaPhong = p.MaPhong"
                + whereClause + " ORDER BY sv.MaSV";

            return dbHelper.ExecuteQuery(query, parameters.ToArray());
        }

        public bool InsertSinhVien(SinhVien sv)
        {
            string query = "INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, CCCD, SDT, GioiTinh) " +
                           "VALUES (@MaSV, @HoTen, @NgaySinh, @CCCD, @SDT, @GioiTinh)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", sv.MaSV),
                new SqlParameter("@HoTen", sv.HoTen),
                new SqlParameter("@NgaySinh", sv.NgaySinh),
                new SqlParameter("@CCCD", sv.CCCD),
                new SqlParameter("@SDT", sv.SDT),
                new SqlParameter("@GioiTinh", sv.GioiTinh)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CheckMaSVExists(string maSV)
        {
            string query = "SELECT COUNT(*) FROM SinhVien WHERE MaSV = @MaSV";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaSV", maSV) };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool CheckCCCDExists(string cccd, string excludeMaSV = null)
        {
            string query = "SELECT COUNT(*) FROM SinhVien WHERE CCCD = @CCCD";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@CCCD", (cccd ?? "").Trim())
            };

            if (!string.IsNullOrWhiteSpace(excludeMaSV))
            {
                query += " AND MaSV <> @ExcludeMaSV";
                parameters.Add(new SqlParameter("@ExcludeMaSV", excludeMaSV.Trim()));
            }

            DataTable dt = dbHelper.ExecuteQuery(query, parameters.ToArray());
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        public bool CheckSinhVienHasHopDong(string maSV)
        {
            try
            {
                string query = "IF OBJECT_ID('HopDong', 'U') IS NOT NULL " +
                               "SELECT COUNT(*) FROM HopDong WHERE MaSV = @MaSV " +
                               "ELSE SELECT 0";
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaSV", maSV) };
                DataTable dt = dbHelper.ExecuteQuery(query, parameters);
                return dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0;
            }
            catch
            {
                return false;
            }
        }

        public int CountSinhVienCoHopDong()
        {
            try
            {
                string query = "IF OBJECT_ID('HopDong', 'U') IS NOT NULL " +
                               "SELECT COUNT(DISTINCT MaSV) FROM HopDong " +
                               "ELSE SELECT 0";
                DataTable dt = dbHelper.ExecuteQuery(query);
                return dt != null && dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public bool UpdateSinhVien(SinhVien sv)
        {
            string query = "UPDATE SinhVien SET HoTen = @HoTen, NgaySinh = @NgaySinh, " +
                           "CCCD = @CCCD, SDT = @SDT, GioiTinh = @GioiTinh WHERE MaSV = @MaSV";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", sv.MaSV),
                new SqlParameter("@HoTen", sv.HoTen),
                new SqlParameter("@NgaySinh", sv.NgaySinh),
                new SqlParameter("@CCCD", sv.CCCD),
                new SqlParameter("@SDT", sv.SDT),
                new SqlParameter("@GioiTinh", sv.GioiTinh)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteSinhVien(string maSV)
        {
            string query = "DELETE FROM SinhVien WHERE MaSV = @MaSV";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSV", maSV)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public DataTable SearchSinhVien(string keyword)
        {
            string query = @"
                SELECT 
                    sv.MaSV, 
                    sv.HoTen, 
                    sv.NgaySinh, 
                    sv.CCCD, 
                    sv.SDT, 
                    sv.GioiTinh,
                    ISNULL(p.TenPhong, N'Chưa xếp') AS TenPhong,
                    CASE 
                        WHEN hd.MaHopDong IS NOT NULL THEN N'Đang lưu trú'
                        ELSE N'Chưa xếp phòng'
                    END AS TrangThai
                FROM SinhVien sv
                LEFT JOIN HopDong hd ON sv.MaSV = hd.MaSV AND hd.TrangThai = N'Đang hiệu lực'
                LEFT JOIN Phong p ON hd.MaPhong = p.MaPhong
                WHERE sv.MaSV LIKE @Keyword OR sv.HoTen LIKE @Keyword OR sv.CCCD LIKE @Keyword OR sv.SDT LIKE @Keyword 
                ORDER BY sv.MaSV";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Keyword", "%" + keyword + "%")
            };

            return dbHelper.ExecuteQuery(query, parameters);
        }
    }
}
    