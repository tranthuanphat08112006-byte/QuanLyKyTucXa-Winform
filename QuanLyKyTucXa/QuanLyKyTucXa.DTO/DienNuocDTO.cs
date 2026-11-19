using System;

namespace QuanLyKyTucXa.DTO
{
    public class DienNuocDTO
    {
        public int MaDienNuoc { get; set; }
        public string MaPhong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int ChiSoDienCu { get; set; }
        public int ChiSoDienMoi { get; set; }
        public int ChiSoNuocCu { get; set; }
        public int ChiSoNuocMoi { get; set; }

        // Thuộc tính tính toán lượng tiêu thụ
        public int DienTieuThu => Math.Max(0, ChiSoDienMoi - ChiSoDienCu);
        public int NuocTieuThu => Math.Max(0, ChiSoNuocMoi - ChiSoNuocCu);

        // Thuộc tính mở rộng phục vụ hiển thị (JOIN với bảng Phong)
        public string TenPhong { get; set; }
        public string TenKhu { get; set; }

        public DienNuocDTO() { }

        public DienNuocDTO(string maPhong, int thang, int nam, int chiSoDienCu, int chiSoDienMoi, int chiSoNuocCu, int chiSoNuocMoi)
        {
            MaPhong = maPhong;
            Thang = thang;
            Nam = nam;
            ChiSoDienCu = chiSoDienCu;
            ChiSoDienMoi = chiSoDienMoi;
            ChiSoNuocCu = chiSoNuocCu;
            ChiSoNuocMoi = chiSoNuocMoi;
        }

        public DienNuocDTO(int maDienNuoc, string maPhong, int thang, int nam, int chiSoDienCu, int chiSoDienMoi, int chiSoNuocCu, int chiSoNuocMoi)
            : this(maPhong, thang, nam, chiSoDienCu, chiSoDienMoi, chiSoNuocCu, chiSoNuocMoi)
        {
            MaDienNuoc = maDienNuoc;
        }
    }
}

