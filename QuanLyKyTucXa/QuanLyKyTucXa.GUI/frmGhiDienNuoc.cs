using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmGhiDienNuoc : Form
    {
        private readonly DienNuocBLL dienNuocBLL = new DienNuocBLL();
        private readonly PhongBLL phongBLL = new PhongBLL();

        private int editingMaDienNuoc = 0;
        private bool isEditMode = false;
        private bool isLoading = false;

        public frmGhiDienNuoc()
        {
            InitializeComponent();
            isEditMode = false;
        }

        public frmGhiDienNuoc(int maDienNuoc) : this()
        {
            editingMaDienNuoc = maDienNuoc;
            isEditMode = true;
        }

        private void frmGhiDienNuoc_Load(object sender, EventArgs e)
        {
            isLoading = true;

            LoadDanhSachPhong();

            if (!isEditMode)
            {
                numThang.Value = DateTime.Today.Month;
                numNam.Value = DateTime.Today.Year;
                lblFormTitle.Text = "GHI NHẬN CHỈ SỐ ĐIỆN NƯỚC MỚI";
                lblFormSub.Text = "Nhập chỉ số điện nước theo phòng và tháng để tính lượng tiêu thụ";
                btnLuu.Text = "Lưu chỉ số";
            }
            else
            {
                lblFormTitle.Text = "CẬP NHẬT CHỈ SỐ ĐIỆN NƯỚC";
                lblFormSub.Text = "Điều chỉnh chỉ số điện nước cho bản ghi #" + editingMaDienNuoc;
                btnLuu.Text = "Cập nhật";
                LoadDataForEdit();
            }

            isLoading = false;

            if (!isEditMode)
            {
                AutoFillPreviousReadings();
            }

            CapNhatLuongTieuThu();
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
                        row["DisplayText"] = $"{maPhong} - {tenPhong} ({tenKhu})";
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

        private void LoadDataForEdit()
        {
            try
            {
                DataRow row = dienNuocBLL.GetDienNuocById(editingMaDienNuoc);
                if (row != null)
                {
                    string maPhong = row["MaPhong"].ToString();
                    cmbPhong.SelectedValue = maPhong;
                    cmbPhong.Enabled = false;

                    numThang.Value = Convert.ToInt32(row["Thang"]);
                    numNam.Value = Convert.ToInt32(row["Nam"]);

                    numChiSoDienCu.Value = Convert.ToDecimal(row["ChiSoDienCu"]);
                    numChiSoDienMoi.Value = Convert.ToDecimal(row["ChiSoDienMoi"]);
                    numChiSoNuocCu.Value = Convert.ToDecimal(row["ChiSoNuocCu"]);
                    numChiSoNuocMoi.Value = Convert.ToDecimal(row["ChiSoNuocMoi"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải thông tin bản ghi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PhongOrThangNam_Changed(object sender, EventArgs e)
        {
            if (isLoading || isEditMode) return;
            AutoFillPreviousReadings();
            CapNhatLuongTieuThu();
        }

        private void AutoFillPreviousReadings()
        {
            if (cmbPhong.SelectedValue == null) return;
            string maPhong = cmbPhong.SelectedValue.ToString();
            int thang = (int)numThang.Value;
            int nam = (int)numNam.Value;

            try
            {
                DataRow prev = dienNuocBLL.GetChiSoThangTruoc(maPhong, thang, nam);
                if (prev != null)
                {
                    decimal prevDien = Convert.ToDecimal(prev["ChiSoDienMoi"]);
                    decimal prevNuoc = Convert.ToDecimal(prev["ChiSoNuocMoi"]);
                    int prevThang = Convert.ToInt32(prev["Thang"]);
                    int prevNam = Convert.ToInt32(prev["Nam"]);

                    numChiSoDienCu.Value = prevDien;
                    numChiSoNuocCu.Value = prevNuoc;

                    if (numChiSoDienMoi.Value < prevDien) numChiSoDienMoi.Value = prevDien;
                    if (numChiSoNuocMoi.Value < prevNuoc) numChiSoNuocMoi.Value = prevNuoc;

                    lblGoiY.Text = $"💡 Đã tự động gợi ý chỉ số cuối tháng {prevThang}/{prevNam} (Điện: {prevDien:N0}, Nước: {prevNuoc:N0}) làm chỉ số cũ.";
                    lblGoiY.ForeColor = Color.FromArgb(22, 163, 74);
                }
                else
                {
                    lblGoiY.Text = "💡 Chưa có chỉ số tháng trước cho phòng này. Vui lòng nhập chỉ số ban đầu.";
                    lblGoiY.ForeColor = Color.FromArgb(100, 116, 139);
                }
            }
            catch
            {
            }
        }

        private void numChiSo_ValueChanged(object sender, EventArgs e)
        {
            CapNhatLuongTieuThu();
        }

        private void CapNhatLuongTieuThu()
        {
            decimal dienTieuThu = numChiSoDienMoi.Value - numChiSoDienCu.Value;
            if (dienTieuThu < 0)
            {
                lblDienTieuThu.Text = "Không hợp lệ!";
                lblDienTieuThu.ForeColor = Color.FromArgb(220, 38, 38);
            }
            else
            {
                lblDienTieuThu.Text = $"{dienTieuThu:N0} kWh";
                lblDienTieuThu.ForeColor = Color.FromArgb(217, 119, 6);
            }

            decimal nuocTieuThu = numChiSoNuocMoi.Value - numChiSoNuocCu.Value;
            if (nuocTieuThu < 0)
            {
                lblNuocTieuThu.Text = "Không hợp lệ!";
                lblNuocTieuThu.ForeColor = Color.FromArgb(220, 38, 38);
            }
            else
            {
                lblNuocTieuThu.Text = $"{nuocTieuThu:N0} m³";
                lblNuocTieuThu.ForeColor = Color.FromArgb(2, 132, 199);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cmbPhong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phòng cần ghi chỉ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPhong.Focus();
                return;
            }

            string maPhong = cmbPhong.SelectedValue.ToString();
            int thang = (int)numThang.Value;
            int nam = (int)numNam.Value;
            int dienCu = (int)numChiSoDienCu.Value;
            int dienMoi = (int)numChiSoDienMoi.Value;
            int nuocCu = (int)numChiSoNuocCu.Value;
            int nuocMoi = (int)numChiSoNuocMoi.Value;

            if (dienMoi < dienCu)
            {
                MessageBox.Show($"Chỉ số điện mới ({dienMoi}) không được nhỏ hơn chỉ số điện cũ ({dienCu})!", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numChiSoDienMoi.Focus();
                return;
            }

            if (nuocMoi < nuocCu)
            {
                MessageBox.Show($"Chỉ số nước mới ({nuocMoi}) không được nhỏ hơn chỉ số nước cũ ({nuocCu})!", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numChiSoNuocMoi.Focus();
                return;
            }

            DienNuocDTO dn = new DienNuocDTO
            {
                MaDienNuoc = editingMaDienNuoc,
                MaPhong = maPhong,
                Thang = thang,
                Nam = nam,
                ChiSoDienCu = dienCu,
                ChiSoDienMoi = dienMoi,
                ChiSoNuocCu = nuocCu,
                ChiSoNuocMoi = nuocMoi
            };

            try
            {
                bool success = false;
                if (!isEditMode)
                {
                    success = dienNuocBLL.InsertDienNuoc(dn);
                    if (success)
                    {
                        MessageBox.Show($"Đã ghi nhận thành công chỉ số điện nước phòng {maPhong} tháng {thang}/{nam}!\n- Điện tiêu thụ: {dn.DienTieuThu:N0} kWh\n- Nước tiêu thụ: {dn.NuocTieuThu:N0} m³", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    success = dienNuocBLL.UpdateDienNuoc(dn);
                    if (success)
                    {
                        MessageBox.Show($"Cập nhật chỉ số điện nước phòng {maPhong} tháng {thang}/{nam} thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (success)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

