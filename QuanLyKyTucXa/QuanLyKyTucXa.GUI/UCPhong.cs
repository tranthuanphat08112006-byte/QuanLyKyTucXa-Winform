using System;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class UCPhong : UserControl
    {
        private PhongBLL phongBLL = new PhongBLL();

        public UCPhong()
        {
            InitializeComponent();
        }

        private void UCPhong_Load(object sender, EventArgs e)
        {
            // 1. Định dạng bảng cho đẹp
            UIHelper.FormatModernDataGridView(dgvPhong);

            // 2. Tải dữ liệu danh sách phòng
            LoadDanhSachPhong();
        }

        private void LoadDanhSachPhong()
        {
            try
            {
                dgvPhong.DataSource = phongBLL.GetAllPhong();

                if (dgvPhong.Columns.Count > 0)
                {
                    dgvPhong.Columns["MaPhong"].HeaderText = "Mã Phòng";
                    dgvPhong.Columns["TenKhu"].HeaderText = "Dãy Nhà (Khu)";
                    dgvPhong.Columns["LoaiPhong"].HeaderText = "Loại Phòng";
                    dgvPhong.Columns["SoGiuongToiDa"].HeaderText = "Sức Chứa (Giường)";
                    dgvPhong.Columns["SoGiuongTrong"].HeaderText = "Giường Trống";
                    dgvPhong.Columns["DonGia"].HeaderText = "Đơn Giá (VNĐ/Tháng)";
                    dgvPhong.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            frmChiTietPhong frm = new frmChiTietPhong();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachPhong();
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            if (dgvPhong.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPhong.SelectedRows[0];

                Phong pSelected = new Phong(
                    row.Cells["MaPhong"].Value.ToString(),
                    row.Cells["TenKhu"].Value.ToString(),
                    row.Cells["LoaiPhong"].Value.ToString(),
                    Convert.ToInt32(row.Cells["SoGiuongToiDa"].Value),
                    Convert.ToInt32(row.Cells["SoGiuongTrong"].Value),
                    Convert.ToDecimal(row.Cells["DonGia"].Value)
                );

                frmChiTietPhong frm = new frmChiTietPhong(pSelected);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadDanhSachPhong();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng trên bảng để sửa!", "Thông báo");
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (dgvPhong.SelectedRows.Count > 0)
            {
                string maPhong = dgvPhong.SelectedRows[0].Cells["MaPhong"].Value.ToString();
                string tenKhu = dgvPhong.SelectedRows[0].Cells["TenKhu"].Value.ToString();

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa phòng {maPhong} ({tenKhu}) không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        if (phongBLL.DeletePhong(maPhong))
                        {
                            MessageBox.Show("Xóa phòng thành công!", "Thông báo");
                            LoadDanhSachPhong();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng trên bảng để xóa!", "Thông báo");
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    LoadDanhSachPhong();
                }
                else
                {
                    dgvPhong.DataSource = phongBLL.SearchPhong(keyword);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
