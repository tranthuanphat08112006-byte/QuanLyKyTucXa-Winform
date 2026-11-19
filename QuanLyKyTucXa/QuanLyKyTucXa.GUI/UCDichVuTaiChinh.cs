using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;

namespace QuanLyKyTucXa.GUI
{
    public partial class UCDichVuTaiChinh : UserControl
    {
        private readonly HoaDonBLL hoaDonBLL = new HoaDonBLL();
        private readonly DienNuocBLL dienNuocBLL = new DienNuocBLL();
        private readonly ThanhToanBLL thanhToanBLL = new ThanhToanBLL();
        private readonly PhongBLL phongBLL = new PhongBLL();

        private int currentTabIndex = 0; // 0 = Hóa đơn & Công nợ, 1 = Chỉ số Điện Nước, 2 = Lịch sử thu tiền
        private bool isFilterLoading = false;

        public UCDichVuTaiChinh()
        {
            InitializeComponent();
        }

        private void UCDichVuTaiChinh_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            UIHelper.FormatModernDataGridView(dgvMain);

            isFilterLoading = true;
            InitFilterControls();
            isFilterLoading = false;

            SwitchTab(0);
        }

        public void RefreshAllData()
        {
            UpdateSummaryCards();
            LoadGridData();
            UpdateActionButtonsState();
        }

        private void InitFilterControls()
        {
            // 1. Nạp danh sách phòng lọc
            try
            {
                DataTable dtPhong = phongBLL.GetAllPhong();
                cmbLocPhong.Items.Clear();
                cmbLocPhong.Items.Add("Tất cả phòng");
                if (dtPhong != null)
                {
                    foreach (DataRow row in dtPhong.Rows)
                    {
                        string maPhong = row["MaPhong"].ToString();
                        string tenPhong = row["TenPhong"]?.ToString() ?? maPhong;
                        cmbLocPhong.Items.Add(maPhong);
                    }
                }
                cmbLocPhong.SelectedIndex = 0;
            }
            catch
            {
                cmbLocPhong.Items.Add("Tất cả phòng");
                cmbLocPhong.SelectedIndex = 0;
            }

            // 2. Nạp tháng
            cmbLocThang.Items.Clear();
            cmbLocThang.Items.Add("Tất cả tháng");
            for (int i = 1; i <= 12; i++)
            {
                cmbLocThang.Items.Add("Tháng " + i);
            }
            cmbLocThang.SelectedIndex = 0;

            // 3. Nạp năm
            cmbLocNam.Items.Clear();
            cmbLocNam.Items.Add("Tất cả năm");
            int namHienTai = DateTime.Today.Year;
            for (int y = namHienTai - 2; y <= namHienTai + 2; y++)
            {
                cmbLocNam.Items.Add(y.ToString());
            }
            cmbLocNam.SelectedIndex = 0;

            // 4. Trạng thái
            if (cmbLocTrangThai.Items.Count > 0)
                cmbLocTrangThai.SelectedIndex = 0;
        }

