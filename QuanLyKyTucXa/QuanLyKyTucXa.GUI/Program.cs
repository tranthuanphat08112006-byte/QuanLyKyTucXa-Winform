using System;
using System.Windows.Forms;

namespace QuanLyKyTucXa.GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Vòng lặp Đăng nhập - Đăng xuất an toàn trong toàn bộ ứng dụng
            while (true)
            {
                using (frmDangNhap loginForm = new frmDangNhap())
                {
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        // Người dùng chọn Thoát ở màn hình đăng nhập
                        break;
                    }
                }

                // Khi đăng nhập thành công, mở màn hình chính MainForm
                using (MainForm mainForm = new MainForm())
                {
                    Application.Run(mainForm);

                    // Nếu người dùng đóng form không phải do Đăng xuất (DialogResult != Retry) thì kết thúc ứng dụng
                    if (mainForm.DialogResult != DialogResult.Retry)
                    {
                        break;
                    }
                }
            }
        }
    }
}