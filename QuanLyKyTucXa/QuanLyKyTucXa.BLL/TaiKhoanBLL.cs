using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using QuanLyKyTucXa.BLL.UTIL;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    public class TaiKhoanBLL
    {
        private TaiKhoanDAL taiKhoanDAL = new TaiKhoanDAL();
        private VaiTroDAL vaiTroDAL = new VaiTroDAL();

        #region 1. Đăng nhập / Đăng xuất & Kiểm tra phiên

        public TaiKhoanDTO Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Vui lòng nhập tên đăng nhập!");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Vui lòng nhập mật khẩu!");

            TaiKhoanDTO user = taiKhoanDAL.GetTaiKhoanByUsername(username.Trim());
            if (user == null)
            {
                // Thông báo chung an toàn để phòng tránh dò quét tài khoản
                throw new Exception("Tên đăng nhập hoặc mật khẩu không chính xác!");
            }

            // Kiểm tra mật khẩu mã hóa BCrypt thông qua UTIL.PasswordCheck
            bool isValidPassword = PasswordCheck.VerifyPassword(password, user.MatKhauHash);

            if (!isValidPassword)
            {
                throw new Exception("Tên đăng nhập hoặc mật khẩu không chính xác!");
            }

            // Kiểm tra trạng thái hoạt động của tài khoản
            if (!user.TrangThai)
            {
                throw new Exception("Tài khoản của bạn đã bị khóa. Vui lòng liên hệ Quản trị viên để được hỗ trợ!");
            }

            // Xóa mã băm mật khẩu trước khi đưa vào bộ nhớ phiên người dùng
            user.MatKhauHash = null;
            return user;
        }

        public bool Register(string username, string password, string confirmPassword, string hoTen, string vaiTro = "Admin")
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Tên đăng nhập không được để trống!");

            string cleanUsername = username.Trim();
            if (cleanUsername.Length < 3 || cleanUsername.Length > 50)
                throw new Exception("Tên đăng nhập phải có độ dài từ 3 đến 50 ký tự!");

            if (!Regex.IsMatch(cleanUsername, @"^[a-zA-Z0-9_]+$"))
                throw new Exception("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới (_), không chứa dấu cách hay ký tự đặc biệt!");

            if (taiKhoanDAL.CheckUsernameExists(cleanUsername))
                throw new Exception($"Tên đăng nhập '{cleanUsername}' đã được sử dụng. Vui lòng chọn tên đăng nhập khác!");

            if (string.IsNullOrWhiteSpace(hoTen))
                throw new Exception("Họ và tên người dùng không được để trống!");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Mật khẩu không được để trống!");

            if (password.Length < 6)
                throw new Exception("Mật khẩu phải có độ dài tối thiểu 6 ký tự!");

            if (!string.Equals(password, confirmPassword))
                throw new Exception("Mật khẩu và xác nhận mật khẩu không trùng khớp!");

            // Xác định vai trò theo lựa chọn của người dùng
            string safeRole = "Admin";
            if (string.Equals(vaiTro, "NhanVien", StringComparison.OrdinalIgnoreCase) || string.Equals(vaiTro, "Nhân viên", StringComparison.OrdinalIgnoreCase))
            {
                safeRole = "NhanVien";
            }
            else if (!string.IsNullOrWhiteSpace(vaiTro) && vaiTroDAL.CheckMaVaiTroExists(vaiTro.Trim()))
            {
                safeRole = vaiTro.Trim();
            }
            else
            {
                safeRole = "Admin";
            }

            TaiKhoanDTO tk = new TaiKhoanDTO
            {
                TenDangNhap = cleanUsername,
                HoTen = hoTen.Trim(),
                VaiTro = safeRole,
                TrangThai = true,
                NgayTao = DateTime.Now,
                MatKhauHash = PasswordCheck.HashPassword(password)
            };

            return taiKhoanDAL.InsertTaiKhoan(tk);
        }

        public void CheckAdminPermission()
        {
            if (!UserSession.IsLoggedIn)
            {
                throw new UnauthorizedAccessException("Phiên đăng nhập đã hết hạn hoặc chưa đăng nhập. Vui lòng đăng nhập lại!");
            }

            if (!UserSession.IsAdmin)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền thực hiện chức năng này. Chỉ Quản trị viên (Admin) mới có quyền truy cập!");
            }
        }

        private void CheckTaiKhoanPermission(string action)
        {
            if (!UserSession.IsLoggedIn)
            {
                throw new UnauthorizedAccessException("Phiên đăng nhập đã hết hạn hoặc chưa đăng nhập. Vui lòng đăng nhập lại!");
            }

            bool allowed = false;
            switch (action)
            {
                case "View": allowed = UserSession.CanView(ModuleSystem.TaiKhoan); break;
                case "Add": allowed = UserSession.CanAdd(ModuleSystem.TaiKhoan); break;
                case "Edit": allowed = UserSession.CanEdit(ModuleSystem.TaiKhoan); break;
                case "Delete": allowed = UserSession.CanDelete(ModuleSystem.TaiKhoan); break;
            }

            if (!allowed)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền thực hiện chức năng này trên module Quản lý Tài khoản!");
            }
        }

        #endregion

        #region 2. Quản lý tài khoản

        public DataTable GetAllTaiKhoan()
        {
            CheckTaiKhoanPermission("View");
            return taiKhoanDAL.GetAllTaiKhoan();
        }

        public DataTable FilterTaiKhoan(string keyword, string vaiTro, string trangThai)
        {
            CheckTaiKhoanPermission("View");
            return taiKhoanDAL.FilterTaiKhoan(keyword ?? "", vaiTro ?? "Tất cả vai trò", trangThai ?? "Tất cả trạng thái");
        }

        public bool InsertTaiKhoan(TaiKhoanDTO tk, string plainPassword)
        {
            CheckTaiKhoanPermission("Add");

            if (tk == null)
                throw new Exception("Dữ liệu tài khoản không hợp lệ!");

            if (string.IsNullOrWhiteSpace(tk.TenDangNhap))
                throw new Exception("Tên đăng nhập không được để trống!");

            string username = tk.TenDangNhap.Trim();
            if (username.Length < 3 || username.Length > 50)
                throw new Exception("Tên đăng nhập phải có độ dài từ 3 đến 50 ký tự!");

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
                throw new Exception("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới (_), không chứa dấu cách hay ký tự đặc biệt!");

            if (taiKhoanDAL.CheckUsernameExists(username))
                throw new Exception($"Tên đăng nhập '{username}' đã được sử dụng. Vui lòng chọn tên đăng nhập khác!");

            ValidatePasswordStrength(plainPassword, "Mật khẩu khởi tạo");

            if (string.IsNullOrWhiteSpace(tk.HoTen))
                throw new Exception("Họ và tên người dùng không được để trống!");

            if (string.IsNullOrWhiteSpace(tk.VaiTro))
                throw new Exception("Vui lòng chọn vai trò cho tài khoản!");

            if (!vaiTroDAL.CheckMaVaiTroExists(tk.VaiTro.Trim()))
                throw new Exception($"Vai trò '{tk.VaiTro}' không tồn tại trong hệ thống!");

            // Mã hóa mật khẩu bằng BCrypt thông qua UTIL.PasswordCheck
            tk.MatKhauHash = PasswordCheck.HashPassword(plainPassword);
            tk.TenDangNhap = username;
            tk.VaiTro = tk.VaiTro.Trim();
            tk.NgayTao = DateTime.Now;

            return taiKhoanDAL.InsertTaiKhoan(tk);
        }

        public bool UpdateTaiKhoan(TaiKhoanDTO tk)
        {
            CheckTaiKhoanPermission("Edit");

            if (tk == null)
                throw new Exception("Dữ liệu tài khoản không hợp lệ!");

            if (tk.MaTaiKhoan <= 0)
                throw new Exception("Mã tài khoản không hợp lệ!");

            if (string.IsNullOrWhiteSpace(tk.HoTen))
                throw new Exception("Họ và tên người dùng không được để trống!");

            if (string.IsNullOrWhiteSpace(tk.VaiTro))
                throw new Exception("Vui lòng chọn vai trò cho tài khoản!");

            if (!vaiTroDAL.CheckMaVaiTroExists(tk.VaiTro.Trim()))
                throw new Exception($"Vai trò '{tk.VaiTro}' không tồn tại trong hệ thống!");

            TaiKhoanDTO currentInDb = taiKhoanDAL.GetTaiKhoanById(tk.MaTaiKhoan);
            if (currentInDb == null)
                throw new Exception("Không tìm thấy thông tin tài khoản cần cập nhật!");

            // Quy tắc an toàn: Không được tự hạ quyền hoặc tự khóa chính mình
            if (UserSession.CurrentUser != null && UserSession.CurrentUser.MaTaiKhoan == tk.MaTaiKhoan)
            {
                if (string.Equals(currentInDb.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(tk.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Bạn không thể tự hạ quyền Quản trị viên của chính mình!");

                if (!tk.TrangThai)
                    throw new Exception("Bạn không thể tự khóa tài khoản của chính mình!");
            }

            // Quy tắc an toàn: Không thể khóa hoặc hạ quyền Admin cuối cùng đang hoạt động
            if (string.Equals(currentInDb.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                bool isDowngradingRole = !string.Equals(tk.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase);
                bool isLocking = !tk.TrangThai;

                if (isDowngradingRole || isLocking)
                {
                    int otherAdmins = taiKhoanDAL.CountActiveAdmin(tk.MaTaiKhoan);
                    if (otherAdmins <= 0)
                    {
                        throw new Exception("Không thể thực hiện! Đây là tài khoản Quản trị viên (Admin) duy nhất đang hoạt động trong hệ thống.");
                    }
                }
            }

            tk.VaiTro = tk.VaiTro.Trim();
            return taiKhoanDAL.UpdateTaiKhoan(tk);
        }

        public bool ToggleTrangThai(int maTaiKhoan)
        {
            CheckTaiKhoanPermission("Edit");

            TaiKhoanDTO target = taiKhoanDAL.GetTaiKhoanById(maTaiKhoan);
            if (target == null)
                throw new Exception("Không tìm thấy tài khoản cần thao tác!");

            if (UserSession.CurrentUser != null && UserSession.CurrentUser.MaTaiKhoan == maTaiKhoan)
            {
                throw new Exception("Bạn không thể tự khóa tài khoản của chính mình!");
            }

            bool newStatus = !target.TrangThai;

            // Nếu chuẩn bị khóa tài khoản Admin
            if (!newStatus && string.Equals(target.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                int otherAdmins = taiKhoanDAL.CountActiveAdmin(maTaiKhoan);
                if (otherAdmins <= 0)
                {
                    throw new Exception("Không thể khóa tài khoản Quản trị viên (Admin) cuối cùng đang hoạt động trong hệ thống!");
                }
            }

            return taiKhoanDAL.UpdateTrangThai(maTaiKhoan, newStatus);
        }

        public bool DeleteTaiKhoan(int maTaiKhoan)
        {
            CheckTaiKhoanPermission("Delete");

            if (UserSession.CurrentUser != null && UserSession.CurrentUser.MaTaiKhoan == maTaiKhoan)
            {
                throw new Exception("Bạn không thể tự xóa tài khoản của chính mình đang đăng nhập!");
            }

            TaiKhoanDTO target = taiKhoanDAL.GetTaiKhoanById(maTaiKhoan);
            if (target == null)
                throw new Exception("Tài khoản không tồn tại hoặc đã bị xóa trước đó!");

            if (string.Equals(target.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                int otherAdmins = taiKhoanDAL.CountActiveAdmin(maTaiKhoan);
                if (otherAdmins <= 0)
                {
                    throw new Exception("Không thể xóa tài khoản Quản trị viên (Admin) cuối cùng trong hệ thống!");
                }
            }

            return taiKhoanDAL.DeleteTaiKhoan(maTaiKhoan);
        }

        public bool AdminResetPassword(int maTaiKhoan, string newPassword)
        {
            CheckTaiKhoanPermission("Edit");

            ValidatePasswordStrength(newPassword, "Mật khẩu mới");

            TaiKhoanDTO target = taiKhoanDAL.GetTaiKhoanById(maTaiKhoan);
            if (target == null)
                throw new Exception("Không tìm thấy tài khoản để đặt lại mật khẩu!");

            string newHash = PasswordCheck.HashPassword(newPassword);
            return taiKhoanDAL.UpdateMatKhau(maTaiKhoan, newHash);
        }

        #endregion

        #region 3. Đổi mật khẩu cá nhân

        public bool ChangePassword(int maTaiKhoan, string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new Exception("Vui lòng nhập mật khẩu hiện tại!");

            if (string.IsNullOrWhiteSpace(confirmPassword))
                throw new Exception("Vui lòng nhập xác nhận lại mật khẩu mới!");

            ValidatePasswordStrength(newPassword, "Mật khẩu mới");

            if (!string.Equals(newPassword, confirmPassword))
                throw new Exception("Mật khẩu mới và xác nhận mật khẩu không trùng khớp!");

            if (string.Equals(currentPassword, newPassword))
                throw new Exception("Mật khẩu mới không được trùng với mật khẩu hiện tại!");

            TaiKhoanDTO user = taiKhoanDAL.GetTaiKhoanById(maTaiKhoan);
            if (user == null)
                throw new Exception("Tài khoản người dùng không tồn tại hoặc đã bị thay đổi!");

            bool isOldPasswordCorrect = PasswordCheck.VerifyPassword(currentPassword, user.MatKhauHash);
            if (!isOldPasswordCorrect)
                throw new Exception("Mật khẩu hiện tại không chính xác!");

            string newHash = PasswordCheck.HashPassword(newPassword);
            return taiKhoanDAL.UpdateMatKhau(maTaiKhoan, newHash);
        }

        #endregion

        #region 4. Sao lưu và Phục hồi cơ sở dữ liệu

        public bool BackupDatabase(string backupFilePath)
        {
            CheckAdminPermission();

            if (string.IsNullOrWhiteSpace(backupFilePath))
                throw new Exception("Đường dẫn file sao lưu không được để trống!");

            string folder = Path.GetDirectoryName(backupFilePath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return taiKhoanDAL.BackupDatabase(backupFilePath);
        }

        public bool RestoreDatabase(string restoreFilePath)
        {
            CheckAdminPermission();

            if (string.IsNullOrWhiteSpace(restoreFilePath))
                throw new Exception("Vui lòng chọn file sao lưu (.bak) để phục hồi!");

            if (!File.Exists(restoreFilePath))
                throw new Exception("File sao lưu không tồn tại hoặc không thể truy cập!");

            if (!string.Equals(Path.GetExtension(restoreFilePath), ".bak", StringComparison.OrdinalIgnoreCase))
                throw new Exception("File phục hồi phải có định dạng .bak chuẩn của SQL Server!");

            return taiKhoanDAL.RestoreDatabase(restoreFilePath);
        }

        #endregion

        #region 5. Bộ lọc & Kiểm tra độ mạnh mật khẩu (Ủy quyền qua UTIL.PasswordCheck)

        /// <summary>
        /// Đánh giá các tiêu chí độ mạnh của mật khẩu để hiển thị trực quan trên giao diện hoặc kiểm tra ở BLL.
        /// </summary>
        public static bool EvaluatePasswordStrength(
            string password,
            out bool hasMinLength,
            out bool hasUpper,
            out bool hasLower,
            out bool hasDigit,
            out bool hasSpecial,
            out bool hasNoWhitespace)
        {
            return PasswordCheck.EvaluatePasswordStrength(
                password,
                out hasMinLength,
                out hasUpper,
                out hasLower,
                out hasDigit,
                out hasSpecial,
                out hasNoWhitespace);
        }

        /// <summary>
        /// Kiểm tra bắt buộc độ mạnh mật khẩu ở tầng BLL khi tạo mới, đặt lại hoặc đổi mật khẩu.
        /// </summary>
        public static void ValidatePasswordStrength(string password, string fieldLabel = "Mật khẩu")
        {
            PasswordCheck.ValidatePasswordStrength(password, fieldLabel);
        }

        #endregion
    }
}

