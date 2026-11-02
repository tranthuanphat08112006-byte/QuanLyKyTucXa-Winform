namespace QuanLyKyTucXa.GUI
{
    partial class frmChiTietPhong
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
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaPhong = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTenKhu = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLoaiPhong = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudSoGiuongToiDa = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudSoGiuongTrong = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoGiuongToiDa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoGiuongTrong)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(388, 30);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(75, 32);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.Location = new System.Drawing.Point(388, 90);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(75, 32);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Mã phòng:";
            // 
            // txtMaPhong
            // 
            this.txtMaPhong.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtMaPhong.Location = new System.Drawing.Point(150, 27);
            this.txtMaPhong.Name = "txtMaPhong";
            this.txtMaPhong.Size = new System.Drawing.Size(150, 20);
            this.txtMaPhong.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Dãy nhà (Khu):";
            // 
            // cmbTenKhu
            // 
            this.cmbTenKhu.FormattingEnabled = true;
            this.cmbTenKhu.Items.AddRange(new object[] {
            "Khu A",
            "Khu B",
            "Khu C",
            "Khu D"});
            this.cmbTenKhu.Location = new System.Drawing.Point(150, 67);
            this.cmbTenKhu.Name = "cmbTenKhu";
            this.cmbTenKhu.Size = new System.Drawing.Size(150, 21);
            this.cmbTenKhu.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Loại phòng:";
            // 
            // cmbLoaiPhong
            // 
            this.cmbLoaiPhong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoaiPhong.FormattingEnabled = true;
            this.cmbLoaiPhong.Items.AddRange(new object[] {
            "Nam",
            "Nữ",
            "Giảng viên"});
            this.cmbLoaiPhong.Location = new System.Drawing.Point(150, 112);
            this.cmbLoaiPhong.Name = "cmbLoaiPhong";
            this.cmbLoaiPhong.Size = new System.Drawing.Size(150, 21);
            this.cmbLoaiPhong.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Sức chứa (Số giường):";
            // 
            // nudSoGiuongToiDa
            // 
            this.nudSoGiuongToiDa.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.nudSoGiuongToiDa.Location = new System.Drawing.Point(150, 158);
            this.nudSoGiuongToiDa.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudSoGiuongToiDa.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSoGiuongToiDa.Name = "nudSoGiuongToiDa";
            this.nudSoGiuongToiDa.Size = new System.Drawing.Size(150, 20);
            this.nudSoGiuongToiDa.TabIndex = 9;
            this.nudSoGiuongToiDa.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudSoGiuongToiDa.ValueChanged += new System.EventHandler(this.nudSoGiuongToiDa_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Số giường trống:";
            // 
            // nudSoGiuongTrong
            // 
            this.nudSoGiuongTrong.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.nudSoGiuongTrong.Location = new System.Drawing.Point(150, 198);
            this.nudSoGiuongTrong.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudSoGiuongTrong.Name = "nudSoGiuongTrong";
            this.nudSoGiuongTrong.Size = new System.Drawing.Size(150, 20);
            this.nudSoGiuongTrong.TabIndex = 11;
            this.nudSoGiuongTrong.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Đơn giá (VNĐ/tháng):";
            // 
            // txtDonGia
            // 
            this.txtDonGia.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtDonGia.Location = new System.Drawing.Point(150, 237);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(150, 20);
            this.txtDonGia.TabIndex = 13;
            this.txtDonGia.Text = "500000";
            // 
            // frmChiTietPhong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 310);
            this.Controls.Add(this.txtDonGia);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudSoGiuongTrong);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudSoGiuongToiDa);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbLoaiPhong);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbTenKhu);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMaPhong);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChiTietPhong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin Phòng & Dãy nhà";
            ((System.ComponentModel.ISupportInitialize)(this.nudSoGiuongToiDa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoGiuongTrong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaPhong;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTenKhu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLoaiPhong;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudSoGiuongToiDa;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudSoGiuongTrong;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDonGia;
    }
}
