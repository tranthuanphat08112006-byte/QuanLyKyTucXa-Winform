using System;
using System.Data;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    public class DienNuocBLL
    {
        private readonly DienNuocDAL dienNuocDAL = new DienNuocDAL();
        private readonly PhongDAL phongDAL = new PhongDAL();

        public DataTable GetAllDienNuoc()
        {
            return dienNuocDAL.GetAllDienNuoc();
        }

        public DataTable FilterDienNuoc(string maPhong, int? thang, int? nam, string keyword)
        {
            return dienNuocDAL.FilterDienNuoc(maPhong, thang, nam, keyword);
        }

        public DataRow GetDienNuocById(int maDienNuoc)
        {
            if (maDienNuoc <= 0)
                throw new Exception("Mã chỉ số điện nước không hợp lệ!");

            return dienNuocDAL.GetDienNuocById(maDienNuoc);
        }

        public DataRow GetDienNuocByPhongThangNam(string maPhong, int thang, int nam)
        {
            if (string.IsNullOrWhiteSpace(maPhong))
                throw new Exception("Vui lòng chọn phòng!");

            return dienNuocDAL.GetDienNuocByPhongThangNam(maPhong.Trim(), thang, nam);
        }

        public DataRow GetChiSoThangTruoc(string maPhong, int thangHienTai, int namHienTai)
        {
            if (string.IsNullOrWhiteSpace(maPhong))
                return null;

            return dienNuocDAL.GetChiSoThangTruoc(maPhong.Trim(), thangHienTai, namHienTai);
        }

        private void ValidateDienNuoc(DienNuocDTO dn, bool isUpdate)
        {
            if (dn == null)
                throw new Exception("Dữ liệu chỉ số điện nước không hợp lệ!");

            if (string.IsNullOrWhiteSpace(dn.MaPhong))
                throw new Exception("Vui lòng chọn phòng cần ghi nhận chỉ số!");

            dn.MaPhong = dn.MaPhong.Trim();

            if (!phongDAL.CheckMaPhongExists(dn.MaPhong))
                throw new Exception($"Phòng '{dn.MaPhong}' không tồn tại trong hệ thống!");

            if (dn.Thang < 1 || dn.Thang > 12)
                throw new Exception("Tháng ghi chỉ số phải từ 1 đến 12!");

            if (dn.Nam < 2000 || dn.Nam > 2100)
                throw new Exception("Năm ghi nhận chỉ số không hợp lệ!");

            if (dn.ChiSoDienCu < 0)
                throw new Exception("Chỉ số điện cũ không được nhỏ hơn 0!");

            if (dn.ChiSoDienMoi < dn.ChiSoDienCu)
                throw new Exception($"Chỉ số điện mới ({dn.ChiSoDienMoi}) không được nhỏ hơn chỉ số điện cũ ({dn.ChiSoDienCu})!");

            if (dn.ChiSoNuocCu < 0)
                throw new Exception("Chỉ số nước cũ không được nhỏ hơn 0!");

            if (dn.ChiSoNuocMoi < dn.ChiSoNuocCu)
                throw new Exception($"Chỉ số nước mới ({dn.ChiSoNuocMoi}) không được nhỏ hơn chỉ số nước cũ ({dn.ChiSoNuocCu})!");

            // Kiểm tra trùng lặp bản ghi cho cùng phòng, tháng và năm
            if (dienNuocDAL.CheckDienNuocExists(dn.MaPhong, dn.Thang, dn.Nam, isUpdate ? (int?)dn.MaDienNuoc : null))
            {
                throw new Exception($"Đã tồn tại bản ghi chỉ số điện nước của phòng {dn.MaPhong} cho tháng {dn.Thang}/{dn.Nam}!");
            }
        }

        public bool InsertDienNuoc(DienNuocDTO dn)
        {
            ValidateDienNuoc(dn, false);
            return dienNuocDAL.InsertDienNuoc(dn);
        }

        public bool UpdateDienNuoc(DienNuocDTO dn)
        {
            if (dn.MaDienNuoc <= 0)
                throw new Exception("Không tìm thấy mã bản ghi cần cập nhật!");

            ValidateDienNuoc(dn, true);
            return dienNuocDAL.UpdateDienNuoc(dn);
        }

        public bool DeleteDienNuoc(int maDienNuoc)
        {
            if (maDienNuoc <= 0)
                throw new Exception("Vui lòng chọn bản ghi chỉ số điện nước cần xóa!");

            DataRow row = dienNuocDAL.GetDienNuocById(maDienNuoc);
            if (row == null)
                throw new Exception("Bản ghi chỉ số điện nước không tồn tại trong hệ thống!");

            string maPhong = row["MaPhong"].ToString();
            int thang = Convert.ToInt32(row["Thang"]);
            int nam = Convert.ToInt32(row["Nam"]);

            if (dienNuocDAL.CheckDienNuocDaCoHoaDon(maPhong, thang, nam))
            {
                throw new Exception($"Không thể xóa chỉ số điện nước này vì phòng {maPhong} đã được lập hóa đơn tháng {thang}/{nam}! Vui lòng xóa hoặc hủy hóa đơn trước.");
            }

            return dienNuocDAL.DeleteDienNuoc(maDienNuoc);
        }
    }
}

