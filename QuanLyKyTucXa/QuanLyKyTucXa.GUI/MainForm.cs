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

        public MainForm()
        {
            InitializeComponent();
            SetupCustomIcons();
            this.pnlHeader.MouseDown += PnlHeader_MouseDown;
            this.lblHeaderTitle.MouseDown += PnlHeader_MouseDown;
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
            this.btnDangXuat.Image = IconHelper.CreateLogoutIcon(Color.FromArgb(248, 113, 113), 18);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Mở mặc định màn hình Quản lý Sinh viên
            btnSinhVien_Click(this.btnSinhVien, EventArgs.Empty);
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
        /// Chuyển đổi trạng thái Active/Inactive cho 3 nút menu chính
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

            selectedBtn.IsActive = true;
            if (selectedBtn == btnSinhVien)
                btnSinhVien.Image = IconHelper.CreateStudentIcon(Color.White, 20);
            else if (selectedBtn == btnPhong)
                btnPhong.Image = IconHelper.CreateRoomIcon(Color.White, 20);
            else if (selectedBtn == btnHopDong)
                btnHopDong.Image = IconHelper.CreateContractIcon(Color.White, 20);
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
