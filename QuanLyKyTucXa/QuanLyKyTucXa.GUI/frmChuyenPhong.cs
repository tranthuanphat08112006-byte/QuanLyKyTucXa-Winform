using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmChuyenPhong : Form
    {
        private readonly HopDongBLL hopDongBLL = new HopDongBLL();
        private readonly PhongBLL phongBLL = new PhongBLL();

        private readonly string _maHopDongCu;
        private readonly string _maSV;
        private readonly string _hoTen;
        private readonly string _maPhongCu;
        private readonly string _tenPhongCu;
        private readonly DateTime _ngayKetThucCu;

        public frmChuyenPhong()
        {
            InitializeComponent();
        }

        public frmChuyenPhong(string maHopDongCu, string maSV, string hoTen, string maPhongCu, string tenPhongCu, DateTime ngayKetThucCu)
            : this()
        {
            _maHopDongCu = maHopDongCu;
            _maSV = maSV;
            _hoTen = hoTen;
            _maPhongCu = maPhongCu;
            _tenPhongCu = tenPhongCu;
            _ngayKetThucCu = ngayKetThucCu;
        }

        private void frmChuyenPhong_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            txtMaHopDongCu.Text = "#" + _maHopDongCu;
            txtSinhVien.Text = $"{_maSV} - {_hoTen}";
            txtPhongHienTai.Text = $"{_tenPhongCu} ({_maPhongCu})";

            dtpNgayBatDau.Value = DateTime.Today;
            dtpNgayKetThuc.Value = _ngayKetThucCu.Date > DateTime.Today
                ? _ngayKetThucCu.Date
                : DateTime.Today.AddMonths(6);

            LoadDanhSachPhongMoi();
        }

        private void LoadDanhSachPhongMoi()
        {
            try
            {
                DataTable dtPhong = phongBLL.GetAllPhong();
                if (dtPhong == null) return;

                if (!dtPhong.Columns.Contains("DisplayText"))
                    dtPhong.Columns.Add("DisplayText", typeof(string));

                DataTable dtFiltered = dtPhong.Clone();

                foreach (DataRow row in dtPhong.Rows)
                {
                    string maPhong = row["MaPhong"]?.ToString() ?? "";
                    if (string.Equals(maPhong, _maPhongCu, StringComparison.OrdinalIgnoreCase))
                        continue;

                    string tenPhong = row["TenPhong"]?.ToString() ?? "";
                    string tenKhu = row["TenKhu"]?.ToString() ?? "";
                    string loaiPhong = row["LoaiPhong"]?.ToString() ?? "";
                    int trong = Convert.ToInt32(row["SoGiuongTrong"]);
                    int toiDa = Convert.ToInt32(row["SoGiuongToiDa"]);

                    row["DisplayText"] = $"{tenPhong} - {tenKhu} ({loaiPhong}) | Trống: {trong}/{toiDa}";
                    dtFiltered.ImportRow(row);
                }

                cmbPhongMoi.DataSource = dtFiltered;
                cmbPhongMoi.DisplayMember = "DisplayText";
                cmbPhongMoi.ValueMember = "MaPhong";

                if (cmbPhongMoi.Items.Count > 0)
                {
                    cmbPhongMoi.SelectedIndex = 0;
                }
                else
                {
                    lblThongTinPhongMoi.Text = "Không có phòng khác để chuyển.";
                    lblThongTinPhongMoi.ForeColor = Color.FromArgb(220, 38, 38);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách phòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbPhongMoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPhongMoi.SelectedItem is DataRowView drv)
            {
                string loaiPhong = drv["LoaiPhong"]?.ToString() ?? "";
                int trong = Convert.ToInt32(drv["SoGiuongTrong"]);
                decimal donGia = Convert.ToDecimal(drv["DonGia"]);
                string trangThai = drv["TrangThai"]?.ToString() ?? "";

                lblThongTinPhongMoi.Text = $"Loại: {loaiPhong}  |  Đơn giá: {donGia:N0} VNĐ/tháng  |  Trạng thái: {trangThai}";
                lblThongTinPhongMoi.ForeColor = (trong > 0 && trangThai != "Bảo trì" && trangThai != "Đang sửa chữa")
                    ? Color.FromArgb(22, 163, 74)
                    : Color.FromArgb(220, 38, 38);
            }
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (cmbPhongMoi.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phòng mới muốn chuyển đến!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                HopDongDTO hdMoi = new HopDongDTO(
                    _maSV,
                    cmbPhongMoi.SelectedValue.ToString(),
                    dtpNgayBatDau.Value.Date,
                    dtpNgayKetThuc.Value.Date
                );

                if (hopDongBLL.ChuyenPhong(_maHopDongCu, _maPhongCu, hdMoi))
                {
                    MessageBox.Show("Chuyển phòng thành công! Hợp đồng cũ đã được lưu lịch sử.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể chuyển phòng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

