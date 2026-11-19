using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmThanhToanHoaDon : Form
    {
        private readonly HoaDonBLL hoaDonBLL = new HoaDonBLL();
        private readonly ThanhToanBLL thanhToanBLL = new ThanhToanBLL();

        private int maHoaDon;
        private decimal tongTien = 0;
        private decimal daThanhToan = 0;
        private decimal conNo = 0;
        private string maPhong = "";
        private int thang = 0;
        private int nam = 0;

        public frmThanhToanHoaDon(int maHoaDon)
        {
            InitializeComponent();
            this.maHoaDon = maHoaDon;
        }

        private void frmThanhToanHoaDon_Load(object sender, EventArgs e)
        {
            UIHelper.FormatModernDataGridView(dgvLichSu);
            cmbPhuongThuc.SelectedIndex = 0;

            LoadThongTinHoaDon();
            LoadLichSuThanhToan();
        }

        private void LoadThongTinHoaDon()
        {
            try
            {
                DataRow row = hoaDonBLL.GetHoaDonById(maHoaDon);
                if (row == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin hóa đơn trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                maPhong = row["MaPhong"].ToString();
                string tenPhong = row["TenPhong"]?.ToString() ?? ("Phòng " + maPhong);
                thang = Convert.ToInt32(row["Thang"]);
                nam = Convert.ToInt32(row["Nam"]);
                tongTien = Convert.ToDecimal(row["TongTien"]);
                daThanhToan = Convert.ToDecimal(row["DaThanhToan"]);
                conNo = Math.Max(0, tongTien - daThanhToan);

                lblThongTinHoaDon.Text = $"Hóa đơn #{maHoaDon} - {tenPhong} - Tháng {thang}/{nam}";
                lblTongTien.Text = $"{tongTien:N0} đ";
                lblDaThanhToan.Text = $"{daThanhToan:N0} đ";
                lblConNo.Text = $"{conNo:N0} đ";

                if (conNo <= 0)
                {
                    numSoTien.Maximum = 0;
                    numSoTien.Value = 0;
                    numSoTien.Enabled = false;
                    btnTraHet.Enabled = false;
                    btnThanhToan.Enabled = false;
                    lblConNoSauThanhToan.Text = "Hóa đơn đã được thanh toán đầy đủ!";
                    lblTrangThaiMoi.Text = "Trạng thái: Đã thanh toán";
                }
                else
                {
                    numSoTien.Maximum = conNo;
                    numSoTien.Value = conNo; // Mặc định gợi ý thanh toán toàn bộ
                    numSoTien.Enabled = true;
                    btnTraHet.Enabled = true;
                    btnThanhToan.Enabled = true;
                    CapNhatSoTienSauThanhToan();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp dữ liệu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLichSuThanhToan()
        {
            try
            {
                DataTable dt = thanhToanBLL.GetLichSuThanhToanByHoaDon(maHoaDon);
                dgvLichSu.DataSource = dt;

                if (dgvLichSu.Columns.Count > 0)
                {
                    if (dgvLichSu.Columns.Contains("MaThanhToan"))
                    {
                        dgvLichSu.Columns["MaThanhToan"].HeaderText = "Mã PT";
                        dgvLichSu.Columns["MaThanhToan"].FillWeight = 50;
                    }
                    if (dgvLichSu.Columns.Contains("MaHoaDon"))
                    {
                        dgvLichSu.Columns["MaHoaDon"].Visible = false;
                    }
                    if (dgvLichSu.Columns.Contains("NgayThanhToan"))
                    {
                        dgvLichSu.Columns["NgayThanhToan"].HeaderText = "Ngày Thu";
                        dgvLichSu.Columns["NgayThanhToan"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                        dgvLichSu.Columns["NgayThanhToan"].FillWeight = 110;
                    }
                    if (dgvLichSu.Columns.Contains("SoTienThanhToan"))
                    {
                        dgvLichSu.Columns["SoTienThanhToan"].HeaderText = "Số Tiền Thu";
                        dgvLichSu.Columns["SoTienThanhToan"].DefaultCellStyle.Format = "N0";
                        dgvLichSu.Columns["SoTienThanhToan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvLichSu.Columns["SoTienThanhToan"].FillWeight = 95;
                    }
                    if (dgvLichSu.Columns.Contains("PhuongThuc"))
                    {
                        dgvLichSu.Columns["PhuongThuc"].HeaderText = "Phương Thức";
                        dgvLichSu.Columns["PhuongThuc"].FillWeight = 95;
                    }
                    if (dgvLichSu.Columns.Contains("NguoiNop"))
                    {
                        dgvLichSu.Columns["NguoiNop"].HeaderText = "Người Nộp";
                        dgvLichSu.Columns["NguoiNop"].FillWeight = 110;
                    }
                    if (dgvLichSu.Columns.Contains("GhiChu"))
                    {
                        dgvLichSu.Columns["GhiChu"].HeaderText = "Ghi Chú";
                        dgvLichSu.Columns["GhiChu"].FillWeight = 110;
                    }
                }
            }
            catch
            {
            }
        }

        private void btnTraHet_Click(object sender, EventArgs e)
        {
            if (conNo > 0)
            {
                numSoTien.Value = conNo;
            }
        }

        private void numSoTien_ValueChanged(object sender, EventArgs e)
        {
            CapNhatSoTienSauThanhToan();
        }

        private void CapNhatSoTienSauThanhToan()
        {
            decimal soTienTra = numSoTien.Value;
            decimal conLai = Math.Max(0, conNo - soTienTra);

            lblConNoSauThanhToan.Text = $"Số tiền còn nợ sau thu: {conLai:N0} đ";

            if (conLai == 0)
            {
                lblTrangThaiMoi.Text = "Trạng thái mới: Đã thanh toán (Hoàn tất 100%)";
                lblTrangThaiMoi.ForeColor = Color.FromArgb(22, 163, 74);
            }
            else
            {
                lblTrangThaiMoi.Text = $"Trạng thái mới: Thanh toán một phần (Còn nợ: {conLai:N0} đ)";
                lblTrangThaiMoi.ForeColor = Color.FromArgb(217, 119, 6);
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            decimal soTienTra = numSoTien.Value;
            if (soTienTra <= 0)
            {
                MessageBox.Show("Số tiền thanh toán phải lớn hơn 0 VNĐ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numSoTien.Focus();
                return;
            }

            if (soTienTra > conNo)
            {
                MessageBox.Show($"Số tiền thanh toán ({soTienTra:N0} đ) vượt quá số tiền còn nợ ({conNo:N0} đ)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numSoTien.Focus();
                return;
            }

            string phuongThuc = cmbPhuongThuc.SelectedItem?.ToString() ?? "Tiền mặt";
            string nguoiNop = txtNguoiNop.Text.Trim();
            string ghiChu = txtGhiChu.Text.Trim();

            DialogResult confirm = MessageBox.Show(
                $"Xác nhận thu tiền cho hóa đơn #{maHoaDon} (Phòng {maPhong} - Tháng {thang}/{nam})?\n\n" +
                $"- Số tiền thanh toán: {soTienTra:N0} đ\n" +
                $"- Phương thức: {phuongThuc}\n" +
                $"- Người nộp: {(string.IsNullOrWhiteSpace(nguoiNop) ? "(Không ghi rõ)" : nguoiNop)}\n" +
                $"- Số tiền còn lại sau thu: {(conNo - soTienTra):N0} đ",
                "Xác nhận thanh toán",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool success = thanhToanBLL.ThanhToanHoaDon(maHoaDon, soTienTra, phuongThuc, nguoiNop, ghiChu);
                if (success)
                {
                    MessageBox.Show($"Thanh toán thành công {soTienTra:N0} đ cho hóa đơn #{maHoaDon}!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

