using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class UCTaiKhoan : UserControl
    {
        private TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
        private bool isFilterLoading = false;

        public UCTaiKhoan()
        {
            InitializeComponent();
        }

        private void UCTaiKhoan_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // Kiểm tra quyền Admin ở tầng giao diện
            if (!UserSession.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập module Quản lý tài khoản!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = false;
                return;
            }

            // Định dạng lưới dữ liệu chuẩn Modern Flat Design
            UIHelper.FormatModernDataGridView(dgvTaiKhoan);

            isFilterLoading = true;
            if (cmbLocTrangThai.Items.Count > 0) cmbLocTrangThai.SelectedIndex = 0;
            isFilterLoading = false;

            RefreshAllData();
        }

        public void RefreshAllData()
        {
            LoadDanhSachTaiKhoan();
            UpdateSummaryCards();
        }

        private void UpdateSummaryCards()
        {
            try
            {
                DataTable dtAll = taiKhoanBLL.GetAllTaiKhoan();
                int tongTK = 0;
                int tongAdmin = 0;
                int tongHoatDong = 0;

                if (dtAll != null)
                {
                    tongTK = dtAll.Rows.Count;
                    foreach (DataRow row in dtAll.Rows)
                    {
                        string vaiTro = row["VaiTro"]?.ToString() ?? "";
                        bool trangThai = row["TrangThai"] != DBNull.Value && Convert.ToBoolean(row["TrangThai"]);

                        if (string.Equals(vaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
                            tongAdmin++;

                        if (trangThai)
                            tongHoatDong++;
                    }
                }

                int tongKhoa = Math.Max(0, tongTK - tongHoatDong);

                lblTongTaiKhoan.Text = tongTK.ToString();
                lblTongHoatDong.Text = tongHoatDong.ToString();
                lblTongKhoa.Text = tongKhoa.ToString();
                lblTongAdmin.Text = tongAdmin.ToString();
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachTaiKhoan()
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                string trangThai = cmbLocTrangThai.Text;

                DataTable dt = taiKhoanBLL.FilterTaiKhoan(keyword, "Tất cả vai trò", trangThai);
                dgvTaiKhoan.DataSource = dt;

                if (dgvTaiKhoan.Columns.Count > 0)
                {
                    if (dgvTaiKhoan.Columns.Contains("MaTaiKhoan"))
                    {
                        dgvTaiKhoan.Columns["MaTaiKhoan"].HeaderText = "Mã TK";
                        dgvTaiKhoan.Columns["MaTaiKhoan"].FillWeight = 50;
                    }
                    if (dgvTaiKhoan.Columns.Contains("TenDangNhap"))
                    {
                        dgvTaiKhoan.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                        dgvTaiKhoan.Columns["TenDangNhap"].FillWeight = 90;
                    }
                    if (dgvTaiKhoan.Columns.Contains("HoTen"))
                    {
                        dgvTaiKhoan.Columns["HoTen"].HeaderText = "Họ và Tên";
                        dgvTaiKhoan.Columns["HoTen"].FillWeight = 120;
                    }
                    if (dgvTaiKhoan.Columns.Contains("VaiTroHienThi"))
                    {
                        dgvTaiKhoan.Columns["VaiTroHienThi"].HeaderText = "Vai trò";
                        dgvTaiKhoan.Columns["VaiTroHienThi"].FillWeight = 85;
                    }
                    if (dgvTaiKhoan.Columns.Contains("TrangThaiHienThi"))
                    {
                        dgvTaiKhoan.Columns["TrangThaiHienThi"].HeaderText = "Trạng thái";
                        dgvTaiKhoan.Columns["TrangThaiHienThi"].FillWeight = 85;
                    }
                    if (dgvTaiKhoan.Columns.Contains("NgayTao"))
                    {
                        dgvTaiKhoan.Columns["NgayTao"].HeaderText = "Ngày tạo";
                        dgvTaiKhoan.Columns["NgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                        dgvTaiKhoan.Columns["NgayTao"].FillWeight = 95;
                    }

                    // Ẩn cột kỹ thuật
                    if (dgvTaiKhoan.Columns.Contains("VaiTro"))
                        dgvTaiKhoan.Columns["VaiTro"].Visible = false;
                    if (dgvTaiKhoan.Columns.Contains("TrangThai"))
                        dgvTaiKhoan.Columns["TrangThai"].Visible = false;
                }
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TaiKhoanDTO GetSelectedTaiKhoanFromGrid()
        {
            if (dgvTaiKhoan.SelectedRows.Count == 0 && dgvTaiKhoan.CurrentRow == null) return null;
            DataGridViewRow row = dgvTaiKhoan.SelectedRows.Count > 0 ? dgvTaiKhoan.SelectedRows[0] : dgvTaiKhoan.CurrentRow;

            if (row.Cells["MaTaiKhoan"].Value == null || row.Cells["MaTaiKhoan"].Value == DBNull.Value) return null;

            int maTK = Convert.ToInt32(row.Cells["MaTaiKhoan"].Value);
            string tenDN = row.Cells["TenDangNhap"].Value?.ToString() ?? "";
            string hoTen = row.Cells["HoTen"].Value?.ToString() ?? "";
            string vaiTro = row.Cells["VaiTro"].Value?.ToString() ?? "NhanVien";
            bool trangThai = row.Cells["TrangThai"].Value != null && Convert.ToBoolean(row.Cells["TrangThai"].Value);
            DateTime ngayTao = row.Cells["NgayTao"].Value != null && row.Cells["NgayTao"].Value != DBNull.Value
                ? Convert.ToDateTime(row.Cells["NgayTao"].Value)
                : DateTime.Now;

            return new TaiKhoanDTO(maTK, tenDN, null, hoTen, vaiTro, trangThai, ngayTao);
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            using (frmChiTietTaiKhoan frm = new frmChiTietTaiKhoan())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO selected = GetSelectedTaiKhoanFromGrid();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản trên danh sách để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (frmChiTietTaiKhoan frm = new frmChiTietTaiKhoan(selected))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
        }

        private void btnKhoa_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO selected = GetSelectedTaiKhoanFromGrid();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để Khóa / Mở khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string actionText = selected.TrangThai ? "khóa" : "mở khóa";
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn {actionText} tài khoản '{selected.TenDangNhap}' ({selected.HoTen})?",
                "Xác nhận thay đổi trạng thái",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    taiKhoanBLL.ToggleTrangThai(selected.MaTaiKhoan);
                    MessageBox.Show($"Đã {actionText} tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshAllData();
                }
                catch (UnauthorizedAccessException uex)
                {
                    MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO selected = GetSelectedTaiKhoanFromGrid();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để đặt lại mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string defaultResetPass = "Ktx@123";
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn đặt lại mật khẩu cho tài khoản '{selected.TenDangNhap}'?\n\nMật khẩu mới sẽ là: {defaultResetPass}",
                "Xác nhận đặt lại mật khẩu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    taiKhoanBLL.AdminResetPassword(selected.MaTaiKhoan, defaultResetPass);
                    MessageBox.Show($"Đặt lại mật khẩu thành công! Mật khẩu mới là '{defaultResetPass}'. Vui lòng thông báo cho người dùng đổi lại mật khẩu khi đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (UnauthorizedAccessException uex)
                {
                    MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO selected = GetSelectedTaiKhoanFromGrid();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"CẢNH BÁO: Bạn có chắc chắn muốn XÓA VĨNH VIỄN tài khoản '{selected.TenDangNhap}' ({selected.HoTen}) khỏi hệ thống?",
                "Xác nhận xóa tài khoản",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    taiKhoanBLL.DeleteTaiKhoan(selected.MaTaiKhoan);
                    MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshAllData();
                }
                catch (UnauthorizedAccessException uex)
                {
                    MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi xóa tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachTaiKhoan();
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            if (!isFilterLoading)
            {
                LoadDanhSachTaiKhoan();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            if (cmbLocTrangThai.Items.Count > 0) cmbLocTrangThai.SelectedIndex = 0;
            RefreshAllData();
        }
    }
}

