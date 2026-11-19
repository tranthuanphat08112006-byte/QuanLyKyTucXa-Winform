namespace QuanLyKyTucXa.GUI
{
    partial class frmLapHoaDon
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
            this.pnlChiTiet = new System.Windows.Forms.Panel();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.lblTongTienLabel = new System.Windows.Forms.Label();
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.numTienDichVuKhac = new System.Windows.Forms.NumericUpDown();
            this.lblTienDichVuKhac = new System.Windows.Forms.Label();
            this.lblThanhTienNuoc = new System.Windows.Forms.Label();
            this.lblChiTietNuoc = new System.Windows.Forms.Label();
            this.lblTienNuocLabel = new System.Windows.Forms.Label();
            this.lblThanhTienDien = new System.Windows.Forms.Label();
            this.lblChiTietDien = new System.Windows.Forms.Label();
            this.lblTienDienLabel = new System.Windows.Forms.Label();
            this.lblTienPhong = new System.Windows.Forms.Label();
            this.lblTienPhongLabel = new System.Windows.Forms.Label();
            this.lblCardTitle = new System.Windows.Forms.Label();
            this.lblThongBaoDienNuoc = new System.Windows.Forms.Label();
            this.numNam = new System.Windows.Forms.NumericUpDown();
            this.lblNam = new System.Windows.Forms.Label();
            this.numThang = new System.Windows.Forms.NumericUpDown();
            this.lblThang = new System.Windows.Forms.Label();
            this.cmbPhong = new System.Windows.Forms.ComboBox();
            this.lblPhong = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTienDichVuKhac)).BeginInit();
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
            this.pnlHeader.Size = new System.Drawing.Size(580, 68);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblFormSub
            // 
            this.lblFormSub.AutoSize = true;
            this.lblFormSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFormSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.lblFormSub.Location = new System.Drawing.Point(24, 39);
            this.lblFormSub.Name = "lblFormSub";
            this.lblFormSub.Size = new System.Drawing.Size(439, 15);
            this.lblFormSub.TabIndex = 1;
            this.lblFormSub.Text = "Tổng hợp tiền phòng, tiền điện, tiền nước và các khoản phí dịch vụ thu định kỳ";
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(22, 12);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(222, 25);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "LẬP HÓA ĐƠN THU TIỀN";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlFooter.Controls.Add(this.btnHuy);
            this.pnlFooter.Controls.Add(this.btnLuu);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 520);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(0, 10, 24, 10);
            this.pnlFooter.Size = new System.Drawing.Size(580, 56);
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
            this.btnHuy.Location = new System.Drawing.Point(456, 10);
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
            this.btnLuu.Location = new System.Drawing.Point(318, 10);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(130, 36);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "Lập hóa đơn";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.Controls.Add(this.pnlChiTiet);
            this.pnlBody.Controls.Add(this.lblThongBaoDienNuoc);
            this.pnlBody.Controls.Add(this.numNam);
            this.pnlBody.Controls.Add(this.lblNam);
            this.pnlBody.Controls.Add(this.numThang);
            this.pnlBody.Controls.Add(this.lblThang);
            this.pnlBody.Controls.Add(this.cmbPhong);
            this.pnlBody.Controls.Add(this.lblPhong);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 68);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            this.pnlBody.Size = new System.Drawing.Size(580, 452);
            this.pnlBody.TabIndex = 2;
            // 
            // pnlChiTiet
            // 
            this.pnlChiTiet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlChiTiet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlChiTiet.Controls.Add(this.lblTongTien);
            this.pnlChiTiet.Controls.Add(this.lblTongTienLabel);
            this.pnlChiTiet.Controls.Add(this.pnlDivider);
            this.pnlChiTiet.Controls.Add(this.txtGhiChu);
            this.pnlChiTiet.Controls.Add(this.lblGhiChu);
            this.pnlChiTiet.Controls.Add(this.numTienDichVuKhac);
            this.pnlChiTiet.Controls.Add(this.lblTienDichVuKhac);
            this.pnlChiTiet.Controls.Add(this.lblThanhTienNuoc);
            this.pnlChiTiet.Controls.Add(this.lblChiTietNuoc);
            this.pnlChiTiet.Controls.Add(this.lblTienNuocLabel);
            this.pnlChiTiet.Controls.Add(this.lblThanhTienDien);
            this.pnlChiTiet.Controls.Add(this.lblChiTietDien);
            this.pnlChiTiet.Controls.Add(this.lblTienDienLabel);
            this.pnlChiTiet.Controls.Add(this.lblTienPhong);
            this.pnlChiTiet.Controls.Add(this.lblTienPhongLabel);
            this.pnlChiTiet.Controls.Add(this.lblCardTitle);
            this.pnlChiTiet.Location = new System.Drawing.Point(27, 114);
            this.pnlChiTiet.Name = "pnlChiTiet";
            this.pnlChiTiet.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.pnlChiTiet.Size = new System.Drawing.Size(526, 320);
            this.pnlChiTiet.TabIndex = 7;
            // 
            // lblTongTien
            // 
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblTongTien.Location = new System.Drawing.Point(290, 278);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(218, 30);
            this.lblTongTien.TabIndex = 15;
            this.lblTongTien.Text = "0 đ";
            this.lblTongTien.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTongTienLabel
            // 
            this.lblTongTienLabel.AutoSize = true;
            this.lblTongTienLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblTongTienLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTongTienLabel.Location = new System.Drawing.Point(15, 283);
            this.lblTongTienLabel.Name = "lblTongTienLabel";
            this.lblTongTienLabel.Size = new System.Drawing.Size(193, 20);
            this.lblTongTienLabel.TabIndex = 14;
            this.lblTongTienLabel.Text = "TỔNG TIỀN THANH TOÁN:";
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.pnlDivider.Location = new System.Drawing.Point(16, 268);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(492, 1);
            this.pnlDivider.TabIndex = 13;
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtGhiChu.Location = new System.Drawing.Point(120, 226);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(388, 24);
            this.txtGhiChu.TabIndex = 12;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblGhiChu.Location = new System.Drawing.Point(15, 229);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(54, 17);
            this.lblGhiChu.TabIndex = 11;
            this.lblGhiChu.Text = "Ghi chú:";
            // 
            // numTienDichVuKhac
            // 
            this.numTienDichVuKhac.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numTienDichVuKhac.Increment = new decimal(new int[] { 5000, 0, 0, 0 });
            this.numTienDichVuKhac.Location = new System.Drawing.Point(368, 185);
            this.numTienDichVuKhac.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            this.numTienDichVuKhac.Name = "numTienDichVuKhac";
            this.numTienDichVuKhac.Size = new System.Drawing.Size(140, 25);
            this.numTienDichVuKhac.TabIndex = 10;
            this.numTienDichVuKhac.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTienDichVuKhac.ValueChanged += new System.EventHandler(this.numTienDichVuKhac_ValueChanged);
            // 
            // lblTienDichVuKhac
            // 
            this.lblTienDichVuKhac.AutoSize = true;
            this.lblTienDichVuKhac.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTienDichVuKhac.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTienDichVuKhac.Location = new System.Drawing.Point(15, 187);
            this.lblTienDichVuKhac.Name = "lblTienDichVuKhac";
            this.lblTienDichVuKhac.Size = new System.Drawing.Size(193, 17);
            this.lblTienDichVuKhac.TabIndex = 9;
            this.lblTienDichVuKhac.Text = "4. Phí dịch vụ khác (vệ sinh, rác):";
            // 
            // lblThanhTienNuoc
            // 
            this.lblThanhTienNuoc.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblThanhTienNuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.lblThanhTienNuoc.Location = new System.Drawing.Point(368, 143);
            this.lblThanhTienNuoc.Name = "lblThanhTienNuoc";
            this.lblThanhTienNuoc.Size = new System.Drawing.Size(140, 20);
            this.lblThanhTienNuoc.TabIndex = 8;
            this.lblThanhTienNuoc.Text = "0 đ";
            this.lblThanhTienNuoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChiTietNuoc
            // 
            this.lblChiTietNuoc.AutoSize = true;
            this.lblChiTietNuoc.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblChiTietNuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblChiTietNuoc.Location = new System.Drawing.Point(135, 146);
            this.lblChiTietNuoc.Name = "lblChiTietNuoc";
            this.lblChiTietNuoc.Size = new System.Drawing.Size(129, 15);
            this.lblChiTietNuoc.TabIndex = 7;
            this.lblChiTietNuoc.Text = "(0 m³ × 15,000 đ/m³)";
            // 
            // lblTienNuocLabel
            // 
            this.lblTienNuocLabel.AutoSize = true;
            this.lblTienNuocLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTienNuocLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTienNuocLabel.Location = new System.Drawing.Point(15, 144);
            this.lblTienNuocLabel.Name = "lblTienNuocLabel";
            this.lblTienNuocLabel.Size = new System.Drawing.Size(89, 17);
            this.lblTienNuocLabel.TabIndex = 6;
            this.lblTienNuocLabel.Text = "3. Tiền nước:";
            // 
            // lblThanhTienDien
            // 
            this.lblThanhTienDien.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblThanhTienDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblThanhTienDien.Location = new System.Drawing.Point(368, 98);
            this.lblThanhTienDien.Name = "lblThanhTienDien";
            this.lblThanhTienDien.Size = new System.Drawing.Size(140, 20);
            this.lblThanhTienDien.TabIndex = 5;
            this.lblThanhTienDien.Text = "0 đ";
            this.lblThanhTienDien.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChiTietDien
            // 
            this.lblChiTietDien.AutoSize = true;
            this.lblChiTietDien.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblChiTietDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblChiTietDien.Location = new System.Drawing.Point(135, 101);
            this.lblChiTietDien.Name = "lblChiTietDien";
            this.lblChiTietDien.Size = new System.Drawing.Size(138, 15);
            this.lblChiTietDien.TabIndex = 4;
            this.lblChiTietDien.Text = "(0 kWh × 3,500 đ/kWh)";
            // 
            // lblTienDienLabel
            // 
            this.lblTienDienLabel.AutoSize = true;
            this.lblTienDienLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTienDienLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTienDienLabel.Location = new System.Drawing.Point(15, 99);
            this.lblTienDienLabel.Name = "lblTienDienLabel";
            this.lblTienDienLabel.Size = new System.Drawing.Size(86, 17);
            this.lblTienDienLabel.TabIndex = 3;
            this.lblTienDienLabel.Text = "2. Tiền điện:";
            // 
            // lblTienPhong
            // 
            this.lblTienPhong.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTienPhong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblTienPhong.Location = new System.Drawing.Point(368, 54);
            this.lblTienPhong.Name = "lblTienPhong";
            this.lblTienPhong.Size = new System.Drawing.Size(140, 20);
            this.lblTienPhong.TabIndex = 2;
            this.lblTienPhong.Text = "0 đ";
            this.lblTienPhong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTienPhongLabel
            // 
            this.lblTienPhongLabel.AutoSize = true;
            this.lblTienPhongLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTienPhongLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTienPhongLabel.Location = new System.Drawing.Point(15, 56);
            this.lblTienPhongLabel.Name = "lblTienPhongLabel";
            this.lblTienPhongLabel.Size = new System.Drawing.Size(169, 17);
            this.lblTienPhongLabel.TabIndex = 1;
            this.lblTienPhongLabel.Text = "1. Tiền phòng (theo biểu phí):";
            // 
            // lblCardTitle
            // 
            this.lblCardTitle.AutoSize = true;
            this.lblCardTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCardTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblCardTitle.Location = new System.Drawing.Point(14, 14);
            this.lblCardTitle.Name = "lblCardTitle";
            this.lblCardTitle.Size = new System.Drawing.Size(188, 19);
            this.lblCardTitle.TabIndex = 0;
            this.lblCardTitle.Text = "CHI TIẾT CÁC KHOẢN THU";
            // 
            // lblThongBaoDienNuoc
            // 
            this.lblThongBaoDienNuoc.AutoEllipsis = true;
            this.lblThongBaoDienNuoc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblThongBaoDienNuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblThongBaoDienNuoc.Location = new System.Drawing.Point(27, 78);
            this.lblThongBaoDienNuoc.Name = "lblThongBaoDienNuoc";
            this.lblThongBaoDienNuoc.Size = new System.Drawing.Size(526, 26);
            this.lblThongBaoDienNuoc.TabIndex = 6;
            this.lblThongBaoDienNuoc.Text = "Vui lòng chọn phòng để tính toán hóa đơn";
            // 
            // numNam
            // 
            this.numNam.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numNam.Location = new System.Drawing.Point(443, 42);
            this.numNam.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            this.numNam.Minimum = new decimal(new int[] { 2020, 0, 0, 0 });
            this.numNam.Name = "numNam";
            this.numNam.Size = new System.Drawing.Size(110, 25);
            this.numNam.TabIndex = 5;
            this.numNam.Value = new decimal(new int[] { 2026, 0, 0, 0 });
            this.numNam.ValueChanged += new System.EventHandler(this.ThongTin_Changed);
            // 
            // lblNam
            // 
            this.lblNam.AutoSize = true;
            this.lblNam.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblNam.Location = new System.Drawing.Point(440, 18);
            this.lblNam.Name = "lblNam";
            this.lblNam.Size = new System.Drawing.Size(40, 17);
            this.lblNam.TabIndex = 4;
            this.lblNam.Text = "Năm:";
            // 
            // numThang
            // 
            this.numThang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numThang.Location = new System.Drawing.Point(333, 42);
            this.numThang.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            this.numThang.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numThang.Name = "numThang";
            this.numThang.Size = new System.Drawing.Size(90, 25);
            this.numThang.TabIndex = 3;
            this.numThang.Value = new decimal(new int[] { 11, 0, 0, 0 });
            this.numThang.ValueChanged += new System.EventHandler(this.ThongTin_Changed);
            // 
            // lblThang
            // 
            this.lblThang.AutoSize = true;
            this.lblThang.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblThang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblThang.Location = new System.Drawing.Point(330, 18);
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
            this.cmbPhong.Location = new System.Drawing.Point(27, 42);
            this.cmbPhong.Name = "cmbPhong";
            this.cmbPhong.Size = new System.Drawing.Size(285, 25);
            this.cmbPhong.TabIndex = 1;
            this.cmbPhong.SelectedIndexChanged += new System.EventHandler(this.ThongTin_Changed);
            // 
            // lblPhong
            // 
            this.lblPhong.AutoSize = true;
            this.lblPhong.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPhong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblPhong.Location = new System.Drawing.Point(24, 18);
            this.lblPhong.Name = "lblPhong";
            this.lblPhong.Size = new System.Drawing.Size(142, 17);
            this.lblPhong.TabIndex = 0;
            this.lblPhong.Text = "Chọn phòng lập phiếu:";
            // 
            // frmLapHoaDon
            // 
            this.AcceptButton = this.btnLuu;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(580, 576);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLapHoaDon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Lập hóa đơn thu tiền";
            this.Load += new System.EventHandler(this.frmLapHoaDon_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.pnlChiTiet.ResumeLayout(false);
            this.pnlChiTiet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTienDichVuKhac)).EndInit();
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
        private System.Windows.Forms.Label lblThongBaoDienNuoc;
        private System.Windows.Forms.Panel pnlChiTiet;
        private System.Windows.Forms.Label lblCardTitle;
        private System.Windows.Forms.Label lblTienPhongLabel;
        private System.Windows.Forms.Label lblTienPhong;
        private System.Windows.Forms.Label lblTienDienLabel;
        private System.Windows.Forms.Label lblChiTietDien;
        private System.Windows.Forms.Label lblThanhTienDien;
        private System.Windows.Forms.Label lblTienNuocLabel;
        private System.Windows.Forms.Label lblChiTietNuoc;
        private System.Windows.Forms.Label lblThanhTienNuoc;
        private System.Windows.Forms.Label lblTienDichVuKhac;
        private System.Windows.Forms.NumericUpDown numTienDichVuKhac;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Label lblTongTienLabel;
        private System.Windows.Forms.Label lblTongTien;
    }
}

