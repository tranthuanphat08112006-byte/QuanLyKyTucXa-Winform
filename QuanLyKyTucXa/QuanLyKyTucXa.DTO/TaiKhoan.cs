
using System;

namespace QuanLyKyTucXa.DTO
{
	public class TaiKhoan
	{
		public int MaTaiKhoan { get; set; }
		public string TenDangNhap { get; set; }
		public string MatKhau { get; set; }
		public string HoTen { get; set; }
		public string VaiTro { get; set; }
		public string MaSV { get; set; }
		public bool TrangThai { get; set; }
		public DateTime NgayTao { get; set; }

		public TaiKhoan()
		{
		}

		public TaiKhoan(string tenDangNhap, string matKhau,
						string hoTen, string vaiTro)
		{
			TenDangNhap = tenDangNhap;
			MatKhau = matKhau;
			HoTen = hoTen;
			VaiTro = vaiTro;
			TrangThai = true;
			NgayTao = DateTime.Now;
		}
	}
}
