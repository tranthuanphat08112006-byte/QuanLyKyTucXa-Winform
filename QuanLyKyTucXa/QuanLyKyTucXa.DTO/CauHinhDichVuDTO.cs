using System;

namespace QuanLyKyTucXa.DTO
{
    public class CauHinhDichVuDTO
    {
        public string MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public decimal DonGia { get; set; }
        public string DonViTinh { get; set; }
        public string GhiChu { get; set; }

        public CauHinhDichVuDTO() { }

        public CauHinhDichVuDTO(string maDichVu, string tenDichVu, decimal donGia, string donViTinh, string ghiChu = "")
        {
            MaDichVu = maDichVu;
            TenDichVu = tenDichVu;
            DonGia = donGia;
            DonViTinh = donViTinh;
            GhiChu = ghiChu;
        }
    }
}

