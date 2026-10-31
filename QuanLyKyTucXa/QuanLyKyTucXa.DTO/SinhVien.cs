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
        public string LoaiPhong { get; set; }
        public decimal DonGiaHienTai { get; set; }
        public int SoChoToiDa { get; set; }
        public string TrangThai { get; set; }

        public Phong() { }

        public Phong(string maPhong, string loaiPhong, decimal donGia, int soCho, string trangThai)
        {
            MaPhong = maPhong;
            LoaiPhong = loaiPhong;
            DonGiaHienTai = donGia;
            SoChoToiDa = soCho;
            TrangThai = trangThai;
        }
    }


}
