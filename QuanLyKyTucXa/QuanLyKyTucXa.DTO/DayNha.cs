using System;

namespace QuanLyKyTucXa.DTO
{
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
