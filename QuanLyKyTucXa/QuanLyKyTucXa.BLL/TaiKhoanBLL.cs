using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyKyTucXa.DAL; // Thêm DAL để gọi dữ liệu nếu cần

namespace QuanLyKyTucXa.BLL
{
    public class TaiKhoanBLL
    {
        public bool DangKy(string tenDangNhap, string matKhau)
        {
            // Viết logic gọi DAL xuống database để thêm tài khoản ở đây
            // Ví dụ tạm thời trả về true để test giao diện:
            return true;
        }
    }
}
