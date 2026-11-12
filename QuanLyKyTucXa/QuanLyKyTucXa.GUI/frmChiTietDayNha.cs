using System;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmChiTietDayNha : Form
    {
        private PhongBLL phongBLL = new PhongBLL();
        private DayNha dayNhaCurrent = null;

        public frmChiTietDayNha()
        {
            InitializeComponent();
            this.Text = "Thêm mới Dãy nhà (Khu)";
            lblFormTitle.Text = "THÊM DÃY NHÀ MỚI";
            lblFormSub.Text = "Thiết lập khu nhà / dãy nhà trực thuộc ký túc xá";

            if (cmbLoaiKhu.Items.Count > 0) cmbLoaiKhu.SelectedIndex = 0;
            if (cmbTrangThai.Items.Count > 0) cmbTrangThai.SelectedIndex = 0;
        }

        public frmChiTietDayNha(DayNha d) : this()
        {
            this.dayNhaCurrent = d;
            this.Text = "Cập nhật Dãy nhà - " + d.TenKhu;
            lblFormTitle.Text = "CẬP NHẬT DÃY NHÀ: " + d.MaKhu;
            lblFormSub.Text = "Chỉnh sửa thông tin dãy nhà và trạng thái vận hành";

            txtMaKhu.Text = d.MaKhu;
            txtMaKhu.Enabled = false;
            txtTenKhu.Text = d.TenKhu;
            cmbLoaiKhu.Text = d.LoaiKhu;
            cmbTrangThai.Text = d.TrangThai;
            txtMoTa.Text = d.MoTa;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                DayNha d = new DayNha(
                    txtMaKhu.Text.Trim(),
                    txtTenKhu.Text.Trim(),
                    cmbLoaiKhu.Text.Trim(),
                    txtMoTa.Text.Trim(),
                    cmbTrangThai.Text.Trim()
                );

                bool isSuccess = false;
                if (dayNhaCurrent == null)
                {
                    isSuccess = phongBLL.InsertDayNha(d);
                }
                else
                {
                    isSuccess = phongBLL.UpdateDayNha(d, dayNhaCurrent.TenKhu);
                }

                if (isSuccess)
                {
                    MessageBox.Show("Lưu thông tin dãy nhà thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
