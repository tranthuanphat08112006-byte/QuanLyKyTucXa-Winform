using System;
using System.Collections.Generic;
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
        private readonly TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
        private readonly VaiTroBLL vaiTroBLL = new VaiTroBLL();
        private bool isFilterLoading = false;
        private bool isPermissionGridUpdating = false;
        private int currentTabIndex = 0; // 0 = Tài khoản, 1 = Vai trò
        private string selectedMaVaiTro = null;

        public UCTaiKhoan()
        {
            InitializeComponent();
        }

        private void UCTaiKhoan_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // Kiểm tra quyền xem module Quản lý Tài khoản & Vai trò
            if (!UserSession.CanView(ModuleSystem.TaiKhoan))
            {
                MessageBox.Show("Bạn không có quyền truy cập module Quản lý tài khoản & phân quyền!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = false;
                return;
            }

            // Định dạng lưới dữ liệu chuẩn Modern Flat Design
            UIHelper.FormatModernDataGridView(dgvTaiKhoan);
            UIHelper.FormatModernDataGridView(dgvVaiTro);
            InitPhanQuyenGrid();

            isFilterLoading = true;
            if (cmbLocTrangThai.Items.Count > 0) cmbLocTrangThai.SelectedIndex = 0;
            isFilterLoading = false;

            ApplyPermissions();
            SwitchTab(0);
            RefreshAllData();
        }

        private void ApplyPermissions()
        {
            bool canAdd = UserSession.CanAdd(ModuleSystem.TaiKhoan);
            bool canEdit = UserSession.CanEdit(ModuleSystem.TaiKhoan);
            bool canDelete = UserSession.CanDelete(ModuleSystem.TaiKhoan);

            btnThemMoi.Visible = canAdd;
            btnThemVaiTro.Visible = canAdd;

            btnSua.Visible = canEdit;
            btnKhoa.Visible = canEdit;
            btnResetPass.Visible = canEdit;
            btnSuaVaiTro.Visible = canEdit;
            btnLuuPhanQuyen.Visible = canEdit;

            btnXoa.Visible = canDelete;
            btnXoaVaiTro.Visible = canDelete;
        }

        private void InitPhanQuyenGrid()
        {
            UIHelper.FormatModernDataGridView(dgvPhanQuyen);
            dgvPhanQuyen.AutoGenerateColumns = false;
            dgvPhanQuyen.Columns.Clear();

            var colMa = new DataGridViewTextBoxColumn
            {
                Name = "MaModule",
                DataPropertyName = "MaModule",
                HeaderText = "Mã module",
                Visible = false
            };

            var colTen = new DataGridViewTextBoxColumn
            {
                Name = "TenModule",
                DataPropertyName = "TenModule",
                HeaderText = "Module chức năng",
                ReadOnly = true,
                FillWeight = 160
            };

            var colXem = new DataGridViewCheckBoxColumn
            {
                Name = "CoQuyenXem",
                DataPropertyName = "CoQuyenXem",
                HeaderText = "Xem",
                FillWeight = 45
            };

            var colThem = new DataGridViewCheckBoxColumn
            {
                Name = "CoQuyenThem",
                DataPropertyName = "CoQuyenThem",
                HeaderText = "Thêm",
                FillWeight = 45
            };

            var colSua = new DataGridViewCheckBoxColumn
            {
                Name = "CoQuyenSua",
                DataPropertyName = "CoQuyenSua",
                HeaderText = "Sửa",
                FillWeight = 45
            };

            var colXoa = new DataGridViewCheckBoxColumn
            {
                Name = "CoQuyenXoa",
                DataPropertyName = "CoQuyenXoa",
                HeaderText = "Xóa",
                FillWeight = 45
            };

            dgvPhanQuyen.Columns.AddRange(new DataGridViewColumn[] { colMa, colTen, colXem, colThem, colSua, colXoa });
            dgvPhanQuyen.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvPhanQuyen.MultiSelect = false;
            dgvPhanQuyen.ReadOnly = false;
            dgvPhanQuyen.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        private void SwitchTab(int tabIndex)
        {
            currentTabIndex = tabIndex;

            Color activeColor = Color.FromArgb(37, 99, 235);
            Color inactiveColor = Color.FromArgb(100, 116, 139);
            Color activeBg = Color.FromArgb(239, 246, 255);

            btnTabTaiKhoan.ForeColor = (tabIndex == 0) ? activeColor : inactiveColor;
            btnTabTaiKhoan.BackColor = (tabIndex == 0) ? activeBg : Color.White;
            btnTabTaiKhoan.Font = new Font("Segoe UI Semibold", 10F, (tabIndex == 0) ? FontStyle.Bold : FontStyle.Regular);

            btnTabVaiTro.ForeColor = (tabIndex == 1) ? activeColor : inactiveColor;
            btnTabVaiTro.BackColor = (tabIndex == 1) ? activeBg : Color.White;
            btnTabVaiTro.Font = new Font("Segoe UI Semibold", 10F, (tabIndex == 1) ? FontStyle.Bold : FontStyle.Regular);

            pnlTabTaiKhoan.Visible = (tabIndex == 0);
            pnlTabVaiTro.Visible = (tabIndex == 1);

            if (tabIndex == 1 && dgvVaiTro.Rows.Count == 0)
            {
                LoadDanhSachVaiTro();
            }
        }

        private void btnTabTaiKhoan_Click(object sender, EventArgs e)
        {
            SwitchTab(0);
        }

        private void btnTabVaiTro_Click(object sender, EventArgs e)
        {
            SwitchTab(1);
            LoadDanhSachVaiTro(selectedMaVaiTro);
        }

        public void RefreshAllData()
        {
            LoadFilterVaiTroOptions();
            LoadDanhSachTaiKhoan();
            LoadDanhSachVaiTro(selectedMaVaiTro);
            UpdateSummaryCards();
        }

        private void LoadFilterVaiTroOptions()
        {
            try
            {
                isFilterLoading = true;
                string previous = cmbLocVaiTro.Text;
                cmbLocVaiTro.Items.Clear();
                cmbLocVaiTro.Items.Add("Tất cả vai trò");

                DataTable dtRoles = vaiTroBLL.GetAllVaiTro();
                if (dtRoles != null)
                {
                    foreach (DataRow row in dtRoles.Rows)
                    {
                        string tenVT = row["TenVaiTro"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(tenVT) && !cmbLocVaiTro.Items.Contains(tenVT))
                        {
                            cmbLocVaiTro.Items.Add(tenVT);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(previous) && cmbLocVaiTro.Items.Contains(previous))
                    cmbLocVaiTro.Text = previous;
                else
                    cmbLocVaiTro.SelectedIndex = 0;
            }
            catch
            {
                if (cmbLocVaiTro.Items.Count == 0)
                {
                    cmbLocVaiTro.Items.Add("Tất cả vai trò");
                    cmbLocVaiTro.SelectedIndex = 0;
                }
            }
            finally
            {
                isFilterLoading = false;
            }
        }

        private void UpdateSummaryCards()
        {
            try
            {
                DataTable dtAll = taiKhoanBLL.GetAllTaiKhoan();
                DataTable dtRoles = vaiTroBLL.GetAllVaiTro();

                int tongTK = 0;
                int tongHoatDong = 0;

                if (dtAll != null)
                {
                    tongTK = dtAll.Rows.Count;
                    foreach (DataRow row in dtAll.Rows)
                    {
                        bool trangThai = row["TrangThai"] != DBNull.Value && Convert.ToBoolean(row["TrangThai"]);
                        if (trangThai)
                            tongHoatDong++;
                    }
                }

                int tongKhoa = Math.Max(0, tongTK - tongHoatDong);
                int tongVaiTro = dtRoles != null ? dtRoles.Rows.Count : 0;

                lblTongTaiKhoan.Text = tongTK.ToString();
                lblTongHoatDong.Text = tongHoatDong.ToString();
                lblTongKhoa.Text = tongKhoa.ToString();
                lblTongVaiTro.Text = tongVaiTro.ToString();
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

        #region Tab 1: Quản lý Tài khoản

        private void LoadDanhSachTaiKhoan()
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                string vaiTro = cmbLocVaiTro.Text;
                string trangThai = cmbLocTrangThai.Text;

                DataTable dt = taiKhoanBLL.FilterTaiKhoan(keyword, vaiTro, trangThai);
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
            isFilterLoading = true;
            txtTimKiem.Clear();
            if (cmbLocVaiTro.Items.Count > 0) cmbLocVaiTro.SelectedIndex = 0;
            if (cmbLocTrangThai.Items.Count > 0) cmbLocTrangThai.SelectedIndex = 0;
            isFilterLoading = false;
            RefreshAllData();
        }

        #endregion

        #region Tab 2: Quản lý Vai trò & Ma trận Phân quyền chi tiết

        private void LoadDanhSachVaiTro(string selectMaVaiTro = null)
        {
            try
            {
                string keyword = txtTimVaiTro.Text.Trim();
                DataTable dt = vaiTroBLL.FilterVaiTro(keyword);
                dgvVaiTro.DataSource = dt;

                if (dgvVaiTro.Columns.Count > 0)
                {
                    if (dgvVaiTro.Columns.Contains("MaVaiTro"))
                    {
                        dgvVaiTro.Columns["MaVaiTro"].HeaderText = "Mã vai trò";
                        dgvVaiTro.Columns["MaVaiTro"].FillWeight = 75;
                    }
                    if (dgvVaiTro.Columns.Contains("TenVaiTro"))
                    {
                        dgvVaiTro.Columns["TenVaiTro"].HeaderText = "Tên vai trò";
                        dgvVaiTro.Columns["TenVaiTro"].FillWeight = 105;
                    }
                    if (dgvVaiTro.Columns.Contains("MoTa"))
                    {
                        dgvVaiTro.Columns["MoTa"].HeaderText = "Mô tả";
                        dgvVaiTro.Columns["MoTa"].FillWeight = 130;
                    }
                    if (dgvVaiTro.Columns.Contains("SoTaiKhoan"))
                    {
                        dgvVaiTro.Columns["SoTaiKhoan"].HeaderText = "Số TK";
                        dgvVaiTro.Columns["SoTaiKhoan"].FillWeight = 45;
                    }
                }

                // Khôi phục dòng đang chọn hoặc chọn dòng đầu tiên
                if (dgvVaiTro.Rows.Count > 0)
                {
                    int targetIndex = 0;
                    if (!string.IsNullOrWhiteSpace(selectMaVaiTro))
                    {
                        for (int i = 0; i < dgvVaiTro.Rows.Count; i++)
                        {
                            string ma = dgvVaiTro.Rows[i].Cells["MaVaiTro"].Value?.ToString();
                            if (string.Equals(ma, selectMaVaiTro, StringComparison.OrdinalIgnoreCase))
                            {
                                targetIndex = i;
                                break;
                            }
                        }
                    }

                    dgvVaiTro.ClearSelection();
                    dgvVaiTro.Rows[targetIndex].Selected = true;
                    dgvVaiTro.CurrentCell = dgvVaiTro.Rows[targetIndex].Cells[0];
                    LoadPhanQuyenForSelectedRole();
                }
                else
                {
                    selectedMaVaiTro = null;
                    dgvPhanQuyen.DataSource = null;
                    lblPhanQuyenTitle.Text = "Phân quyền chi tiết";
                    lblPhanQuyenNote.Text = "Không có vai trò nào được chọn";
                    btnLuuPhanQuyen.Enabled = false;
                }
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách vai trò: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private VaiTroDTO GetSelectedVaiTroFromGrid()
        {
            if (dgvVaiTro.SelectedRows.Count == 0 && dgvVaiTro.CurrentRow == null) return null;
            DataGridViewRow row = dgvVaiTro.SelectedRows.Count > 0 ? dgvVaiTro.SelectedRows[0] : dgvVaiTro.CurrentRow;

            if (row == null || row.Cells["MaVaiTro"].Value == null || row.Cells["MaVaiTro"].Value == DBNull.Value)
                return null;

            string ma = row.Cells["MaVaiTro"].Value.ToString();
            string ten = row.Cells["TenVaiTro"].Value?.ToString() ?? "";
            string moTa = row.Cells["MoTa"].Value?.ToString() ?? "";
            int soTK = row.Cells["SoTaiKhoan"].Value != null && row.Cells["SoTaiKhoan"].Value != DBNull.Value
                ? Convert.ToInt32(row.Cells["SoTaiKhoan"].Value)
                : 0;

            return new VaiTroDTO(ma, ten, moTa, soTK);
        }

        private void dgvVaiTro_SelectionChanged(object sender, EventArgs e)
        {
            LoadPhanQuyenForSelectedRole();
        }

        private void LoadPhanQuyenForSelectedRole()
        {
            VaiTroDTO selected = GetSelectedVaiTroFromGrid();
            if (selected == null)
                return;

            selectedMaVaiTro = selected.MaVaiTro;
            bool isAdminRole = string.Equals(selected.MaVaiTro, "Admin", StringComparison.OrdinalIgnoreCase);
            bool canEdit = UserSession.CanEdit(ModuleSystem.TaiKhoan);

            try
            {
                isPermissionGridUpdating = true;
                List<PhanQuyenDTO> permissions = vaiTroBLL.GetPhanQuyenByVaiTro(selected.MaVaiTro);
                dgvPhanQuyen.DataSource = new BindingList<PhanQuyenDTO>(permissions);

                lblPhanQuyenTitle.Text = $"Phân quyền: {selected.TenVaiTro} ({selected.MaVaiTro})";

                if (isAdminRole)
                {
                    lblPhanQuyenNote.Text = "Vai trò Quản trị viên mặc định có toàn quyền hệ thống (chỉ xem).";
                    dgvPhanQuyen.ReadOnly = true;
                    btnLuuPhanQuyen.Enabled = false;
                }
                else if (!canEdit)
                {
                    lblPhanQuyenNote.Text = "Bạn chỉ có quyền xem ma trận phân quyền.";
                    dgvPhanQuyen.ReadOnly = true;
                    btnLuuPhanQuyen.Enabled = false;
                }
                else
                {
                    lblPhanQuyenNote.Text = "Tick Thêm/Sửa/Xóa sẽ tự bật Xem; bỏ Xem sẽ tự tắt các quyền thao tác.";
                    dgvPhanQuyen.ReadOnly = false;
                    if (dgvPhanQuyen.Columns.Contains("TenModule"))
                        dgvPhanQuyen.Columns["TenModule"].ReadOnly = true;
                    btnLuuPhanQuyen.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp phân quyền vai trò: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isPermissionGridUpdating = false;
            }
        }

        private void dgvPhanQuyen_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPhanQuyen.IsCurrentCellDirty && dgvPhanQuyen.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvPhanQuyen.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvPhanQuyen_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isPermissionGridUpdating || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string colName = dgvPhanQuyen.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = dgvPhanQuyen.Rows[e.RowIndex];

            try
            {
                isPermissionGridUpdating = true;

                if (colName == "CoQuyenThem" || colName == "CoQuyenSua" || colName == "CoQuyenXoa")
                {
                    bool checkedVal = row.Cells[colName].Value != null && Convert.ToBoolean(row.Cells[colName].Value);
                    if (checkedVal)
                    {
                        row.Cells["CoQuyenXem"].Value = true;
                    }
                }
                else if (colName == "CoQuyenXem")
                {
                    bool xemVal = row.Cells["CoQuyenXem"].Value != null && Convert.ToBoolean(row.Cells["CoQuyenXem"].Value);
                    if (!xemVal)
                    {
                        row.Cells["CoQuyenThem"].Value = false;
                        row.Cells["CoQuyenSua"].Value = false;
                        row.Cells["CoQuyenXoa"].Value = false;
                    }
                }
            }
            finally
            {
                isPermissionGridUpdating = false;
            }
        }

        private void btnThemVaiTro_Click(object sender, EventArgs e)
        {
            using (frmChiTietVaiTro frm = new frmChiTietVaiTro())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    selectedMaVaiTro = frm.SavedMaVaiTro;
                    SwitchTab(1);
                    RefreshAllData();
                }
            }
        }

        private void btnSuaVaiTro_Click(object sender, EventArgs e)
        {
            VaiTroDTO selected = GetSelectedVaiTroFromGrid();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một vai trò để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (frmChiTietVaiTro frm = new frmChiTietVaiTro(selected))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    selectedMaVaiTro = frm.SavedMaVaiTro;
                    RefreshAllData();
                }
            }
        }

        private void btnXoaVaiTro_Click(object sender, EventArgs e)
        {
            VaiTroDTO selected = GetSelectedVaiTroFromGrid();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một vai trò để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa vai trò '{selected.TenVaiTro}' ({selected.MaVaiTro}) khỏi hệ thống?",
                "Xác nhận xóa vai trò",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    vaiTroBLL.DeleteVaiTro(selected.MaVaiTro);
                    MessageBox.Show("Xóa vai trò thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    selectedMaVaiTro = null;
                    RefreshAllData();
                }
                catch (UnauthorizedAccessException uex)
                {
                    MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Không thể xóa vai trò", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuuPhanQuyen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedMaVaiTro))
            {
                MessageBox.Show("Vui lòng chọn một vai trò để lưu phân quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                dgvPhanQuyen.EndEdit();
                var list = new List<PhanQuyenDTO>();
                foreach (DataGridViewRow row in dgvPhanQuyen.Rows)
                {
                    if (row.IsNewRow) continue;
                    string maMod = row.Cells["MaModule"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(maMod)) continue;

                    bool xem = row.Cells["CoQuyenXem"].Value != null && Convert.ToBoolean(row.Cells["CoQuyenXem"].Value);
                    bool them = row.Cells["CoQuyenThem"].Value != null && Convert.ToBoolean(row.Cells["CoQuyenThem"].Value);
                    bool sua = row.Cells["CoQuyenSua"].Value != null && Convert.ToBoolean(row.Cells["CoQuyenSua"].Value);
                    bool xoa = row.Cells["CoQuyenXoa"].Value != null && Convert.ToBoolean(row.Cells["CoQuyenXoa"].Value);

                    list.Add(new PhanQuyenDTO(selectedMaVaiTro, maMod, xem, them, sua, xoa));
                }

                vaiTroBLL.SavePhanQuyen(selectedMaVaiTro, list);
                MessageBox.Show($"Đã lưu phân quyền chi tiết cho vai trò '{selectedMaVaiTro}' thành công!\nQuyền mới sẽ áp dụng khi tài khoản thuộc vai trò này đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPhanQuyenForSelectedRole();
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi lưu phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimVaiTro_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachVaiTro(selectedMaVaiTro);
        }

        private void btnLamMoiVaiTro_Click(object sender, EventArgs e)
        {
            txtTimVaiTro.Clear();
            LoadDanhSachVaiTro(selectedMaVaiTro);
        }

        #endregion
    }
}
