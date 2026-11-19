using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyKyTucXa.BLL.UTIL
{
    /// <summary>
    /// Tiện ích kiểm tra độ mạnh mật khẩu và mã hóa/xác thực mật khẩu bằng BCrypt.
    /// </summary>
    public static class PasswordCheck
    {
        private const int DefaultWorkFactor = 11;

        /// <summary>
        /// Mã hóa mật khẩu thuần bằng thuật toán BCrypt.
        /// </summary>
        public static string HashPassword(string plainPassword, int workFactor = DefaultWorkFactor)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: workFactor);
        }

        /// <summary>
        /// Xác thực mật khẩu nhập vào với chuỗi băm BCrypt trong cơ sở dữ liệu.
        /// </summary>
        public static bool VerifyPassword(string plainPassword, string passwordHash)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(passwordHash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(plainPassword, passwordHash);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Đánh giá các tiêu chí độ mạnh của mật khẩu để hiển thị trực quan trên giao diện hoặc kiểm tra ở tầng BLL.
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
            if (string.IsNullOrEmpty(password))
            {
                hasMinLength = false;
                hasUpper = false;
                hasLower = false;
                hasDigit = false;
                hasSpecial = false;
                hasNoWhitespace = true;
                return false;
            }

            hasMinLength = password.Length >= 6;
            hasUpper = password.Any(char.IsUpper);
            hasLower = password.Any(char.IsLower);
            hasDigit = password.Any(char.IsDigit);
            hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch));
            hasNoWhitespace = !password.Any(char.IsWhiteSpace);

            return hasMinLength && hasUpper && hasLower && hasDigit && hasSpecial && hasNoWhitespace;
        }

        /// <summary>
        /// Kiểm tra bắt buộc độ mạnh mật khẩu khi tạo mới, đặt lại hoặc đổi mật khẩu.
        /// </summary>
        public static void ValidatePasswordStrength(string password, string fieldLabel = "Mật khẩu")
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception($"{fieldLabel} không được để trống!");

            bool hasMinLength, hasUpper, hasLower, hasDigit, hasSpecial, hasNoWhitespace;
            bool isValid = EvaluatePasswordStrength(
                password,
                out hasMinLength,
                out hasUpper,
                out hasLower,
                out hasDigit,
                out hasSpecial,
                out hasNoWhitespace);

            if (isValid) return;

            if (!hasNoWhitespace)
                throw new Exception($"{fieldLabel} không được chứa khoảng trắng!");

            List<string> missing = new List<string>();
            if (!hasMinLength) missing.Add("tối thiểu 6 ký tự");
            if (!hasUpper) missing.Add("ít nhất 1 chữ in hoa (A-Z)");
            if (!hasLower) missing.Add("ít nhất 1 chữ thường (a-z)");
            if (!hasDigit) missing.Add("ít nhất 1 chữ số (0-9)");
            if (!hasSpecial) missing.Add("ít nhất 1 ký tự đặc biệt (VD: @, #, $, !...)");

            throw new Exception($"{fieldLabel} chưa đạt yêu cầu bảo mật! Cần bổ sung:\n- " + string.Join("\n- ", missing));
        }
    }
}
