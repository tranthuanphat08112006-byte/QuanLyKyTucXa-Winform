using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKyTucXa.DTO
{
    public class SinhVien
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string CCCD { get; set; }
        public string SDT { get; set; }
        public string GioiTinh { get; set; }
        public string TenPhong { get; set; }
        public string TrangThai { get; set; }

        public SinhVien() { }

        public SinhVien(string maSV, string hoTen, DateTime ngaySinh, string cccd, string sdt, string gioiTinh)
        {
            MaSV = maSV;
            HoTen = hoTen;
            NgaySinh = ngaySinh;
            CCCD = cccd;
            SDT = sdt;
            GioiTinh = gioiTinh;
        }
    }

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

    public class DayNha
    {
        public string MaKhu { get; set; }
        public string TenKhu { get; set; }
        public string LoaiKhu { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }

        public DayNha() { }

        public DayNha(string maKhu, string tenKhu, string loaiKhu, string moTa, string trangThai = "Hoạt động")
        {
            MaKhu = maKhu;
            TenKhu = tenKhu;
            LoaiKhu = loaiKhu;
            MoTa = moTa;
            TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "Hoạt động" : trangThai;
        }
    }
}
