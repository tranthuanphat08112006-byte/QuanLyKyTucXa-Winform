namespace QuanLyKyTucXa.GUI
{
    partial class frmGhiDienNuoc
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblFormSub = new System.Windows.Forms.Label();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.lblGoiY = new System.Windows.Forms.Label();
            this.grpNuoc = new System.Windows.Forms.GroupBox();
            this.lblNuocTieuThu = new System.Windows.Forms.Label();
            this.lblNuocTieuThuLabel = new System.Windows.Forms.Label();
            this.numChiSoNuocMoi = new System.Windows.Forms.NumericUpDown();
            this.lblChiSoNuocMoi = new System.Windows.Forms.Label();
            this.numChiSoNuocCu = new System.Windows.Forms.NumericUpDown();
            this.lblChiSoNuocCu = new System.Windows.Forms.Label();
            this.grpDien = new System.Windows.Forms.GroupBox();
            this.lblDienTieuThu = new System.Windows.Forms.Label();
            this.lblDienTieuThuLabel = new System.Windows.Forms.Label();
            this.numChiSoDienMoi = new System.Windows.Forms.NumericUpDown();
            this.lblChiSoDienMoi = new System.Windows.Forms.Label();
            this.numChiSoDienCu = new System.Windows.Forms.NumericUpDown();
            this.lblChiSoDienCu = new System.Windows.Forms.Label();
            this.numNam = new System.Windows.Forms.NumericUpDown();
            this.lblNam = new System.Windows.Forms.Label();
            this.numThang = new System.Windows.Forms.NumericUpDown();
            this.lblThang = new System.Windows.Forms.Label();
            this.cmbPhong = new System.Windows.Forms.ComboBox();
            this.lblPhong = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.grpNuoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChiSoNuocMoi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChiSoNuocCu)).BeginInit();
            this.grpDien.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChiSoDienMoi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChiSoDienCu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThang)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.pnlHeader.Controls.Add(this.lblFormSub);
            this.pnlHeader.Controls.Add(this.lblFormTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(560, 68);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblFormSub
            // 
            this.lblFormSub.AutoSize = true;
            this.lblFormSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFormSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.lblFormSub.Location = new System.Drawing.Point(24, 39);
            this.lblFormSub.Name = "lblFormSub";
            this.lblFormSub.Size = new System.Drawing.Size(395, 15);
            this.lblFormSub.TabIndex = 1;
            this.lblFormSub.Text = "Nhập chỉ số điện nước theo phòng và tháng để tính toán lượng tiêu thụ";
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(22, 12);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(277, 25);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "GHI NHẬN CHỈ SỐ ĐIỆN NƯỚC";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlFooter.Controls.Add(this.btnHuy);
            this.pnlFooter.Controls.Add(this.btnLuu);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 430);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(0, 10, 24, 10);
            this.pnlFooter.Size = new System.Drawing.Size(560, 56);
            this.pnlFooter.TabIndex = 1;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnHuy.Location = new System.Drawing.Point(436, 10);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(100, 36);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy bỏ";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(308, 10);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(120, 36);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "Lưu chỉ số";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.Controls.Add(this.lblGoiY);
            this.pnlBody.Controls.Add(this.grpNuoc);
            this.pnlBody.Controls.Add(this.grpDien);
            this.pnlBody.Controls.Add(this.numNam);
            this.pnlBody.Controls.Add(this.lblNam);
            this.pnlBody.Controls.Add(this.numThang);
            this.pnlBody.Controls.Add(this.lblThang);
            this.pnlBody.Controls.Add(this.cmbPhong);
            this.pnlBody.Controls.Add(this.lblPhong);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 68);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(24, 20, 24, 10);
            this.pnlBody.Size = new System.Drawing.Size(560, 362);
            this.pnlBody.TabIndex = 2;
            // 
            // lblGoiY
            // 
            this.lblGoiY.AutoSize = true;
            this.lblGoiY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblGoiY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblGoiY.Location = new System.Drawing.Point(24, 328);
            this.lblGoiY.Name = "lblGoiY";
            this.lblGoiY.Size = new System.Drawing.Size(350, 15);
            this.lblGoiY.TabIndex = 8;
            this.lblGoiY.Text = "💡 Chỉ số tiêu thụ được tự động tính: Chỉ số mới - Chỉ số cũ";
            // 
            // grpNuoc
            // 
            this.grpNuoc.Controls.Add(this.lblNuocTieuThu);
            this.grpNuoc.Controls.Add(this.lblNuocTieuThuLabel);
            this.grpNuoc.Controls.Add(this.numChiSoNuocMoi);
            this.grpNuoc.Controls.Add(this.lblChiSoNuocMoi);
            this.grpNuoc.Controls.Add(this.numChiSoNuocCu);
            this.grpNuoc.Controls.Add(this.lblChiSoNuocCu);
            this.grpNuoc.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.grpNuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.grpNuoc.Location = new System.Drawing.Point(27, 210);
            this.grpNuoc.Name = "grpNuoc";
            this.grpNuoc.Size = new System.Drawing.Size(506, 108);
            this.grpNuoc.TabIndex = 7;
            this.grpNuoc.TabStop = false;
            this.grpNuoc.Text = "💧 Chỉ số Nước sinh hoạt (m³)";
            // 
            // lblNuocTieuThu
            // 
            this.lblNuocTieuThu.AutoSize = true;
            this.lblNuocTieuThu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblNuocTieuThu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.lblNuocTieuThu.Location = new System.Drawing.Point(365, 59);
            this.lblNuocTieuThu.Name = "lblNuocTieuThu";
            this.lblNuocTieuThu.Size = new System.Drawing.Size(52, 21);
            this.lblNuocTieuThu.TabIndex = 5;
            this.lblNuocTieuThu.Text = "0  m³";
            // 
            // lblNuocTieuThuLabel
            // 
            this.lblNuocTieuThuLabel.AutoSize = true;
            this.lblNuocTieuThuLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblNuocTieuThuLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblNuocTieuThuLabel.Location = new System.Drawing.Point(365, 33);
            this.lblNuocTieuThuLabel.Name = "lblNuocTieuThuLabel";
            this.lblNuocTieuThuLabel.Size = new System.Drawing.Size(95, 17);
            this.lblNuocTieuThuLabel.TabIndex = 4;
            this.lblNuocTieuThuLabel.Text = "Nước tiêu thụ:";
            // 
            // numChiSoNuocMoi
            // 
            this.numChiSoNuocMoi.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.numChiSoNuocMoi.Location = new System.Drawing.Point(195, 57);
            this.numChiSoNuocMoi.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            this.numChiSoNuocMoi.Name = "numChiSoNuocMoi";
            this.numChiSoNuocMoi.Size = new System.Drawing.Size(140, 26);
            this.numChiSoNuocMoi.TabIndex = 3;
            this.numChiSoNuocMoi.ValueChanged += new System.EventHandler(this.numChiSo_ValueChanged);
            // 
            // lblChiSoNuocMoi
            // 
            this.lblChiSoNuocMoi.AutoSize = true;
            this.lblChiSoNuocMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblChiSoNuocMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblChiSoNuocMoi.Location = new System.Drawing.Point(192, 33);
            this.lblChiSoNuocMoi.Name = "lblChiSoNuocMoi";
            this.lblChiSoNuocMoi.Size = new System.Drawing.Size(107, 17);
            this.lblChiSoNuocMoi.TabIndex = 2;
            this.lblChiSoNuocMoi.Text = "Chỉ số nước mới:";
            // 
            // numChiSoNuocCu
            // 
            this.numChiSoNuocCu.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.numChiSoNuocCu.Location = new System.Drawing.Point(23, 57);
            this.numChiSoNuocCu.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            this.numChiSoNuocCu.Name = "numChiSoNuocCu";
            this.numChiSoNuocCu.Size = new System.Drawing.Size(140, 26);
            this.numChiSoNuocCu.TabIndex = 1;
            this.numChiSoNuocCu.ValueChanged += new System.EventHandler(this.numChiSo_ValueChanged);
            // 
            // lblChiSoNuocCu
            // 
            this.lblChiSoNuocCu.AutoSize = true;
            this.lblChiSoNuocCu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblChiSoNuocCu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblChiSoNuocCu.Location = new System.Drawing.Point(20, 33);
            this.lblChiSoNuocCu.Name = "lblChiSoNuocCu";
            this.lblChiSoNuocCu.Size = new System.Drawing.Size(100, 17);
            this.lblChiSoNuocCu.TabIndex = 0;
            this.lblChiSoNuocCu.Text = "Chỉ số nước cũ:";
            // 
            // grpDien
            // 
            this.grpDien.Controls.Add(this.lblDienTieuThu);
            this.grpDien.Controls.Add(this.lblDienTieuThuLabel);
            this.grpDien.Controls.Add(this.numChiSoDienMoi);
            this.grpDien.Controls.Add(this.lblChiSoDienMoi);
            this.grpDien.Controls.Add(this.numChiSoDienCu);
            this.grpDien.Controls.Add(this.lblChiSoDienCu);
            this.grpDien.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.grpDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.grpDien.Location = new System.Drawing.Point(27, 90);
            this.grpDien.Name = "grpDien";
            this.grpDien.Size = new System.Drawing.Size(506, 108);
            this.grpDien.TabIndex = 6;
            this.grpDien.TabStop = false;
            this.grpDien.Text = "⚡ Chỉ số Điện sinh hoạt (kWh)";
            // 
            // lblDienTieuThu
            // 
            this.lblDienTieuThu.AutoSize = true;
            this.lblDienTieuThu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDienTieuThu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblDienTieuThu.Location = new System.Drawing.Point(365, 59);
            this.lblDienTieuThu.Name = "lblDienTieuThu";
            this.lblDienTieuThu.Size = new System.Drawing.Size(65, 21);
            this.lblDienTieuThu.TabIndex = 5;
            this.lblDienTieuThu.Text = "0  kWh";
            // 
            // lblDienTieuThuLabel
            // 
            this.lblDienTieuThuLabel.AutoSize = true;
            this.lblDienTieuThuLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDienTieuThuLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblDienTieuThuLabel.Location = new System.Drawing.Point(365, 33);
            this.lblDienTieuThuLabel.Name = "lblDienTieuThuLabel";
            this.lblDienTieuThuLabel.Size = new System.Drawing.Size(91, 17);
            this.lblDienTieuThuLabel.TabIndex = 4;
            this.lblDienTieuThuLabel.Text = "Điện tiêu thụ:";
            // 
            // numChiSoDienMoi
            // 
            this.numChiSoDienMoi.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.numChiSoDienMoi.Location = new System.Drawing.Point(195, 57);
            this.numChiSoDienMoi.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            this.numChiSoDienMoi.Name = "numChiSoDienMoi";
            this.numChiSoDienMoi.Size = new System.Drawing.Size(140, 26);
            this.numChiSoDienMoi.TabIndex = 3;
            this.numChiSoDienMoi.ValueChanged += new System.EventHandler(this.numChiSo_ValueChanged);
            // 
            // lblChiSoDienMoi
            // 
            this.lblChiSoDienMoi.AutoSize = true;
            this.lblChiSoDienMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblChiSoDienMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblChiSoDienMoi.Location = new System.Drawing.Point(192, 33);
            this.lblChiSoDienMoi.Name = "lblChiSoDienMoi";
            this.lblChiSoDienMoi.Size = new System.Drawing.Size(103, 17);
            this.lblChiSoDienMoi.TabIndex = 2;
            this.lblChiSoDienMoi.Text = "Chỉ số điện mới:";
            // 
            // numChiSoDienCu
            // 
            this.numChiSoDienCu.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.numChiSoDienCu.Location = new System.Drawing.Point(23, 57);
            this.numChiSoDienCu.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            this.numChiSoDienCu.Name = "numChiSoDienCu";
            this.numChiSoDienCu.Size = new System.Drawing.Size(140, 26);
            this.numChiSoDienCu.TabIndex = 1;
            this.numChiSoDienCu.ValueChanged += new System.EventHandler(this.numChiSo_ValueChanged);
            // 
            // lblChiSoDienCu
            // 
            this.lblChiSoDienCu.AutoSize = true;
            this.lblChiSoDienCu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblChiSoDienCu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblChiSoDienCu.Location = new System.Drawing.Point(20, 33);
            this.lblChiSoDienCu.Name = "lblChiSoDienCu";
            this.lblChiSoDienCu.Size = new System.Drawing.Size(96, 17);
            this.lblChiSoDienCu.TabIndex = 0;
            this.lblChiSoDienCu.Text = "Chỉ số điện cũ:";
            // 
            // numNam
            // 
            this.numNam.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numNam.Location = new System.Drawing.Point(423, 45);
            this.numNam.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            this.numNam.Minimum = new decimal(new int[] { 2020, 0, 0, 0 });
            this.numNam.Name = "numNam";
            this.numNam.Size = new System.Drawing.Size(110, 25);
            this.numNam.TabIndex = 5;
            this.numNam.Value = new decimal(new int[] { 2026, 0, 0, 0 });
            this.numNam.ValueChanged += new System.EventHandler(this.PhongOrThangNam_Changed);
            // 
            // lblNam
            // 
            this.lblNam.AutoSize = true;
            this.lblNam.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblNam.Location = new System.Drawing.Point(420, 20);
            this.lblNam.Name = "lblNam";
            this.lblNam.Size = new System.Drawing.Size(40, 17);
            this.lblNam.TabIndex = 4;
            this.lblNam.Text = "Năm:";
            // 
            // numThang
            // 
            this.numThang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numThang.Location = new System.Drawing.Point(313, 45);
            this.numThang.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            this.numThang.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numThang.Name = "numThang";
            this.numThang.Size = new System.Drawing.Size(90, 25);
            this.numThang.TabIndex = 3;
            this.numThang.Value = new decimal(new int[] { 11, 0, 0, 0 });
            this.numThang.ValueChanged += new System.EventHandler(this.PhongOrThangNam_Changed);
            // 
            // lblThang
            // 
            this.lblThang.AutoSize = true;
            this.lblThang.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblThang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblThang.Location = new System.Drawing.Point(310, 20);
            this.lblThang.Name = "lblThang";
            this.lblThang.Size = new System.Drawing.Size(49, 17);
            this.lblThang.TabIndex = 2;
            this.lblThang.Text = "Tháng:";
            // 
            // cmbPhong
            // 
            this.cmbPhong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPhong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPhong.FormattingEnabled = true;
            this.cmbPhong.Location = new System.Drawing.Point(27, 45);
            this.cmbPhong.Name = "cmbPhong";
            this.cmbPhong.Size = new System.Drawing.Size(265, 25);
            this.cmbPhong.TabIndex = 1;
            this.cmbPhong.SelectedIndexChanged += new System.EventHandler(this.PhongOrThangNam_Changed);
            // 
            // lblPhong
            // 
            this.lblPhong.AutoSize = true;
            this.lblPhong.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPhong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblPhong.Location = new System.Drawing.Point(24, 20);
            this.lblPhong.Name = "lblPhong";
            this.lblPhong.Size = new System.Drawing.Size(126, 17);
            this.lblPhong.TabIndex = 0;
            this.lblPhong.Text = "Chọn phòng ghi số:";
            // 
            // frmGhiDienNuoc
            // 
            this.AcceptButton = this.btnLuu;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(560, 486);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGhiDienNuoc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ghi nhận chỉ số điện nước";
            this.Load += new System.EventHandler(this.frmGhiDienNuoc_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.grpNuoc.ResumeLayout(false);
            this.grpNuoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChiSoNuocMoi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChiSoNuocCu)).EndInit();
            this.grpDien.ResumeLayout(false);
            this.grpDien.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChiSoDienMoi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChiSoDienCu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThang)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblFormSub;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblPhong;
        private System.Windows.Forms.ComboBox cmbPhong;
        private System.Windows.Forms.Label lblThang;
        private System.Windows.Forms.NumericUpDown numThang;
        private System.Windows.Forms.Label lblNam;
        private System.Windows.Forms.NumericUpDown numNam;
        private System.Windows.Forms.GroupBox grpDien;
        private System.Windows.Forms.Label lblChiSoDienCu;
        private System.Windows.Forms.NumericUpDown numChiSoDienCu;
        private System.Windows.Forms.Label lblChiSoDienMoi;
        private System.Windows.Forms.NumericUpDown numChiSoDienMoi;
        private System.Windows.Forms.Label lblDienTieuThuLabel;
        private System.Windows.Forms.Label lblDienTieuThu;
        private System.Windows.Forms.GroupBox grpNuoc;
        private System.Windows.Forms.Label lblNuocTieuThu;
        private System.Windows.Forms.Label lblNuocTieuThuLabel;
        private System.Windows.Forms.NumericUpDown numChiSoNuocMoi;
        private System.Windows.Forms.Label lblChiSoNuocMoi;
        private System.Windows.Forms.NumericUpDown numChiSoNuocCu;
        private System.Windows.Forms.Label lblChiSoNuocCu;
        private System.Windows.Forms.Label lblGoiY;
    }
}

