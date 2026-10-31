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

            // Lệnh này bảo Winform hãy chạy MainForm đầu tiên khi mở app
            Application.Run(new MainForm());
        }
    }
}