using System;
using System.Windows.Forms;
using QuanLyKyTucXa.DTO;
using QuanLyKyTucXa.BLL;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmChiTietPhong : Form
    {
        private PhongBLL phongBLL = new PhongBLL();
        private Phong phongCurrent = null; // Biến lưu phòng đang được sửa (null = Thêm mới)

        // Constructor mặc định (Dùng cho Thêm mới)
        public frmChiTietPhong()
        {
            InitializeComponent();
            this.Text = "Thêm mới Phòng & Dãy nhà";
            nudSoGiuongToiDa.Value = 8;
            nudSoGiuongTrong.Value = 8;
            if (cmbTenKhu.Items.Count > 0) cmbTenKhu.SelectedIndex = 0;
            if (cmbLoaiPhong.Items.Count > 0) cmbLoaiPhong.SelectedIndex = 0;
        }

        // Constructor overload (Dùng cho Chức năng Sửa - nhận đối tượng Phong từ UCPhong truyền sang)
        public frmChiTietPhong(Phong p) : this()
        {
            this.phongCurrent = p;
            this.Text = "Cập nhật thông tin Phòng (Thiết lập giá, sức chứa)";

            // Đổ dữ liệu cũ lên các ô nhập liệu
            txtMaPhong.Text = p.MaPhong;
            txtMaPhong.Enabled = false; // Khóa ô Mã Phòng không cho sửa (Khóa chính)
            cmbTenKhu.Text = p.TenKhu;
            cmbLoaiPhong.Text = p.LoaiPhong;
            nudSoGiuongToiDa.Value = p.SoGiuongToiDa;
            nudSoGiuongTrong.Value = p.SoGiuongTrong;
            txtDonGia.Text = p.DonGia.ToString("0");
        }

        private void nudSoGiuongToiDa_ValueChanged(object sender, EventArgs e)
        {
            // Khi thêm mới phòng, mặc định số giường trống bằng sức chứa tối đa
            if (phongCurrent == null)
            {
                nudSoGiuongTrong.Value = nudSoGiuongToiDa.Value;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                decimal donGia = 0;
                if (!decimal.TryParse(txtDonGia.Text.Trim(), out donGia))
                {
                    MessageBox.Show("Đơn giá phải là số hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Phong p = new Phong(
                    txtMaPhong.Text.Trim(),
                    cmbTenKhu.Text.Trim(),
                    cmbLoaiPhong.Text.Trim(),
                    (int)nudSoGiuongToiDa.Value,
                    (int)nudSoGiuongTrong.Value,
                    donGia
                );

                bool isSuccess = false;

                if (phongCurrent == null)
                {
                    // Chế độ Thêm mới
                    isSuccess = phongBLL.InsertPhong(p);
                }
                else
                {
                    // Chế độ Cập nhật (Sửa)
                    isSuccess = phongBLL.UpdatePhong(p);
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
