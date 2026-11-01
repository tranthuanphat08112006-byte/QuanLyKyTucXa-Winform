using System;
using System.Data;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    public class PhongBLL
    {
        private PhongDAL phongDAL = new PhongDAL();

        // Lấy danh sách Phòng cung cấp cho Giao diện
        public DataTable GetAllPhong()
        {
            return phongDAL.GetAllPhong();
        }

        // Kiểm duyệt và gọi Insert
        public bool InsertPhong(Phong p)
        {
            if (string.IsNullOrWhiteSpace(p.MaPhong) || string.IsNullOrWhiteSpace(p.TenKhu) || string.IsNullOrWhiteSpace(p.LoaiPhong))
                throw new Exception("Mã phòng, Dãy nhà (Khu) và Loại phòng không được để trống!");

            if (p.SoGiuongToiDa <= 0)
                throw new Exception("Sức chứa (Số giường tối đa) phải lớn hơn 0!");

            if (p.SoGiuongTrong < 0 || p.SoGiuongTrong > p.SoGiuongToiDa)
                throw new Exception("Số giường trống phải từ 0 đến Số giường tối đa!");

            if (p.DonGia < 0)
                throw new Exception("Đơn giá phòng không được âm!");

            if (phongDAL.CheckMaPhongExists(p.MaPhong))
                throw new Exception("Mã phòng này đã tồn tại trong hệ thống!");

            return phongDAL.InsertPhong(p);
        }

        // Kiểm duyệt và gọi Update
        public bool UpdatePhong(Phong p)
        {
            if (string.IsNullOrWhiteSpace(p.TenKhu) || string.IsNullOrWhiteSpace(p.LoaiPhong))
                throw new Exception("Dãy nhà (Khu) và Loại phòng không được để trống!");

            if (p.SoGiuongToiDa <= 0)
                throw new Exception("Sức chứa (Số giường tối đa) phải lớn hơn 0!");

            if (p.SoGiuongTrong < 0 || p.SoGiuongTrong > p.SoGiuongToiDa)
                throw new Exception("Số giường trống phải từ 0 đến Số giường tối đa!");

            if (p.DonGia < 0)
                throw new Exception("Đơn giá phòng không được âm!");

            return phongDAL.UpdatePhong(p);
        }

        // Kiểm duyệt và gọi Delete
        public bool DeletePhong(string maPhong)
        {
            if (string.IsNullOrWhiteSpace(maPhong))
                throw new Exception("Vui lòng chọn phòng cần xóa!");

            return phongDAL.DeletePhong(maPhong);
        }

        // Tìm kiếm phòng
        public DataTable SearchPhong(string keyword)
        {
            keyword = keyword.Trim();
            return phongDAL.SearchPhong(keyword);
        }
    }
}
