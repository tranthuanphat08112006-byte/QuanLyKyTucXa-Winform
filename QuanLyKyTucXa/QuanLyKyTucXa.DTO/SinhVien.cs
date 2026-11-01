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
        public string TenKhu { get; set; }
        public string LoaiPhong { get; set; }
        public int SoGiuongToiDa { get; set; }
        public int SoGiuongTrong { get; set; }
        public decimal DonGia { get; set; }

        public Phong() { }

        public Phong(string maPhong, string tenKhu, string loaiPhong, int soGiuongToiDa, int soGiuongTrong, decimal donGia)
        {
            MaPhong = maPhong;
            TenKhu = tenKhu;
            LoaiPhong = loaiPhong;
            SoGiuongToiDa = soGiuongToiDa;
            SoGiuongTrong = soGiuongTrong;
            DonGia = donGia;
        }
    }
}
