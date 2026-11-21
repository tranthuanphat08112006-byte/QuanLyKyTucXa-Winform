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
    public partial class UCPhong : UserControl
    {
        private PhongBLL phongBLL = new PhongBLL();
        private bool isFilterLoading = false;

        public UCPhong()
        {
            InitializeComponent();
        }

        private void UCPhong_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // 1. Định dạng bảng cho đẹp
            UIHelper.FormatModernDataGridView(dgvPhong);
            UIHelper.FormatModernDataGridView(dgvDayNha);

            isFilterLoading = true;
            if (cmbLocTrangThai.Items.Count > 0) cmbLocTrangThai.SelectedIndex = 0;
            isFilterLoading = false;

            // 2. Tải dữ liệu danh sách phòng và dãy nhà
            ApplyPermissions();
            RefreshAllData();
        }

        private void ApplyPermissions()
        {
            bool canAdd = UserSession.CanAdd(ModuleSystem.Phong);
            bool canEdit = UserSession.CanEdit(ModuleSystem.Phong);
            bool canDelete = UserSession.CanDelete(ModuleSystem.Phong);

            btnThemMoi.Visible = canAdd;
            btnThemDayNha.Visible = canAdd;
            btn_Sua.Visible = canEdit;
            btnSuaDayNha.Visible = canEdit;
            btn_Xoa.Visible = canDelete;
            btnXoaDayNha.Visible = canDelete;
        }

        public void RefreshAllData()
        {
            LoadFilterDayNhaOptions();
            LoadDanhSachPhong();
            LoadDanhSachDayNha();
            UpdateSummaryCards();
        }

        private void LoadFilterDayNhaOptions()
        {
            try
            {
                isFilterLoading = true;
                string previousSelection = cmbLocDayNha.Text;
                cmbLocDayNha.Items.Clear();
                cmbLocDayNha.Items.Add("Tất cả dãy nhà");

                DataTable dtDayNha = phongBLL.GetAllDayNha();
                if (dtDayNha != null)
                {
                    foreach (DataRow row in dtDayNha.Rows)
                    {
                        string tenKhu = row["TenKhu"].ToString();
                        if (!string.IsNullOrWhiteSpace(tenKhu) && !cmbLocDayNha.Items.Contains(tenKhu))
                        {
                            cmbLocDayNha.Items.Add(tenKhu);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(previousSelection) && cmbLocDayNha.Items.Contains(previousSelection))
                {
                    cmbLocDayNha.Text = previousSelection;
                }
                else
                {
                    cmbLocDayNha.SelectedIndex = 0;
                }
            }
            catch
            {
                if (cmbLocDayNha.Items.Count == 0)
                {
                    cmbLocDayNha.Items.Add("Tất cả dãy nhà");
                    cmbLocDayNha.SelectedIndex = 0;
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
                DataTable dtPhong = phongBLL.GetAllPhong();
                DataTable dtDayNha = phongBLL.GetAllDayNha();

                HashSet<string> dayNhaSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (dtDayNha != null)
                {
                    foreach (DataRow r in dtDayNha.Rows)
                    {
                        string tk = r["TenKhu"].ToString().Trim();
                        if (!string.IsNullOrEmpty(tk)) dayNhaSet.Add(tk);
                    }
                }

                int tongPhong = 0;
                int phongConTrong = 0;
                int phongDaDay = 0;
                int tongSucChua = 0;
                int tongGiuongTrong = 0;
                int tongDangO = 0;

                if (dtPhong != null)
                {
                    tongPhong = dtPhong.Rows.Count;
                    foreach (DataRow row in dtPhong.Rows)
                    {
                        string tk = row["TenKhu"].ToString().Trim();
                        if (!string.IsNullOrEmpty(tk)) dayNhaSet.Add(tk);

                        int sucChua = Convert.ToInt32(row["SoGiuongToiDa"]);
                        int giuongTrong = Convert.ToInt32(row["SoGiuongTrong"]);
                        int dangO = Math.Max(0, sucChua - giuongTrong);
                        string trangThai = row["TrangThai"].ToString();

                        tongSucChua += sucChua;
                        tongGiuongTrong += Math.Max(0, giuongTrong);
                        tongDangO += dangO;

                        if (trangThai == "Đã đầy" || giuongTrong <= 0)
                            phongDaDay++;
                        else if (trangThai == "Còn trống")
                            phongConTrong++;
                    }
                }

                lblTongDayNha.Text = dayNhaSet.Count.ToString();
                lblTongPhong.Text = tongPhong.ToString();
                lblPhongTrong.Text = phongConTrong.ToString();
                lblPhongDay.Text = phongDaDay.ToString();

                lblCard2Sub.Text = $"Tổng sức chứa: {tongSucChua} giường";
                lblCard3Sub.Text = $"Còn {tongGiuongTrong} giường trống sẵn sàng";
                lblCard4Sub.Text = $"Đang lưu trú: {tongDangO} sinh viên";
            }
            catch
            {
            }
        }

        private void LoadDanhSachPhong()
        {
            try
            {
                string keyword = txtTimKiem.Text;
                string tenKhu = cmbLocDayNha.Text;
                string trangThai = cmbLocTrangThai.Text;

                dgvPhong.DataSource = phongBLL.FilterPhong(keyword, tenKhu, trangThai);

                if (dgvPhong.Columns.Count > 0)
                {
                    if (dgvPhong.Columns.Contains("MaPhong"))
                        dgvPhong.Columns["MaPhong"].HeaderText = "Mã Phòng";
                    if (dgvPhong.Columns.Contains("TenPhong"))
                        dgvPhong.Columns["TenPhong"].HeaderText = "Tên / Số Phòng";
                    if (dgvPhong.Columns.Contains("TenKhu"))
                        dgvPhong.Columns["TenKhu"].HeaderText = "Dãy Nhà (Khu)";
                    if (dgvPhong.Columns.Contains("LoaiPhong"))
                        dgvPhong.Columns["LoaiPhong"].HeaderText = "Loại Phòng";
                    if (dgvPhong.Columns.Contains("SoGiuongToiDa"))
                        dgvPhong.Columns["SoGiuongToiDa"].HeaderText = "Sức Chứa";
                    if (dgvPhong.Columns.Contains("SoNguoiHienTai"))
                        dgvPhong.Columns["SoNguoiHienTai"].HeaderText = "Đang Ở";
                    if (dgvPhong.Columns.Contains("SoGiuongTrong"))
                        dgvPhong.Columns["SoGiuongTrong"].HeaderText = "Giường Trống";
                    if (dgvPhong.Columns.Contains("DonGia"))
                    {
                        dgvPhong.Columns["DonGia"].HeaderText = "Đơn Giá (VNĐ/Tháng)";
                        dgvPhong.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                    }
                    if (dgvPhong.Columns.Contains("TrangThai"))
                        dgvPhong.Columns["TrangThai"].HeaderText = "Trạng Thái";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Phong GetSelectedPhongFromGrid()
        {
            if (dgvPhong.SelectedRows.Count == 0 && dgvPhong.CurrentRow == null) return null;
            DataGridViewRow row = dgvPhong.SelectedRows.Count > 0 ? dgvPhong.SelectedRows[0] : dgvPhong.CurrentRow;

            string maPhong = row.Cells["MaPhong"].Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(maPhong)) return null;

            string tenPhong = dgvPhong.Columns.Contains("TenPhong")
                ? (row.Cells["TenPhong"].Value?.ToString() ?? ("Phòng " + maPhong))
                : ("Phòng " + maPhong);
            string tenKhu = row.Cells["TenKhu"].Value?.ToString() ?? "";
            string loaiPhong = row.Cells["LoaiPhong"].Value?.ToString() ?? "";
            int soGiuongToiDa = Convert.ToInt32(row.Cells["SoGiuongToiDa"].Value ?? 0);
            int soGiuongTrong = Convert.ToInt32(row.Cells["SoGiuongTrong"].Value ?? 0);
            decimal donGia = Convert.ToDecimal(row.Cells["DonGia"].Value ?? 0);
            string trangThai = dgvPhong.Columns.Contains("TrangThai")
                ? (row.Cells["TrangThai"].Value?.ToString() ?? "")
                : (soGiuongTrong <= 0 ? "Đã đầy" : "Còn trống");

            return new Phong(maPhong, tenPhong, tenKhu, loaiPhong, soGiuongToiDa, soGiuongTrong, donGia, trangThai);
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            frmChiTietPhong frm = new frmChiTietPhong();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                RefreshAllData();
            }
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            Phong pSelected = GetSelectedPhongFromGrid();
            if (pSelected != null)
            {
                frmChiTietPhong frm = new frmChiTietPhong(pSelected, true);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng trên bảng để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            Phong pSelected = GetSelectedPhongFromGrid();
            if (pSelected != null)
            {
                frmChiTietPhong frm = new frmChiTietPhong(pSelected, false);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng trên bảng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            Phong pSelected = GetSelectedPhongFromGrid();
            if (pSelected != null)
            {
                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa phòng {pSelected.MaPhong} ({pSelected.TenKhu}) không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        if (phongBLL.DeletePhong(pSelected.MaPhong))
                        {
                            MessageBox.Show("Xóa phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshAllData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng trên bảng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (isFilterLoading) return;
            LoadDanhSachPhong();
        }

        private void FilterPhong_Changed(object sender, EventArgs e)
        {
            if (isFilterLoading) return;
            LoadDanhSachPhong();
        }

        private void btnLamMoiPhong_Click(object sender, EventArgs e)
        {
            isFilterLoading = true;
            txtTimKiem.Clear();
            if (cmbLocDayNha.Items.Count > 0) cmbLocDayNha.SelectedIndex = 0;
            if (cmbLocTrangThai.Items.Count > 0) cmbLocTrangThai.SelectedIndex = 0;
            isFilterLoading = false;
            RefreshAllData();
        }

        private void dgvPhong_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvPhong.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string status = e.Value.ToString();
                e.CellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                if (status == "Còn trống")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74);
                }
                else if (status == "Đã đầy")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                }
                else if (status == "Bảo trì")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);
                }
            }
        }

        private void dgvPhong_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnXemChiTiet_Click(sender, e);
            }
        }

        // =====================================================
        // QUẢN LÝ DÃY NHÀ (KHU)
        // =====================================================

        private void LoadDanhSachDayNha()
        {
            try
            {
                string keyword = txtTimDayNha.Text;
                dgvDayNha.DataSource = phongBLL.GetAllDayNha(keyword);

                if (dgvDayNha.Columns.Count > 0)
                {
                    if (dgvDayNha.Columns.Contains("MaKhu"))
                        dgvDayNha.Columns["MaKhu"].HeaderText = "Mã Dãy Nhà";
                    if (dgvDayNha.Columns.Contains("TenKhu"))
                        dgvDayNha.Columns["TenKhu"].HeaderText = "Tên Dãy Nhà";
                    if (dgvDayNha.Columns.Contains("LoaiKhu"))
                        dgvDayNha.Columns["LoaiKhu"].HeaderText = "Đối Tượng";
                    if (dgvDayNha.Columns.Contains("SoPhong"))
                        dgvDayNha.Columns["SoPhong"].HeaderText = "Số Phòng";
                    if (dgvDayNha.Columns.Contains("TongSucChua"))
                        dgvDayNha.Columns["TongSucChua"].HeaderText = "Tổng Sức Chứa";
                    if (dgvDayNha.Columns.Contains("DangO"))
                        dgvDayNha.Columns["DangO"].HeaderText = "Đang Lưu Trú";
                    if (dgvDayNha.Columns.Contains("MoTa"))
                        dgvDayNha.Columns["MoTa"].HeaderText = "Mô Tả";
                    if (dgvDayNha.Columns.Contains("TrangThai"))
                        dgvDayNha.Columns["TrangThai"].HeaderText = "Trạng Thái";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách dãy nhà: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DayNha GetSelectedDayNhaFromGrid()
        {
            if (dgvDayNha.SelectedRows.Count == 0 && dgvDayNha.CurrentRow == null) return null;
            DataGridViewRow row = dgvDayNha.SelectedRows.Count > 0 ? dgvDayNha.SelectedRows[0] : dgvDayNha.CurrentRow;

            string maKhu = row.Cells["MaKhu"].Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(maKhu)) return null;

            return new DayNha(
                maKhu,
                row.Cells["TenKhu"].Value?.ToString() ?? "",
                row.Cells["LoaiKhu"].Value?.ToString() ?? "Nam",
                row.Cells["MoTa"].Value?.ToString() ?? "",
                row.Cells["TrangThai"].Value?.ToString() ?? "Hoạt động"
            );
        }

        private void btnThemDayNha_Click(object sender, EventArgs e)
        {
            frmChiTietDayNha frm = new frmChiTietDayNha();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                RefreshAllData();
                tabMain.SelectedTab = tabDayNha;
            }
        }

        private void btnSuaDayNha_Click(object sender, EventArgs e)
        {
            DayNha dSelected = GetSelectedDayNhaFromGrid();
            if (dSelected != null)
            {
                frmChiTietDayNha frm = new frmChiTietDayNha(dSelected);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dãy nhà trên bảng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoaDayNha_Click(object sender, EventArgs e)
        {
            DayNha dSelected = GetSelectedDayNhaFromGrid();
            if (dSelected != null)
            {
                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa dãy nhà '{dSelected.TenKhu}' ({dSelected.MaKhu}) không?",
                    "Xác nhận xóa dãy nhà",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        if (phongBLL.DeleteDayNha(dSelected.MaKhu, dSelected.TenKhu))
                        {
                            MessageBox.Show("Xóa dãy nhà thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshAllData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Không thể xóa dãy nhà", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dãy nhà trên bảng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtTimDayNha_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachDayNha();
        }

        private void btnLamMoiDayNha_Click(object sender, EventArgs e)
        {
            txtTimDayNha.Clear();
            RefreshAllData();
        }

        private void dgvDayNha_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && UserSession.CanEdit(ModuleSystem.Phong))
            {
                btnSuaDayNha_Click(sender, e);
            }
        }
    }
}
