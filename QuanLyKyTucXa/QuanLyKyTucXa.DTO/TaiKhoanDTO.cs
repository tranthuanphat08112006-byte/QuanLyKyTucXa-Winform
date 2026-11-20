using System;

namespace QuanLyKyTucXa.DTO
{
    public class TaiKhoanDTO
    {
        public int MaTaiKhoan { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhauHash { get; set; }
        public string HoTen { get; set; }
        public string VaiTro { get; set; }
        public string TenVaiTro { get; set; }
        public bool TrangThai { get; set; } = true;
        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Thuộc tính tiện ích hiển thị lên bảng dữ liệu
        public string TrangThaiHienThi => TrangThai ? "Hoạt động" : "Bị khóa";
        public string VaiTroHienThi
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(TenVaiTro))
                    return TenVaiTro;
                if (string.Equals(VaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
                    return "Quản trị viên";
                if (string.Equals(VaiTro, "NhanVien", StringComparison.OrdinalIgnoreCase))
                    return "Nhân viên";
                if (string.Equals(VaiTro, "SinhVien", StringComparison.OrdinalIgnoreCase))
                    return "Sinh viên";
                return VaiTro ?? "";
            }
        }

        public TaiKhoanDTO()
        {
            TrangThai = true;
            NgayTao = DateTime.Now;
        }

        public TaiKhoanDTO(string tenDangNhap, string matKhauHash, string hoTen, string vaiTro, bool trangThai = true)
        {
            TenDangNhap = tenDangNhap;
            MatKhauHash = matKhauHash;
            HoTen = hoTen;
            VaiTro = vaiTro;
            TrangThai = trangThai;
            NgayTao = DateTime.Now;
        }

        public TaiKhoanDTO(int maTaiKhoan, string tenDangNhap, string matKhauHash, string hoTen, string vaiTro, bool trangThai, DateTime ngayTao)
        {
            MaTaiKhoan = maTaiKhoan;
            TenDangNhap = tenDangNhap;
            MatKhauHash = matKhauHash;
            HoTen = hoTen;
            VaiTro = vaiTro;
            TrangThai = trangThai;
            NgayTao = ngayTao;
        }
    }
}

