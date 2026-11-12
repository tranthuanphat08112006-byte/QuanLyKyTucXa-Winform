using System;
using System.Data;
using System.Windows.Forms;
using QuanLyKyTucXa.DTO;
using QuanLyKyTucXa.BLL;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmChiTietPhong : Form
    {
        private PhongBLL phongBLL = new PhongBLL();
        private Phong phongCurrent = null;
        private bool isReadOnly = false;
        private bool isSyncingBeds = false;

        public frmChiTietPhong()
        {
            InitializeComponent();
            LoadDayNhaDropdown();

            this.Text = "Thêm mới Phòng Ký túc xá";
            lblFormTitle.Text = "THÊM PHÒNG KÝ TÚC XÁ MỚI";
            lblFormSub.Text = "Nhập đầy đủ thông tin phòng, sức chứa tối đa và đơn giá lưu trú";

            if (cmbTenKhu.Items.Count > 0) cmbTenKhu.SelectedIndex = 0;
            if (cmbLoaiPhong.Items.Count > 0) cmbLoaiPhong.SelectedIndex = 0;
            if (cmbTrangThai.Items.Count > 0) cmbTrangThai.SelectedIndex = 0;

            SyncBedsFromSucChuaAndDangO();
        }

        public frmChiTietPhong(Phong p, bool readOnlyMode = false) : this()
        {
            this.phongCurrent = p;
            this.isReadOnly = readOnlyMode;

            txtMaPhong.Text = p.MaPhong;
            txtMaPhong.Enabled = false;
            txtTenPhong.Text = string.IsNullOrWhiteSpace(p.TenPhong) ? ("Phòng " + p.MaPhong) : p.TenPhong;

            if (!string.IsNullOrWhiteSpace(p.TenKhu) && !cmbTenKhu.Items.Contains(p.TenKhu))
            {
                cmbTenKhu.Items.Add(p.TenKhu);
            }
            cmbTenKhu.Text = p.TenKhu;
            cmbLoaiPhong.Text = p.LoaiPhong;

            isSyncingBeds = true;
            numSoGiuongToiDa.Value = p.SoGiuongToiDa > 0 ? p.SoGiuongToiDa : 1;
            int dangO = Math.Max(0, p.SoGiuongToiDa - p.SoGiuongTrong);
            numSoNguoiHienTai.Maximum = numSoGiuongToiDa.Value;
            numSoNguoiHienTai.Value = Math.Min(dangO, (int)numSoGiuongToiDa.Value);
            numSoGiuongTrong.Value = Math.Max(0, p.SoGiuongTrong);
            isSyncingBeds = false;

            txtDonGia.Text = p.DonGia.ToString("0");
            cmbTrangThai.Text = string.IsNullOrWhiteSpace(p.TrangThai)
                ? (p.SoGiuongTrong <= 0 ? "Đã đầy" : "Còn trống")
                : p.TrangThai;

            if (readOnlyMode)
            {
                this.Text = "Chi tiết thông tin Phòng - " + p.MaPhong;
                lblFormTitle.Text = "CHI TIẾT PHÒNG: " + p.MaPhong;
                lblFormSub.Text = "Chế độ xem thông tin chi tiết phòng và trạng thái lưu trú";

                txtTenPhong.ReadOnly = true;
                cmbTenKhu.Enabled = false;
                cmbLoaiPhong.Enabled = false;
                numSoGiuongToiDa.Enabled = false;
                numSoNguoiHienTai.Enabled = false;
                txtDonGia.ReadOnly = true;
                cmbTrangThai.Enabled = false;

                btnLuu.Visible = false;
                btnHuy.Text = "Đóng";
            }
            else
            {
                this.Text = "Cập nhật thông tin Phòng - " + p.MaPhong;
                lblFormTitle.Text = "CẬP NHẬT PHÒNG: " + p.MaPhong;
                lblFormSub.Text = "Chỉnh sửa thông tin dãy nhà, sức chứa, đơn giá và trạng thái phòng";
            }
        }

        private void LoadDayNhaDropdown()
        {
            try
            {
                cmbTenKhu.Items.Clear();
                DataTable dt = phongBLL.GetAllDayNha();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string tenKhu = row["TenKhu"].ToString();
                        if (!string.IsNullOrWhiteSpace(tenKhu) && !cmbTenKhu.Items.Contains(tenKhu))
                        {
                            cmbTenKhu.Items.Add(tenKhu);
                        }
                    }
                }
            }
            catch
            {
            }

            if (cmbTenKhu.Items.Count == 0)
            {
                cmbTenKhu.Items.AddRange(new object[] { "Khu A", "Khu B", "Khu C", "Khu D" });
            }
        }

        private void SyncBedsFromSucChuaAndDangO()
        {
            if (isSyncingBeds) return;
            isSyncingBeds = true;
            try
            {
                int sucChua = (int)numSoGiuongToiDa.Value;
                numSoNguoiHienTai.Maximum = sucChua;
                if (numSoNguoiHienTai.Value > sucChua)
                {
                    numSoNguoiHienTai.Value = sucChua;
                }

                int dangO = (int)numSoNguoiHienTai.Value;
                int giuongTrong = Math.Max(0, sucChua - dangO);
                numSoGiuongTrong.Maximum = sucChua;
                numSoGiuongTrong.Value = giuongTrong;

                if (cmbTrangThai.Text != "Bảo trì")
                {
                    cmbTrangThai.Text = giuongTrong <= 0 ? "Đã đầy" : "Còn trống";
                }
            }
            finally
            {
                isSyncingBeds = false;
            }
        }

        private void numSoGiuongToiDa_ValueChanged(object sender, EventArgs e)
        {
            SyncBedsFromSucChuaAndDangO();
        }

        private void numSoNguoiHienTai_ValueChanged(object sender, EventArgs e)
        {
            SyncBedsFromSucChuaAndDangO();
        }

        private void txtMaPhong_TextChanged(object sender, EventArgs e)
        {
            if (phongCurrent == null && (string.IsNullOrWhiteSpace(txtTenPhong.Text) || txtTenPhong.Text.StartsWith("Phòng ")))
            {
                txtTenPhong.Text = string.IsNullOrWhiteSpace(txtMaPhong.Text) ? "" : ("Phòng " + txtMaPhong.Text.Trim());
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (isReadOnly) return;

            try
            {
                string rawGia = txtDonGia.Text.Trim().Replace(",", "").Replace(".", "");
                decimal donGia;
                if (!decimal.TryParse(rawGia, out donGia) || donGia <= 0)
                {
                    MessageBox.Show("Vui lòng nhập Đơn giá phòng là số hợp lệ lớn hơn 0!", "Cảnh báo dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                    return;
                }

                int sucChua = (int)numSoGiuongToiDa.Value;
                int dangO = (int)numSoNguoiHienTai.Value;
                int giuongTrong = Math.Max(0, sucChua - dangO);

                string tenPhong = string.IsNullOrWhiteSpace(txtTenPhong.Text)
                    ? ("Phòng " + txtMaPhong.Text.Trim())
                    : txtTenPhong.Text.Trim();

                string trangThai = cmbTrangThai.Text.Trim();
                if (trangThai != "Bảo trì")
                {
                    trangThai = giuongTrong <= 0 ? "Đã đầy" : "Còn trống";
                }

                Phong p = new Phong(
                    txtMaPhong.Text.Trim(),
                    tenPhong,
                    cmbTenKhu.Text.Trim(),
                    cmbLoaiPhong.Text.Trim(),
                    sucChua,
                    giuongTrong,
                    donGia,
                    trangThai
                );

                bool isSuccess = false;

                if (phongCurrent == null)
                {
                    isSuccess = phongBLL.InsertPhong(p);
                }
                else
                {
                    isSuccess = phongBLL.UpdatePhong(p);
                }

                if (isSuccess)
                {
                    MessageBox.Show("Lưu thông tin phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
