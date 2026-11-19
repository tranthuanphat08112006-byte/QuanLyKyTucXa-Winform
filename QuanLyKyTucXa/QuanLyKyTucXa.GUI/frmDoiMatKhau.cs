using System;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmDoiMatKhau : Form
    {
        private TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();

        public frmDoiMatKhau()
        {
            InitializeComponent();
            txtMatKhauMoi.TextChanged += TxtMatKhauMoi_TextChanged;
            UpdatePasswordStrengthHint();
        }

        private void TxtMatKhauMoi_TextChanged(object sender, EventArgs e)
        {
            UpdatePasswordStrengthHint();
        }

        private void UpdatePasswordStrengthHint()
        {
            UIHelper.UpdatePasswordStrengthLabel(
                lblGhiChu,
                txtMatKhauMoi.Text,
                "💡 Yêu cầu: ≥6 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt.");
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (!UserSession.IsLoggedIn)
            {
                MessageBox.Show("Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            string oldPass = txtMatKhauCu.Text;
            string newPass = txtMatKhauMoi.Text;
            string confirmPass = txtXacNhan.Text;

            try
            {
                taiKhoanBLL.ChangePassword(UserSession.CurrentUser.MaTaiKhoan, oldPass, newPass, confirmPass);
                MessageBox.Show("Đổi mật khẩu thành công! Mật khẩu mới đã được cập nhật.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhauCu.Focus();
                txtMatKhauCu.SelectAll();
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

