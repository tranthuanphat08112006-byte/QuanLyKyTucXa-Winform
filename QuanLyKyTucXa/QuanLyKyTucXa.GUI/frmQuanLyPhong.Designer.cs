namespace QuanLyKyTucXa.GUI
{
    partial class frmQuanLyPhong
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
            this.ucPhong1 = new QuanLyKyTucXa.GUI.UCPhong();
            this.SuspendLayout();
            // 
            // ucPhong1
            // 
            this.ucPhong1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPhong1.Location = new System.Drawing.Point(0, 0);
            this.ucPhong1.Name = "ucPhong1";
            this.ucPhong1.Size = new System.Drawing.Size(800, 450);
            this.ucPhong1.TabIndex = 0;
            // 
            // frmQuanLyPhong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ucPhong1);
            this.Name = "frmQuanLyPhong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý Phòng và Dãy nhà (Thiết lập giá, sức chứa)";
            this.ResumeLayout(false);

        }

        #endregion

        private UCPhong ucPhong1;
    }
}
