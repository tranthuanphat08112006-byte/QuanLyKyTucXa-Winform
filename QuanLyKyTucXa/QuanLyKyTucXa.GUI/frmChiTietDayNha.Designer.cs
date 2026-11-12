namespace QuanLyKyTucXa.GUI
{
    partial class frmChiTietDayNha
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
            this.lblMaKhu = new System.Windows.Forms.Label();
            this.txtMaKhu = new System.Windows.Forms.TextBox();
            this.lblTenKhu = new System.Windows.Forms.Label();
            this.txtTenKhu = new System.Windows.Forms.TextBox();
            this.lblLoaiKhu = new System.Windows.Forms.Label();
            this.cmbLoaiKhu = new System.Windows.Forms.ComboBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cmbTrangThai = new System.Windows.Forms.ComboBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(148)))), ((int)(((byte)(136)))));
            this.pnlHeader.Controls.Add(this.lblFormTitle);
            this.pnlHeader.Controls.Add(this.lblFormSub);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(520, 68);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(22, 12);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(210, 25);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "THÔNG TIN DÃY NHÀ";
            // 
            // lblFormSub
            // 
            this.lblFormSub.AutoSize = true;
            this.lblFormSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFormSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
            this.lblFormSub.Location = new System.Drawing.Point(24, 39);
            this.lblFormSub.Name = "lblFormSub";
            this.lblFormSub.Size = new System.Drawing.Size(288, 15);
            this.lblFormSub.TabIndex = 1;
            this.lblFormSub.Text = "Thiết lập khu nhà / dãy nhà trực thuộc ký túc xá";
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.Controls.Add(this.lblMaKhu);
            this.pnlBody.Controls.Add(this.txtMaKhu);
            this.pnlBody.Controls.Add(this.lblTenKhu);
            this.pnlBody.Controls.Add(this.txtTenKhu);
            this.pnlBody.Controls.Add(this.lblLoaiKhu);
            this.pnlBody.Controls.Add(this.cmbLoaiKhu);
            this.pnlBody.Controls.Add(this.lblTrangThai);
            this.pnlBody.Controls.Add(this.cmbTrangThai);
            this.pnlBody.Controls.Add(this.lblMoTa);
            this.pnlBody.Controls.Add(this.txtMoTa);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 68);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(520, 272);
            this.pnlBody.TabIndex = 1;
            // 
            // lblMaKhu
            // 
            this.lblMaKhu.AutoSize = true;
            this.lblMaKhu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMaKhu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblMaKhu.Location = new System.Drawing.Point(24, 18);
            this.lblMaKhu.Name = "lblMaKhu";
            this.lblMaKhu.Size = new System.Drawing.Size(135, 17);
            this.lblMaKhu.TabIndex = 0;
            this.lblMaKhu.Text = "Mã Dãy nhà (*):";
            // 
            // txtMaKhu
            // 
            this.txtMaKhu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMaKhu.Location = new System.Drawing.Point(24, 40);
            this.txtMaKhu.Name = "txtMaKhu";
            this.txtMaKhu.Size = new System.Drawing.Size(220, 25);
            this.txtMaKhu.TabIndex = 1;
            // 
            // lblTenKhu
            // 
            this.lblTenKhu.AutoSize = true;
            this.lblTenKhu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTenKhu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTenKhu.Location = new System.Drawing.Point(272, 18);
            this.lblTenKhu.Name = "lblTenKhu";
            this.lblTenKhu.Size = new System.Drawing.Size(145, 17);
            this.lblTenKhu.TabIndex = 2;
            this.lblTenKhu.Text = "Tên Dãy nhà / Khu (*):";
            // 
            // txtTenKhu
            // 
            this.txtTenKhu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTenKhu.Location = new System.Drawing.Point(272, 40);
            this.txtTenKhu.Name = "txtTenKhu";
            this.txtTenKhu.Size = new System.Drawing.Size(220, 25);
            this.txtTenKhu.TabIndex = 3;
            // 
            // lblLoaiKhu
            // 
            this.lblLoaiKhu.AutoSize = true;
            this.lblLoaiKhu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblLoaiKhu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblLoaiKhu.Location = new System.Drawing.Point(24, 82);
            this.lblLoaiKhu.Name = "lblLoaiKhu";
            this.lblLoaiKhu.Size = new System.Drawing.Size(140, 17);
            this.lblLoaiKhu.TabIndex = 4;
            this.lblLoaiKhu.Text = "Đối tượng lưu trú (*):";
            // 
            // cmbLoaiKhu
            // 
            this.cmbLoaiKhu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoaiKhu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbLoaiKhu.FormattingEnabled = true;
            this.cmbLoaiKhu.Items.AddRange(new object[] {
            "Nam",
            "Nữ",
            "Hỗn hợp"});
            this.cmbLoaiKhu.Location = new System.Drawing.Point(24, 104);
            this.cmbLoaiKhu.Name = "cmbLoaiKhu";
            this.cmbLoaiKhu.Size = new System.Drawing.Size(220, 25);
            this.cmbLoaiKhu.TabIndex = 5;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTrangThai.Location = new System.Drawing.Point(272, 82);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(150, 17);
            this.lblTrangThai.TabIndex = 6;
            this.lblTrangThai.Text = "Trạng thái hoạt động:";
            // 
            // cmbTrangThai
            // 
            this.cmbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbTrangThai.FormattingEnabled = true;
            this.cmbTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Bảo trì",
            "Tạm ngưng"});
            this.cmbTrangThai.Location = new System.Drawing.Point(272, 104);
            this.cmbTrangThai.Name = "cmbTrangThai";
            this.cmbTrangThai.Size = new System.Drawing.Size(220, 25);
            this.cmbTrangThai.TabIndex = 7;
            // 
            // lblMoTa
            // 
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMoTa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblMoTa.Location = new System.Drawing.Point(24, 148);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(125, 17);
            this.lblMoTa.TabIndex = 8;
            this.lblMoTa.Text = "Mô tả / Ghi chú:";
            // 
            // txtMoTa
            // 
            this.txtMoTa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMoTa.Location = new System.Drawing.Point(24, 170);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.Size = new System.Drawing.Size(468, 76);
            this.txtMoTa.TabIndex = 9;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlFooter.Controls.Add(this.btnLuu);
            this.pnlFooter.Controls.Add(this.btnHuy);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 340);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(520, 62);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(148)))), ((int)(((byte)(136)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(362, 13);
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
            this.btnHuy.Location = new System.Drawing.Point(252, 13);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(100, 36);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy bỏ";
            this.btnHuy.UseVisualStyleBackColor = false;
            // 
            // frmChiTietDayNha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(520, 402);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChiTietDayNha";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thông tin Dãy nhà";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblFormSub;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblMaKhu;
        private System.Windows.Forms.TextBox txtMaKhu;
        private System.Windows.Forms.Label lblTenKhu;
        private System.Windows.Forms.TextBox txtTenKhu;
        private System.Windows.Forms.Label lblLoaiKhu;
        private System.Windows.Forms.ComboBox cmbLoaiKhu;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cmbTrangThai;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}
