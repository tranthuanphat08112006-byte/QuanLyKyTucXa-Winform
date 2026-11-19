using System;
using System.Data;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmCauHinhDonGia : Form
    {
        private readonly HoaDonBLL hoaDonBLL = new HoaDonBLL();
        private string selectedMaDichVu = "";

        public frmCauHinhDonGia()
        {
            InitializeComponent();
        }

        private void frmCauHinhDonGia_Load(object sender, EventArgs e)
        {
            UIHelper.FormatModernDataGridView(dgvDichVu);
            LoadDanhSachDichVu();
        }

        private void LoadDanhSachDichVu()
        {
            try
            {
                DataTable dt = hoaDonBLL.GetAllCauHinhDichVu();
                dgvDichVu.DataSource = dt;

                if (dgvDichVu.Columns.Count > 0)
                {
                    if (dgvDichVu.Columns.Contains("MaDichVu"))
                    {
                        dgvDichVu.Columns["MaDichVu"].HeaderText = "Mã DV";
                        dgvDichVu.Columns["MaDichVu"].FillWeight = 60;
                    }
                    if (dgvDichVu.Columns.Contains("TenDichVu"))
                    {
                        dgvDichVu.Columns["TenDichVu"].HeaderText = "Tên Dịch Vụ";
                        dgvDichVu.Columns["TenDichVu"].FillWeight = 140;
                    }
                    if (dgvDichVu.Columns.Contains("DonGia"))
                    {
                        dgvDichVu.Columns["DonGia"].HeaderText = "Đơn Giá (VNĐ)";
                        dgvDichVu.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                        dgvDichVu.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvDichVu.Columns["DonGia"].FillWeight = 100;
                    }
                    if (dgvDichVu.Columns.Contains("DonViTinh"))
                    {
                        dgvDichVu.Columns["DonViTinh"].HeaderText = "ĐVT";
                        dgvDichVu.Columns["DonViTinh"].FillWeight = 80;
                    }
                    if (dgvDichVu.Columns.Contains("GhiChu"))
                    {
                        dgvDichVu.Columns["GhiChu"].HeaderText = "Ghi Chú";
                        dgvDichVu.Columns["GhiChu"].FillWeight = 130;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục dịch vụ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDichVu_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDichVu.CurrentRow == null || dgvDichVu.CurrentRow.Index < 0) return;

            DataGridViewRow row = dgvDichVu.CurrentRow;
            selectedMaDichVu = row.Cells["MaDichVu"].Value?.ToString() ?? "";
            txtTenDichVu.Text = row.Cells["TenDichVu"].Value?.ToString() ?? "";
            numDonGia.Value = Convert.ToDecimal(row.Cells["DonGia"].Value ?? 0);
            lblDonViTinh.Text = "đ/" + (row.Cells["DonViTinh"].Value?.ToString() ?? "");
            txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString() ?? "";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedMaDichVu))
            {
                MessageBox.Show("Vui lòng chọn một dịch vụ trong danh sách để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal donGiaMoi = numDonGia.Value;
            string ghiChu = txtGhiChu.Text.Trim();

            try
            {
                if (hoaDonBLL.UpdateCauHinhDichVu(selectedMaDichVu, donGiaMoi, ghiChu))
                {
                    MessageBox.Show($"Cập nhật đơn giá dịch vụ '{txtTenDichVu.Text}' thành {donGiaMoi:N0} đ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachDichVu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

