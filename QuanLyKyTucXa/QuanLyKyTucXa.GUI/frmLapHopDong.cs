using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmLapHopDong : Form
    {
        private readonly HopDongBLL hopDongBLL = new HopDongBLL();
        private readonly SinhVienBLL svBLL = new SinhVienBLL();
        private readonly PhongBLL phongBLL = new PhongBLL();

        private DataTable dtSinhVien;
        private DataTable dtPhong;

        public frmLapHopDong()
        {
            InitializeComponent();
        }

        private void frmLapHopDong_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            dtpNgayBatDau.Value = DateTime.Today;
            dtpNgayKetThuc.Value = DateTime.Today.AddMonths(6);

            LoadDanhSachSinhVien();
            LoadDanhSachPhong();
        }

        private void LoadDanhSachSinhVien()
        {
            try
            {
                dtSinhVien = svBLL.GetAllSinhVien();
                if (dtSinhVien == null) return;

                if (!dtSinhVien.Columns.Contains("DisplayText"))
                    dtSinhVien.Columns.Add("DisplayText", typeof(string));

                foreach (DataRow row in dtSinhVien.Rows)
                {
                    string maSV = row["MaSV"]?.ToString() ?? "";
                    string hoTen = row["HoTen"]?.ToString() ?? "";
                    string gioiTinh = row["GioiTinh"]?.ToString() ?? "";
                    string trangThai = row["TrangThai"]?.ToString() ?? "";
                    row["DisplayText"] = $"{maSV} - {hoTen} ({gioiTinh}) [{trangThai}]";
                }

                // Ưu tiên đưa sinh viên chưa xếp phòng lên đầu danh sách
                dtSinhVien.DefaultView.Sort = "TrangThai ASC, MaSV ASC";
                DataTable sorted = dtSinhVien.DefaultView.ToTable();
                dtSinhVien = sorted;

                cmbSinhVien.DataSource = dtSinhVien;
                cmbSinhVien.DisplayMember = "DisplayText";
                cmbSinhVien.ValueMember = "MaSV";

                if (cmbSinhVien.Items.Count > 0)
                    cmbSinhVien.SelectedIndex = 0;
                else
                    lblThongTinSV.Text = "Chưa có sinh viên nào trong hệ thống.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachPhong()
        {
            try
            {
                dtPhong = phongBLL.GetAllPhong();
                if (dtPhong == null) return;

                if (!dtPhong.Columns.Contains("DisplayText"))
                    dtPhong.Columns.Add("DisplayText", typeof(string));

                foreach (DataRow row in dtPhong.Rows)
                {
                    string tenPhong = row["TenPhong"]?.ToString() ?? "";
                    string tenKhu = row["TenKhu"]?.ToString() ?? "";
                    string loaiPhong = row["LoaiPhong"]?.ToString() ?? "";
                    int trong = Convert.ToInt32(row["SoGiuongTrong"]);
                    int toiDa = Convert.ToInt32(row["SoGiuongToiDa"]);
                    row["DisplayText"] = $"{tenPhong} - {tenKhu} ({loaiPhong}) | Trống: {trong}/{toiDa}";
                }

                cmbPhong.DataSource = dtPhong;
                cmbPhong.DisplayMember = "DisplayText";
                cmbPhong.ValueMember = "MaPhong";

                if (cmbPhong.Items.Count > 0)
                    cmbPhong.SelectedIndex = 0;
                else
                    lblThongTinPhong.Text = "Chưa có phòng nào trong hệ thống.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách phòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSinhVien.SelectedItem is DataRowView drv)
            {
                string gioiTinh = drv["GioiTinh"]?.ToString() ?? "";
                string sdt = drv["SDT"]?.ToString() ?? "";
                string trangThai = drv["TrangThai"]?.ToString() ?? "";

                lblThongTinSV.Text = $"Giới tính: {gioiTinh}  |  SĐT: {sdt}  |  {trangThai}";
                lblThongTinSV.ForeColor = trangThai == "Đang lưu trú"
                    ? Color.FromArgb(220, 38, 38)
                    : Color.FromArgb(22, 163, 74);
            }
        }

        private void cmbPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPhong.SelectedItem is DataRowView drv)
            {
                string loaiPhong = drv["LoaiPhong"]?.ToString() ?? "";
                int trong = Convert.ToInt32(drv["SoGiuongTrong"]);
                decimal donGia = Convert.ToDecimal(drv["DonGia"]);
                string trangThai = drv["TrangThai"]?.ToString() ?? "";

                lblThongTinPhong.Text = $"Loại: {loaiPhong}  |  Đơn giá: {donGia:N0} VNĐ/tháng  |  Trạng thái: {trangThai}";
                lblThongTinPhong.ForeColor = (trong > 0 && trangThai != "Bảo trì" && trangThai != "Đang sửa chữa")
                    ? Color.FromArgb(22, 163, 74)
                    : Color.FromArgb(220, 38, 38);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cmbSinhVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần lập hợp đồng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPhong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phòng cần xếp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                HopDongDTO hd = new HopDongDTO(
                    cmbSinhVien.SelectedValue.ToString(),
                    cmbPhong.SelectedValue.ToString(),
                    dtpNgayBatDau.Value.Date,
                    dtpNgayKetThuc.Value.Date
                );

                if (hopDongBLL.LapHopDongMoi(hd))
                {
                    MessageBox.Show("Lập hợp đồng lưu trú mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể lập hợp đồng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

