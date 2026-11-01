using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.DAL
{
    public class PhongDAL
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        // Lấy toàn bộ danh sách Phòng
        public DataTable GetAllPhong()
        {
            string query = "SELECT MaPhong, TenKhu, LoaiPhong, SoGiuongToiDa, SoGiuongTrong, DonGia FROM Phong";
            return dbHelper.ExecuteQuery(query);
        }

        // Thêm mới Phòng
        public bool InsertPhong(Phong p)
        {
            string query = "INSERT INTO Phong (MaPhong, TenKhu, LoaiPhong, SoGiuongToiDa, SoGiuongTrong, DonGia) " +
                           "VALUES (@MaPhong, @TenKhu, @LoaiPhong, @SoGiuongToiDa, @SoGiuongTrong, @DonGia)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", p.MaPhong),
                new SqlParameter("@TenKhu", p.TenKhu),
                new SqlParameter("@LoaiPhong", p.LoaiPhong),
                new SqlParameter("@SoGiuongToiDa", p.SoGiuongToiDa),
                new SqlParameter("@SoGiuongTrong", p.SoGiuongTrong),
                new SqlParameter("@DonGia", p.DonGia)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        // Kiểm tra trùng Mã Phòng
        public bool CheckMaPhongExists(string maPhong)
        {
            string query = "SELECT COUNT(*) FROM Phong WHERE MaPhong = @MaPhong";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaPhong", maPhong) };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }

        // Cập nhật thông tin Phòng (giá, sức chứa, dãy nhà, loại phòng)
        public bool UpdatePhong(Phong p)
        {
            string query = "UPDATE Phong SET TenKhu = @TenKhu, LoaiPhong = @LoaiPhong, " +
                           "SoGiuongToiDa = @SoGiuongToiDa, SoGiuongTrong = @SoGiuongTrong, DonGia = @DonGia " +
                           "WHERE MaPhong = @MaPhong";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", p.MaPhong),
                new SqlParameter("@TenKhu", p.TenKhu),
                new SqlParameter("@LoaiPhong", p.LoaiPhong),
                new SqlParameter("@SoGiuongToiDa", p.SoGiuongToiDa),
                new SqlParameter("@SoGiuongTrong", p.SoGiuongTrong),
                new SqlParameter("@DonGia", p.DonGia)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        // Xóa Phòng theo Mã Phòng
        public bool DeletePhong(string maPhong)
        {
            string query = "DELETE FROM Phong WHERE MaPhong = @MaPhong";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaPhong", maPhong)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        // Tìm kiếm Phòng theo Mã Phòng, Tên Khu (Dãy nhà) hoặc Loại Phòng
        public DataTable SearchPhong(string keyword)
        {
            string query = "SELECT MaPhong, TenKhu, LoaiPhong, SoGiuongToiDa, SoGiuongTrong, DonGia FROM Phong " +
                           "WHERE MaPhong LIKE @Keyword OR TenKhu LIKE @Keyword OR LoaiPhong LIKE @Keyword";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Keyword", "%" + keyword + "%")
            };

            return dbHelper.ExecuteQuery(query, parameters);
        }
    }
}
