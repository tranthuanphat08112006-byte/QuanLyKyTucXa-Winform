using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class UCSinhVien : UserControl
    {
        private SinhVienBLL svBLL = new SinhVienBLL();
        private bool isFilterLoading = false;

        public UCSinhVien()
        {
            InitializeComponent();
        }

        private void UCSinhVien_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // 1. Định dạng bảng dữ liệu hiện đại
            UIHelper.FormatModernDataGridView(dgvSinhVien);

            isFilterLoading = true;
            if (cmbLocGioiTinh.Items.Count > 0) cmbLocGioiTinh.SelectedIndex = 0;
            isFilterLoading = false;

            // 2. Tải dữ liệu và cập nhật thẻ thống kê
            RefreshAllData();
        }

        public void RefreshAllData()
        {
            LoadDanhSachSinhVien();
            UpdateSummaryCards();
        }

        private void UpdateSummaryCards()
        {
            try
            {
                DataTable dtAll = svBLL.GetAllSinhVien();
                int tongSV = 0;
                int svNam = 0;
                int svNu = 0;

                if (dtAll != null)
                {
                    tongSV = dtAll.Rows.Count;
                    foreach (DataRow row in dtAll.Rows)
                    {
                        string gioiTinh = row["GioiTinh"]?.ToString().Trim() ?? "";
                        if (string.Equals(gioiTinh, "Nam", StringComparison.OrdinalIgnoreCase))
                            svNam++;
                        else if (string.Equals(gioiTinh, "Nữ", StringComparison.OrdinalIgnoreCase))
                            svNu++;
                    }
                }

                int svHopDong = svBLL.CountSinhVienCoHopDong();

                lblTongSinhVien.Text = tongSV.ToString();
                lblSinhVienNam.Text = svNam.ToString();
                lblSinhVienNu.Text = svNu.ToString();
                lblSinhVienHopDong.Text = svHopDong.ToString();

                double pctNam = tongSV > 0 ? Math.Round((double)svNam * 100 / tongSV, 1) : 0;
                double pctNu = tongSV > 0 ? Math.Round((double)svNu * 100 / tongSV, 1) : 0;

                lblCard2Sub.Text = $"Chiếm {pctNam}% tổng số sinh viên";
                lblCard3Sub.Text = $"Chiếm {pctNu}% tổng số sinh viên";
                lblCard4Sub.Text = $"Chưa xếp phòng: {Math.Max(0, tongSV - svHopDong)} sinh viên";
            }
            catch
            {
            }
        }

        private void LoadDanhSachSinhVien()
        {
            try
            {
                string keyword = txtTimKiem.Text;
                string gioiTinh = cmbLocGioiTinh.Text;

                dgvSinhVien.DataSource = svBLL.FilterSinhVien(keyword, gioiTinh);

                if (dgvSinhVien.Columns.Count > 0)
                {
                    if (dgvSinhVien.Columns.Contains("MaSV"))
                        dgvSinhVien.Columns["MaSV"].HeaderText = "Mã Sinh Viên";
                    if (dgvSinhVien.Columns.Contains("HoTen"))
                        dgvSinhVien.Columns["HoTen"].HeaderText = "Họ và Tên";
                    if (dgvSinhVien.Columns.Contains("NgaySinh"))
                    {
                        dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                        dgvSinhVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                    if (dgvSinhVien.Columns.Contains("GioiTinh"))
                        dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới Tính";
                    if (dgvSinhVien.Columns.Contains("CCCD"))
                        dgvSinhVien.Columns["CCCD"].HeaderText = "Số CCCD / CMND";
                    if (dgvSinhVien.Columns.Contains("SDT"))
                        dgvSinhVien.Columns["SDT"].HeaderText = "Số Điện Thoại";
                    if (dgvSinhVien.Columns.Contains("TenPhong"))
                    {
                        dgvSinhVien.Columns["TenPhong"].HeaderText = "Phòng";
                        dgvSinhVien.Columns["TenPhong"].FillWeight = 75;
                    }
                    if (dgvSinhVien.Columns.Contains("TrangThai"))
                    {
                        dgvSinhVien.Columns["TrangThai"].HeaderText = "Trạng Thái";
                        dgvSinhVien.Columns["TrangThai"].FillWeight = 110;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SinhVien GetSelectedSinhVienFromGrid()
        {
            if (dgvSinhVien.SelectedRows.Count == 0 && dgvSinhVien.CurrentRow == null) return null;
            DataGridViewRow row = dgvSinhVien.SelectedRows.Count > 0 ? dgvSinhVien.SelectedRows[0] : dgvSinhVien.CurrentRow;

            string maSV = row.Cells["MaSV"].Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(maSV)) return null;

            string hoTen = row.Cells["HoTen"].Value?.ToString() ?? "";
            DateTime ngaySinh = row.Cells["NgaySinh"].Value != null && row.Cells["NgaySinh"].Value != DBNull.Value
                ? Convert.ToDateTime(row.Cells["NgaySinh"].Value)
                : DateTime.Now;
            string cccd = row.Cells["CCCD"].Value?.ToString() ?? "";
            string sdt = row.Cells["SDT"].Value?.ToString() ?? "";
            string gioiTinh = row.Cells["GioiTinh"].Value?.ToString() ?? "Nam";

            return new SinhVien(maSV, hoTen, ngaySinh, cccd, sdt, gioiTinh);
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            frmChiTietSinhVien frm = new frmChiTietSinhVien();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                RefreshAllData();
            }
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            SinhVien svSelected = GetSelectedSinhVienFromGrid();
            if (svSelected != null)
            {
                frmChiTietSinhVien frm = new frmChiTietSinhVien(svSelected, true);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên trên bảng để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            SinhVien svSelected = GetSelectedSinhVienFromGrid();
            if (svSelected != null)
            {
                frmChiTietSinhVien frm = new frmChiTietSinhVien(svSelected, false);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên trên bảng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            SinhVien svSelected = GetSelectedSinhVienFromGrid();
            if (svSelected != null)
            {
                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa hồ sơ sinh viên {svSelected.HoTen} ({svSelected.MaSV}) không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        if (svBLL.DeleteSinhVien(svSelected.MaSV))
                        {
                            MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Vui lòng chọn một sinh viên trên bảng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (isFilterLoading) return;
            LoadDanhSachSinhVien();
        }

        private void cmbLocGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isFilterLoading) return;
            LoadDanhSachSinhVien();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            isFilterLoading = true;
            txtTimKiem.Clear();
            if (cmbLocGioiTinh.Items.Count > 0) cmbLocGioiTinh.SelectedIndex = 0;
            isFilterLoading = false;
            RefreshAllData();
        }

        private void dgvSinhVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvSinhVien.Columns[e.ColumnIndex].Name == "GioiTinh" && e.Value != null)
            {
                string gt = e.Value.ToString();
                e.CellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                if (gt == "Nam")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);
                }
                else if (gt == "Nữ")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(13, 148, 136);
                }
            }

            if (dgvSinhVien.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string trangThai = e.Value.ToString();
                e.CellStyle.Font = new Font(dgvSinhVien.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = (trangThai == "Đang lưu trú")
                    ? Color.FromArgb(22, 163, 74)   // Xanh lá
                    : Color.FromArgb(234, 88, 12);  // Cam cảnh báo
            }
        }

        private void dgvSinhVien_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnXemChiTiet_Click(sender, e);
            }
        }
    }
}
