using System;
using System.Collections.Generic;

namespace QuanLyKyTucXa.DTO
{
    /// <summary>
    /// Lưu trữ thông tin phiên làm việc và ma trận phân quyền của người dùng hiện tại trong toàn bộ ứng dụng
    /// </summary>
    public static class UserSession
    {
        private static Dictionary<string, PhanQuyenDTO> _permissions =
            new Dictionary<string, PhanQuyenDTO>(StringComparer.OrdinalIgnoreCase);

        public static TaiKhoanDTO CurrentUser { get; private set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static bool IsAdmin => CurrentUser != null &&
            string.Equals(CurrentUser.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase);

        public static bool IsNhanVien => CurrentUser != null &&
            string.Equals(CurrentUser.VaiTro, "NhanVien", StringComparison.OrdinalIgnoreCase);

        public static void Login(TaiKhoanDTO user, IEnumerable<PhanQuyenDTO> permissions = null)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            CurrentUser = user;
            UpdatePermissions(permissions);
        }

        public static void UpdatePermissions(IEnumerable<PhanQuyenDTO> permissions)
        {
            _permissions = new Dictionary<string, PhanQuyenDTO>(StringComparer.OrdinalIgnoreCase);
            if (permissions == null)
                return;

            foreach (var p in permissions)
            {
                if (p != null && !string.IsNullOrWhiteSpace(p.MaModule))
                {
                    _permissions[p.MaModule.Trim()] = p;
                }
            }
        }

        public static PhanQuyenDTO GetPermission(string maModule)
        {
            if (!IsLoggedIn || string.IsNullOrWhiteSpace(maModule))
                return null;

            if (IsAdmin)
                return new PhanQuyenDTO("Admin", maModule, true, true, true, true);

            if (_permissions.TryGetValue(maModule.Trim(), out PhanQuyenDTO perm))
                return perm;

            return null;
        }

        public static bool CanView(string maModule)
        {
            if (!IsLoggedIn) return false;
            if (IsAdmin) return true;
            PhanQuyenDTO p = GetPermission(maModule);
            return p != null && p.CoQuyenXem;
        }

        public static bool CanAdd(string maModule)
        {
            if (!IsLoggedIn) return false;
            if (IsAdmin) return true;
            PhanQuyenDTO p = GetPermission(maModule);
            return p != null && p.CoQuyenThem;
        }

        public static bool CanEdit(string maModule)
        {
            if (!IsLoggedIn) return false;
            if (IsAdmin) return true;
            PhanQuyenDTO p = GetPermission(maModule);
            return p != null && p.CoQuyenSua;
        }

        public static bool CanDelete(string maModule)
        {
            if (!IsLoggedIn) return false;
            if (IsAdmin) return true;
            PhanQuyenDTO p = GetPermission(maModule);
            return p != null && p.CoQuyenXoa;
        }

        public static void Logout()
        {
            CurrentUser = null;
            _permissions.Clear();
        }
    }
}
