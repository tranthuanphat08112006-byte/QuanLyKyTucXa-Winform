namespace QuanLyKyTucXa.GUI
{
    partial class frmThanhToanHoaDon
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
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlLichSu = new System.Windows.Forms.Panel();
            this.dgvLichSu = new System.Windows.Forms.DataGridView();
            this.lblLichSuTitle = new System.Windows.Forms.Label();
            this.grpThanhToan = new System.Windows.Forms.GroupBox();
            this.lblTrangThaiMoi = new System.Windows.Forms.Label();
            this.lblConNoSauThanhToan = new System.Windows.Forms.Label();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.txtNguoiNop = new System.Windows.Forms.TextBox();
            this.lblNguoiNop = new System.Windows.Forms.Label();
            this.cmbPhuongThuc = new System.Windows.Forms.ComboBox();
            this.lblPhuongThuc = new System.Windows.Forms.Label();
            this.btnTraHet = new System.Windows.Forms.Button();
            this.numSoTien = new System.Windows.Forms.NumericUpDown();
            this.lblSoTien = new System.Windows.Forms.Label();
            this.pnlInfoCard = new System.Windows.Forms.Panel();
            this.lblConNo = new System.Windows.Forms.Label();
            this.lblConNoTitle = new System.Windows.Forms.Label();
            this.lblDaThanhToan = new System.Windows.Forms.Label();
            this.lblDaThanhToanTitle = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.lblTongTienTitle = new System.Windows.Forms.Label();
            this.lblThongTinHoaDon = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlLichSu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSu)).BeginInit();
            this.grpThanhToan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoTien)).BeginInit();
            this.pnlInfoCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.pnlHeader.Controls.Add(this.lblFormSub);
            this.pnlHeader.Controls.Add(this.lblFormTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(620, 68);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblFormSub
            // 
            this.lblFormSub.AutoSize = true;
            this.lblFormSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFormSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(252)))), ((int)(((byte)(231)))));
            this.lblFormSub.Location = new System.Drawing.Point(24, 39);
            this.lblFormSub.Name = "lblFormSub";
            this.lblFormSub.Size = new System.Drawing.Size(468, 15);
            this.lblFormSub.TabIndex = 1;
            this.lblFormSub.Text = "Thu tiền hóa đơn, cập nhật số tiền đã trả, tính tiền còn nợ và quản lý trạng thái" +
    " thanh toán";
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(22, 12);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(315, 25);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "THANH TOÁN HÓA ĐƠN TIỀN PHÒNG";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlFooter.Controls.Add(this.btnHuy);
            this.pnlFooter.Controls.Add(this.btnThanhToan);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 560);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(0, 10, 24, 10);
            this.pnlFooter.Size = new System.Drawing.Size(620, 56);
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
            this.btnHuy.Location = new System.Drawing.Point(496, 10);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(100, 36);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Đóng";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnThanhToan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThanhToan.FlatAppearance.BorderSize = 0;
            this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.Location = new System.Drawing.Point(328, 10);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(160, 36);
            this.btnThanhToan.TabIndex = 0;
            this.btnThanhToan.Text = "Xác nhận thanh toán";
            this.btnThanhToan.UseVisualStyleBackColor = false;
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.Controls.Add(this.pnlLichSu);
            this.pnlBody.Controls.Add(this.grpThanhToan);
            this.pnlBody.Controls.Add(this.pnlInfoCard);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 68);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            this.pnlBody.Size = new System.Drawing.Size(620, 492);
            this.pnlBody.TabIndex = 2;
            // 
            // pnlLichSu
            // 
            this.pnlLichSu.Controls.Add(this.dgvLichSu);
            this.pnlLichSu.Controls.Add(this.lblLichSuTitle);
            this.pnlLichSu.Location = new System.Drawing.Point(24, 320);
            this.pnlLichSu.Name = "pnlLichSu";
            this.pnlLichSu.Size = new System.Drawing.Size(572, 160);
            this.pnlLichSu.TabIndex = 2;
            // 
            // dgvLichSu
            // 
            this.dgvLichSu.BackgroundColor = System.Drawing.Color.White;
            this.dgvLichSu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvLichSu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLichSu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLichSu.Location = new System.Drawing.Point(0, 24);
            this.dgvLichSu.Name = "dgvLichSu";
            this.dgvLichSu.ReadOnly = true;
            this.dgvLichSu.RowHeadersVisible = false;
            this.dgvLichSu.Size = new System.Drawing.Size(572, 136);
            this.dgvLichSu.TabIndex = 1;
            // 
            // lblLichSuTitle
            // 
            this.lblLichSuTitle.AutoSize = true;
            this.lblLichSuTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLichSuTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblLichSuTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblLichSuTitle.Location = new System.Drawing.Point(0, 0);
            this.lblLichSuTitle.Name = "lblLichSuTitle";
            this.lblLichSuTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.lblLichSuTitle.Size = new System.Drawing.Size(206, 24);
            this.lblLichSuTitle.TabIndex = 0;
            this.lblLichSuTitle.Text = "Lịch sử các lần thu tiền trước đó:";
            // 
            // grpThanhToan
            // 
            this.grpThanhToan.Controls.Add(this.lblTrangThaiMoi);
            this.grpThanhToan.Controls.Add(this.lblConNoSauThanhToan);
            this.grpThanhToan.Controls.Add(this.txtGhiChu);
            this.grpThanhToan.Controls.Add(this.lblGhiChu);
            this.grpThanhToan.Controls.Add(this.txtNguoiNop);
            this.grpThanhToan.Controls.Add(this.lblNguoiNop);
            this.grpThanhToan.Controls.Add(this.cmbPhuongThuc);
            this.grpThanhToan.Controls.Add(this.lblPhuongThuc);
            this.grpThanhToan.Controls.Add(this.btnTraHet);
            this.grpThanhToan.Controls.Add(this.numSoTien);
            this.grpThanhToan.Controls.Add(this.lblSoTien);
            this.grpThanhToan.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.grpThanhToan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.grpThanhToan.Location = new System.Drawing.Point(24, 110);
            this.grpThanhToan.Name = "grpThanhToan";
            this.grpThanhToan.Size = new System.Drawing.Size(572, 196);
            this.grpThanhToan.TabIndex = 1;
            this.grpThanhToan.TabStop = false;
            this.grpThanhToan.Text = "THÔNG TIN PHIẾU THU TIỀN";
            // 
            // lblTrangThaiMoi
            // 
            this.lblTrangThaiMoi.AutoSize = true;
            this.lblTrangThaiMoi.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTrangThaiMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblTrangThaiMoi.Location = new System.Drawing.Point(308, 164);
            this.lblTrangThaiMoi.Name = "lblTrangThaiMoi";
            this.lblTrangThaiMoi.Size = new System.Drawing.Size(161, 17);
            this.lblTrangThaiMoi.TabIndex = 10;
            this.lblTrangThaiMoi.Text = "Trạng thái: Đã thanh toán";
            // 
            // lblConNoSauThanhToan
            // 
            this.lblConNoSauThanhToan.AutoSize = true;
            this.lblConNoSauThanhToan.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblConNoSauThanhToan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblConNoSauThanhToan.Location = new System.Drawing.Point(18, 164);
            this.lblConNoSauThanhToan.Name = "lblConNoSauThanhToan";
            this.lblConNoSauThanhToan.Size = new System.Drawing.Size(183, 17);
            this.lblConNoSauThanhToan.TabIndex = 9;
            this.lblConNoSauThanhToan.Text = "Số tiền còn nợ sau thu: 0 đ";
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtGhiChu.Location = new System.Drawing.Point(125, 126);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(425, 24);
            this.txtGhiChu.TabIndex = 8;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblGhiChu.Location = new System.Drawing.Point(18, 129);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(54, 17);
            this.lblGhiChu.TabIndex = 7;
            this.lblGhiChu.Text = "Ghi chú:";
            // 
            // txtNguoiNop
            // 
            this.txtNguoiNop.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtNguoiNop.Location = new System.Drawing.Point(395, 80);
            this.txtNguoiNop.Name = "txtNguoiNop";
            this.txtNguoiNop.Size = new System.Drawing.Size(155, 24);
            this.txtNguoiNop.TabIndex = 6;
            // 
            // lblNguoiNop
            // 
            this.lblNguoiNop.AutoSize = true;
            this.lblNguoiNop.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblNguoiNop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblNguoiNop.Location = new System.Drawing.Point(315, 83);
            this.lblNguoiNop.Name = "lblNguoiNop";
            this.lblNguoiNop.Size = new System.Drawing.Size(74, 17);
            this.lblNguoiNop.TabIndex = 5;
            this.lblNguoiNop.Text = "Người nộp:";
            // 
            // cmbPhuongThuc
            // 
            this.cmbPhuongThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbPhuongThuc.FormattingEnabled = true;
            this.cmbPhuongThuc.Items.AddRange(new object[] {
            "Tiền mặt",
            "Chuyển khoản ngân hàng",
            "Quét mã QR / Ví điện tử"});
            this.cmbPhuongThuc.Location = new System.Drawing.Point(125, 80);
            this.cmbPhuongThuc.Name = "cmbPhuongThuc";
            this.cmbPhuongThuc.Size = new System.Drawing.Size(175, 24);
            this.cmbPhuongThuc.TabIndex = 4;
            // 
            // lblPhuongThuc
            // 
            this.lblPhuongThuc.AutoSize = true;
            this.lblPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPhuongThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblPhuongThuc.Location = new System.Drawing.Point(18, 83);
            this.lblPhuongThuc.Name = "lblPhuongThuc";
            this.lblPhuongThuc.Size = new System.Drawing.Size(83, 17);
            this.lblPhuongThuc.TabIndex = 3;
            this.lblPhuongThuc.Text = "Phương thức:";
            // 
            // btnTraHet
            // 
            this.btnTraHet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(253)))), ((int)(((byte)(244)))));
            this.btnTraHet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTraHet.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(239)))), ((int)(((byte)(172)))));
            this.btnTraHet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTraHet.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnTraHet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnTraHet.Location = new System.Drawing.Point(365, 34);
            this.btnTraHet.Name = "btnTraHet";
            this.btnTraHet.Size = new System.Drawing.Size(185, 27);
            this.btnTraHet.TabIndex = 2;
            this.btnTraHet.Text = "Thanh toán toàn bộ số nợ";
            this.btnTraHet.UseVisualStyleBackColor = false;
            this.btnTraHet.Click += new System.EventHandler(this.btnTraHet_Click);
            // 
            // numSoTien
            // 
            this.numSoTien.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.numSoTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.numSoTien.Increment = new decimal(new int[] { 10000, 0, 0, 0 });
            this.numSoTien.Location = new System.Drawing.Point(125, 34);
            this.numSoTien.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            this.numSoTien.Name = "numSoTien";
            this.numSoTien.Size = new System.Drawing.Size(225, 27);
            this.numSoTien.TabIndex = 1;
            this.numSoTien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSoTien.ValueChanged += new System.EventHandler(this.numSoTien_ValueChanged);
            // 
            // lblSoTien
            // 
            this.lblSoTien.AutoSize = true;
            this.lblSoTien.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSoTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblSoTien.Location = new System.Drawing.Point(18, 38);
            this.lblSoTien.Name = "lblSoTien";
            this.lblSoTien.Size = new System.Drawing.Size(96, 17);
            this.lblSoTien.TabIndex = 0;
            this.lblSoTien.Text = "Số tiền thu (đ):";
            // 
            // pnlInfoCard
            // 
            this.pnlInfoCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlInfoCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfoCard.Controls.Add(this.lblConNo);
            this.pnlInfoCard.Controls.Add(this.lblConNoTitle);
            this.pnlInfoCard.Controls.Add(this.lblDaThanhToan);
            this.pnlInfoCard.Controls.Add(this.lblDaThanhToanTitle);
            this.pnlInfoCard.Controls.Add(this.lblTongTien);
            this.pnlInfoCard.Controls.Add(this.lblTongTienTitle);
            this.pnlInfoCard.Controls.Add(this.lblThongTinHoaDon);
            this.pnlInfoCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfoCard.Location = new System.Drawing.Point(24, 16);
            this.pnlInfoCard.Name = "pnlInfoCard";
            this.pnlInfoCard.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.pnlInfoCard.Size = new System.Drawing.Size(572, 80);
            this.pnlInfoCard.TabIndex = 0;
            // 
            // lblConNo
            // 
            this.lblConNo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblConNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblConNo.Location = new System.Drawing.Point(395, 45);
            this.lblConNo.Name = "lblConNo";
            this.lblConNo.Size = new System.Drawing.Size(155, 22);
            this.lblConNo.TabIndex = 6;
            this.lblConNo.Text = "0 đ";
            this.lblConNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblConNoTitle
            // 
            this.lblConNoTitle.AutoSize = true;
            this.lblConNoTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblConNoTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblConNoTitle.Location = new System.Drawing.Point(400, 28);
            this.lblConNoTitle.Name = "lblConNoTitle";
            this.lblConNoTitle.Size = new System.Drawing.Size(91, 15);
            this.lblConNoTitle.TabIndex = 5;
            this.lblConNoTitle.Text = "Số tiền còn nợ:";
            // 
            // lblDaThanhToan
            // 
            this.lblDaThanhToan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDaThanhToan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblDaThanhToan.Location = new System.Drawing.Point(205, 45);
            this.lblDaThanhToan.Name = "lblDaThanhToan";
            this.lblDaThanhToan.Size = new System.Drawing.Size(155, 22);
            this.lblDaThanhToan.TabIndex = 4;
            this.lblDaThanhToan.Text = "0 đ";
            this.lblDaThanhToan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDaThanhToanTitle
            // 
            this.lblDaThanhToanTitle.AutoSize = true;
            this.lblDaThanhToanTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDaThanhToanTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblDaThanhToanTitle.Location = new System.Drawing.Point(210, 28);
            this.lblDaThanhToanTitle.Name = "lblDaThanhToanTitle";
            this.lblDaThanhToanTitle.Size = new System.Drawing.Size(84, 15);
            this.lblDaThanhToanTitle.TabIndex = 3;
            this.lblDaThanhToanTitle.Text = "Đã thanh toán:";
            // 
            // lblTongTien
            // 
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTongTien.Location = new System.Drawing.Point(16, 45);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(155, 22);
            this.lblTongTien.TabIndex = 2;
            this.lblTongTien.Text = "0 đ";
            this.lblTongTien.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTongTienTitle
            // 
            this.lblTongTienTitle.AutoSize = true;
            this.lblTongTienTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTongTienTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTongTienTitle.Location = new System.Drawing.Point(18, 28);
            this.lblTongTienTitle.Name = "lblTongTienTitle";
            this.lblTongTienTitle.Size = new System.Drawing.Size(85, 15);
            this.lblTongTienTitle.TabIndex = 1;
            this.lblTongTienTitle.Text = "Tổng hóa đơn:";
            // 
            // lblThongTinHoaDon
            // 
            this.lblThongTinHoaDon.AutoSize = true;
            this.lblThongTinHoaDon.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblThongTinHoaDon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblThongTinHoaDon.Location = new System.Drawing.Point(16, 6);
            this.lblThongTinHoaDon.Name = "lblThongTinHoaDon";
            this.lblThongTinHoaDon.Size = new System.Drawing.Size(201, 19);
            this.lblThongTinHoaDon.TabIndex = 0;
            this.lblThongTinHoaDon.Text = "Hóa đơn #0 - Phòng: Chưa chọn";
            // 
            // frmThanhToanHoaDon
            // 
            this.AcceptButton = this.btnThanhToan;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(620, 616);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmThanhToanHoaDon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thanh toán hóa đơn";
            this.Load += new System.EventHandler(this.frmThanhToanHoaDon_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlLichSu.ResumeLayout(false);
            this.pnlLichSu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSu)).EndInit();
            this.grpThanhToan.ResumeLayout(false);
            this.grpThanhToan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoTien)).EndInit();
            this.pnlInfoCard.ResumeLayout(false);
            this.pnlInfoCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblFormSub;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlInfoCard;
        private System.Windows.Forms.Label lblThongTinHoaDon;
        private System.Windows.Forms.Label lblTongTienTitle;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblDaThanhToanTitle;
        private System.Windows.Forms.Label lblDaThanhToan;
        private System.Windows.Forms.Label lblConNoTitle;
        private System.Windows.Forms.Label lblConNo;
        private System.Windows.Forms.GroupBox grpThanhToan;
        private System.Windows.Forms.Label lblSoTien;
        private System.Windows.Forms.NumericUpDown numSoTien;
        private System.Windows.Forms.Button btnTraHet;
        private System.Windows.Forms.Label lblPhuongThuc;
        private System.Windows.Forms.ComboBox cmbPhuongThuc;
        private System.Windows.Forms.Label lblNguoiNop;
        private System.Windows.Forms.TextBox txtNguoiNop;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label lblConNoSauThanhToan;
        private System.Windows.Forms.Label lblTrangThaiMoi;
        private System.Windows.Forms.Panel pnlLichSu;
        private System.Windows.Forms.Label lblLichSuTitle;
        private System.Windows.Forms.DataGridView dgvLichSu;
    }
}

