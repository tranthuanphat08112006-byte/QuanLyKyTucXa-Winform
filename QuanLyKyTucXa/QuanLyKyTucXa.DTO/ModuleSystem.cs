using System.Collections.Generic;

namespace QuanLyKyTucXa.DTO
{
    /// <summary>
    /// Định nghĩa mã và tên hiển thị chuẩn của các module chức năng phục vụ phân quyền chi tiết
    /// </summary>
    public static class ModuleSystem
    {
        public const string SinhVien = "SinhVien";
        public const string Phong = "Phong";
        public const string HopDong = "HopDong";
        public const string DichVuTaiChinh = "DichVuTaiChinh";
        public const string TaiKhoan = "TaiKhoan";

        public static readonly IReadOnlyList<KeyValuePair<string, string>> AllModules =
            new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(SinhVien, "Quản lý Sinh viên"),
                new KeyValuePair<string, string>(Phong, "Quản lý Phòng & Dãy nhà"),
                new KeyValuePair<string, string>(HopDong, "Quản lý Hợp đồng"),
                new KeyValuePair<string, string>(DichVuTaiChinh, "Dịch vụ & Tài chính"),
                new KeyValuePair<string, string>(TaiKhoan, "Quản lý Tài khoản & Vai trò")
            }.AsReadOnly();

        public static string GetDisplayName(string maModule)
        {
            foreach (var item in AllModules)
            {
                if (string.Equals(item.Key, maModule, System.StringComparison.OrdinalIgnoreCase))
                    return item.Value;
            }
            return maModule ?? "";
        }
    }
}
