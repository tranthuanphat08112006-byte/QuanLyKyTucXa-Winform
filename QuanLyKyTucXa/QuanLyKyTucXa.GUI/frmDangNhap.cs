using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmDangNhap : Form
    {
        // Kéo rê form không viền
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
        private bool isShowPassword = false;

        public frmDangNhap()
        {
            InitializeComponent();
            this.pnlTop.MouseDown += PnlTop_MouseDown;
            this.lblSysTitle.MouseDown += PnlTop_MouseDown;
            this.lblSysSub.MouseDown += PnlTop_MouseDown;
        }

        private void PnlTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);
            }
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            // Tải icon vector GDI+
            picLogo.Image = IconHelper.CreateDormLogoIcon(Color.FromArgb(96, 165, 250), 34);
            UpdateEyeIcon();

            txtTenDangNhap.Focus();
        }

        private void UpdateEyeIcon()
        {
            btnTogglePass.Image = IconHelper.CreateEyeIcon(Color.FromArgb(100, 116, 139), 16, isShowPassword);
            txtMatKhau.UseSystemPasswordChar = !isShowPassword;
        }

        private void btnTogglePass_Click(object sender, EventArgs e)
        {
            isShowPassword = !isShowPassword;
            UpdateEyeIcon();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            string username = txtTenDangNhap.Text.Trim();
            string password = txtMatKhau.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Vui lòng nhập tên đăng nhập!");
                txtTenDangNhap.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Vui lòng nhập mật khẩu!");
                txtMatKhau.Focus();
                return;
            }

            try
            {
                btnDangNhap.Enabled = false;
                btnDangNhap.Text = "ĐANG XÁC THỰC...";

                TaiKhoanDTO user = taiKhoanBLL.Authenticate(username, password);

                // Lưu thông tin phiên đăng nhập
                UserSession.Login(user);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                txtMatKhau.SelectAll();
                txtMatKhau.Focus();
            }
            finally
            {
                btnDangNhap.Enabled = true;
                btnDangNhap.Text = "ĐĂNG NHẬP HỆ THỐNG";
            }
        }

        private void ShowError(string message)
        {
            lblError.ForeColor = Color.FromArgb(239, 68, 68);
            lblError.Text = "⚠️ " + message;
            lblError.Visible = true;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

