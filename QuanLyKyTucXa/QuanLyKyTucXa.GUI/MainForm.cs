using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyKyTucXa.GUI
{
    public partial class MainForm : Form
    {
        // Windows API để kéo rê form khi FormBorderStyle = None
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private Form activeForm = null;
        private UCSinhVien ucSinhVien;
        private UCPhong ucPhong;
        private UCHopDong ucHopDong;
        private UCDichVuTaiChinh ucDichVuTaiChinh;
        private UCThongKe ucThongKe;
        private UCTaiKhoan ucTaiKhoan;

        public MainForm()
        {
            InitializeComponent();
            SetupCustomIcons();
            this.pnlHeader.MouseDown += PnlHeader_MouseDown;
            this.lblHeaderTitle.MouseDown += PnlHeader_MouseDown;

            // Đăng ký sự kiện nhấp vào hồ sơ để đổi mật khẩu cá nhân
            this.pnlUserInfo.Cursor = Cursors.Hand;
            this.picAvatar.Cursor = Cursors.Hand;
            this.lblUserName.Cursor = Cursors.Hand;
            this.lblUserRole.Cursor = Cursors.Hand;
            this.pnlUserInfo.Click += PnlUserInfo_Click;
            this.picAvatar.Click += PnlUserInfo_Click;
            this.lblUserName.Click += PnlUserInfo_Click;
            this.lblUserRole.Click += PnlUserInfo_Click;
        }

        /// <summary>
        /// Khởi tạo các Vector Icon GDI+ siêu nét, hoàn toàn không phụ thuộc font chữ emoji
        /// </summary>
        private void SetupCustomIcons()
        {
            this.picLogo.Image = IconHelper.CreateDormLogoIcon(Color.FromArgb(96, 165, 250), 30);
            this.picAvatar.Image = IconHelper.CreateUserAvatarIcon(Color.FromArgb(37, 99, 235), Color.White, 36);

            this.btnSinhVien.Image = IconHelper.CreateStudentIcon(Color.White, 20);
            this.btnPhong.Image = IconHelper.CreateRoomIcon(Color.FromArgb(203, 213, 225), 20);
            this.btnHopDong.Image = IconHelper.CreateContractIcon(Color.FromArgb(203, 213, 225), 20);
            this.btnDichVuTaiChinh.Image = IconHelper.CreateFinanceIcon(Color.FromArgb(203, 213, 225), 20);
            this.btnThongKe.Image = IconHelper.CreateChartIcon(Color.FromArgb(203, 213, 225), 20);
            this.btnTaiKhoan.Image = IconHelper.CreateAccountIcon(Color.FromArgb(203, 213, 225), 20);
            this.btnDoiMatKhau.Image = IconHelper.CreateKeyIcon(Color.FromArgb(203, 213, 225), 18);
            this.btnDangXuat.Image = IconHelper.CreateLogoutIcon(Color.FromArgb(248, 113, 113), 18);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin người dùng đang đăng nhập
            if (QuanLyKyTucXa.DTO.UserSession.IsLoggedIn)
            {
                this.lblUserName.Text = QuanLyKyTucXa.DTO.UserSession.CurrentUser.HoTen;
                this.lblUserRole.Text = "● Vai trò: " + QuanLyKyTucXa.DTO.UserSession.CurrentUser.VaiTroHienThi;
            }

            // Phân quyền giao diện theo ma trận quyền của vai trò đang đăng nhập
            bool canViewSinhVien = QuanLyKyTucXa.DTO.UserSession.CanView(QuanLyKyTucXa.DTO.ModuleSystem.SinhVien);
            bool canViewPhong = QuanLyKyTucXa.DTO.UserSession.CanView(QuanLyKyTucXa.DTO.ModuleSystem.Phong);
            bool canViewHopDong = QuanLyKyTucXa.DTO.UserSession.CanView(QuanLyKyTucXa.DTO.ModuleSystem.HopDong);
            bool canViewDichVu = QuanLyKyTucXa.DTO.UserSession.CanView(QuanLyKyTucXa.DTO.ModuleSystem.DichVuTaiChinh);
            bool canViewThongKe = QuanLyKyTucXa.DTO.UserSession.CanView(QuanLyKyTucXa.DTO.ModuleSystem.ThongKe);
            bool canViewTaiKhoan = QuanLyKyTucXa.DTO.UserSession.CanView(QuanLyKyTucXa.DTO.ModuleSystem.TaiKhoan);

            this.btnSinhVien.Visible = canViewSinhVien;
            this.pnlSpacer1.Visible = canViewSinhVien;
            this.btnPhong.Visible = canViewPhong;
            this.pnlSpacer2.Visible = canViewPhong;
            this.btnHopDong.Visible = canViewHopDong;
            this.pnlSpacer3.Visible = canViewHopDong;
            this.btnDichVuTaiChinh.Visible = canViewDichVu;
            this.pnlSpacer4.Visible = canViewDichVu;
            this.btnThongKe.Visible = canViewThongKe;
            this.pnlSpacer6.Visible = canViewThongKe;
            this.btnTaiKhoan.Visible = canViewTaiKhoan;
            this.pnlSpacer5.Visible = canViewTaiKhoan;

            // Tự động mở màn hình đầu tiên mà tài khoản có quyền xem
            if (canViewSinhVien)
                btnSinhVien_Click(this.btnSinhVien, EventArgs.Empty);
            else if (canViewPhong)
                btnPhong_Click(this.btnPhong, EventArgs.Empty);
            else if (canViewHopDong)
                btnHopDong_Click(this.btnHopDong, EventArgs.Empty);
            else if (canViewDichVu)
                btnDichVuTaiChinh_Click(this.btnDichVuTaiChinh, EventArgs.Empty);
            else if (canViewThongKe)
                btnThongKe_Click(this.btnThongKe, EventArgs.Empty);
            else if (canViewTaiKhoan)
                btnTaiKhoan_Click(this.btnTaiKhoan, EventArgs.Empty);
            else
            {
                pnlContainer.Controls.Clear();
                lblHeaderTitle.Text = "Không có quyền truy cập";
                Label lblNoPerm = new Label
                {
                    Text = "Tài khoản của bạn chưa được cấp quyền xem module nào.\nVui lòng liên hệ Quản trị viên để được phân quyền.",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(100, 116, 139)
                };
                pnlContainer.Controls.Add(lblNoPerm);
            }
        }

        private void PnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);
            }
        }

        /// <summary>
        /// Mở Form con (Child Form) và nhúng vào pnlContainer theo chuẩn Modern Flat Design
        /// </summary>
        public void OpenChildForm(Form childForm, string pageTitle = "")
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm.Dispose();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add(childForm);
            pnlContainer.Tag = childForm;

            if (!string.IsNullOrEmpty(pageTitle))
            {
                lblHeaderTitle.Text = pageTitle;
            }

            childForm.BringToFront();
            childForm.Show();
        }

        /// <summary>
        /// Nạp UserControl vào pnlContainer và tự động làm mới dữ liệu khi chuyển qua lại
        /// </summary>
        public void LoadUserControl(UserControl uc, string pageTitle = "")
        {
            pnlContainer.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(uc);

            if (!string.IsNullOrEmpty(pageTitle))
            {
                lblHeaderTitle.Text = pageTitle;
            }

            uc.BringToFront();
        }

        /// <summary>
        /// Chuyển đổi trạng thái Active/Inactive cho các nút menu chính
        /// </summary>
        private void SetActiveButton(ModernMenuButton selectedBtn)
        {
            Color inactiveColor = Color.FromArgb(203, 213, 225);

            btnSinhVien.IsActive = false;
            btnSinhVien.Image = IconHelper.CreateStudentIcon(inactiveColor, 20);

            btnPhong.IsActive = false;
            btnPhong.Image = IconHelper.CreateRoomIcon(inactiveColor, 20);

            btnHopDong.IsActive = false;
            btnHopDong.Image = IconHelper.CreateContractIcon(inactiveColor, 20);

            btnDichVuTaiChinh.IsActive = false;
            btnDichVuTaiChinh.Image = IconHelper.CreateFinanceIcon(inactiveColor, 20);

            btnThongKe.IsActive = false;
            btnThongKe.Image = IconHelper.CreateChartIcon(inactiveColor, 20);

            btnTaiKhoan.IsActive = false;
            btnTaiKhoan.Image = IconHelper.CreateAccountIcon(inactiveColor, 20);

            selectedBtn.IsActive = true;
            if (selectedBtn == btnSinhVien)
                btnSinhVien.Image = IconHelper.CreateStudentIcon(Color.White, 20);
            else if (selectedBtn == btnPhong)
                btnPhong.Image = IconHelper.CreateRoomIcon(Color.White, 20);
            else if (selectedBtn == btnHopDong)
                btnHopDong.Image = IconHelper.CreateContractIcon(Color.White, 20);
            else if (selectedBtn == btnDichVuTaiChinh)
                btnDichVuTaiChinh.Image = IconHelper.CreateFinanceIcon(Color.White, 20);
            else if (selectedBtn == btnThongKe)
                btnThongKe.Image = IconHelper.CreateChartIcon(Color.White, 20);
            else if (selectedBtn == btnTaiKhoan)
                btnTaiKhoan.Image = IconHelper.CreateAccountIcon(Color.White, 20);
        }

        private void btnSinhVien_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSinhVien);
            bool isFirstLoad = ucSinhVien == null;
            if (isFirstLoad) ucSinhVien = new UCSinhVien();

            LoadUserControl(ucSinhVien, "Quản lý Sinh viên");

            // Tự động làm mới dữ liệu khi chuyển từ màn hình khác quay lại
            if (!isFirstLoad) ucSinhVien.RefreshAllData();
        }

        private void btnPhong_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnPhong);
            bool isFirstLoad = ucPhong == null;
            if (isFirstLoad) ucPhong = new UCPhong();

            LoadUserControl(ucPhong, "Quản lý Phòng và Dãy nhà");

            // Tự động làm mới dữ liệu khi chuyển từ màn hình khác quay lại
            if (!isFirstLoad) ucPhong.RefreshAllData();
        }

        private void btnHopDong_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnHopDong);
            bool isFirstLoad = ucHopDong == null;
            if (isFirstLoad) ucHopDong = new UCHopDong();

            LoadUserControl(ucHopDong, "Quản lý Hợp đồng");

            // Tự động làm mới dữ liệu khi chuyển từ màn hình khác quay lại
            if (!isFirstLoad) ucHopDong.RefreshAllData();
        }

        private void btnDichVuTaiChinh_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDichVuTaiChinh);
            bool isFirstLoad = ucDichVuTaiChinh == null;
            if (isFirstLoad) ucDichVuTaiChinh = new UCDichVuTaiChinh();

            LoadUserControl(ucDichVuTaiChinh, "Quản lý Dịch vụ & Tài chính");

            // Tự động làm mới dữ liệu khi chuyển từ màn hình khác quay lại
            if (!isFirstLoad) ucDichVuTaiChinh.RefreshAllData();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (!QuanLyKyTucXa.DTO.UserSession.CanView(QuanLyKyTucXa.DTO.ModuleSystem.ThongKe))
            {
                MessageBox.Show("Bạn không có quyền truy cập module Thống kê & Báo cáo!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetActiveButton(btnThongKe);
            bool isFirstLoad = ucThongKe == null;
            if (isFirstLoad) ucThongKe = new UCThongKe();

            LoadUserControl(ucThongKe, "Quản lý Thống kê & Báo cáo");

            if (!isFirstLoad) ucThongKe.RefreshAllData();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            // Kiểm tra quyền nghiệp vụ
            if (!QuanLyKyTucXa.DTO.UserSession.CanView(QuanLyKyTucXa.DTO.ModuleSystem.TaiKhoan))
            {
                MessageBox.Show("Bạn không có quyền truy cập module Quản lý tài khoản & phân quyền!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetActiveButton(btnTaiKhoan);
            bool isFirstLoad = ucTaiKhoan == null;
            if (isFirstLoad) ucTaiKhoan = new UCTaiKhoan();

            LoadUserControl(ucTaiKhoan, "Quản lý Tài khoản & Phân quyền");

            if (!isFirstLoad) ucTaiKhoan.RefreshAllData();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            using (frmDoiMatKhau frm = new frmDoiMatKhau())
            {
                frm.ShowDialog();
            }
        }

        private void PnlUserInfo_Click(object sender, EventArgs e)
        {
            using (frmDoiMatKhau frm = new frmDoiMatKhau())
            {
                frm.ShowDialog();
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                QuanLyKyTucXa.DTO.UserSession.Logout();
                this.DialogResult = DialogResult.Retry; // Báo hiệu cho Program.cs hiển thị lại form Đăng nhập
                this.Close();
            }
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaxApp_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                btnMaxApp.Text = "🗗";
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                btnMaxApp.Text = "🗖";
            }
        }

        private void btnMinApp_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pnlContainer_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pnlMenu_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
