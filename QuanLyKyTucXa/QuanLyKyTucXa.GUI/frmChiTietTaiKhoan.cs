using System;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmChiTietTaiKhoan : Form
    {
        private TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
        private TaiKhoanDTO tkCurrent = null;
        private bool isEditMode = false;

        // Constructor Thêm mới
        public frmChiTietTaiKhoan()
        {
            InitializeComponent();
            isEditMode = false;
            this.Text = "Thêm tài khoản quản trị mới";
            lblFormTitle.Text = "THÊM TÀI KHOẢN MỚI";
            lblFormSub.Text = "Khởi tạo tài khoản quản trị viên và thiết lập mật khẩu ban đầu";
            txtMatKhau.TextChanged += TxtMatKhau_TextChanged;
            UpdatePasswordHint();
        }

        // Constructor Chỉnh sửa
        public frmChiTietTaiKhoan(TaiKhoanDTO tk) : this()
        {
            this.tkCurrent = tk;
            this.isEditMode = true;

            this.Text = "Chỉnh sửa tài khoản - " + tk.TenDangNhap;
            lblFormTitle.Text = "CHỈNH SỬA TÀI KHOẢN: " + tk.TenDangNhap;
            lblFormSub.Text = "Cập nhật họ tên, mật khẩu mới và trạng thái hoạt động";

            txtTenDangNhap.Text = tk.TenDangNhap;
            txtTenDangNhap.ReadOnly = true;

            // Cho phép nhập mật khẩu mới nếu cần đổi (đề phòng quên mật khẩu)
            lblMatKhau.Text = "Mật khẩu mới (nếu đổi):";
            txtMatKhau.Text = "";
            txtMatKhau.ReadOnly = false;
            txtMatKhau.Enabled = true;

            txtHoTen.Text = tk.HoTen;
            chkTrangThai.Checked = tk.TrangThai;
            UpdatePasswordHint();
        }

        private void TxtMatKhau_TextChanged(object sender, EventArgs e)
        {
            UpdatePasswordHint();
        }

        private void UpdatePasswordHint()
        {
            string emptyHint = isEditMode
                ? "💡 Để trống nếu giữ nguyên mật khẩu (hoặc nhập ≥6 ký tự, hoa, thường, số, ký tự ĐB)."
                : "💡 Mật khẩu cần: ≥6 ký tự, chữ hoa, chữ thường, số và ký tự đặc biệt.";
            UIHelper.UpdatePasswordStrengthLabel(lblGhiChuVaiTro, txtMatKhau.Text, emptyHint);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isEditMode)
                {
                    // Thêm mới - Mặc định là quyền Quản trị viên (Admin)
                    TaiKhoanDTO tkNew = new TaiKhoanDTO
                    {
                        TenDangNhap = txtTenDangNhap.Text.Trim(),
                        HoTen = txtHoTen.Text.Trim(),
                        VaiTro = "Admin",
                        TrangThai = chkTrangThai.Checked
                    };

                    taiKhoanBLL.InsertTaiKhoan(tkNew, txtMatKhau.Text);
                    MessageBox.Show("Thêm mới tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Kiểm tra bộ lọc độ mạnh mật khẩu trước nếu người dùng có nhập mật khẩu mới
                    string newPassword = txtMatKhau.Text;
                    bool hasNewPassword = !string.IsNullOrEmpty(newPassword);
                    if (hasNewPassword)
                    {
                        TaiKhoanBLL.ValidatePasswordStrength(newPassword, "Mật khẩu mới");
                    }

                    // Cập nhật thông tin
                    tkCurrent.HoTen = txtHoTen.Text.Trim();
                    if (string.IsNullOrEmpty(tkCurrent.VaiTro))
                    {
                        tkCurrent.VaiTro = "Admin";
                    }
                    tkCurrent.TrangThai = chkTrangThai.Checked;

                    taiKhoanBLL.UpdateTaiKhoan(tkCurrent);

                    if (hasNewPassword)
                    {
                        taiKhoanBLL.AdminResetPassword(tkCurrent.MaTaiKhoan, newPassword);
                        MessageBox.Show("Cập nhật thông tin và đổi mật khẩu tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thông tin tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi kiểm tra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