        private void UpdateSummaryCards()
        {
            try
            {
                DataTable dt = hoaDonBLL.GetThongKeTaiChinh();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    int tongHD = r["TongHoaDon"] != DBNull.Value ? Convert.ToInt32(r["TongHoaDon"]) : 0;
                    decimal phaiThu = r["TongTienPhaiThu"] != DBNull.Value ? Convert.ToDecimal(r["TongTienPhaiThu"]) : 0;
                    decimal daThu = r["TongTienDaThu"] != DBNull.Value ? Convert.ToDecimal(r["TongTienDaThu"]) : 0;
                    decimal conNo = r["TongTienConNo"] != DBNull.Value ? Convert.ToDecimal(r["TongTienConNo"]) : 0;

                    lblTongHoaDon.Text = tongHD.ToString("N0");
                    lblTongTienPhaiThu.Text = $"{phaiThu:N0} đ";
                    lblTongTienDaThu.Text = $"{daThu:N0} đ";
                    lblTongTienConNo.Text = $"{conNo:N0} đ";
                }
            }
            catch
            {
            }
        }

        private void SwitchTab(int tabIndex)
        {
            currentTabIndex = tabIndex;

            Color activeColor = Color.FromArgb(37, 99, 235);
            Color inactiveColor = Color.FromArgb(100, 116, 139);

            btnTabHoaDon.ForeColor = (tabIndex == 0) ? activeColor : inactiveColor;
            btnTabHoaDon.Font = new Font("Segoe UI Semibold", 9.5F, (tabIndex == 0) ? FontStyle.Bold : FontStyle.Regular);

            btnTabDienNuoc.ForeColor = (tabIndex == 1) ? activeColor : inactiveColor;
            btnTabDienNuoc.Font = new Font("Segoe UI Semibold", 9.5F, (tabIndex == 1) ? FontStyle.Bold : FontStyle.Regular);

            btnTabLichSuThu.ForeColor = (tabIndex == 2) ? activeColor : inactiveColor;
            btnTabLichSuThu.Font = new Font("Segoe UI Semibold", 9.5F, (tabIndex == 2) ? FontStyle.Bold : FontStyle.Regular);

            // Bật / tắt bộ lọc trạng thái chỉ dùng cho Tab Hóa đơn
            pnlTrangThaiGroup.Visible = (tabIndex == 0);

            // Cập nhật cấu hình nút hành động
            if (tabIndex == 0)
            {
                btnThanhToan.Visible = true;
                btnSua.Visible = false;
                btnXoa.Visible = true;
                btnXoa.Text = "Xóa HĐ";
            }
            else if (tabIndex == 1)
            {
                btnThanhToan.Visible = false;
                btnSua.Visible = true;
                btnSua.Text = "Sửa số";
                btnXoa.Visible = true;
                btnXoa.Text = "Xóa số";
            }
            else
            {
                btnThanhToan.Visible = false;
                btnSua.Visible = false;
                btnXoa.Visible = false;
            }

            RefreshAllData();
        }

        private void LoadGridData()
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                string maPhong = cmbLocPhong.SelectedItem?.ToString();
                if (maPhong == "Tất cả phòng") maPhong = null;

                int? thang = null;
                if (cmbLocThang.SelectedIndex > 0)
                    thang = cmbLocThang.SelectedIndex;

                int? nam = null;
                if (cmbLocNam.SelectedIndex > 0 && int.TryParse(cmbLocNam.SelectedItem.ToString(), out int y))
                    nam = y;

                string trangThai = cmbLocTrangThai.SelectedItem?.ToString();

                if (currentTabIndex == 0)
                {
                    // TAB 0: HÓA ĐƠN & CÔNG NỢ
                    DataTable dt = hoaDonBLL.FilterHoaDon(maPhong, thang, nam, trangThai, keyword);
                    dgvMain.DataSource = dt;
                    FormatGridHoaDon();
                }
                else if (currentTabIndex == 1)
                {
                    // TAB 1: CHỈ SỐ ĐIỆN - NƯỚC
                    DataTable dt = dienNuocBLL.FilterDienNuoc(maPhong, thang, nam, keyword);
                    dgvMain.DataSource = dt;
                    FormatGridDienNuoc();
                }
                else
                {
                    // TAB 2: LỊCH SỬ THU TIỀN
                    DataTable dt = thanhToanBLL.GetAllLichSuThanhToan(null, keyword);
                    dgvMain.DataSource = dt;
                    FormatGridLichSu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateActionButtonsState();
            }
        }

        private void FormatGridHoaDon()
        {
            if (dgvMain.Columns.Count == 0) return;

            if (dgvMain.Columns.Contains("MaHoaDon"))
            {
                dgvMain.Columns["MaHoaDon"].HeaderText = "Mã HĐ";
                dgvMain.Columns["MaHoaDon"].FillWeight = 55;
            }
            if (dgvMain.Columns.Contains("MaPhong"))
            {
                dgvMain.Columns["MaPhong"].HeaderText = "Phòng";
                dgvMain.Columns["MaPhong"].FillWeight = 65;
            }
            if (dgvMain.Columns.Contains("TenPhong"))
            {
                dgvMain.Columns["TenPhong"].HeaderText = "Tên Phòng";
                dgvMain.Columns["TenPhong"].FillWeight = 85;
            }
            if (dgvMain.Columns.Contains("TenKhu"))
            {
                dgvMain.Columns["TenKhu"].HeaderText = "Dãy Nhà";
                dgvMain.Columns["TenKhu"].FillWeight = 75;
            }
            if (dgvMain.Columns.Contains("Thang"))
            {
                dgvMain.Columns["Thang"].HeaderText = "Tháng";
                dgvMain.Columns["Thang"].FillWeight = 50;
            }
            if (dgvMain.Columns.Contains("Nam"))
            {
                dgvMain.Columns["Nam"].HeaderText = "Năm";
                dgvMain.Columns["Nam"].FillWeight = 55;
            }
            if (dgvMain.Columns.Contains("TienPhong"))
            {
                dgvMain.Columns["TienPhong"].HeaderText = "Tiền Phòng";
                dgvMain.Columns["TienPhong"].DefaultCellStyle.Format = "N0";
                dgvMain.Columns["TienPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["TienPhong"].FillWeight = 90;
            }
            if (dgvMain.Columns.Contains("TienDien"))
            {
                dgvMain.Columns["TienDien"].HeaderText = "Tiền Điện";
                dgvMain.Columns["TienDien"].DefaultCellStyle.Format = "N0";
                dgvMain.Columns["TienDien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["TienDien"].FillWeight = 85;
            }
            if (dgvMain.Columns.Contains("TienNuoc"))
            {
                dgvMain.Columns["TienNuoc"].HeaderText = "Tiền Nước";
                dgvMain.Columns["TienNuoc"].DefaultCellStyle.Format = "N0";
                dgvMain.Columns["TienNuoc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["TienNuoc"].FillWeight = 85;
            }
            if (dgvMain.Columns.Contains("TienDichVuKhac"))
            {
                dgvMain.Columns["TienDichVuKhac"].HeaderText = "DV Khác";
                dgvMain.Columns["TienDichVuKhac"].DefaultCellStyle.Format = "N0";
                dgvMain.Columns["TienDichVuKhac"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["TienDichVuKhac"].FillWeight = 80;
            }
            if (dgvMain.Columns.Contains("TongTien"))
            {
                dgvMain.Columns["TongTien"].HeaderText = "Tổng Tiền";
                dgvMain.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                dgvMain.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["TongTien"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                dgvMain.Columns["TongTien"].FillWeight = 100;
            }
            if (dgvMain.Columns.Contains("DaThanhToan"))
            {
                dgvMain.Columns["DaThanhToan"].HeaderText = "Đã Thu";
                dgvMain.Columns["DaThanhToan"].DefaultCellStyle.Format = "N0";
                dgvMain.Columns["DaThanhToan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["DaThanhToan"].FillWeight = 95;
            }
            if (dgvMain.Columns.Contains("ConNo"))
            {
                dgvMain.Columns["ConNo"].HeaderText = "Còn Nợ";
                dgvMain.Columns["ConNo"].DefaultCellStyle.Format = "N0";
                dgvMain.Columns["ConNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["ConNo"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                dgvMain.Columns["ConNo"].FillWeight = 95;
            }
            if (dgvMain.Columns.Contains("TrangThai"))
            {
                dgvMain.Columns["TrangThai"].HeaderText = "Trạng Thái";
                dgvMain.Columns["TrangThai"].FillWeight = 110;
            }
            if (dgvMain.Columns.Contains("NgayLap"))
            {
                dgvMain.Columns["NgayLap"].HeaderText = "Ngày Lập";
                dgvMain.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvMain.Columns["NgayLap"].FillWeight = 85;
            }
            if (dgvMain.Columns.Contains("NgayThanhToan")) dgvMain.Columns["NgayThanhToan"].Visible = false;
            if (dgvMain.Columns.Contains("GhiChu")) dgvMain.Columns["GhiChu"].Visible = false;
            if (dgvMain.Columns.Contains("SoDienTieuThu")) dgvMain.Columns["SoDienTieuThu"].Visible = false;
            if (dgvMain.Columns.Contains("SoNuocTieuThu")) dgvMain.Columns["SoNuocTieuThu"].Visible = false;
        }

        private void FormatGridDienNuoc()
        {
            if (dgvMain.Columns.Count == 0) return;

            if (dgvMain.Columns.Contains("MaDienNuoc"))
            {
                dgvMain.Columns["MaDienNuoc"].HeaderText = "Mã";
                dgvMain.Columns["MaDienNuoc"].FillWeight = 50;
            }
            if (dgvMain.Columns.Contains("MaPhong"))
            {
                dgvMain.Columns["MaPhong"].HeaderText = "Phòng";
                dgvMain.Columns["MaPhong"].FillWeight = 70;
            }
            if (dgvMain.Columns.Contains("TenPhong"))
            {
                dgvMain.Columns["TenPhong"].HeaderText = "Tên Phòng";
                dgvMain.Columns["TenPhong"].FillWeight = 100;
            }
            if (dgvMain.Columns.Contains("TenKhu"))
            {
                dgvMain.Columns["TenKhu"].HeaderText = "Dãy Nhà";
                dgvMain.Columns["TenKhu"].FillWeight = 80;
            }
            if (dgvMain.Columns.Contains("Thang"))
            {
                dgvMain.Columns["Thang"].HeaderText = "Tháng";
                dgvMain.Columns["Thang"].FillWeight = 55;
            }
            if (dgvMain.Columns.Contains("Nam"))
            {
                dgvMain.Columns["Nam"].HeaderText = "Năm";
                dgvMain.Columns["Nam"].FillWeight = 60;
            }
            if (dgvMain.Columns.Contains("ChiSoDienCu"))
            {
                dgvMain.Columns["ChiSoDienCu"].HeaderText = "Điện Cũ (kWh)";
                dgvMain.Columns["ChiSoDienCu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["ChiSoDienCu"].FillWeight = 95;
            }
            if (dgvMain.Columns.Contains("ChiSoDienMoi"))
            {
                dgvMain.Columns["ChiSoDienMoi"].HeaderText = "Điện Mới (kWh)";
                dgvMain.Columns["ChiSoDienMoi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["ChiSoDienMoi"].FillWeight = 95;
            }
            if (dgvMain.Columns.Contains("DienTieuThu"))
            {
                dgvMain.Columns["DienTieuThu"].HeaderText = "⚡ Tiêu Thụ (kWh)";
                dgvMain.Columns["DienTieuThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["DienTieuThu"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                dgvMain.Columns["DienTieuThu"].FillWeight = 110;
            }
            if (dgvMain.Columns.Contains("ChiSoNuocCu"))
            {
                dgvMain.Columns["ChiSoNuocCu"].HeaderText = "Nước Cũ (m³)";
                dgvMain.Columns["ChiSoNuocCu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["ChiSoNuocCu"].FillWeight = 95;
            }
            if (dgvMain.Columns.Contains("ChiSoNuocMoi"))
            {
                dgvMain.Columns["ChiSoNuocMoi"].HeaderText = "Nước Mới (m³)";
                dgvMain.Columns["ChiSoNuocMoi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["ChiSoNuocMoi"].FillWeight = 95;
            }
            if (dgvMain.Columns.Contains("NuocTieuThu"))
            {
                dgvMain.Columns["NuocTieuThu"].HeaderText = "💧 Tiêu Thụ (m³)";
                dgvMain.Columns["NuocTieuThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["NuocTieuThu"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                dgvMain.Columns["NuocTieuThu"].FillWeight = 110;
            }
        }

        private void FormatGridLichSu()
        {
            if (dgvMain.Columns.Count == 0) return;

            if (dgvMain.Columns.Contains("MaThanhToan"))
            {
                dgvMain.Columns["MaThanhToan"].HeaderText = "Mã PT";
                dgvMain.Columns["MaThanhToan"].FillWeight = 60;
            }
            if (dgvMain.Columns.Contains("MaHoaDon"))
            {
                dgvMain.Columns["MaHoaDon"].HeaderText = "Mã HĐ";
                dgvMain.Columns["MaHoaDon"].FillWeight = 65;
            }
            if (dgvMain.Columns.Contains("MaPhong"))
            {
                dgvMain.Columns["MaPhong"].HeaderText = "Phòng";
                dgvMain.Columns["MaPhong"].FillWeight = 70;
            }
            if (dgvMain.Columns.Contains("TenPhong"))
            {
                dgvMain.Columns["TenPhong"].HeaderText = "Tên Phòng";
                dgvMain.Columns["TenPhong"].FillWeight = 90;
            }
            if (dgvMain.Columns.Contains("Thang"))
            {
                dgvMain.Columns["Thang"].HeaderText = "Tháng";
                dgvMain.Columns["Thang"].FillWeight = 50;
            }
            if (dgvMain.Columns.Contains("Nam"))
            {
                dgvMain.Columns["Nam"].HeaderText = "Năm";
                dgvMain.Columns["Nam"].FillWeight = 55;
            }
            if (dgvMain.Columns.Contains("NgayThanhToan"))
            {
                dgvMain.Columns["NgayThanhToan"].HeaderText = "Ngày Nộp";
                dgvMain.Columns["NgayThanhToan"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvMain.Columns["NgayThanhToan"].FillWeight = 110;
            }
            if (dgvMain.Columns.Contains("SoTienThanhToan"))
            {
                dgvMain.Columns["SoTienThanhToan"].HeaderText = "Số Tiền Đã Thu";
                dgvMain.Columns["SoTienThanhToan"].DefaultCellStyle.Format = "N0";
                dgvMain.Columns["SoTienThanhToan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMain.Columns["SoTienThanhToan"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                dgvMain.Columns["SoTienThanhToan"].FillWeight = 105;
            }
            if (dgvMain.Columns.Contains("PhuongThuc"))
            {
                dgvMain.Columns["PhuongThuc"].HeaderText = "Phương Thức";
                dgvMain.Columns["PhuongThuc"].FillWeight = 100;
            }
            if (dgvMain.Columns.Contains("NguoiNop"))
            {
                dgvMain.Columns["NguoiNop"].HeaderText = "Người Nộp";
                dgvMain.Columns["NguoiNop"].FillWeight = 110;
            }
            if (dgvMain.Columns.Contains("GhiChu"))
            {
                dgvMain.Columns["GhiChu"].HeaderText = "Ghi Chú";
                dgvMain.Columns["GhiChu"].FillWeight = 120;
            }
        }

        private DataGridViewRow GetSelectedRow()
        {
            if (dgvMain.SelectedRows.Count > 0)
                return dgvMain.SelectedRows[0];
            if (dgvMain.CurrentRow != null && dgvMain.CurrentRow.Index >= 0)
                return dgvMain.CurrentRow;
            return null;
        }

        private void UpdateActionButtonsState()
        {
            DataGridViewRow row = GetSelectedRow();

            if (currentTabIndex == 0) // HÓA ĐƠN
            {
                bool hasRow = row != null;
                decimal conNo = 0;
                if (hasRow && dgvMain.Columns.Contains("ConNo"))
                {
                    conNo = Convert.ToDecimal(row.Cells["ConNo"].Value ?? 0);
                }

                btnThanhToan.Enabled = hasRow && conNo > 0;
                btnXoa.Enabled = hasRow;

                if (btnThanhToan.Enabled)
                {
                    btnThanhToan.BackColor = Color.FromArgb(22, 163, 74);
                    btnThanhToan.ForeColor = Color.White;
                }
                else
                {
                    btnThanhToan.BackColor = Color.FromArgb(226, 232, 240);
                    btnThanhToan.ForeColor = Color.FromArgb(148, 163, 184);
                }
            }
            else if (currentTabIndex == 1) // ĐIỆN NƯỚC
            {
                bool hasRow = row != null;
                btnSua.Enabled = hasRow;
                btnXoa.Enabled = hasRow;
            }
        }

        private void btnTabHoaDon_Click(object sender, EventArgs e)
        {
            SwitchTab(0);
        }

        private void btnTabDienNuoc_Click(object sender, EventArgs e)
        {
            SwitchTab(1);
        }

        private void btnTabLichSuThu_Click(object sender, EventArgs e)
        {
            SwitchTab(2);
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            if (isFilterLoading) return;
            LoadGridData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            isFilterLoading = true;
            txtTimKiem.Clear();
            if (cmbLocPhong.Items.Count > 0) cmbLocPhong.SelectedIndex = 0;
            if (cmbLocThang.Items.Count > 0) cmbLocThang.SelectedIndex = 0;
            if (cmbLocNam.Items.Count > 0) cmbLocNam.SelectedIndex = 0;
            if (cmbLocTrangThai.Items.Count > 0) cmbLocTrangThai.SelectedIndex = 0;
            isFilterLoading = false;

            RefreshAllData();
        }

        private void btnLapHoaDon_Click(object sender, EventArgs e)
        {
            using (frmLapHoaDon frm = new frmLapHoaDon())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
        }

        private void btnGhiDienNuoc_Click(object sender, EventArgs e)
        {
            using (frmGhiDienNuoc frm = new frmGhiDienNuoc())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
        }

        private void btnCauHinhGia_Click(object sender, EventArgs e)
        {
            using (frmCauHinhDonGia frm = new frmCauHinhDonGia())
            {
                frm.ShowDialog();
                RefreshAllData();
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn cần thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int maHoaDon = Convert.ToInt32(row.Cells["MaHoaDon"].Value);
            decimal conNo = Convert.ToDecimal(row.Cells["ConNo"].Value ?? 0);

            if (conNo <= 0)
            {
                MessageBox.Show($"Hóa đơn #{maHoaDon} đã được thanh toán đầy đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (frmThanhToanHoaDon frm = new frmThanhToanHoaDon(maHoaDon))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (currentTabIndex == 1) // SỬA CHỈ SỐ ĐIỆN NƯỚC
            {
                DataGridViewRow row = GetSelectedRow();
                if (row == null)
                {
                    MessageBox.Show("Vui lòng chọn bản ghi chỉ số cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int maDienNuoc = Convert.ToInt32(row.Cells["MaDienNuoc"].Value);
                using (frmGhiDienNuoc frm = new frmGhiDienNuoc(maDienNuoc))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        RefreshAllData();
                    }
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn bản ghi cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (currentTabIndex == 0) // XÓA HÓA ĐƠN
            {
                int maHoaDon = Convert.ToInt32(row.Cells["MaHoaDon"].Value);
                string maPhong = row.Cells["MaPhong"].Value?.ToString() ?? "";
                string thang = row.Cells["Thang"].Value?.ToString() ?? "";
                string nam = row.Cells["Nam"].Value?.ToString() ?? "";
                decimal daThanhToan = Convert.ToDecimal(row.Cells["DaThanhToan"].Value ?? 0);

                if (daThanhToan > 0)
                {
                    MessageBox.Show($"Không thể xóa hóa đơn #{maHoaDon} vì đã có số tiền thanh toán ({daThanhToan:N0} đ)!\nChỉ được xóa những hóa đơn hoàn toàn chưa thanh toán.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa hóa đơn #{maHoaDon} của phòng {maPhong} (tháng {thang}/{nam})?",
                    "Xác nhận xóa hóa đơn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        if (hoaDonBLL.DeleteHoaDon(maHoaDon))
                        {
                            MessageBox.Show("Đã xóa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshAllData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (currentTabIndex == 1) // XÓA CHỈ SỐ ĐIỆN NƯỚC
            {
                int maDienNuoc = Convert.ToInt32(row.Cells["MaDienNuoc"].Value);
                string maPhong = row.Cells["MaPhong"].Value?.ToString() ?? "";
                string thang = row.Cells["Thang"].Value?.ToString() ?? "";
                string nam = row.Cells["Nam"].Value?.ToString() ?? "";

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa chỉ số điện nước của phòng {maPhong} (tháng {thang}/{nam})?",
                    "Xác nhận xóa chỉ số",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        if (dienNuocBLL.DeleteDienNuoc(maDienNuoc))
                        {
                            MessageBox.Show("Đã xóa chỉ số điện nước thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshAllData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void dgvMain_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActionButtonsState();
        }

        private void dgvMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = dgvMain.Columns[e.ColumnIndex].Name;

            if (colName == "TrangThai" && e.Value != null)
            {
                string trangThai = e.Value.ToString().Trim();
                e.CellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

                if (trangThai == "Đã thanh toán" || trangThai == "Da thanh toan")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74);
                }
                else if (trangThai == "Thanh toán một phần")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                }
            }
            else if (colName == "ConNo" && e.Value != null)
            {
                decimal val = Convert.ToDecimal(e.Value);
                if (val > 0)
                {
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                    e.CellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74);
                }
            }
        }
    }
}

