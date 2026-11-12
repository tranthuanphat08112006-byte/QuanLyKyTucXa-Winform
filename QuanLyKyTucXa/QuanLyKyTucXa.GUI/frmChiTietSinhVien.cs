using System;
using System.Windows.Forms;
using QuanLyKyTucXa.DTO;
using QuanLyKyTucXa.BLL;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmChiTietSinhVien : Form
    {
        private SinhVienBLL svBLL = new SinhVienBLL();
        private SinhVien svCurrent = null; // Biến lưu sinh viên đang được sửa (null = Thêm mới)
        private bool isReadOnly = false;

        // Constructor mặc định (Dùng cho Thêm mới)
        public frmChiTietSinhVien()
        {
            InitializeComponent();
            this.Text = "Thêm mới Sinh viên";
            lblFormTitle.Text = "THÊM HỒ SƠ SINH VIÊN MỚI";
            lblFormSub.Text = "Nhập đầy đủ thông tin cá nhân và giấy tờ định danh của sinh viên";
            if (cmbGioiTinh.Items.Count > 0) cmbGioiTinh.SelectedIndex = 0;
        }

        // Constructor overload (Dùng cho Chức năng Sửa hoặc Xem chi tiết)
        public frmChiTietSinhVien(SinhVien sv, bool readOnlyMode = false) : this()
        {
            this.svCurrent = sv;
            this.isReadOnly = readOnlyMode;

            // Đổ dữ liệu cũ lên các ô nhập liệu
            txtMaSV.Text = sv.MaSV;
            txtMaSV.Enabled = false; // Khóa ô Mã SV không cho sửa (Khóa chính)
            txtHoTen.Text = sv.HoTen;
            dtpNgaySinh.Value = sv.NgaySinh;
            txtCCCD.Text = sv.CCCD;
            txtSDT.Text = sv.SDT;
            cmbGioiTinh.Text = sv.GioiTinh;

            if (readOnlyMode)
            {
                this.Text = "Chi tiết hồ sơ Sinh viên - " + sv.MaSV;
                lblFormTitle.Text = "HỒ SƠ SINH VIÊN: " + sv.MaSV;
                lblFormSub.Text = "Chế độ xem thông tin chi tiết hồ sơ sinh viên";

                txtHoTen.ReadOnly = true;
                dtpNgaySinh.Enabled = false;
                cmbGioiTinh.Enabled = false;
                txtCCCD.ReadOnly = true;
                txtSDT.ReadOnly = true;

                btnLuu.Visible = false;
                btnHuy.Text = "Đóng";
            }
            else
            {
                this.Text = "Cập nhật thông tin Sinh viên - " + sv.MaSV;
                lblFormTitle.Text = "CẬP NHẬT HỒ SƠ: " + sv.MaSV;
                lblFormSub.Text = "Chỉnh sửa thông tin cá nhân và số liên lạc của sinh viên";
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (isReadOnly) return;

            try
            {
                SinhVien sv = new SinhVien(
                    txtMaSV.Text.Trim(),
                    txtHoTen.Text.Trim(),
                    dtpNgaySinh.Value,
                    txtCCCD.Text.Trim(),
                    txtSDT.Text.Trim(),
                    cmbGioiTinh.Text.Trim()
                );

                bool isSuccess = false;

                if (svCurrent == null)
                {
                    // Chế độ Thêm mới
                    isSuccess = svBLL.InsertSinhVien(sv);
                }
                else
                {
                    // Chế độ Cập nhật (Sửa)
                    isSuccess = svBLL.UpdateSinhVien(sv);
                }

                if (isSuccess)
                {
                    MessageBox.Show("Lưu thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
