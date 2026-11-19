using System;
using System.Data;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    public class HoaDonBLL
    {
        private readonly HoaDonDAL hoaDonDAL = new HoaDonDAL();
        private readonly DienNuocDAL dienNuocDAL = new DienNuocDAL();
        private readonly PhongDAL phongDAL = new PhongDAL();

        public DataTable GetAllHoaDon()
        {
            return hoaDonDAL.GetAllHoaDon();
        }

        public DataTable FilterHoaDon(string maPhong, int? thang, int? nam, string trangThai, string keyword)
        {
            return hoaDonDAL.FilterHoaDon(maPhong, thang, nam, trangThai, keyword);
        }

        public DataRow GetHoaDonById(int maHoaDon)
        {
            if (maHoaDon <= 0)
                throw new Exception("Mã hóa đơn không hợp lệ!");

            return hoaDonDAL.GetHoaDonById(maHoaDon);
        }

        public decimal GetDonGiaDien()
        {
            return hoaDonDAL.GetDonGiaDichVu("DIEN", 3500);
        }

        public decimal GetDonGiaNuoc()
        {
            return hoaDonDAL.GetDonGiaDichVu("NUOC", 15000);
        }

        public decimal GetPhiDichVuMacDinh()
        {
            return hoaDonDAL.GetDonGiaDichVu("RAC", 30000);
        }

        public DataTable GetAllCauHinhDichVu()
        {
            return hoaDonDAL.GetAllCauHinhDichVu();
        }

        public bool UpdateCauHinhDichVu(string maDichVu, decimal donGia, string ghiChu = null)
        {
            if (string.IsNullOrWhiteSpace(maDichVu))
                throw new Exception("Mã dịch vụ không được để trống!");

            if (donGia < 0)
                throw new Exception("Đơn giá dịch vụ không được nhỏ hơn 0!");

            return hoaDonDAL.UpdateCauHinhDichVu(maDichVu.Trim(), donGia, ghiChu);
        }

        /// <summary>
        /// Tính toán chi tiết các khoản thu trước khi lưu hóa đơn (phục vụ hiển thị preview trên giao diện).
        /// </summary>
        public HoaDonDTO TinhToanHoaDonDuKien(string maPhong, int thang, int nam, decimal tienDichVuKhac = 0)
        {
            if (string.IsNullOrWhiteSpace(maPhong))
                throw new Exception("Vui lòng chọn phòng cần tính hóa đơn!");

            maPhong = maPhong.Trim();

            // 1. Lấy thông tin phòng
            DataRow rowPhong = hoaDonDAL.GetThongTinPhongChoHoaDon(maPhong);
            if (rowPhong == null)
                throw new Exception($"Không tìm thấy phòng '{maPhong}' trong hệ thống!");

            decimal donGiaPhong = Convert.ToDecimal(rowPhong["DonGia"]);
            string tenPhong = rowPhong["TenPhong"]?.ToString() ?? ("Phòng " + maPhong);
            string tenKhu = rowPhong["TenKhu"]?.ToString() ?? "";

            // 2. Lấy thông tin chỉ số điện nước tháng này
            DataRow rowDN = hoaDonDAL.GetThongTinDienNuocChoHoaDon(maPhong, thang, nam);
            if (rowDN == null)
            {
                throw new Exception($"Phòng '{tenPhong}' chưa được ghi chỉ số điện nước cho tháng {thang}/{nam}!\nVui lòng vào mục 'Ghi chỉ số Điện - Nước' để cập nhật chỉ số trước khi lập hóa đơn.");
            }

            int soDien = Convert.ToInt32(rowDN["DienTieuThu"]);
            int soNuoc = Convert.ToInt32(rowDN["NuocTieuThu"]);

            // 3. Đơn giá
            decimal donGiaDien = GetDonGiaDien();
            decimal donGiaNuoc = GetDonGiaNuoc();

            // 4. Thành tiền
            decimal tienDien = soDien * donGiaDien;
            decimal tienNuoc = soNuoc * donGiaNuoc;
            decimal tongTien = donGiaPhong + tienDien + tienNuoc + tienDichVuKhac;

            return new HoaDonDTO
            {
                MaPhong = maPhong,
                TenPhong = tenPhong,
                TenKhu = tenKhu,
                Thang = thang,
                Nam = nam,
                TienPhong = donGiaPhong,
                SoDienTieuThu = soDien,
                TienDien = tienDien,
                SoNuocTieuThu = soNuoc,
                TienNuoc = tienNuoc,
                TienDichVuKhac = tienDichVuKhac,
                TongTien = tongTien,
                DaThanhToan = 0,
                TrangThai = "Chưa thanh toán",
                NgayLap = DateTime.Now
            };
        }

        public bool LapHoaDonMoi(HoaDonDTO hd)
        {
            if (hd == null)
                throw new Exception("Thông tin hóa đơn không hợp lệ!");

            if (string.IsNullOrWhiteSpace(hd.MaPhong))
                throw new Exception("Vui lòng chọn phòng cần lập hóa đơn!");

            hd.MaPhong = hd.MaPhong.Trim();

            if (!phongDAL.CheckMaPhongExists(hd.MaPhong))
                throw new Exception($"Phòng '{hd.MaPhong}' không tồn tại!");

            if (hd.Thang < 1 || hd.Thang > 12)
                throw new Exception("Tháng lập hóa đơn phải từ 1 đến 12!");

            if (hd.Nam < 2000 || hd.Nam > 2100)
                throw new Exception("Năm lập hóa đơn không hợp lệ!");

            // Kiểm tra trùng lặp
            if (hoaDonDAL.CheckHoaDonExists(hd.MaPhong, hd.Thang, hd.Nam))
                throw new Exception($"Hóa đơn của phòng {hd.MaPhong} cho tháng {hd.Thang}/{hd.Nam} đã được lập trước đó!");

            // Kiểm tra bản ghi điện nước bắt buộc
            DataRow rowDN = hoaDonDAL.GetThongTinDienNuocChoHoaDon(hd.MaPhong, hd.Thang, hd.Nam);
            if (rowDN == null)
                throw new Exception($"Phòng {hd.MaPhong} chưa có chỉ số điện nước tháng {hd.Thang}/{hd.Nam}! Vui lòng ghi chỉ số trước khi lập hóa đơn.");

            if (hd.TienPhong < 0 || hd.TienDien < 0 || hd.TienNuoc < 0 || hd.TienDichVuKhac < 0)
                throw new Exception("Các khoản tiền trong hóa đơn không được mang giá trị âm!");

            hd.TongTien = hd.TienPhong + hd.TienDien + hd.TienNuoc + hd.TienDichVuKhac;
            hd.DaThanhToan = 0;
            hd.TrangThai = "Chưa thanh toán";
            hd.NgayLap = DateTime.Now;

            return hoaDonDAL.InsertHoaDon(hd);
        }

        public bool UpdateHoaDon(HoaDonDTO hd)
        {
            if (hd == null || hd.MaHoaDon <= 0)
                throw new Exception("Mã hóa đơn không hợp lệ!");

            if (hd.TienPhong < 0 || hd.TienDien < 0 || hd.TienNuoc < 0 || hd.TienDichVuKhac < 0)
                throw new Exception("Các khoản tiền trong hóa đơn không được mang giá trị âm!");

            hd.TongTien = hd.TienPhong + hd.TienDien + hd.TienNuoc + hd.TienDichVuKhac;

            if (hd.DaThanhToan >= hd.TongTien)
                hd.TrangThai = "Đã thanh toán";
            else if (hd.DaThanhToan > 0)
                hd.TrangThai = "Thanh toán một phần";
            else
                hd.TrangThai = "Chưa thanh toán";

            return hoaDonDAL.UpdateHoaDon(hd);
        }

        public bool DeleteHoaDon(int maHoaDon)
        {
            if (maHoaDon <= 0)
                throw new Exception("Vui lòng chọn hóa đơn cần xóa!");

            DataRow row = hoaDonDAL.GetHoaDonById(maHoaDon);
            if (row == null)
                throw new Exception("Hóa đơn không tồn tại trong hệ thống!");

            decimal daThanhToan = Convert.ToDecimal(row["DaThanhToan"]);
            if (daThanhToan > 0)
            {
                throw new Exception($"Không thể xóa hóa đơn #{maHoaDon} vì đã phát sinh số tiền thanh toán ({daThanhToan:N0} đ)! Chỉ được xóa hóa đơn ở trạng thái chưa thanh toán.");
            }

            return hoaDonDAL.DeleteHoaDon(maHoaDon);
        }

        public DataTable GetThongKeTaiChinh()
        {
            return hoaDonDAL.GetThongKeTaiChinh();
        }
    }
}

