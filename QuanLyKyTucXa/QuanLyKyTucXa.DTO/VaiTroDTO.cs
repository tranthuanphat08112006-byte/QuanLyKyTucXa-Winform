using System;

namespace QuanLyKyTucXa.DTO
{
    public class VaiTroDTO
    {
        public string MaVaiTro { get; set; }
        public string TenVaiTro { get; set; }
        public string MoTa { get; set; }
        public int SoTaiKhoan { get; set; }

        public VaiTroDTO() { }

        public VaiTroDTO(string maVaiTro, string tenVaiTro, string moTa, int soTaiKhoan = 0)
        {
            MaVaiTro = maVaiTro;
            TenVaiTro = tenVaiTro;
            MoTa = moTa;
            SoTaiKhoan = soTaiKhoan;
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(TenVaiTro) ? MaVaiTro : TenVaiTro;
        }
    }
}
