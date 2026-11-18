using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;

namespace QuanLyKyTucXa.GUI
{
    public partial class UCHopDong : UserControl
    {
        private readonly HopDongBLL hopDongBLL = new HopDongBLL();
        private bool isFilterLoading = false;

        public UCHopDong()
        {
            InitializeComponent();
        }

        private void UCHopDong_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            UIHelper.FormatModernDataGridView(dgvHopDong);

            isFilterLoading = true;
            if (cmbLocTrangThai.Items.Count > 0) cmbLocTrangThai.SelectedIndex = 0;
            isFilterLoading = false;

            // Tự động quét và cập nhật các hợp đồng quá hạn khi mở giao diện
            try
            {
                int soHetHan = hopDongBLL.CapNhatHopDongHetHan();
                if (soHetHan > 0)
                {
                    lblSubtitle.Text = $"Đã tự động cập nhật {soHetHan} hợp đồng quá hạn và hoàn trả giường trống.";
                }
            }
            catch
            {
            }

            RefreshAllData();
        }

        public void RefreshAllData()
        {
            LoadDanhSachHopDong();
            UpdateSummaryCards();
            UpdateActionButtonsState();
        }

        private void UpdateSummaryCards()
        {
            try
            {
                DataTable dtAll = hopDongBLL.GetAllHopDong();
                int tongHD = 0;
                int dangHieuLuc = 0;
                int sapHetHan = 0;
                int daKetThuc = 0;

                if (dtAll != null)
                {
                    tongHD = dtAll.Rows.Count;
                    DateTime today = DateTime.Today;

                    foreach (DataRow row in dtAll.Rows)
                    {
                        string trangThai = row["TrangThai"]?.ToString().Trim() ?? "";
                        if (string.Equals(trangThai, "Đang hiệu lực", StringComparison.OrdinalIgnoreCase))
                        {
                            dangHieuLuc++;

                            if (row["NgayKetThuc"] != DBNull.Value)
                            {
                                DateTime ngayKetThuc = Convert.ToDateTime(row["NgayKetThuc"]).Date;
                                double daysLeft = (ngayKetThuc - today).TotalDays;
                                if (daysLeft >= 0 && daysLeft <= 30)
                                {
                                    sapHetHan++;
                                }
                            }
                        }
                        else
                        {
                            daKetThuc++;
                        }
                    }
                }

                lblTongHopDong.Text = tongHD.ToString();
                lblHopDongHieuLuc.Text = dangHieuLuc.ToString();
                lblSapHetHan.Text = sapHetHan.ToString();
                lblDaKetThuc.Text = daKetThuc.ToString();
            }
            catch
            {
            }
        }

