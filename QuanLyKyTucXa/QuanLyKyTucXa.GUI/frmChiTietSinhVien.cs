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

        // Constructor mặc định (Dùng cho Thêm mới)
        public frmChiTietSinhVien()
        {
            InitializeComponent();
            this.Text = "Thêm mới Sinh viên";
        }

        // Constructor overload (Dùng cho Chức năng Sửa - nhận đối tượng SinhVien từ UCSinhVien truyền sang)
        public frmChiTietSinhVien(SinhVien sv) : this()
        {
            this.svCurrent = sv;
            this.Text = "Cập nhật thông tin Sinh viên";

            // Đổ dữ liệu cũ lên các ô nhập liệu
            txtMaSV.Text = sv.MaSV;
            txtMaSV.Enabled = false; // Khóa ô Mã SV không cho sửa (Khóa chính)
            txtHoTen.Text = sv.HoTen;
            dtpNgaySinh.Value = sv.NgaySinh;
            txtCCCD.Text = sv.CCCD;
            txtSDT.Text = sv.SDT;
            cmbGioiTinh.Text = sv.GioiTinh;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
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
                    MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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