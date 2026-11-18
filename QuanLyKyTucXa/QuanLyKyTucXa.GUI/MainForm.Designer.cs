namespace QuanLyKyTucXa.GUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.pnlNavList = new System.Windows.Forms.Panel();
            this.btnHopDong = new QuanLyKyTucXa.GUI.ModernMenuButton();
            this.pnlSpacer3 = new System.Windows.Forms.Panel();
            this.btnPhong = new QuanLyKyTucXa.GUI.ModernMenuButton();
            this.pnlSpacer2 = new System.Windows.Forms.Panel();
            this.btnSinhVien = new QuanLyKyTucXa.GUI.ModernMenuButton();
            this.pnlSpacer1 = new System.Windows.Forms.Panel();
            this.lblMenuCategory = new System.Windows.Forms.Label();
            this.pnlUserSection = new System.Windows.Forms.Panel();
            this.btnDangXuat = new QuanLyKyTucXa.GUI.ModernMenuButton();
            this.pnlUserInfo = new System.Windows.Forms.Panel();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.pnlUserDivider = new System.Windows.Forms.Panel();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblLogoSub = new System.Windows.Forms.Label();
            this.lblLogo = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.btnMinApp = new System.Windows.Forms.Button();
            this.btnMaxApp = new System.Windows.Forms.Button();
            this.btnCloseApp = new System.Windows.Forms.Button();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.pnlMenu.SuspendLayout();
            this.pnlNavList.SuspendLayout();
            this.pnlUserSection.SuspendLayout();
            this.pnlUserInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.pnlMenu.Controls.Add(this.pnlNavList);
            this.pnlMenu.Controls.Add(this.pnlLogo);
            this.pnlMenu.Controls.Add(this.pnlUserSection);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(260, 800);
            this.pnlMenu.TabIndex = 0;
            // 
            // pnlNavList
            // 
            this.pnlNavList.BackColor = System.Drawing.Color.Transparent;
            this.pnlNavList.Controls.Add(this.btnHopDong);
            this.pnlNavList.Controls.Add(this.pnlSpacer3);
            this.pnlNavList.Controls.Add(this.btnPhong);
            this.pnlNavList.Controls.Add(this.pnlSpacer2);
            this.pnlNavList.Controls.Add(this.btnSinhVien);
            this.pnlNavList.Controls.Add(this.pnlSpacer1);
            this.pnlNavList.Controls.Add(this.lblMenuCategory);
            this.pnlNavList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNavList.Location = new System.Drawing.Point(0, 80);
            this.pnlNavList.Name = "pnlNavList";
            this.pnlNavList.Padding = new System.Windows.Forms.Padding(14, 12, 14, 10);
            this.pnlNavList.Size = new System.Drawing.Size(260, 580);
            this.pnlNavList.TabIndex = 1;
            // 
            // btnHopDong
            // 
            this.btnHopDong.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnHopDong.ActiveForeColor = System.Drawing.Color.White;
            this.btnHopDong.BorderRadius = 10;
            this.btnHopDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHopDong.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHopDong.FlatAppearance.BorderSize = 0;
            this.btnHopDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHopDong.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHopDong.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(45)))), ((int)(((byte)(75)))));
            this.btnHopDong.HoverForeColor = System.Drawing.Color.White;
            this.btnHopDong.IsActive = false;
            this.btnHopDong.Location = new System.Drawing.Point(14, 172);
            this.btnHopDong.Name = "btnHopDong";
            this.btnHopDong.NormalBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(38)))), ((int)(((byte)(66)))));
            this.btnHopDong.NormalForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnHopDong.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnHopDong.Size = new System.Drawing.Size(232, 44);
            this.btnHopDong.TabIndex = 5;
            this.btnHopDong.Text = "Quản lý Hợp đồng";
            this.btnHopDong.UseVisualStyleBackColor = true;
            this.btnHopDong.Click += new System.EventHandler(this.btnHopDong_Click);
            // 
            // pnlSpacer3
            // 
            this.pnlSpacer3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacer3.Location = new System.Drawing.Point(14, 164);
            this.pnlSpacer3.Name = "pnlSpacer3";
            this.pnlSpacer3.Size = new System.Drawing.Size(232, 8);
            this.pnlSpacer3.TabIndex = 4;
            // 
            // btnPhong
            // 
            this.btnPhong.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnPhong.ActiveForeColor = System.Drawing.Color.White;
            this.btnPhong.BorderRadius = 10;
            this.btnPhong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPhong.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPhong.FlatAppearance.BorderSize = 0;
            this.btnPhong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPhong.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhong.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(45)))), ((int)(((byte)(75)))));
            this.btnPhong.HoverForeColor = System.Drawing.Color.White;
            this.btnPhong.IsActive = false;
            this.btnPhong.Location = new System.Drawing.Point(14, 120);
            this.btnPhong.Name = "btnPhong";
            this.btnPhong.NormalBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(38)))), ((int)(((byte)(66)))));
            this.btnPhong.NormalForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnPhong.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnPhong.Size = new System.Drawing.Size(232, 44);
            this.btnPhong.TabIndex = 3;
            this.btnPhong.Text = "Quản lý Phòng và Dãy nhà";
            this.btnPhong.UseVisualStyleBackColor = true;
            this.btnPhong.Click += new System.EventHandler(this.btnPhong_Click);
            // 
            // pnlSpacer2
            // 
            this.pnlSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacer2.Location = new System.Drawing.Point(14, 112);
            this.pnlSpacer2.Name = "pnlSpacer2";
            this.pnlSpacer2.Size = new System.Drawing.Size(232, 8);
            this.pnlSpacer2.TabIndex = 2;
            // 
            // btnSinhVien
            // 
            this.btnSinhVien.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnSinhVien.ActiveForeColor = System.Drawing.Color.White;
            this.btnSinhVien.BorderRadius = 10;
            this.btnSinhVien.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSinhVien.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSinhVien.FlatAppearance.BorderSize = 0;
            this.btnSinhVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSinhVien.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSinhVien.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(45)))), ((int)(((byte)(75)))));
            this.btnSinhVien.HoverForeColor = System.Drawing.Color.White;
            this.btnSinhVien.IsActive = true;
            this.btnSinhVien.Location = new System.Drawing.Point(14, 68);
            this.btnSinhVien.Name = "btnSinhVien";
            this.btnSinhVien.NormalBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(38)))), ((int)(((byte)(66)))));
            this.btnSinhVien.NormalForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnSinhVien.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnSinhVien.Size = new System.Drawing.Size(232, 44);
            this.btnSinhVien.TabIndex = 1;
            this.btnSinhVien.Text = "Quản lý Sinh viên";
            this.btnSinhVien.UseVisualStyleBackColor = true;
            this.btnSinhVien.Click += new System.EventHandler(this.btnSinhVien_Click);
            // 
            // pnlSpacer1
            // 
            this.pnlSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacer1.Location = new System.Drawing.Point(14, 60);
            this.pnlSpacer1.Name = "pnlSpacer1";
            this.pnlSpacer1.Size = new System.Drawing.Size(232, 8);
            this.pnlSpacer1.TabIndex = 0;
            // 
            // lblMenuCategory
            // 
            this.lblMenuCategory.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMenuCategory.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblMenuCategory.Location = new System.Drawing.Point(14, 12);
            this.lblMenuCategory.Name = "lblMenuCategory";
            this.lblMenuCategory.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblMenuCategory.Size = new System.Drawing.Size(232, 28);
            this.lblMenuCategory.TabIndex = 4;
            this.lblMenuCategory.Text = "DANH MỤC HỆ THỐNG";
            this.lblMenuCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlUserSection
            // 
            this.pnlUserSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.pnlUserSection.Controls.Add(this.btnDangXuat);
            this.pnlUserSection.Controls.Add(this.pnlUserInfo);
            this.pnlUserSection.Controls.Add(this.pnlUserDivider);
            this.pnlUserSection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUserSection.Location = new System.Drawing.Point(0, 660);
            this.pnlUserSection.Name = "pnlUserSection";
            this.pnlUserSection.Padding = new System.Windows.Forms.Padding(14, 8, 14, 12);
            this.pnlUserSection.Size = new System.Drawing.Size(260, 140);
            this.pnlUserSection.TabIndex = 2;
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnDangXuat.ActiveForeColor = System.Drawing.Color.White;
            this.btnDangXuat.BorderRadius = 10;
            this.btnDangXuat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDangXuat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDangXuat.FlatAppearance.BorderSize = 0;
            this.btnDangXuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDangXuat.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnDangXuat.HoverForeColor = System.Drawing.Color.White;
            this.btnDangXuat.IsActive = false;
            this.btnDangXuat.Location = new System.Drawing.Point(14, 88);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.NormalBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(25)))), ((int)(((byte)(35)))));
            this.btnDangXuat.NormalForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.btnDangXuat.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.btnDangXuat.Size = new System.Drawing.Size(232, 40);
            this.btnDangXuat.TabIndex = 2;
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.UseVisualStyleBackColor = true;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // pnlUserInfo
            // 
            this.pnlUserInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlUserInfo.Controls.Add(this.lblUserRole);
            this.pnlUserInfo.Controls.Add(this.lblUserName);
            this.pnlUserInfo.Controls.Add(this.picAvatar);
            this.pnlUserInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUserInfo.Location = new System.Drawing.Point(14, 9);
            this.pnlUserInfo.Name = "pnlUserInfo";
            this.pnlUserInfo.Size = new System.Drawing.Size(232, 65);
            this.pnlUserInfo.TabIndex = 1;
            // 
            // lblUserRole
            // 
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(211)))), ((int)(((byte)(153)))));
            this.lblUserRole.Location = new System.Drawing.Point(46, 34);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(125, 13);
            this.lblUserRole.TabIndex = 2;
            this.lblUserRole.Text = "● Vai trò: Quản trị viên";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(46, 12);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(89, 17);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "Ban Quản Lý";
            // 
            // picAvatar
            // 
            this.picAvatar.Location = new System.Drawing.Point(2, 10);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(38, 38);
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picAvatar.TabIndex = 0;
            this.picAvatar.TabStop = false;
            // 
            // pnlUserDivider
            // 
            this.pnlUserDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(72)))));
            this.pnlUserDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUserDivider.Location = new System.Drawing.Point(14, 8);
            this.pnlUserDivider.Name = "pnlUserDivider";
            this.pnlUserDivider.Size = new System.Drawing.Size(232, 1);
            this.pnlUserDivider.TabIndex = 0;
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.pnlLogo.Controls.Add(this.lblLogoSub);
            this.pnlLogo.Controls.Add(this.lblLogo);
            this.pnlLogo.Controls.Add(this.picLogo);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(260, 80);
            this.pnlLogo.TabIndex = 0;
            // 
            // lblLogoSub
            // 
            this.lblLogoSub.AutoSize = true;
            this.lblLogoSub.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblLogoSub.Location = new System.Drawing.Point(62, 44);
            this.lblLogoSub.Name = "lblLogoSub";
            this.lblLogoSub.Size = new System.Drawing.Size(109, 12);
            this.lblLogoSub.TabIndex = 2;
            this.lblLogoSub.Text = "HỆ THỐNG QUẢN LÝ";
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(60, 20);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(107, 23);
            this.lblLogo.TabIndex = 1;
            this.lblLogo.Text = "KÝ TÚC XÁ";
            // 
            // picLogo
            // 
            this.picLogo.Location = new System.Drawing.Point(16, 20);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(38, 38);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Controls.Add(this.btnMinApp);
            this.pnlHeader.Controls.Add(this.btnMaxApp);
            this.pnlHeader.Controls.Add(this.btnCloseApp);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(260, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(940, 60);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnCloseApp
            // 
            this.btnCloseApp.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCloseApp.FlatAppearance.BorderSize = 0;
            this.btnCloseApp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnCloseApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseApp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnCloseApp.Location = new System.Drawing.Point(892, 0);
            this.btnCloseApp.Name = "btnCloseApp";
            this.btnCloseApp.Size = new System.Drawing.Size(48, 60);
            this.btnCloseApp.TabIndex = 3;
            this.btnCloseApp.Text = "✕";
            this.btnCloseApp.UseVisualStyleBackColor = true;
            this.btnCloseApp.Click += new System.EventHandler(this.btnCloseApp_Click);
            // 
            // btnMaxApp
            // 
            this.btnMaxApp.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMaxApp.FlatAppearance.BorderSize = 0;
            this.btnMaxApp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnMaxApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaxApp.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaxApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnMaxApp.Location = new System.Drawing.Point(844, 0);
            this.btnMaxApp.Name = "btnMaxApp";
            this.btnMaxApp.Size = new System.Drawing.Size(48, 60);
            this.btnMaxApp.TabIndex = 2;
            this.btnMaxApp.Text = "🗖";
            this.btnMaxApp.UseVisualStyleBackColor = true;
            this.btnMaxApp.Click += new System.EventHandler(this.btnMaxApp_Click);
            // 
            // btnMinApp
            // 
            this.btnMinApp.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinApp.FlatAppearance.BorderSize = 0;
            this.btnMinApp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnMinApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinApp.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnMinApp.Location = new System.Drawing.Point(796, 0);
            this.btnMinApp.Name = "btnMinApp";
            this.btnMinApp.Size = new System.Drawing.Size(48, 60);
            this.btnMinApp.TabIndex = 1;
            this.btnMinApp.Text = "🗕";
            this.btnMinApp.UseVisualStyleBackColor = true;
            this.btnMinApp.Click += new System.EventHandler(this.btnMinApp_Click);
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(0, 0);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblHeaderTitle.Size = new System.Drawing.Size(796, 60);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "BẢNG ĐIỀU KHIỂN";
            this.lblHeaderTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(260, 60);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(940, 740);
            this.pnlContainer.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống Quản lý Ký túc xá";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlMenu.ResumeLayout(false);
            this.pnlNavList.ResumeLayout(false);
            this.pnlUserSection.ResumeLayout(false);
            this.pnlUserInfo.ResumeLayout(false);
            this.pnlUserInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblLogoSub;
        private System.Windows.Forms.Panel pnlNavList;
        private System.Windows.Forms.Label lblMenuCategory;
        private System.Windows.Forms.Panel pnlSpacer1;
        private System.Windows.Forms.Panel pnlSpacer2;
        private System.Windows.Forms.Panel pnlSpacer3;
        private QuanLyKyTucXa.GUI.ModernMenuButton btnSinhVien;
        private QuanLyKyTucXa.GUI.ModernMenuButton btnPhong;
        private QuanLyKyTucXa.GUI.ModernMenuButton btnHopDong;
        private System.Windows.Forms.Panel pnlUserSection;
        private System.Windows.Forms.Panel pnlUserDivider;
        private System.Windows.Forms.Panel pnlUserInfo;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserRole;
        private QuanLyKyTucXa.GUI.ModernMenuButton btnDangXuat;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Button btnMinApp;
        private System.Windows.Forms.Button btnMaxApp;
        private System.Windows.Forms.Button btnCloseApp;
    }
}
