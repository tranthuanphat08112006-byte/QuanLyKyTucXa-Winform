using System;
using System.Data;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    public class SinhVienBLL
    {
        private SinhVienDAL svDAL = new SinhVienDAL();

        public DataTable GetAllSinhVien()
        {
            return svDAL.GetAllSinhVien();
        }

        public DataTable FilterSinhVien(string keyword, string gioiTinh)
        {
            return svDAL.FilterSinhVien(keyword ?? "", gioiTinh ?? "Tất cả giới tính");
        }

        public int CountSinhVienCoHopDong()
        {
            return svDAL.CountSinhVienCoHopDong();
        }

        private void ValidateSinhVien(SinhVien sv, bool isUpdate)
        {
            if (sv == null)
                throw new Exception("Dữ liệu sinh viên không hợp lệ!");

            sv.MaSV = (sv.MaSV ?? "").Trim();
            sv.HoTen = (sv.HoTen ?? "").Trim();
            sv.CCCD = (sv.CCCD ?? "").Trim();
            sv.SDT = (sv.SDT ?? "").Trim();
            sv.GioiTinh = (sv.GioiTinh ?? "").Trim();

            if (string.IsNullOrWhiteSpace(sv.MaSV) || string.IsNullOrWhiteSpace(sv.HoTen))
                throw new Exception("Mã sinh viên và Họ tên không được để trống!");

            if (string.IsNullOrWhiteSpace(sv.GioiTinh))
                throw new Exception("Vui lòng chọn giới tính của sinh viên!");

            if (string.IsNullOrWhiteSpace(sv.CCCD))
                throw new Exception("Số CCCD / CMND không được để trống (yêu cầu duy nhất cho mỗi sinh viên)!");

            if (!isUpdate && svDAL.CheckMaSVExists(sv.MaSV))
                throw new Exception("Mã sinh viên này đã tồn tại trong hệ thống!");

            if (svDAL.CheckCCCDExists(sv.CCCD, isUpdate ? sv.MaSV : null))
                throw new Exception("Số CCCD / CMND '" + sv.CCCD + "' đã tồn tại ở sinh viên khác trong hệ thống! Mỗi sinh viên phải có một số CCCD riêng biệt.");
        }

        public bool InsertSinhVien(SinhVien sv)
        {
            ValidateSinhVien(sv, false);
            return svDAL.InsertSinhVien(sv);
        }

        public bool UpdateSinhVien(SinhVien sv)
        {
            ValidateSinhVien(sv, true);
            return svDAL.UpdateSinhVien(sv);
        }

        public bool DeleteSinhVien(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                throw new Exception("Vui lòng chọn sinh viên cần xóa!");
            }

            if (svDAL.CheckSinhVienHasHopDong(maSV))
            {
                throw new Exception("Không thể xóa sinh viên đang có dữ liệu Hợp đồng lưu trú trong hệ thống!");
            }

            return svDAL.DeleteSinhVien(maSV);
        }

        public DataTable SearchSinhVien(string keyword)
        {
            keyword = (keyword ?? "").Trim();
            return svDAL.SearchSinhVien(keyword);
        }
    }
}
