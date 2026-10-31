using System;
using System.Data;
using QuanLyKyTucXa.DAL; // Nhận diện tầng DAL
using QuanLyKyTucXa.DTO; // (Sẽ dùng khi làm chức năng Thêm/Sửa/Xóa)

namespace QuanLyKyTucXa.BLL
{
    public class SinhVienBLL
    {
        // Khởi tạo đối tượng DAL để gọi lệnh SQL
        private SinhVienDAL svDAL = new SinhVienDAL();

        // Hàm lấy danh sách cung cấp cho Giao diện
        public DataTable GetAllSinhVien()
        {
            // Tương lai bạn có thể thêm các logic kiểm tra quyền ở đây trước khi gọi DAL
            return svDAL.GetAllSinhVien();
        }

        public bool InsertSinhVien(SinhVien sv)
        {
            if (string.IsNullOrWhiteSpace(sv.MaSV) || string.IsNullOrWhiteSpace(sv.HoTen))
                throw new Exception("Mã sinh viên và Họ tên không được để trống!");

            // Chặn trùng mã sinh viên
            if (svDAL.CheckMaSVExists(sv.MaSV))
                throw new Exception("Mã sinh viên này đã tồn tại trong hệ thống!");

            return svDAL.InsertSinhVien(sv);
        }

        // 1. Kiểm duyệt và gọi Update
        public bool UpdateSinhVien(SinhVien sv)
        {
            if (string.IsNullOrWhiteSpace(sv.HoTen))
            {
                throw new Exception("Họ tên sinh viên không được để trống!");
            }
            return svDAL.UpdateSinhVien(sv);
        }

        // 2. Kiểm duyệt và gọi Delete
        public bool DeleteSinhVien(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                throw new Exception("Vui lòng chọn sinh viên cần xóa!");
            }
            return svDAL.DeleteSinhVien(maSV);
        }
        public DataTable SearchSinhVien(string keyword)
        {
            // Loại bỏ các khoảng trắng thừa trước khi tìm kiếm
            keyword = keyword.Trim();
            return svDAL.SearchSinhVien(keyword);
        }
    }
}