using System;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL; // Nhận diện tầng BLL
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
            // 1. Định dạng bảng cho đẹp (Hàm hôm trước đã viết)
            UIHelper.FormatModernDataGridView(dgvSinhVien);

            // 2. Tải dữ liệu
            LoadDanhSachSinhVien();
        }

        // Hàm tải dữ liệu (Tách riêng để tái sử dụng khi thêm/sửa/xóa xong)
        private void LoadDanhSachSinhVien()
        {
            try
            {
                // Gọi BLL và đổ dữ liệu thẳng vào bảng. Chỉ 1 dòng code duy nhất!
                dgvSinhVien.DataSource = svBLL.GetAllSinhVien();

                // Đổi lại tên cột hiển thị tiếng Việt cho thân thiện với người dùng
                if (dgvSinhVien.Columns.Count > 0)
                {
                    dgvSinhVien.Columns["MaSV"].HeaderText = "Mã SV";
                    dgvSinhVien.Columns["HoTen"].HeaderText = "Họ Tên";
                    dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                    dgvSinhVien.Columns["CCCD"].HeaderText = "Số CCCD";
                    dgvSinhVien.Columns["SDT"].HeaderText = "Điện Thoại";
                    dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới Tính";
                }
            }
            catch (Exception ex)
            {
                // Bọc try-catch để nếu SQL Server chưa bật, phần mềm sẽ báo lỗi lịch sự chứ không bị văng (crash)
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            frmChiTietSinhVien frm = new frmChiTietSinhVien();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Thay vì hiển thị câu thông báo, giờ ta gọi lệnh tải lại danh sách
                LoadDanhSachSinhVien();
            }
        }
        private void btn_Sua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng có đang chọn dòng nào trên bảng không
            if (dgvSinhVien.SelectedRows.Count > 0)
            {
                // Lấy dữ liệu từ dòng đang được chọn (không dùng e.RowIndex nữa)
                DataGridViewRow row = dgvSinhVien.SelectedRows[0];

                SinhVien svSelected = new SinhVien(
                    row.Cells["MaSV"].Value.ToString(),
                    row.Cells["HoTen"].Value.ToString(),
                    Convert.ToDateTime(row.Cells["NgaySinh"].Value),
                    row.Cells["CCCD"].Value.ToString(),
                    row.Cells["SDT"].Value.ToString(),
                    row.Cells["GioiTinh"].Value.ToString()
                );

                // Truyền đối tượng svSelected sang Form chi tiết
                frmChiTietSinhVien frm = new frmChiTietSinhVien(svSelected);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadDanhSachSinhVien(); // Tải lại bảng sau khi Sửa thành công
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên trên bảng để sửa!", "Thông báo");
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count > 0)
            {
                // Lấy Mã SV của dòng đang được chọn
                string maSV = dgvSinhVien.SelectedRows[0].Cells["MaSV"].Value.ToString();
                string hoTen = dgvSinhVien.SelectedRows[0].Cells["HoTen"].Value.ToString();

                // Hỏi xác nhận trước khi xóa
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
                            MessageBox.Show("Xóa sinh viên thành công!", "Thông báo");
                            LoadDanhSachSinhVien(); // Tải lại bảng sau khi Xóa
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
                MessageBox.Show("Vui lòng chọn một sinh viên trên bảng để xóa!", "Thông báo");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    // Nếu ô tìm kiếm trống, tải lại toàn bộ danh sách
                    LoadDanhSachSinhVien();
                }
                else
                {
                    // Nếu có từ khóa, gọi hàm tìm kiếm và đổ lại vào DataGridView
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