using System;
using System.Data;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    public class PhongBLL
    {
        private PhongDAL phongDAL = new PhongDAL();

        public DataTable GetAllPhong()
        {
            return phongDAL.GetAllPhong();
        }

        public DataTable FilterPhong(string keyword, string tenKhu, string trangThai)
        {
            return phongDAL.FilterPhong(keyword ?? "", tenKhu ?? "Tất cả dãy nhà", trangThai ?? "Tất cả trạng thái");
        }

        public bool InsertPhong(Phong p)
        {
            if (string.IsNullOrWhiteSpace(p.MaPhong) || string.IsNullOrWhiteSpace(p.TenKhu))
                throw new Exception("Mã phòng và Dãy nhà không được để trống!");

            if (p.SoGiuongToiDa <= 0)
                throw new Exception("Sức chứa tối đa phải lớn hơn 0!");

            if (p.SoGiuongTrong < 0 || p.SoGiuongTrong > p.SoGiuongToiDa)
                throw new Exception("Số lượng sinh viên hiện tại / Số giường trống không hợp lệ (không được vượt quá sức chứa tối đa)!");

            if (p.DonGia <= 0)
                throw new Exception("Đơn giá phòng phải lớn hơn 0 VNĐ!");

            if (phongDAL.CheckMaPhongExists(p.MaPhong))
                throw new Exception("Mã phòng '" + p.MaPhong + "' đã tồn tại trong hệ thống!");

            return phongDAL.InsertPhong(p);
        }

        public bool UpdatePhong(Phong p)
        {
            if (string.IsNullOrWhiteSpace(p.TenKhu))
                throw new Exception("Dãy nhà trực thuộc không được để trống!");

            if (p.SoGiuongToiDa <= 0)
                throw new Exception("Sức chứa tối đa phải lớn hơn 0!");

            if (p.SoGiuongTrong < 0 || p.SoGiuongTrong > p.SoGiuongToiDa)
                throw new Exception("Số lượng sinh viên hiện tại / Số giường trống không hợp lệ (không được vượt quá sức chứa tối đa)!");

            if (p.DonGia <= 0)
                throw new Exception("Đơn giá phòng phải lớn hơn 0 VNĐ!");

            return phongDAL.UpdatePhong(p);
        }

        public bool DeletePhong(string maPhong)
        {
            if (string.IsNullOrWhiteSpace(maPhong))
                throw new Exception("Vui lòng chọn phòng cần xóa!");

            if (phongDAL.CheckPhongHasActiveStudentsOrContracts(maPhong))
                throw new Exception("Không thể xóa phòng đang có sinh viên lưu trú hoặc đang có hợp đồng liên kết!");

            return phongDAL.DeletePhong(maPhong);
        }

        public DataTable SearchPhong(string keyword)
        {
            keyword = (keyword ?? "").Trim();
            return phongDAL.SearchPhong(keyword);
        }

        // ========================================================
        // QUẢN LÝ DÃY NHÀ (KHU NHÀ)
        // ========================================================

        public DataTable GetAllDayNha(string keyword = "")
        {
            return phongDAL.GetAllDayNha(keyword);
        }

        public bool InsertDayNha(DayNha d)
        {
            if (string.IsNullOrWhiteSpace(d.MaKhu) || string.IsNullOrWhiteSpace(d.TenKhu))
                throw new Exception("Mã dãy nhà và Tên dãy nhà không được để trống!");

            if (phongDAL.CheckMaKhuExists(d.MaKhu))
                throw new Exception("Mã dãy nhà '" + d.MaKhu + "' đã tồn tại!");

            if (phongDAL.CheckTenKhuExists(d.TenKhu))
                throw new Exception("Tên dãy nhà '" + d.TenKhu + "' đã tồn tại!");

            return phongDAL.InsertDayNha(d);
        }

        public bool UpdateDayNha(DayNha d, string oldTenKhu = null)
        {
            if (string.IsNullOrWhiteSpace(d.MaKhu) || string.IsNullOrWhiteSpace(d.TenKhu))
                throw new Exception("Mã dãy nhà và Tên dãy nhà không được để trống!");

            if (phongDAL.CheckTenKhuExists(d.TenKhu, d.MaKhu))
                throw new Exception("Tên dãy nhà '" + d.TenKhu + "' đã được sử dụng cho dãy nhà khác!");

            return phongDAL.UpdateDayNha(d, oldTenKhu);
        }

        public bool DeleteDayNha(string maKhu, string tenKhu)
        {
            if (string.IsNullOrWhiteSpace(maKhu))
                throw new Exception("Vui lòng chọn dãy nhà cần xóa!");

            int countPhong = phongDAL.CountPhongInDayNha(tenKhu);
            if (countPhong > 0)
            {
                throw new Exception($"Không thể xóa dãy nhà '{tenKhu}' vì vẫn còn {countPhong} phòng trực thuộc! Vui lòng chuyển hoặc xóa các phòng trước.");
            }

            return phongDAL.DeleteDayNha(maKhu);
        }
    }
}
