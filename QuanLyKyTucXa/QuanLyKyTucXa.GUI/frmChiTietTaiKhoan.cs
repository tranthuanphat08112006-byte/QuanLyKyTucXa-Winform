using System;
using System.Data;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmChiTietTaiKhoan : Form
    {
        private readonly TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
        private readonly VaiTroBLL vaiTroBLL = new VaiTroBLL();
        private TaiKhoanDTO tkCurrent = null;
        private bool isEditMode = false;

        // Constructor Thêm mới
        public frmChiTietTaiKhoan()
        {
            InitializeComponent();
            isEditMode = false;
            this.Text = "Thêm tài khoản mới";
            lblFormTitle.Text = "THÊM TÀI KHOẢN MỚI";
            lblFormSub.Text = "Khởi tạo tài khoản người dùng, gán vai trò và thiết lập mật khẩu ban đầu";
            txtMatKhau.TextChanged += TxtMatKhau_TextChanged;
            LoadDanhSachVaiTro("NhanVien");
            UpdatePasswordHint();
        }

        // Constructor Chỉnh sửa
        public frmChiTietTaiKhoan(TaiKhoanDTO tk) : this()
        {
            this.tkCurrent = tk;
            this.isEditMode = true;

            this.Text = "Chỉnh sửa tài khoản - " + tk.TenDangNhap;
            lblFormTitle.Text = "CHỈNH SỬA TÀI KHOẢN: " + tk.TenDangNhap;
            lblFormSub.Text = "Cập nhật họ tên, vai trò, mật khẩu mới và trạng thái hoạt động";

            txtTenDangNhap.Text = tk.TenDangNhap;
            txtTenDangNhap.ReadOnly = true;

            // Cho phép nhập mật khẩu mới nếu cần đổi (đề phòng quên mật khẩu)
            lblMatKhau.Text = "Mật khẩu mới (nếu đổi):";
            txtMatKhau.Text = "";
            txtMatKhau.ReadOnly = false;
            txtMatKhau.Enabled = true;

            txtHoTen.Text = tk.HoTen;
            chkTrangThai.Checked = tk.TrangThai;

            LoadDanhSachVaiTro(tk.VaiTro);
            UpdatePasswordHint();
        }

        private void LoadDanhSachVaiTro(string selectedMaVaiTro)
        {
            try
            {
                DataTable dt = vaiTroBLL.GetAllVaiTro();
                cmbVaiTro.DisplayMember = "TenVaiTro";
                cmbVaiTro.ValueMember = "MaVaiTro";
                cmbVaiTro.DataSource = dt;

                if (!string.IsNullOrWhiteSpace(selectedMaVaiTro) && dt != null)
                {
                    cmbVaiTro.SelectedValue = selectedMaVaiTro;
                    if (cmbVaiTro.SelectedIndex < 0 && cmbVaiTro.Items.Count > 0)
                        cmbVaiTro.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể nạp danh sách vai trò: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                string selectedRole = cmbVaiTro.SelectedValue?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(selectedRole))
                {
                    MessageBox.Show("Vui lòng chọn vai trò cho tài khoản!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!isEditMode)
                {
                    TaiKhoanDTO tkNew = new TaiKhoanDTO
                    {
                        TenDangNhap = txtTenDangNhap.Text.Trim(),
                        HoTen = txtHoTen.Text.Trim(),
                        VaiTro = selectedRole,
                        TrangThai = chkTrangThai.Checked
                    };

                    taiKhoanBLL.InsertTaiKhoan(tkNew, txtMatKhau.Text);
                    MessageBox.Show("Thêm mới tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string newPassword = txtMatKhau.Text;
                    bool hasNewPassword = !string.IsNullOrEmpty(newPassword);
                    if (hasNewPassword)
                    {
                        TaiKhoanBLL.ValidatePasswordStrength(newPassword, "Mật khẩu mới");
                    }

                    tkCurrent.HoTen = txtHoTen.Text.Trim();
                    tkCurrent.VaiTro = selectedRole;
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
