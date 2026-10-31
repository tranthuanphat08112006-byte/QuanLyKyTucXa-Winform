using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKyTucXa.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void LoadUserControl(UserControl uc)
        {
            // BƯỚC 1: Hủy (Dispose) control cũ đang hiển thị để giải phóng RAM
            if (pnlContainer.Controls.Count > 0)
            {
                pnlContainer.Controls[0].Dispose();
                pnlContainer.Controls.Clear();
            }

            // BƯỚC 2: Cài đặt control mới lấp đầy khoảng trống
            uc.Dock = DockStyle.Fill;

            // BƯỚC 3: Đưa control mới vào panel và hiển thị
            pnlContainer.Controls.Add(uc);
            uc.BringToFront();
        }

        private void btnSinhVien_Click(object sender, EventArgs e)
        {
            frmQuanLySinhVien frm = new frmQuanLySinhVien();

            // Dùng ShowDialog() để cửa sổ bật lên, bắt buộc người dùng thao tác xong đóng lại 
            // mới được bấm tiếp các nút menu bên dưới màn hình chính.
            frm.ShowDialog();
        }

        private void pnlContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlMenu_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
