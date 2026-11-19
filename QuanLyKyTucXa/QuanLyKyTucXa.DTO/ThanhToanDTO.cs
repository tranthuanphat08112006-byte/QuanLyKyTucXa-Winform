using System;

namespace QuanLyKyTucXa.DTO
{
    public class ThanhToanDTO
    {
        public int MaThanhToan { get; set; }
        public int MaHoaDon { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public decimal SoTienThanhToan { get; set; }
        public string PhuongThuc { get; set; }
        public string NguoiNop { get; set; }
        public string GhiChu { get; set; }

        public ThanhToanDTO()
        {
            NgayThanhToan = DateTime.Now;
            PhuongThuc = "Tiền mặt";
        }

        public ThanhToanDTO(int maHoaDon, decimal soTienThanhToan, string phuongThuc = "Tiền mặt", string nguoiNop = "", string ghiChu = "")
        {
            MaHoaDon = maHoaDon;
            SoTienThanhToan = soTienThanhToan;
            PhuongThuc = string.IsNullOrWhiteSpace(phuongThuc) ? "Tiền mặt" : phuongThuc;
            NguoiNop = nguoiNop;
            GhiChu = ghiChu;
            NgayThanhToan = DateTime.Now;
        }
    }
}
