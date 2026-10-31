using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyKyTucXa.DTO; // Đã nhận diện được DTO nhờ Add Reference

namespace QuanLyKyTucXa.DAL
{
    public class SinhVienDAL
    {
        // Khởi tạo công cụ DatabaseHelper đã viết ở bước trước
        private DatabaseHelper dbHelper = new DatabaseHelper();

        // Hàm lấy toàn bộ danh sách Sinh Viên
        public DataTable GetAllSinhVien()
        {
            // Câu lệnh SQL thuần túy
            string query = "SELECT MaSV, HoTen, NgaySinh, CCCD, SDT, GioiTinh FROM SinhVien";

            // Gọi hàm ExecuteQuery để lấy kết quả dạng bảng (DataTable)
            return dbHelper.ExecuteQuery(query);
        }

        // Thêm hàm này vào class SinhVienDAL
        public bool InsertSinhVien(SinhVien sv)
        {
            string query = "INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, CCCD, SDT, GioiTinh) " +
                           "VALUES (@MaSV, @HoTen, @NgaySinh, @CCCD, @SDT, @GioiTinh)";

            // Đóng gói tham số an toàn
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@MaSV", sv.MaSV),
        new SqlParameter("@HoTen", sv.HoTen),
        new SqlParameter("@NgaySinh", sv.NgaySinh),
        new SqlParameter("@CCCD", sv.CCCD),
        new SqlParameter("@SDT", sv.SDT),
        new SqlParameter("@GioiTinh", sv.GioiTinh)
            };

            // Gọi ExecuteNonQuery, nếu số dòng ảnh hưởng > 0 nghĩa là Insert thành công
            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CheckMaSVExists(string maSV)
        {
            string query = "SELECT COUNT(*) FROM SinhVien WHERE MaSV = @MaSV";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaSV", maSV) };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        // 1. Hàm Cập nhật thông tin sinh viên
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

        // 2. Hàm Xóa sinh viên theo Mã SV
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
            string query = "SELECT MaSV, HoTen, NgaySinh, CCCD, SDT, GioiTinh FROM SinhVien " +
                           "WHERE MaSV LIKE @Keyword OR HoTen LIKE @Keyword";

            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@Keyword", "%" + keyword + "%")
            };

            return dbHelper.ExecuteQuery(query, parameters);
        }
    }
}