        private void LoadDanhSachHopDong()
        {
            try
            {
                string keyword = txtTimKiem.Text;
                string trangThai = cmbLocTrangThai.Text;

                dgvHopDong.DataSource = hopDongBLL.FilterHopDong(keyword, trangThai);

                if (dgvHopDong.Columns.Count > 0)
                {
                    if (dgvHopDong.Columns.Contains("MaHopDong"))
                    {
                        dgvHopDong.Columns["MaHopDong"].HeaderText = "Mã HĐ";
                        dgvHopDong.Columns["MaHopDong"].FillWeight = 55;
                    }
                    if (dgvHopDong.Columns.Contains("MaSV"))
                    {
                        dgvHopDong.Columns["MaSV"].HeaderText = "Mã SV";
                        dgvHopDong.Columns["MaSV"].FillWeight = 80;
                    }
                    if (dgvHopDong.Columns.Contains("HoTen"))
                    {
                        dgvHopDong.Columns["HoTen"].HeaderText = "Họ và Tên Sinh Viên";
                        dgvHopDong.Columns["HoTen"].FillWeight = 135;
                    }
                    if (dgvHopDong.Columns.Contains("MaPhong"))
                    {
                        dgvHopDong.Columns["MaPhong"].Visible = false;
                    }
                    if (dgvHopDong.Columns.Contains("TenPhong"))
                    {
                        dgvHopDong.Columns["TenPhong"].HeaderText = "Phòng";
                        dgvHopDong.Columns["TenPhong"].FillWeight = 90;
                    }
                    if (dgvHopDong.Columns.Contains("NgayBatDau"))
                    {
                        dgvHopDong.Columns["NgayBatDau"].HeaderText = "Ngày Bắt Đầu";
                        dgvHopDong.Columns["NgayBatDau"].DefaultCellStyle.Format = "dd/MM/yyyy";
                        dgvHopDong.Columns["NgayBatDau"].FillWeight = 85;
                    }
                    if (dgvHopDong.Columns.Contains("NgayKetThuc"))
                    {
                        dgvHopDong.Columns["NgayKetThuc"].HeaderText = "Ngày Kết Thúc";
                        dgvHopDong.Columns["NgayKetThuc"].DefaultCellStyle.Format = "dd/MM/yyyy";
                        dgvHopDong.Columns["NgayKetThuc"].FillWeight = 85;
                    }
                    if (dgvHopDong.Columns.Contains("TrangThai"))
                    {
                        dgvHopDong.Columns["TrangThai"].HeaderText = "Trạng Thái";
                        dgvHopDong.Columns["TrangThai"].FillWeight = 105;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateActionButtonsState();
            }
        }

        private DataGridViewRow GetSelectedRow()
        {
            if (dgvHopDong.SelectedRows.Count > 0)
                return dgvHopDong.SelectedRows[0];
            if (dgvHopDong.CurrentRow != null && dgvHopDong.CurrentRow.Index >= 0)
                return dgvHopDong.CurrentRow;
            return null;
        }

        /// <summary>
        /// Khóa 2 nút Chuyển phòng và Thanh lý nếu chưa chọn dòng hoặc hợp đồng đã kết thúc
        /// </summary>
        private void UpdateActionButtonsState()
        {
            DataGridViewRow row = GetSelectedRow();
            bool isActive = false;

            if (row != null && dgvHopDong.Columns.Contains("TrangThai"))
            {
                string trangThai = row.Cells["TrangThai"].Value?.ToString()?.Trim() ?? "";
                isActive = string.Equals(trangThai, "Đang hiệu lực", StringComparison.OrdinalIgnoreCase);
            }

            btnChuyenPhong.Enabled = isActive;
            btnThanhLy.Enabled = isActive;

            if (isActive)
            {
                btnChuyenPhong.BackColor = Color.FromArgb(2, 132, 199);
                btnChuyenPhong.ForeColor = Color.White;

                btnThanhLy.BackColor = Color.FromArgb(220, 38, 38);
                btnThanhLy.ForeColor = Color.White;
            }
            else
            {
                btnChuyenPhong.BackColor = Color.FromArgb(226, 232, 240);
                btnChuyenPhong.ForeColor = Color.FromArgb(148, 163, 184);

                btnThanhLy.BackColor = Color.FromArgb(226, 232, 240);
                btnThanhLy.ForeColor = Color.FromArgb(148, 163, 184);
            }
        }

        private void btnLapHopDong_Click(object sender, EventArgs e)
        {
            using (frmLapHopDong frm = new frmLapHopDong())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
        }

        private void btnChuyenPhong_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng đang hiệu lực để chuyển phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string trangThai = row.Cells["TrangThai"].Value?.ToString()?.Trim() ?? "";
            if (!string.Equals(trangThai, "Đang hiệu lực", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Chỉ có thể chuyển phòng cho hợp đồng đang ở trạng thái 'Đang hiệu lực'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maHopDong = row.Cells["MaHopDong"].Value?.ToString() ?? "";
            string maSV = row.Cells["MaSV"].Value?.ToString() ?? "";
            string hoTen = row.Cells["HoTen"].Value?.ToString() ?? "";
            string maPhong = row.Cells["MaPhong"].Value?.ToString() ?? "";
            string tenPhong = row.Cells["TenPhong"].Value?.ToString() ?? "";
            DateTime ngayKetThuc = row.Cells["NgayKetThuc"].Value != null && row.Cells["NgayKetThuc"].Value != DBNull.Value
                ? Convert.ToDateTime(row.Cells["NgayKetThuc"].Value)
                : DateTime.Today.AddMonths(6);

            using (frmChuyenPhong frm = new frmChuyenPhong(maHopDong, maSV, hoTen, maPhong, tenPhong, ngayKetThuc))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
        }

        private void btnThanhLy_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng đang hiệu lực để thanh lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string trangThai = row.Cells["TrangThai"].Value?.ToString()?.Trim() ?? "";
            if (!string.Equals(trangThai, "Đang hiệu lực", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Chỉ có thể thanh lý hợp đồng đang ở trạng thái 'Đang hiệu lực'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maHopDong = row.Cells["MaHopDong"].Value?.ToString() ?? "";
            string maSV = row.Cells["MaSV"].Value?.ToString() ?? "";
            string hoTen = row.Cells["HoTen"].Value?.ToString() ?? "";
            string maPhong = row.Cells["MaPhong"].Value?.ToString() ?? "";
            string tenPhong = row.Cells["TenPhong"].Value?.ToString() ?? "";

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn thanh lý hợp đồng #{maHopDong} của sinh viên {hoTen} ({maSV}) tại {tenPhong}?\n\nGiường trống của phòng sẽ được hoàn trả lại sau khi thanh lý.",
                "Xác nhận thanh lý hợp đồng",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (hopDongBLL.ThanhLyHopDong(maHopDong, maPhong))
                    {
                        MessageBox.Show("Thanh lý hợp đồng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshAllData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể thanh lý hợp đồng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (isFilterLoading) return;
            LoadDanhSachHopDong();
        }

        private void cmbLocTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isFilterLoading) return;
            LoadDanhSachHopDong();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            isFilterLoading = true;
            txtTimKiem.Clear();
            if (cmbLocTrangThai.Items.Count > 0) cmbLocTrangThai.SelectedIndex = 0;
            isFilterLoading = false;

            try
            {
                hopDongBLL.CapNhatHopDongHetHan();
            }
            catch
            {
            }

            RefreshAllData();
        }

        private void dgvHopDong_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActionButtonsState();
        }

        private void dgvHopDong_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            UpdateActionButtonsState();
        }

        private void dgvHopDong_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvHopDong.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string trangThai = e.Value.ToString().Trim();
                e.CellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

                if (trangThai == "Đang hiệu lực")
                    e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74);
                else if (trangThai == "Đã chuyển phòng")
                    e.CellStyle.ForeColor = Color.FromArgb(2, 132, 199);
                else if (trangThai == "Hết hạn")
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                else
                    e.CellStyle.ForeColor = Color.FromArgb(100, 116, 139);
            }
        }
    }
}

