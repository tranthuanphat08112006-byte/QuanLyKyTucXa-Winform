using System;

namespace QuanLyKyTucXa.DTO
{
    public class HoaDonDTO
    {
        public int MaHoaDon { get; set; }
        public string MaPhong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public decimal TienPhong { get; set; }
        public decimal TienDien { get; set; }
        public decimal TienNuoc { get; set; }
        public decimal TienDichVuKhac { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } // "Chưa thanh toán", "Thanh toán một phần", "Đã thanh toán"
        public decimal DaThanhToan { get; set; }
        public decimal ConNo => Math.Max(0, TongTien - DaThanhToan);
        public DateTime NgayLap { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public string GhiChu { get; set; }

        // Thuộc tính mở rộng phục vụ hiển thị
        public string TenPhong { get; set; }
        public string TenKhu { get; set; }
        public int SoDienTieuThu { get; set; }
        public int SoNuocTieuThu { get; set; }

        public HoaDonDTO()
        {
            TrangThai = "Chưa thanh toán";
            NgayLap = DateTime.Now;
        }

        public HoaDonDTO(string maPhong, int thang, int nam, decimal tienPhong, decimal tienDien, decimal tienNuoc, decimal tienDichVuKhac = 0, string ghiChu = "")
        {
            MaPhong = maPhong;
            Thang = thang;
            Nam = nam;
            TienPhong = tienPhong;
            TienDien = tienDien;
            TienNuoc = tienNuoc;
            TienDichVuKhac = tienDichVuKhac;
            TongTien = tienPhong + tienDien + tienNuoc + tienDichVuKhac;
            DaThanhToan = 0;
            TrangThai = "Chưa thanh toán";
            NgayLap = DateTime.Now;
            GhiChu = ghiChu;
        }
    }
}

