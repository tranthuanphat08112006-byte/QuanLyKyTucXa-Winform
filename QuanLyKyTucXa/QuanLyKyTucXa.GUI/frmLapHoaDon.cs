using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmLapHoaDon : Form
    {
        private readonly HoaDonBLL hoaDonBLL = new HoaDonBLL();
        private readonly PhongBLL phongBLL = new PhongBLL();
        private readonly DienNuocBLL dienNuocBLL = new DienNuocBLL();

        private HoaDonDTO currentDuKien = null;
        private bool isLoading = false;

        public frmLapHoaDon()
        {
            InitializeComponent();
        }

        public frmLapHoaDon(string preSelectMaPhong, int preSelectThang, int preSelectNam) : this()
        {
            // Constructor để gọi nhanh từ danh sách điện nước
            this.Tag = new Tuple<string, int, int>(preSelectMaPhong, preSelectThang, preSelectNam);
        }

        private void frmLapHoaDon_Load(object sender, EventArgs e)
        {
            isLoading = true;

            numTienDichVuKhac.Value = hoaDonBLL.GetPhiDichVuMacDinh();
            numThang.Value = DateTime.Today.Month;
            numNam.Value = DateTime.Today.Year;

            LoadDanhSachPhong();

            if (this.Tag is Tuple<string, int, int> pre)
            {
                if (!string.IsNullOrWhiteSpace(pre.Item1)) cmbPhong.SelectedValue = pre.Item1;
                if (pre.Item2 >= 1 && pre.Item2 <= 12) numThang.Value = pre.Item2;
                if (pre.Item3 >= 2020) numNam.Value = pre.Item3;
            }

            isLoading = false;

            CapNhatHoaDonDuKien();
        }

        private void LoadDanhSachPhong()
        {
            try
            {
                DataTable dt = phongBLL.GetAllPhong();
                if (dt != null)
                {
                    if (!dt.Columns.Contains("DisplayText"))
                        dt.Columns.Add("DisplayText", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        string maPhong = row["MaPhong"]?.ToString() ?? "";
                        string tenPhong = row["TenPhong"]?.ToString() ?? "";
                        string tenKhu = row["TenKhu"]?.ToString() ?? "";
                        decimal donGia = Convert.ToDecimal(row["DonGia"]);
                        row["DisplayText"] = $"{maPhong} - {tenPhong} ({tenKhu}) | {donGia:N0} đ/tháng";
                    }

                    cmbPhong.DataSource = dt;
                    cmbPhong.DisplayMember = "DisplayText";
                    cmbPhong.ValueMember = "MaPhong";

                    if (cmbPhong.Items.Count > 0)
                        cmbPhong.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ThongTin_Changed(object sender, EventArgs e)
        {
            if (isLoading) return;
            CapNhatHoaDonDuKien();
        }

        private void numTienDichVuKhac_ValueChanged(object sender, EventArgs e)
        {
            if (isLoading || currentDuKien == null) return;
            currentDuKien.TienDichVuKhac = numTienDichVuKhac.Value;
            currentDuKien.TongTien = currentDuKien.TienPhong + currentDuKien.TienDien + currentDuKien.TienNuoc + currentDuKien.TienDichVuKhac;
            lblTongTien.Text = $"{currentDuKien.TongTien:N0} đ";
        }

        private void CapNhatHoaDonDuKien()
        {
            if (cmbPhong.SelectedValue == null)
            {
                lblThongBaoDienNuoc.Text = "Vui lòng chọn phòng để tính toán.";
                lblThongBaoDienNuoc.ForeColor = Color.FromArgb(100, 116, 139);
                btnLuu.Enabled = false;
                return;
            }

            string maPhong = cmbPhong.SelectedValue.ToString();
            int thang = (int)numThang.Value;
            int nam = (int)numNam.Value;
            decimal tienDichVuKhac = numTienDichVuKhac.Value;

            try
            {
                currentDuKien = hoaDonBLL.TinhToanHoaDonDuKien(maPhong, thang, nam, tienDichVuKhac);

                lblTienPhong.Text = $"{currentDuKien.TienPhong:N0} đ";

                decimal donGiaDien = hoaDonBLL.GetDonGiaDien();
                decimal donGiaNuoc = hoaDonBLL.GetDonGiaNuoc();

                lblChiTietDien.Text = $"({currentDuKien.SoDienTieuThu:N0} kWh × {donGiaDien:N0} đ/kWh)";
                lblThanhTienDien.Text = $"{currentDuKien.TienDien:N0} đ";

                lblChiTietNuoc.Text = $"({currentDuKien.SoNuocTieuThu:N0} m³ × {donGiaNuoc:N0} đ/m³)";
                lblThanhTienNuoc.Text = $"{currentDuKien.TienNuoc:N0} đ";

                lblTongTien.Text = $"{currentDuKien.TongTien:N0} đ";

                // Kiểm tra xem hóa đơn tháng này của phòng đã có trong DB chưa
                DataTable dtCheck = hoaDonBLL.FilterHoaDon(maPhong, thang, nam, null, null);
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    lblThongBaoDienNuoc.Text = $"⚠️ Hóa đơn của phòng {maPhong} cho tháng {thang}/{nam} đã được lập trước đó! Không thể tạo trùng lặp.";
                    lblThongBaoDienNuoc.ForeColor = Color.FromArgb(220, 38, 38);
                    btnLuu.Enabled = false;
                }
                else
                {
                    lblThongBaoDienNuoc.Text = $"✓ Đã đồng bộ thành công dữ liệu tiền phòng và chỉ số điện nước tháng {thang}/{nam}.";
                    lblThongBaoDienNuoc.ForeColor = Color.FromArgb(22, 163, 74);
                    btnLuu.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                currentDuKien = null;
                lblTienPhong.Text = "0 đ";
                lblChiTietDien.Text = "(Chưa có số liệu)";
                lblThanhTienDien.Text = "0 đ";
                lblChiTietNuoc.Text = "(Chưa có số liệu)";
                lblThanhTienNuoc.Text = "0 đ";
                lblTongTien.Text = "0 đ";

                lblThongBaoDienNuoc.Text = "⚠️ " + ex.Message;
                lblThongBaoDienNuoc.ForeColor = Color.FromArgb(220, 38, 38);
                btnLuu.Enabled = false;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (currentDuKien == null)
            {
                MessageBox.Show("Không thể lập hóa đơn khi chưa có đầy đủ thông tin hoặc chưa có chỉ số điện nước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            currentDuKien.TienDichVuKhac = numTienDichVuKhac.Value;
            currentDuKien.TongTien = currentDuKien.TienPhong + currentDuKien.TienDien + currentDuKien.TienNuoc + currentDuKien.TienDichVuKhac;
            currentDuKien.GhiChu = txtGhiChu.Text.Trim();

            DialogResult confirm = MessageBox.Show(
                $"Xác nhận lập hóa đơn thu tiền cho phòng {currentDuKien.MaPhong} tháng {currentDuKien.Thang}/{currentDuKien.Nam}?\n\n" +
                $"- Tiền phòng: {currentDuKien.TienPhong:N0} đ\n" +
                $"- Tiền điện ({currentDuKien.SoDienTieuThu:N0} kWh): {currentDuKien.TienDien:N0} đ\n" +
                $"- Tiền nước ({currentDuKien.SoNuocTieuThu:N0} m³): {currentDuKien.TienNuoc:N0} đ\n" +
                $"- Dịch vụ khác: {currentDuKien.TienDichVuKhac:N0} đ\n" +
                $"------------------------------------\n" +
                $"Tổng cộng: {currentDuKien.TongTien:N0} đ",
                "Xác nhận lập hóa đơn",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes) return;

            try
            {
                if (hoaDonBLL.LapHoaDonMoi(currentDuKien))
                {
                    MessageBox.Show($"Lập hóa đơn thành công cho phòng {currentDuKien.MaPhong} tháng {currentDuKien.Thang}/{currentDuKien.Nam}!\nTổng tiền: {currentDuKien.TongTien:N0} đ.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lập hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

