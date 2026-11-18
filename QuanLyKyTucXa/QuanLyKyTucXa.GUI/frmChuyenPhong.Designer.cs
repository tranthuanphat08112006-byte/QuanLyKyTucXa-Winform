namespace QuanLyKyTucXa.GUI
{
    partial class frmChuyenPhong
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
            this.lblThongTinPhongMoi = new System.Windows.Forms.Label();
            this.dtpNgayKetThuc = new System.Windows.Forms.DateTimePicker();
            this.lblNgayKetThuc = new System.Windows.Forms.Label();
            this.dtpNgayBatDau = new System.Windows.Forms.DateTimePicker();
            this.lblNgayBatDau = new System.Windows.Forms.Label();
            this.cmbPhongMoi = new System.Windows.Forms.ComboBox();
            this.lblPhongMoi = new System.Windows.Forms.Label();
            this.txtPhongHienTai = new System.Windows.Forms.TextBox();
            this.lblPhongHienTai = new System.Windows.Forms.Label();
            this.txtSinhVien = new System.Windows.Forms.TextBox();
            this.lblSinhVien = new System.Windows.Forms.Label();
            this.txtMaHopDongCu = new System.Windows.Forms.TextBox();
            this.lblMaHDCu = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnXacNhan = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.pnlHeader.Controls.Add(this.lblFormTitle);
            this.pnlHeader.Controls.Add(this.lblFormSub);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(560, 68);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(22, 12);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(262, 25);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "CHUYỂN PHÒNG LƯU TRÚ";
            // 
            // lblFormSub
            // 
            this.lblFormSub.AutoSize = true;
            this.lblFormSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFormSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.lblFormSub.Location = new System.Drawing.Point(24, 39);
            this.lblFormSub.Name = "lblFormSub";
            this.lblFormSub.Size = new System.Drawing.Size(392, 15);
            this.lblFormSub.TabIndex = 1;
            this.lblFormSub.Text = "Đóng hợp đồng hiện tại, hoàn trả giường cũ và tạo hợp đồng phòng mới";
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.Controls.Add(this.lblThongTinPhongMoi);
            this.pnlBody.Controls.Add(this.dtpNgayKetThuc);
            this.pnlBody.Controls.Add(this.lblNgayKetThuc);
            this.pnlBody.Controls.Add(this.dtpNgayBatDau);
            this.pnlBody.Controls.Add(this.lblNgayBatDau);
            this.pnlBody.Controls.Add(this.cmbPhongMoi);
            this.pnlBody.Controls.Add(this.lblPhongMoi);
            this.pnlBody.Controls.Add(this.txtPhongHienTai);
            this.pnlBody.Controls.Add(this.lblPhongHienTai);
            this.pnlBody.Controls.Add(this.txtSinhVien);
            this.pnlBody.Controls.Add(this.lblSinhVien);
            this.pnlBody.Controls.Add(this.txtMaHopDongCu);
            this.pnlBody.Controls.Add(this.lblMaHDCu);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 68);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(560, 290);
            this.pnlBody.TabIndex = 1;
            // 
            // lblMaHDCu
            // 
            this.lblMaHDCu.AutoSize = true;
            this.lblMaHDCu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMaHDCu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblMaHDCu.Location = new System.Drawing.Point(24, 20);
            this.lblMaHDCu.Name = "lblMaHDCu";
            this.lblMaHDCu.Size = new System.Drawing.Size(112, 17);
            this.lblMaHDCu.TabIndex = 0;
            this.lblMaHDCu.Text = "Mã hợp đồng cũ:";
            // 
            // txtMaHopDongCu
            // 
            this.txtMaHopDongCu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtMaHopDongCu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtMaHopDongCu.Location = new System.Drawing.Point(168, 17);
            this.txtMaHopDongCu.Name = "txtMaHopDongCu";
            this.txtMaHopDongCu.ReadOnly = true;
            this.txtMaHopDongCu.Size = new System.Drawing.Size(364, 24);
            this.txtMaHopDongCu.TabIndex = 1;
            // 
            // lblSinhVien
            // 
            this.lblSinhVien.AutoSize = true;
            this.lblSinhVien.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSinhVien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblSinhVien.Location = new System.Drawing.Point(24, 58);
            this.lblSinhVien.Name = "lblSinhVien";
            this.lblSinhVien.Size = new System.Drawing.Size(75, 17);
            this.lblSinhVien.TabIndex = 2;
            this.lblSinhVien.Text = "Sinh viên:";
            // 
            // txtSinhVien
            // 
            this.txtSinhVien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtSinhVien.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSinhVien.Location = new System.Drawing.Point(168, 55);
            this.txtSinhVien.Name = "txtSinhVien";
            this.txtSinhVien.ReadOnly = true;
            this.txtSinhVien.Size = new System.Drawing.Size(364, 24);
            this.txtSinhVien.TabIndex = 3;
            // 
            // lblPhongHienTai
            // 
            this.lblPhongHienTai.AutoSize = true;
            this.lblPhongHienTai.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPhongHienTai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblPhongHienTai.Location = new System.Drawing.Point(24, 96);
            this.lblPhongHienTai.Name = "lblPhongHienTai";
            this.lblPhongHienTai.Size = new System.Drawing.Size(102, 17);
            this.lblPhongHienTai.TabIndex = 4;
            this.lblPhongHienTai.Text = "Phòng hiện tại:";
            // 
            // txtPhongHienTai
            // 
            this.txtPhongHienTai.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtPhongHienTai.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPhongHienTai.Location = new System.Drawing.Point(168, 93);
            this.txtPhongHienTai.Name = "txtPhongHienTai";
            this.txtPhongHienTai.ReadOnly = true;
            this.txtPhongHienTai.Size = new System.Drawing.Size(364, 24);
            this.txtPhongHienTai.TabIndex = 5;
            // 
            // lblPhongMoi
            // 
            this.lblPhongMoi.AutoSize = true;
            this.lblPhongMoi.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPhongMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblPhongMoi.Location = new System.Drawing.Point(24, 138);
            this.lblPhongMoi.Name = "lblPhongMoi";
            this.lblPhongMoi.Size = new System.Drawing.Size(124, 17);
            this.lblPhongMoi.TabIndex = 6;
            this.lblPhongMoi.Text = "Chuyển sang phòng (*):";
            // 
            // cmbPhongMoi
            // 
            this.cmbPhongMoi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPhongMoi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPhongMoi.FormattingEnabled = true;
            this.cmbPhongMoi.Location = new System.Drawing.Point(168, 135);
            this.cmbPhongMoi.Name = "cmbPhongMoi";
            this.cmbPhongMoi.Size = new System.Drawing.Size(364, 25);
            this.cmbPhongMoi.TabIndex = 7;
            this.cmbPhongMoi.SelectedIndexChanged += new System.EventHandler(this.cmbPhongMoi_SelectedIndexChanged);
            // 
            // lblThongTinPhongMoi
            // 
            this.lblThongTinPhongMoi.AutoSize = true;
            this.lblThongTinPhongMoi.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblThongTinPhongMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblThongTinPhongMoi.Location = new System.Drawing.Point(168, 165);
            this.lblThongTinPhongMoi.Name = "lblThongTinPhongMoi";
            this.lblThongTinPhongMoi.Size = new System.Drawing.Size(115, 15);
            this.lblThongTinPhongMoi.TabIndex = 8;
            this.lblThongTinPhongMoi.Text = "Chưa chọn phòng mới";
            // 
            // lblNgayBatDau
            // 
            this.lblNgayBatDau.AutoSize = true;
            this.lblNgayBatDau.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNgayBatDau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblNgayBatDau.Location = new System.Drawing.Point(24, 198);
            this.lblNgayBatDau.Name = "lblNgayBatDau";
            this.lblNgayBatDau.Size = new System.Drawing.Size(112, 17);
            this.lblNgayBatDau.TabIndex = 9;
            this.lblNgayBatDau.Text = "Ngày bắt đầu mới:";
            // 
            // dtpNgayBatDau
            // 
            this.dtpNgayBatDau.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayBatDau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNgayBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayBatDau.Location = new System.Drawing.Point(168, 194);
            this.dtpNgayBatDau.Name = "dtpNgayBatDau";
            this.dtpNgayBatDau.Size = new System.Drawing.Size(180, 25);
            this.dtpNgayBatDau.TabIndex = 10;
            // 
            // lblNgayKetThuc
            // 
            this.lblNgayKetThuc.AutoSize = true;
            this.lblNgayKetThuc.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNgayKetThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblNgayKetThuc.Location = new System.Drawing.Point(24, 240);
            this.lblNgayKetThuc.Name = "lblNgayKetThuc";
            this.lblNgayKetThuc.Size = new System.Drawing.Size(115, 17);
            this.lblNgayKetThuc.TabIndex = 11;
            this.lblNgayKetThuc.Text = "Ngày kết thúc mới:";
            // 
            // dtpNgayKetThuc
            // 
            this.dtpNgayKetThuc.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayKetThuc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNgayKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayKetThuc.Location = new System.Drawing.Point(168, 236);
            this.dtpNgayKetThuc.Name = "dtpNgayKetThuc";
            this.dtpNgayKetThuc.Size = new System.Drawing.Size(180, 25);
            this.dtpNgayKetThuc.TabIndex = 12;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlFooter.Controls.Add(this.btnHuy);
            this.pnlFooter.Controls.Add(this.btnXacNhan);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 358);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(560, 62);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXacNhan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.btnXacNhan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXacNhan.FlatAppearance.BorderSize = 0;
            this.btnXacNhan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnXacNhan.ForeColor = System.Drawing.Color.White;
            this.btnXacNhan.Location = new System.Drawing.Point(276, 13);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(160, 36);
            this.btnXacNhan.TabIndex = 0;
            this.btnXacNhan.Text = "Xác nhận chuyển phòng";
            this.btnXacNhan.UseVisualStyleBackColor = false;
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnHuy.Location = new System.Drawing.Point(446, 13);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(86, 36);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            // 
            // frmChuyenPhong
            // 
            this.AcceptButton = this.btnXacNhan;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(560, 420);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChuyenPhong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chuyển phòng lưu trú";
            this.Load += new System.EventHandler(this.frmChuyenPhong_Load);
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
        private System.Windows.Forms.Label lblMaHDCu;
        private System.Windows.Forms.TextBox txtMaHopDongCu;
        private System.Windows.Forms.Label lblSinhVien;
        private System.Windows.Forms.TextBox txtSinhVien;
        private System.Windows.Forms.Label lblPhongHienTai;
        private System.Windows.Forms.TextBox txtPhongHienTai;
        private System.Windows.Forms.Label lblPhongMoi;
        private System.Windows.Forms.ComboBox cmbPhongMoi;
        private System.Windows.Forms.Label lblThongTinPhongMoi;
        private System.Windows.Forms.Label lblNgayBatDau;
        private System.Windows.Forms.DateTimePicker dtpNgayBatDau;
        private System.Windows.Forms.Label lblNgayKetThuc;
        private System.Windows.Forms.DateTimePicker dtpNgayKetThuc;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnXacNhan;
        private System.Windows.Forms.Button btnHuy;
    }
}

