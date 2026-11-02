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
}
