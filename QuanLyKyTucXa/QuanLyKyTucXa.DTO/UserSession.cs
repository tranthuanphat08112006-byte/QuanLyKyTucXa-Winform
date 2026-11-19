using System;

namespace QuanLyKyTucXa.DTO
{
    /// <summary>
    /// Lưu trữ thông tin phiên làm việc của người dùng hiện tại trong toàn bộ ứng dụng
    /// </summary>
    public static class UserSession
    {
        public static TaiKhoanDTO CurrentUser { get; private set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static bool IsAdmin => CurrentUser != null &&
            string.Equals(CurrentUser.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase);

        public static bool IsNhanVien => CurrentUser != null &&
            string.Equals(CurrentUser.VaiTro, "NhanVien", StringComparison.OrdinalIgnoreCase);

        public static void Login(TaiKhoanDTO user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}

