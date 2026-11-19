using System;

namespace QuanLyKyTucXa.DTO
{
    public class Phong
    {
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }
        public string TenKhu { get; set; }
        public string LoaiPhong { get; set; }
        public int SoGiuongToiDa { get; set; }
        public int SoGiuongTrong { get; set; }
        public int SoNguoiHienTai
        {
            get
            {
                int count = SoGiuongToiDa - SoGiuongTrong;
                return count < 0 ? 0 : count;
            }
        }
        public decimal DonGia { get; set; }
        public string TrangThai { get; set; }

        public Phong() { }

        public Phong(string maPhong, string tenKhu, string loaiPhong, int soGiuongToiDa, int soGiuongTrong, decimal donGia)
        {
            MaPhong = maPhong;
            TenPhong = "Phòng " + maPhong;
            TenKhu = tenKhu;
            LoaiPhong = loaiPhong;
            SoGiuongToiDa = soGiuongToiDa;
            SoGiuongTrong = soGiuongTrong;
            DonGia = donGia;
            TrangThai = soGiuongTrong <= 0 ? "Đã đầy" : "Còn trống";
        }

        public Phong(string maPhong, string tenPhong, string tenKhu, string loaiPhong, int soGiuongToiDa, int soGiuongTrong, decimal donGia, string trangThai)
        {
            MaPhong = maPhong;
            TenPhong = string.IsNullOrWhiteSpace(tenPhong) ? ("Phòng " + maPhong) : tenPhong;
            TenKhu = tenKhu;
            LoaiPhong = loaiPhong;
            SoGiuongToiDa = soGiuongToiDa;
            SoGiuongTrong = soGiuongTrong;
            DonGia = donGia;
            TrangThai = string.IsNullOrWhiteSpace(trangThai) ? (soGiuongTrong <= 0 ? "Đã đầy" : "Còn trống") : trangThai;
        }
    }
}
