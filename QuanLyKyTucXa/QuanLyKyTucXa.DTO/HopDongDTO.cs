using System;

namespace QuanLyKyTucXa.DTO
{
    public class HopDongDTO
    {
        public string MaHopDong { get; set; }
        public string MaSV { get; set; }
        public string MaPhong { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string TrangThai { get; set; }

        // Thuộc tính mở rộng phục vụ hiển thị (JOIN)
        public string HoTen { get; set; }
        public string TenPhong { get; set; }

        public HopDongDTO()
        {
            TrangThai = "Đang hiệu lực";
        }

        public HopDongDTO(string maSV, string maPhong, DateTime ngayBatDau, DateTime ngayKetThuc, string trangThai = "Đang hiệu lực")
        {
            MaSV = maSV;
            MaPhong = maPhong;
            NgayBatDau = ngayBatDau;
            NgayKetThuc = ngayKetThuc;
            TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "Đang hiệu lực" : trangThai;
        }

        public HopDongDTO(string maHopDong, string maSV, string maPhong, DateTime ngayBatDau, DateTime ngayKetThuc, string trangThai)
        {
            MaHopDong = maHopDong;
            MaSV = maSV;
            MaPhong = maPhong;
            NgayBatDau = ngayBatDau;
            NgayKetThuc = ngayKetThuc;
            TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "Đang hiệu lực" : trangThai;
        }
    }
}

