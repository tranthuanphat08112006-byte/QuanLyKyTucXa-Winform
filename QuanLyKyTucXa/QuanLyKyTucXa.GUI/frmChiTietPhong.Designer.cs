namespace QuanLyKyTucXa.GUI
{
    partial class frmChiTietPhong
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
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblFormSub = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaPhong = new System.Windows.Forms.TextBox();
            this.lblTenPhong = new System.Windows.Forms.Label();
            this.txtTenPhong = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTenKhu = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLoaiPhong = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numSoGiuongToiDa = new System.Windows.Forms.NumericUpDown();
            this.lblDangO = new System.Windows.Forms.Label();
            this.numSoNguoiHienTai = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numSoGiuongTrong = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cmbTrangThai = new System.Windows.Forms.ComboBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoGiuongToiDa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNguoiHienTai)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoGiuongTrong)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.pnlHeader.Controls.Add(this.lblFormTitle);
            this.pnlHeader.Controls.Add(this.lblFormSub);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(540, 68);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(22, 12);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(218, 25);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "THÔNG TIN PHÒNG";
            // 
            // lblFormSub
            // 
            this.lblFormSub.AutoSize = true;
            this.lblFormSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFormSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.lblFormSub.Location = new System.Drawing.Point(24, 39);
            this.lblFormSub.Name = "lblFormSub";
            this.lblFormSub.Size = new System.Drawing.Size(344, 15);
            this.lblFormSub.TabIndex = 1;
            this.lblFormSub.Text = "Nhập đầy đủ thông tin phòng, sức chứa tối đa và đơn giá lưu trú";
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.Controls.Add(this.lblTrangThai);
            this.pnlBody.Controls.Add(this.cmbTrangThai);
            this.pnlBody.Controls.Add(this.txtDonGia);
            this.pnlBody.Controls.Add(this.label6);
            this.pnlBody.Controls.Add(this.numSoGiuongTrong);
            this.pnlBody.Controls.Add(this.label5);
            this.pnlBody.Controls.Add(this.numSoNguoiHienTai);
            this.pnlBody.Controls.Add(this.lblDangO);
            this.pnlBody.Controls.Add(this.numSoGiuongToiDa);
            this.pnlBody.Controls.Add(this.label4);
            this.pnlBody.Controls.Add(this.cmbLoaiPhong);
            this.pnlBody.Controls.Add(this.label3);
            this.pnlBody.Controls.Add(this.cmbTenKhu);
            this.pnlBody.Controls.Add(this.label2);
            this.pnlBody.Controls.Add(this.txtTenPhong);
            this.pnlBody.Controls.Add(this.lblTenPhong);
            this.pnlBody.Controls.Add(this.txtMaPhong);
            this.pnlBody.Controls.Add(this.label1);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 68);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(540, 310);
            this.pnlBody.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.label1.Location = new System.Drawing.Point(24, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã phòng (*):";
            // 
            // txtMaPhong
            // 
            this.txtMaPhong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMaPhong.Location = new System.Drawing.Point(24, 40);
            this.txtMaPhong.Name = "txtMaPhong";
            this.txtMaPhong.Size = new System.Drawing.Size(230, 25);
            this.txtMaPhong.TabIndex = 1;
            this.txtMaPhong.TextChanged += new System.EventHandler(this.txtMaPhong_TextChanged);
            // 
            // lblTenPhong
            // 
            this.lblTenPhong.AutoSize = true;
            this.lblTenPhong.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTenPhong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTenPhong.Location = new System.Drawing.Point(282, 18);
            this.lblTenPhong.Name = "lblTenPhong";
            this.lblTenPhong.Size = new System.Drawing.Size(128, 17);
            this.lblTenPhong.TabIndex = 2;
            this.lblTenPhong.Text = "Tên / Số phòng (*):";
            // 
            // txtTenPhong
            // 
            this.txtTenPhong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTenPhong.Location = new System.Drawing.Point(282, 40);
            this.txtTenPhong.Name = "txtTenPhong";
            this.txtTenPhong.Size = new System.Drawing.Size(230, 25);
            this.txtTenPhong.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.label2.Location = new System.Drawing.Point(24, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Dãy nhà trực thuộc (*):";
            // 
            // cmbTenKhu
            // 
            this.cmbTenKhu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTenKhu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbTenKhu.FormattingEnabled = true;
            this.cmbTenKhu.Location = new System.Drawing.Point(24, 102);
            this.cmbTenKhu.Name = "cmbTenKhu";
            this.cmbTenKhu.Size = new System.Drawing.Size(230, 25);
            this.cmbTenKhu.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.label3.Location = new System.Drawing.Point(282, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Loại phòng (*):";
            // 
            // cmbLoaiPhong
            // 
            this.cmbLoaiPhong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoaiPhong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbLoaiPhong.FormattingEnabled = true;
            this.cmbLoaiPhong.Items.AddRange(new object[] {
            "Phòng 4 người (Dịch vụ)",
            "Phòng 6 người (Tiêu chuẩn)",
            "Phòng 8 người (Tiêu chuẩn)"});
            this.cmbLoaiPhong.Location = new System.Drawing.Point(282, 102);
            this.cmbLoaiPhong.Name = "cmbLoaiPhong";
            this.cmbLoaiPhong.Size = new System.Drawing.Size(230, 25);
            this.cmbLoaiPhong.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.label4.Location = new System.Drawing.Point(24, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Sức chứa tối đa (*):";
            // 
            // numSoGiuongToiDa
            // 
            this.numSoGiuongToiDa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numSoGiuongToiDa.Location = new System.Drawing.Point(24, 166);
            this.numSoGiuongToiDa.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numSoGiuongToiDa.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSoGiuongToiDa.Name = "numSoGiuongToiDa";
            this.numSoGiuongToiDa.Size = new System.Drawing.Size(145, 25);
            this.numSoGiuongToiDa.TabIndex = 9;
            this.numSoGiuongToiDa.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numSoGiuongToiDa.ValueChanged += new System.EventHandler(this.numSoGiuongToiDa_ValueChanged);
            // 
            // lblDangO
            // 
            this.lblDangO.AutoSize = true;
            this.lblDangO.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDangO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblDangO.Location = new System.Drawing.Point(195, 144);
            this.lblDangO.Name = "lblDangO";
            this.lblDangO.Size = new System.Drawing.Size(118, 17);
            this.lblDangO.TabIndex = 10;
            this.lblDangO.Text = "Số SV hiện tại (*):";
            // 
            // numSoNguoiHienTai
            // 
            this.numSoNguoiHienTai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numSoNguoiHienTai.Location = new System.Drawing.Point(195, 166);
            this.numSoNguoiHienTai.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numSoNguoiHienTai.Name = "numSoNguoiHienTai";
            this.numSoNguoiHienTai.Size = new System.Drawing.Size(145, 25);
            this.numSoNguoiHienTai.TabIndex = 11;
            this.numSoNguoiHienTai.ValueChanged += new System.EventHandler(this.numSoNguoiHienTai_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.label5.Location = new System.Drawing.Point(367, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Giường còn trống:";
            // 
            // numSoGiuongTrong
            // 
            this.numSoGiuongTrong.Enabled = false;
            this.numSoGiuongTrong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.numSoGiuongTrong.Location = new System.Drawing.Point(367, 166);
            this.numSoGiuongTrong.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numSoGiuongTrong.Name = "numSoGiuongTrong";
            this.numSoGiuongTrong.Size = new System.Drawing.Size(145, 25);
            this.numSoGiuongTrong.TabIndex = 13;
            this.numSoGiuongTrong.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.label6.Location = new System.Drawing.Point(24, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "Đơn giá phòng (VNĐ) (*):";
            // 
            // txtDonGia
            // 
            this.txtDonGia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDonGia.Location = new System.Drawing.Point(24, 232);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(230, 25);
            this.txtDonGia.TabIndex = 15;
            this.txtDonGia.Text = "500000";
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTrangThai.Location = new System.Drawing.Point(282, 210);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(115, 17);
            this.lblTrangThai.TabIndex = 16;
            this.lblTrangThai.Text = "Trạng thái phòng:";
            // 
            // cmbTrangThai
            // 
            this.cmbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbTrangThai.FormattingEnabled = true;
            this.cmbTrangThai.Items.AddRange(new object[] {
            "Còn trống",
            "Đã đầy",
            "Bảo trì"});
            this.cmbTrangThai.Location = new System.Drawing.Point(282, 232);
            this.cmbTrangThai.Name = "cmbTrangThai";
            this.cmbTrangThai.Size = new System.Drawing.Size(230, 25);
            this.cmbTrangThai.TabIndex = 17;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlFooter.Controls.Add(this.btnLuu);
            this.pnlFooter.Controls.Add(this.btnHuy);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 378);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(540, 62);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(382, 13);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(130, 36);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "Lưu thông tin";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnHuy.Location = new System.Drawing.Point(272, 13);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(100, 36);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy bỏ";
            this.btnHuy.UseVisualStyleBackColor = false;
            // 
            // frmChiTietPhong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(540, 440);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChiTietPhong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thông tin Phòng";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoGiuongToiDa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNguoiHienTai)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoGiuongTrong)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblFormSub;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaPhong;
        private System.Windows.Forms.Label lblTenPhong;
        private System.Windows.Forms.TextBox txtTenPhong;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTenKhu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLoaiPhong;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSoGiuongToiDa;
        private System.Windows.Forms.Label lblDangO;
        private System.Windows.Forms.NumericUpDown numSoNguoiHienTai;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numSoGiuongTrong;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cmbTrangThai;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}
