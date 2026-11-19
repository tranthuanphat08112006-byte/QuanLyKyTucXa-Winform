using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyKyTucXa.GUI
{
    public static class UIHelper
    {
        public static void FormatModernDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgv.RowTemplate.Height = 35;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 240, 241);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Cập nhật nhãn hiển thị trạng thái bộ lọc độ mạnh mật khẩu theo thời gian thực.
        /// </summary>
        public static void UpdatePasswordStrengthLabel(Label lbl, string password, string emptyHint)
        {
            if (lbl == null) return;

            if (string.IsNullOrEmpty(password))
            {
                lbl.Text = emptyHint;
                lbl.ForeColor = Color.FromArgb(100, 116, 139);
                return;
            }

            bool hasMinLength, hasUpper, hasLower, hasDigit, hasSpecial, hasNoWhitespace;
            bool isValid = QuanLyKyTucXa.BLL.TaiKhoanBLL.EvaluatePasswordStrength(
                password,
                out hasMinLength,
                out hasUpper,
                out hasLower,
                out hasDigit,
                out hasSpecial,
                out hasNoWhitespace);

            lbl.Text = string.Format(
                "{0} ≥6 ký tự  {1} Hoa  {2} Thường  {3} Số  {4} Ký tự ĐB{5}",
                hasMinLength ? "✓" : "✗",
                hasUpper ? "✓" : "✗",
                hasLower ? "✓" : "✗",
                hasDigit ? "✓" : "✗",
                hasSpecial ? "✓" : "✗",
                hasNoWhitespace ? "" : "  ✗ Không khoảng trắng");

            lbl.ForeColor = isValid
                ? Color.FromArgb(22, 163, 74)
                : Color.FromArgb(220, 38, 38);
        }
    }

    /// <summary>
    /// Nút bấm bo góc chuẩn Modern Flat Design, loại bỏ triệt để viền vuông phía sau
    /// </summary>
    public class ModernMenuButton : Button
    {
        private int _borderRadius = 10;
        private bool _isActive = false;
        private bool _isHovered = false;

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; UpdateRegion(); Invalidate(); }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                Invalidate();
            }
        }

        public Color NormalBackColor { get; set; } = Color.FromArgb(24, 30, 54);
        public Color HoverBackColor { get; set; } = Color.FromArgb(37, 45, 75);
        public Color ActiveBackColor { get; set; } = Color.FromArgb(37, 99, 235);
        public Color NormalForeColor { get; set; } = Color.FromArgb(203, 213, 225);
        public Color ActiveForeColor { get; set; } = Color.White;
        public Color HoverForeColor { get; set; } = Color.White;

        public ModernMenuButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Cursor = Cursors.Hand;
            this.DoubleBuffered = true;
            this.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            this.Padding = new Padding(14, 0, 0, 0);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            if (Width > 0 && Height > 0)
            {
                using (GraphicsPath path = GetRoundedPath(new Rectangle(0, 0, Width, Height), _borderRadius))
                {
                    this.Region = new Region(path);
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Color bg = _isActive ? ActiveBackColor : (_isHovered ? HoverBackColor : NormalBackColor);
            Color fg = _isActive ? ActiveForeColor : (_isHovered ? HoverForeColor : NormalForeColor);

            Rectangle rect = new Rectangle(0, 0, Width, Height);

            // 1. Vẽ nền bo góc (Đảm bảo Region cắt sạch các góc vuông bên ngoài)
            using (GraphicsPath path = GetRoundedPath(rect, _borderRadius))
            {
                using (SolidBrush brush = new SolidBrush(bg))
                {
                    g.FillPath(brush, path);
                }
            }

            // 2. Vẽ Icon sắc nét
            int textX = Padding.Left;
            if (Image != null)
            {
                int iconY = (Height - Image.Height) / 2;
                g.DrawImage(Image, textX, iconY, Image.Width, Image.Height);
                textX += Image.Width + 12;
            }

            // 3. Vẽ Tiêu đề chữ
            if (!string.IsNullOrEmpty(Text))
            {
                using (SolidBrush textBrush = new SolidBrush(fg))
                {
                    using (StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter
                    })
                    {
                        Rectangle textRect = new Rectangle(textX, 0, Width - textX - Padding.Right, Height);
                        g.DrawString(Text, Font, textBrush, textRect, sf);
                    }
                }
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            int d = radius * 2;
            Rectangle arc = new Rectangle(rect.X, rect.Y, d, d);

            path.AddArc(arc, 180, 90);

            arc.X = rect.Right - d;
            path.AddArc(arc, 270, 90);

            arc.Y = rect.Bottom - d;
            path.AddArc(arc, 0, 90);

            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Bộ sinh Icon Vector GDI+ thuần tuý, không lo bị lỗi font hay ô vuông trên Windows
    /// </summary>
    public static class IconHelper
    {
        public static Bitmap CreateStudentIcon(Color color, int size = 20)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(color))
                using (Pen pen = new Pen(color, Math.Max(1.5f, size * 0.08f)))
                {
                    // Mũ cử nhân
                    PointF[] diamond = new PointF[]
                    {
                        new PointF(size * 0.5f, size * 0.18f),
                        new PointF(size * 0.95f, size * 0.40f),
                        new PointF(size * 0.5f, size * 0.62f),
                        new PointF(size * 0.05f, size * 0.40f)
                    };
                    g.FillPolygon(brush, diamond);
                    g.FillRectangle(brush, size * 0.28f, size * 0.60f, size * 0.44f, size * 0.22f);

                    // Dải tua nón
                    g.DrawLine(pen, size * 0.85f, size * 0.45f, size * 0.85f, size * 0.72f);
                    g.FillEllipse(brush, size * 0.80f, size * 0.70f, size * 0.12f, size * 0.12f);
                }
            }
            return bmp;
        }

        public static Bitmap CreateRoomIcon(Color color, int size = 20)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(color))
                using (Pen pen = new Pen(color, Math.Max(1.6f, size * 0.08f)))
                {
                    // Thân toà nhà
                    g.DrawRectangle(pen, size * 0.15f, size * 0.18f, size * 0.70f, size * 0.74f);
                    g.FillRectangle(brush, size * 0.10f, size * 0.10f, size * 0.80f, size * 0.10f);

                    // Các ô cửa sổ
                    float wSize = size * 0.14f;
                    g.FillRectangle(brush, size * 0.26f, size * 0.30f, wSize, wSize);
                    g.FillRectangle(brush, size * 0.60f, size * 0.30f, wSize, wSize);
                    g.FillRectangle(brush, size * 0.26f, size * 0.52f, wSize, wSize);
                    g.FillRectangle(brush, size * 0.60f, size * 0.52f, wSize, wSize);

                    // Cửa chính
                    g.FillRectangle(brush, size * 0.42f, size * 0.70f, size * 0.18f, size * 0.22f);
                }
            }
            return bmp;
        }

        public static Bitmap CreateContractIcon(Color color, int size = 20)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(color))
                using (Pen pen = new Pen(color, Math.Max(1.6f, size * 0.08f)))
                {
                    // Khung tài liệu hợp đồng
                    g.DrawRectangle(pen, size * 0.18f, size * 0.10f, size * 0.64f, size * 0.80f);

                    // Các dòng nội dung
                    g.DrawLine(pen, size * 0.30f, size * 0.32f, size * 0.70f, size * 0.32f);
                    g.DrawLine(pen, size * 0.30f, size * 0.50f, size * 0.70f, size * 0.50f);
                    g.DrawLine(pen, size * 0.30f, size * 0.68f, size * 0.54f, size * 0.68f);

                    // Dấu tích / chữ ký nhỏ góc dưới phải
                    g.FillEllipse(brush, size * 0.60f, size * 0.64f, size * 0.14f, size * 0.14f);
                }
            }
            return bmp;
        }

        public static Bitmap CreateLogoutIcon(Color color, int size = 20)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(color))
                using (Pen pen = new Pen(color, Math.Max(1.8f, size * 0.09f)))
                {
                    // Khung cửa
                    g.DrawLine(pen, size * 0.55f, size * 0.15f, size * 0.20f, size * 0.15f);
                    g.DrawLine(pen, size * 0.20f, size * 0.15f, size * 0.20f, size * 0.85f);
                    g.DrawLine(pen, size * 0.20f, size * 0.85f, size * 0.55f, size * 0.85f);

                    // Mũi tên thoát
                    g.DrawLine(pen, size * 0.38f, size * 0.50f, size * 0.82f, size * 0.50f);
                    PointF[] arrow = new PointF[]
                    {
                        new PointF(size * 0.85f, size * 0.50f),
                        new PointF(size * 0.65f, size * 0.33f),
                        new PointF(size * 0.65f, size * 0.67f)
                    };
                    g.FillPolygon(brush, arrow);
                }
            }
            return bmp;
        }

        public static Bitmap CreateDormLogoIcon(Color color, int size = 28)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(color))
                using (Pen pen = new Pen(color, 2f))
                {
                    // Mái tam giác
                    PointF[] roof = new PointF[]
                    {
                        new PointF(size * 0.5f, size * 0.08f),
                        new PointF(size * 0.95f, size * 0.38f),
                        new PointF(size * 0.05f, size * 0.38f)
                    };
                    g.FillPolygon(brush, roof);

                    // Dầm mái
                    g.FillRectangle(brush, size * 0.08f, size * 0.40f, size * 0.84f, size * 0.08f);

                    // 3 Cột trụ
                    float colWidth = size * 0.12f;
                    float colHeight = size * 0.36f;
                    g.FillRectangle(brush, size * 0.15f, size * 0.50f, colWidth, colHeight);
                    g.FillRectangle(brush, size * 0.44f, size * 0.50f, colWidth, colHeight);
                    g.FillRectangle(brush, size * 0.73f, size * 0.50f, colWidth, colHeight);

                    // Nền móng
                    g.FillRectangle(brush, size * 0.05f, size * 0.88f, size * 0.90f, size * 0.08f);
                }
            }
            return bmp;
        }

        public static Bitmap CreateUserAvatarIcon(Color bgColor, Color fgColor, int size = 36)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush bgBrush = new SolidBrush(bgColor))
                {
                    g.FillEllipse(bgBrush, 0, 0, size - 1, size - 1);
                }

                using (SolidBrush fgBrush = new SolidBrush(fgColor))
                {
                    // Đầu
                    float headSize = size * 0.34f;
                    float headX = (size - headSize) / 2;
                    float headY = size * 0.18f;
                    g.FillEllipse(fgBrush, headX, headY, headSize, headSize);

                    // Vai
                    GraphicsPath bodyPath = new GraphicsPath();
                    bodyPath.AddArc(size * 0.18f, size * 0.55f, size * 0.64f, size * 0.60f, 180, 180);
                    bodyPath.CloseFigure();
                    g.FillPath(fgBrush, bodyPath);
                }
            }
            return bmp;
        }

        public static Bitmap CreateAccountIcon(Color color, int size = 20)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(color))
                using (Pen pen = new Pen(color, Math.Max(1.5f, size * 0.08f)))
                {
                    // Đầu người dùng
                    float headSize = size * 0.32f;
                    float headX = (size - headSize) / 2;
                    float headY = size * 0.12f;
                    g.DrawEllipse(pen, headX, headY, headSize, headSize);

                    // Thân người dùng
                    GraphicsPath bodyPath = new GraphicsPath();
                    bodyPath.AddArc(size * 0.15f, size * 0.48f, size * 0.70f, size * 0.50f, 180, 180);
                    g.DrawPath(pen, bodyPath);

                    // Dấu khiên bảo mật nhỏ góc dưới
                    PointF[] shield = new PointF[]
                    {
                        new PointF(size * 0.65f, size * 0.60f),
                        new PointF(size * 0.90f, size * 0.60f),
                        new PointF(size * 0.90f, size * 0.78f),
                        new PointF(size * 0.775f, size * 0.92f),
                        new PointF(size * 0.65f, size * 0.78f)
                    };
                    g.FillPolygon(brush, shield);
                }
            }
            return bmp;
        }

        public static Bitmap CreateKeyIcon(Color color, int size = 20)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, Math.Max(1.8f, size * 0.09f)))
                using (SolidBrush brush = new SolidBrush(color))
                {
                    // Vòng tròn đầu chìa khóa
                    g.DrawEllipse(pen, size * 0.12f, size * 0.15f, size * 0.38f, size * 0.38f);
                    // Lỗ khóa nhỏ
                    g.FillEllipse(brush, size * 0.25f, size * 0.28f, size * 0.12f, size * 0.12f);

                    // Thân chìa khóa
                    g.DrawLine(pen, size * 0.45f, size * 0.45f, size * 0.85f, size * 0.85f);
                    // Răng chìa khóa
                    g.DrawLine(pen, size * 0.70f, size * 0.70f, size * 0.80f, size * 0.60f);
                    g.DrawLine(pen, size * 0.80f, size * 0.80f, size * 0.90f, size * 0.70f);
                }
            }
            return bmp;
        }

        public static Bitmap CreateBackupIcon(Color color, int size = 20)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, Math.Max(1.5f, size * 0.08f)))
                using (SolidBrush brush = new SolidBrush(color))
                {
                    // 3 đĩa ổ cứng dữ liệu (Database Stack)
                    float w = size * 0.65f;
                    float h = size * 0.20f;
                    float x = size * 0.175f;

                    // Đĩa 1 (trên cùng)
                    g.DrawEllipse(pen, x, size * 0.12f, w, h);
                    // Đĩa 2 (giữa)
                    g.DrawArc(pen, x, size * 0.36f, w, h, 0, 180);
                    // Đĩa 3 (dưới)
                    g.DrawArc(pen, x, size * 0.60f, w, h, 0, 180);
                    // Đường thẳng hai bên nối đĩa
                    g.DrawLine(pen, x, size * 0.22f, x, size * 0.70f);
                    g.DrawLine(pen, x + w, size * 0.22f, x + w, size * 0.70f);
                }
            }
            return bmp;
        }

        public static Bitmap CreateEyeIcon(Color color, int size = 16, bool isOpen = true)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 1.5f))
                using (SolidBrush brush = new SolidBrush(color))
                {
                    // Đường cong mí mắt
                    GraphicsPath eyePath = new GraphicsPath();
                    eyePath.AddArc(size * 0.1f, size * 0.25f, size * 0.8f, size * 0.6f, 200, 140);
                    eyePath.AddArc(size * 0.1f, size * 0.15f, size * 0.8f, size * 0.6f, 20, 140);
                    g.DrawPath(pen, eyePath);

                    if (isOpen)
                    {
                        // Con ngươi mắt
                        g.FillEllipse(brush, size * 0.38f, size * 0.38f, size * 0.24f, size * 0.24f);
                    }
                    else
                    {
                        // Gạch chéo mắt khi ẩn
                        g.DrawLine(pen, size * 0.18f, size * 0.82f, size * 0.82f, size * 0.18f);
                    }
                }
            }
            return bmp;
        }
    }
}
