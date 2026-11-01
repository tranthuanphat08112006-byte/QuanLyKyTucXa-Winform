using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKyTucXa.DTO
{
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
