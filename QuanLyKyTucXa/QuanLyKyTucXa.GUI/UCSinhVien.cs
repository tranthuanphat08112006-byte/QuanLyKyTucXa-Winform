using System;
using System.ComponentModel;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class UCSinhVien : UserControl
    {
        // Khởi tạo BLL
        private SinhVienBLL svBLL = new SinhVienBLL();

        public UCSinhVien()
        {
            InitializeComponent();
        }

        // Sự kiện khi form vừa mở lên
        private void UCSinhVien_Load(object sender, EventArgs e)
        {
            // Bỏ qua load dữ liệu khi đang ở chế độ Designer của Visual Studio
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // 1. Định dạng bảng cho đẹp
            UIHelper.FormatModernDataGridView(dgvSinhVien);

            // 2. Tải dữ liệu
            LoadDanhSachSinhVien();
        }

        // Hàm tải dữ liệu (Tách riêng để tái sử dụng khi thêm/sửa/xóa xong)
        private void LoadDanhSachSinhVien()
        {
            try
            {
                // Gọi BLL và đổ dữ liệu thẳng vào bảng
                dgvSinhVien.DataSource = svBLL.GetAllSinhVien();

                // Đổi lại tên cột hiển thị tiếng Việt an toàn
                if (dgvSinhVien.Columns.Count > 0)
                {
                    if (dgvSinhVien.Columns.Contains("MaSV")) dgvSinhVien.Columns["MaSV"].HeaderText = "Mã SV";
                    if (dgvSinhVien.Columns.Contains("HoTen")) dgvSinhVien.Columns["HoTen"].HeaderText = "Họ Tên";
                    if (dgvSinhVien.Columns.Contains("NgaySinh")) dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                    if (dgvSinhVien.Columns.Contains("CCCD")) dgvSinhVien.Columns["CCCD"].HeaderText = "Số CCCD";
                    if (dgvSinhVien.Columns.Contains("SDT")) dgvSinhVien.Columns["SDT"].HeaderText = "Điện Thoại";
                    if (dgvSinhVien.Columns.Contains("GioiTinh")) dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới Tính";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            frmChiTietSinhVien frm = new frmChiTietSinhVien();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachSinhVien();
            }
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng có đang chọn dòng nào trên bảng không
            if (dgvSinhVien.SelectedRows.Count > 0 || dgvSinhVien.CurrentRow != null)
            {
                DataGridViewRow row = dgvSinhVien.SelectedRows.Count > 0 ? dgvSinhVien.SelectedRows[0] : dgvSinhVien.CurrentRow;

                string maSV = row.Cells["MaSV"].Value?.ToString() ?? "";
                string hoTen = row.Cells["HoTen"].Value?.ToString() ?? "";
                DateTime ngaySinh = DateTime.Now;
                if (row.Cells["NgaySinh"].Value != null && row.Cells["NgaySinh"].Value != DBNull.Value)
                {
                    DateTime.TryParse(row.Cells["NgaySinh"].Value.ToString(), out ngaySinh);
                }
                string cccd = row.Cells["CCCD"].Value?.ToString() ?? "";
                string sdt = row.Cells["SDT"].Value?.ToString() ?? "";
                string gioiTinh = row.Cells["GioiTinh"].Value?.ToString() ?? "Nam";

                SinhVien svSelected = new SinhVien(maSV, hoTen, ngaySinh, cccd, sdt, gioiTinh);

                frmChiTietSinhVien frm = new frmChiTietSinhVien(svSelected);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadDanhSachSinhVien();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên trên bảng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count > 0 || dgvSinhVien.CurrentRow != null)
            {
                DataGridViewRow row = dgvSinhVien.SelectedRows.Count > 0 ? dgvSinhVien.SelectedRows[0] : dgvSinhVien.CurrentRow;
                string maSV = row.Cells["MaSV"].Value?.ToString() ?? "";
                string hoTen = row.Cells["HoTen"].Value?.ToString() ?? "";

                if (string.IsNullOrEmpty(maSV))
                {
                    MessageBox.Show("Vui lòng chọn dòng sinh viên hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sinh viên {hoTen} ({maSV}) không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        if (svBLL.DeleteSinhVien(maSV))
                        {
                            MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDanhSachSinhVien();
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
                MessageBox.Show("Vui lòng chọn một sinh viên trên bảng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    LoadDanhSachSinhVien();
                }
                else
                {
                    dgvSinhVien.DataSource = svBLL.SearchSinhVien(keyword);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}