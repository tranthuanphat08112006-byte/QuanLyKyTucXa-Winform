namespace QuanLyKyTucXa.GUI
{
    partial class frmChiTietVaiTro
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
            this.lblMaVaiTro = new System.Windows.Forms.Label();
            this.txtMaVaiTro = new System.Windows.Forms.TextBox();
            this.lblTenVaiTro = new System.Windows.Forms.Label();
            this.txtTenVaiTro = new System.Windows.Forms.TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.lblHint = new System.Windows.Forms.Label();
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
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.pnlHeader.Controls.Add(this.lblFormTitle);
            this.pnlHeader.Controls.Add(this.lblFormSub);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            this.pnlHeader.Size = new System.Drawing.Size(500, 80);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(22, 16);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(190, 23);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "THÊM VAI TRÒ MỚI";
            // 
            // lblFormSub
            // 
            this.lblFormSub.AutoSize = true;
            this.lblFormSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFormSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.lblFormSub.Location = new System.Drawing.Point(24, 43);
            this.lblFormSub.Name = "lblFormSub";
            this.lblFormSub.Size = new System.Drawing.Size(320, 15);
            this.lblFormSub.TabIndex = 1;
            this.lblFormSub.Text = "Nhập mã, tên hiển thị và mô tả cho vai trò hệ thống";
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.Controls.Add(this.lblHint);
            this.pnlBody.Controls.Add(this.txtMoTa);
            this.pnlBody.Controls.Add(this.lblMoTa);
            this.pnlBody.Controls.Add(this.txtTenVaiTro);
            this.pnlBody.Controls.Add(this.lblTenVaiTro);
            this.pnlBody.Controls.Add(this.txtMaVaiTro);
            this.pnlBody.Controls.Add(this.lblMaVaiTro);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 80);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(28, 20, 28, 20);
            this.pnlBody.Size = new System.Drawing.Size(500, 250);
            this.pnlBody.TabIndex = 1;
            // 
            // lblMaVaiTro
            // 
            this.lblMaVaiTro.AutoSize = true;
            this.lblMaVaiTro.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMaVaiTro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblMaVaiTro.Location = new System.Drawing.Point(28, 16);
            this.lblMaVaiTro.Name = "lblMaVaiTro";
            this.lblMaVaiTro.Size = new System.Drawing.Size(96, 17);
            this.lblMaVaiTro.TabIndex = 0;
            this.lblMaVaiTro.Text = "Mã vai trò (*):";
            // 
            // txtMaVaiTro
            // 
            this.txtMaVaiTro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMaVaiTro.Location = new System.Drawing.Point(30, 38);
            this.txtMaVaiTro.MaxLength = 20;
            this.txtMaVaiTro.Name = "txtMaVaiTro";
            this.txtMaVaiTro.Size = new System.Drawing.Size(185, 25);
            this.txtMaVaiTro.TabIndex = 1;
            // 
            // lblTenVaiTro
            // 
            this.lblTenVaiTro.AutoSize = true;
            this.lblTenVaiTro.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTenVaiTro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblTenVaiTro.Location = new System.Drawing.Point(234, 16);
            this.lblTenVaiTro.Name = "lblTenVaiTro";
            this.lblTenVaiTro.Size = new System.Drawing.Size(102, 17);
            this.lblTenVaiTro.TabIndex = 2;
            this.lblTenVaiTro.Text = "Tên vai trò (*):";
            // 
            // txtTenVaiTro
            // 
            this.txtTenVaiTro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTenVaiTro.Location = new System.Drawing.Point(236, 38);
            this.txtTenVaiTro.MaxLength = 50;
            this.txtTenVaiTro.Name = "txtTenVaiTro";
            this.txtTenVaiTro.Size = new System.Drawing.Size(234, 25);
            this.txtTenVaiTro.TabIndex = 3;
            // 
            // lblMoTa
            // 
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMoTa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblMoTa.Location = new System.Drawing.Point(28, 82);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(118, 17);
            this.lblMoTa.TabIndex = 4;
            this.lblMoTa.Text = "Mô tả chức năng:";
            // 
            // txtMoTa
            // 
            this.txtMoTa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMoTa.Location = new System.Drawing.Point(30, 104);
            this.txtMoTa.MaxLength = 255;
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.Size = new System.Drawing.Size(440, 82);
            this.txtMoTa.TabIndex = 5;
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblHint.Location = new System.Drawing.Point(28, 198);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(420, 13);
            this.lblHint.TabIndex = 6;
            this.lblHint.Text = "Gợi ý: Mã vai trò viết liền không dấu (VD: KeToan, QuanLyPhong, NhanVienTruc).";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFooter.Controls.Add(this.btnLuu);
            this.pnlFooter.Controls.Add(this.btnHuy);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 330);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(500, 60);
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
            this.btnLuu.Location = new System.Drawing.Point(262, 12);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(115, 34);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "Lưu vai trò";
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
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnHuy.Location = new System.Drawing.Point(387, 12);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(85, 34);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy bỏ";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // frmChiTietVaiTro
            // 
            this.AcceptButton = this.btnLuu;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(500, 390);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChiTietVaiTro";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thông tin Vai trò";
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
        private System.Windows.Forms.Label lblMaVaiTro;
        private System.Windows.Forms.TextBox txtMaVaiTro;
        private System.Windows.Forms.Label lblTenVaiTro;
        private System.Windows.Forms.TextBox txtTenVaiTro;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}
