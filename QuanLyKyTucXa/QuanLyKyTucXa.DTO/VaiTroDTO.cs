using System;

namespace QuanLyKyTucXa.DTO
{
    public class VaiTroDTO
    {
        public string MaVaiTro { get; set; }
        public string TenVaiTro { get; set; }
        public string MoTa { get; set; }

        public VaiTroDTO() { }

        public VaiTroDTO(string maVaiTro, string tenVaiTro, string moTa)
        {
            MaVaiTro = maVaiTro;
            TenVaiTro = tenVaiTro;
            MoTa = moTa;
        }
    }
}

