using System;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class frmChiTietVaiTro : Form
    {
        private readonly VaiTroBLL vaiTroBLL = new VaiTroBLL();
        private readonly VaiTroDTO vtCurrent = null;
        private readonly bool isEditMode = false;

        public string SavedMaVaiTro { get; private set; }

        public frmChiTietVaiTro()
        {
            InitializeComponent();
            isEditMode = false;
            this.Text = "Thêm vai trò mới";
            lblFormTitle.Text = "THÊM VAI TRÒ MỚI";
            lblFormSub.Text = "Khởi tạo vai trò và cấu hình phân quyền ở bảng ma trận bên phải";
        }

        public frmChiTietVaiTro(VaiTroDTO vt) : this()
        {
            this.vtCurrent = vt;
            this.isEditMode = true;

            this.Text = "Chỉnh sửa vai trò - " + vt.MaVaiTro;
            lblFormTitle.Text = "CHỈNH SỬA VAI TRÒ: " + vt.MaVaiTro;
            lblFormSub.Text = "Cập nhật tên hiển thị và mô tả chức năng của vai trò";

            txtMaVaiTro.Text = vt.MaVaiTro;
            txtMaVaiTro.ReadOnly = true;
            txtTenVaiTro.Text = vt.TenVaiTro;
            txtMoTa.Text = vt.MoTa;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isEditMode)
                {
                    VaiTroDTO vtNew = new VaiTroDTO
                    {
                        MaVaiTro = txtMaVaiTro.Text.Trim(),
                        TenVaiTro = txtTenVaiTro.Text.Trim(),
                        MoTa = txtMoTa.Text.Trim()
                    };

                    vaiTroBLL.InsertVaiTro(vtNew);
                    SavedMaVaiTro = vtNew.MaVaiTro;
                    MessageBox.Show("Thêm mới vai trò thành công! Bạn có thể chọn vai trò vừa tạo để phân quyền chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    vtCurrent.TenVaiTro = txtTenVaiTro.Text.Trim();
                    vtCurrent.MoTa = txtMoTa.Text.Trim();

                    vaiTroBLL.UpdateVaiTro(vtCurrent);
                    SavedMaVaiTro = vtCurrent.MaVaiTro;
                    MessageBox.Show("Cập nhật thông tin vai trò thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message, "Cảnh báo phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi kiểm tra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
