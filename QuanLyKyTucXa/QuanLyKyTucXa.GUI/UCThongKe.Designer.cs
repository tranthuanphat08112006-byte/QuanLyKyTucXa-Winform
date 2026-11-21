using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKyTucXa.GUI
{
    partial class UCThongKe
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.BackColor = Color.FromArgb(243, 244, 246);
            this.Font = new Font("Segoe UI", 9F);
            this.Name = "UCThongKe";
            this.Size = new Size(1060, 740);
            this.Padding = new Padding(14, 10, 14, 12);

            BuildTopNavBar();
            BuildPageDashboard();
            BuildPagePhong();
            BuildPageDoanhThu();
            BuildPageCongNo();

            this.Controls.Add(this.pnlPageDashboard);
            this.Controls.Add(this.pnlPagePhong);
            this.Controls.Add(this.pnlPageDoanhThu);
            this.Controls.Add(this.pnlPageCongNo);
            this.Controls.Add(this.pnlTopNav);

            this.Load += new EventHandler(this.UCThongKe_Load);
            this.ResumeLayout(false);
        }

        private void BuildTopNavBar()
        {
            pnlTopNav = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 2, 0, 8)
            };

            btnTabDashboard = CreateTabButton("1. Tổng quan Dashboard", 0, 185, btnTabDashboard_Click);
            btnTabPhong = CreateTabButton("2. Thống kê Phòng", 193, 165, btnTabPhong_Click);
            btnTabDoanhThu = CreateTabButton("3. Báo cáo Doanh thu", 366, 175, btnTabDoanhThu_Click);
            btnTabCongNo = CreateTabButton("4. Công nợ & Xuất Excel", 549, 185, btnTabCongNo_Click);

            btnRefreshAll = new Button
            {
                Text = "↻ Làm mới dữ liệu",
                Size = new Size(140, 34),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefreshAll.FlatAppearance.BorderSize = 0;
            btnRefreshAll.Click += btnRefreshAll_Click;

            pnlTopNav.Controls.Add(btnRefreshAll);
            pnlTopNav.Controls.Add(btnTabDashboard);
            pnlTopNav.Controls.Add(btnTabPhong);
            pnlTopNav.Controls.Add(btnTabDoanhThu);
            pnlTopNav.Controls.Add(btnTabCongNo);
        }

        // =====================================================================
        // PAGE 0: DASHBOARD TỔNG QUAN (4 KHU VỰC)
        // =====================================================================
        private void BuildPageDashboard()
        {
            pnlPageDashboard = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            lblDashWarning = CreateWarningBanner();

            // Khu vực 1: 7 thẻ thông tin tổng quan
            TableLayoutPanel tlpCards = CreateCardGrid(7, 82);
            tlpCards.Controls.Add(CreateKpiCard("SV ĐANG LƯU TRÚ", Color.FromArgb(37, 99, 235), out lblDashSVVal, out lblDashSVSub), 0, 0);
            tlpCards.Controls.Add(CreateKpiCard("TỔNG SỐ PHÒNG", Color.FromArgb(14, 165, 233), out lblDashRoomVal, out lblDashRoomSub), 1, 0);
            tlpCards.Controls.Add(CreateKpiCard("SỐ GIƯỜNG TRỐNG", Color.FromArgb(16, 185, 129), out lblDashEmptyBedVal, out lblDashEmptyBedSub), 2, 0);
            tlpCards.Controls.Add(CreateKpiCard("PHÒNG ĐÃ ĐẦY", Color.FromArgb(139, 92, 246), out lblDashFullRoomVal, out lblDashFullRoomSub), 3, 0);
            tlpCards.Controls.Add(CreateKpiCard("TỔNG DOANH THU", Color.FromArgb(245, 158, 11), out lblDashRevVal, out lblDashRevSub), 4, 0);
            tlpCards.Controls.Add(CreateKpiCard("SV ĐANG NỢ PHÍ", Color.FromArgb(239, 68, 68), out lblDashDebtSVVal, out lblDashDebtSVSub), 5, 0);
            tlpCards.Controls.Add(CreateKpiCard("TỔNG TIỀN CÔNG NỢ", Color.FromArgb(220, 38, 38), out lblDashDebtMoneyVal, out lblDashDebtMoneySub), 6, 0);

            // Khu vực 2 & 3: Thống kê phòng + Báo cáo doanh thu (nửa trên)
            TableLayoutPanel tlpCharts = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 260,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(0, 6, 0, 6)
            };
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Khu vực 2: Biểu đồ phòng + thanh chỉ số giường
            Panel pnlDashRoomBox = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 6, 0) };
            lblDashOccupancyInfo = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 26,
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Tỷ lệ lấp đầy: 0%"
            };
            pnlChartDashPie = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            pnlChartDashPie.Paint += pnlChartDashPie_Paint;
            pnlChartDashPie.Resize += (s, e) => pnlChartDashPie.Invalidate();
            pnlDashRoomBox.Controls.Add(pnlChartDashPie);
            pnlDashRoomBox.Controls.Add(lblDashOccupancyInfo);

            // Khu vực 3A: Biểu đồ doanh thu tháng + chọn năm
            Panel pnlDashMonthBox = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(3, 0, 3, 0) };
            cboDashYear = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 82,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlDashMonthBox.Width - 92, 6),
                Font = new Font("Segoe UI", 8.5F)
            };
            cboDashYear.SelectedIndexChanged += cboDashYear_SelectedIndexChanged;
            pnlChartDashMonth = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            pnlChartDashMonth.Paint += pnlChartDashMonth_Paint;
            pnlChartDashMonth.Resize += (s, e) => pnlChartDashMonth.Invalidate();
            pnlDashMonthBox.Controls.Add(cboDashYear);
            pnlDashMonthBox.Controls.Add(pnlChartDashMonth);
            cboDashYear.BringToFront();

            // Khu vực 3B: Biểu đồ doanh thu quý
            pnlChartDashQuarter = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(6, 0, 0, 0) };
            pnlChartDashQuarter.Paint += pnlChartDashQuarter_Paint;
            pnlChartDashQuarter.Resize += (s, e) => pnlChartDashQuarter.Invalidate();

            tlpCharts.Controls.Add(pnlDashRoomBox, 0, 0);
            tlpCharts.Controls.Add(pnlDashMonthBox, 1, 0);
            tlpCharts.Controls.Add(pnlChartDashQuarter, 2, 0);

            // Khu vực 4: Công nợ sinh viên (bảng + tổng hợp + nút xuất Excel)
            Panel pnlDashDebtBox = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10, 6, 10, 8) };
            Panel pnlDashDebtHeader = new Panel { Dock = DockStyle.Top, Height = 36, BackColor = Color.White };
            Label lblDashDebtTitle = new Label
            {
                Text = "CÔNG NỢ SINH VIÊN (TOP CÁC KHOẢN NỢ CẦN THU)",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(2, 8)
            };
            lblDashDebtSummary = new Label
            {
                Text = "Tổng SV nợ: 0",
                Font = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                AutoSize = true,
                Location = new Point(350, 9)
            };
            btnDashExport = CreateActionButton("Xuất Excel (.xlsx)", Color.FromArgb(22, 163, 74), 140);
            btnDashExport.Dock = DockStyle.Right;
            btnDashExport.Click += btnDashExport_Click;

            pnlDashDebtHeader.Controls.Add(btnDashExport);
            pnlDashDebtHeader.Controls.Add(lblDashDebtTitle);
            pnlDashDebtHeader.Controls.Add(lblDashDebtSummary);

            dgvDashDebt = new DataGridView { Dock = DockStyle.Fill };
            dgvDashDebt.CellFormatting += dgvDebt_CellFormatting;

            pnlDashDebtBox.Controls.Add(dgvDashDebt);
            pnlDashDebtBox.Controls.Add(pnlDashDebtHeader);

            pnlPageDashboard.Controls.Add(pnlDashDebtBox);
            pnlPageDashboard.Controls.Add(tlpCharts);
            pnlPageDashboard.Controls.Add(tlpCards);
            pnlPageDashboard.Controls.Add(lblDashWarning);
        }

        // =====================================================================
        // PAGE 1: THỐNG KÊ TÌNH TRẠNG PHÒNG
        // =====================================================================
        private void BuildPagePhong()
        {
            pnlPagePhong = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Visible = false };
            lblRoomWarning = CreateWarningBanner();

            TableLayoutPanel tlpCards = CreateCardGrid(6, 82);
            tlpCards.Controls.Add(CreateKpiCard("TỔNG SỐ PHÒNG", Color.FromArgb(37, 99, 235), out lblRoomTotalVal, out lblRoomTotalSub), 0, 0);
            tlpCards.Controls.Add(CreateKpiCard("TỔNG SỐ GIƯỜNG", Color.FromArgb(14, 165, 233), out lblRoomBedsVal, out lblRoomBedsSub), 1, 0);
            tlpCards.Controls.Add(CreateKpiCard("GIƯỜNG ĐANG DÙNG", Color.FromArgb(16, 185, 129), out lblRoomUsedBedsVal, out lblRoomUsedBedsSub), 2, 0);
            tlpCards.Controls.Add(CreateKpiCard("PHÒNG TRỐNG (0 SV)", Color.FromArgb(100, 116, 139), out lblRoomEmptyVal, out lblRoomEmptySub), 3, 0);
            tlpCards.Controls.Add(CreateKpiCard("PHÒNG CÒN CHỖ", Color.FromArgb(245, 158, 11), out lblRoomAvailVal, out lblRoomAvailSub), 4, 0);
            tlpCards.Controls.Add(CreateKpiCard("TỶ LỆ LẤP ĐẦY", Color.FromArgb(139, 92, 246), out lblRoomRateVal, out lblRoomRateSub), 5, 0);

            // Thanh bộ lọc
            Panel pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.White,
                Padding = new Padding(10, 8, 10, 8)
            };
            pnlFilter.Controls.Add(CreateFilterLabel("Tìm phòng:", 10, 13));
            txtRoomSearch = new TextBox { Location = new Point(82, 10), Width = 160, Font = new Font("Segoe UI", 9F) };
            txtRoomSearch.TextChanged += RoomFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("Khu/Dãy:", 256, 13));
            cboRoomArea = new ComboBox { Location = new Point(318, 9), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };
            cboRoomArea.SelectedIndexChanged += RoomFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("Loại phòng:", 472, 13));
            cboRoomType = new ComboBox { Location = new Point(546, 9), Width = 135, DropDownStyle = ComboBoxStyle.DropDownList };
            cboRoomType.SelectedIndexChanged += RoomFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("Trạng thái:", 695, 13));
            cboRoomStatus = new ComboBox { Location = new Point(765, 9), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };
            cboRoomStatus.SelectedIndexChanged += RoomFilter_Changed;

            btnRoomReset = CreateActionButton("Đặt lại", Color.FromArgb(100, 116, 139), 80);
            btnRoomReset.Location = new Point(915, 8);
            btnRoomReset.Height = 28;
            btnRoomReset.Click += btnRoomReset_Click;

            pnlFilter.Controls.AddRange(new Control[] { txtRoomSearch, cboRoomArea, cboRoomType, cboRoomStatus, btnRoomReset });

            // Phần chính: bên trái biểu đồ tròn, bên phải bảng chi tiết phòng
            TableLayoutPanel tlpBody = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0, 8, 0, 0)
            };
            tlpBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            tlpBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 69F));
            tlpBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            pnlChartRoomPie = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 8, 0) };
            pnlChartRoomPie.Paint += pnlChartRoomPie_Paint;
            pnlChartRoomPie.Resize += (s, e) => pnlChartRoomPie.Invalidate();

            Panel pnlRoomGridBox = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10, 6, 10, 8) };
            lblRoomTableTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Text = "DANH SÁCH CHI TIẾT PHÒNG",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                TextAlign = ContentAlignment.MiddleLeft
            };
            dgvRoom = new DataGridView { Dock = DockStyle.Fill };
            dgvRoom.CellFormatting += dgvRoom_CellFormatting;
            pnlRoomGridBox.Controls.Add(dgvRoom);
            pnlRoomGridBox.Controls.Add(lblRoomTableTitle);

            tlpBody.Controls.Add(pnlChartRoomPie, 0, 0);
            tlpBody.Controls.Add(pnlRoomGridBox, 1, 0);

            pnlPagePhong.Controls.Add(tlpBody);
            pnlPagePhong.Controls.Add(pnlFilter);
            pnlPagePhong.Controls.Add(tlpCards);
            pnlPagePhong.Controls.Add(lblRoomWarning);
        }

        // =====================================================================
        // PAGE 2: BÁO CÁO DOANH THU THEO THÁNG / QUÝ
        // =====================================================================
        private void BuildPageDoanhThu()
        {
            pnlPageDoanhThu = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Visible = false };
            lblRevWarning = CreateWarningBanner();

            TableLayoutPanel tlpCards = CreateCardGrid(6, 82);
            tlpCards.Controls.Add(CreateKpiCard("TỔNG DOANH THU (ĐÃ THU)", Color.FromArgb(22, 163, 74), out lblRevTotalVal, out lblRevTotalSub), 0, 0);
            tlpCards.Controls.Add(CreateKpiCard("DOANH THU TIỀN PHÒNG", Color.FromArgb(37, 99, 235), out lblRevRoomVal, out lblRevRoomSub), 1, 0);
            tlpCards.Controls.Add(CreateKpiCard("DOANH THU DỊCH VỤ", Color.FromArgb(14, 165, 233), out lblRevServiceVal, out lblRevServiceSub), 2, 0);
            tlpCards.Controls.Add(CreateKpiCard("KHOẢN THU KHÁC", Color.FromArgb(245, 158, 11), out lblRevOtherVal, out lblRevOtherSub), 3, 0);
            tlpCards.Controls.Add(CreateKpiCard("GIAO DỊCH & TB THÁNG", Color.FromArgb(139, 92, 246), out lblRevTxVal, out lblRevTxSub), 4, 0);
            tlpCards.Controls.Add(CreateKpiCard("CÒN PHẢI THU TRONG KỲ", Color.FromArgb(220, 38, 38), out lblRevRemainVal, out lblRevRemainSub), 5, 0);

            // Bộ lọc năm & khoảng tháng
            Panel pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.White,
                Padding = new Padding(10, 7, 10, 7)
            };
            pnlFilter.Controls.Add(CreateFilterLabel("Năm thống kê:", 10, 12));
            cboRevYear = new ComboBox { Location = new Point(105, 8), Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            cboRevYear.SelectedIndexChanged += RevFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("Từ tháng:", 225, 12));
            cboRevFromMonth = new ComboBox { Location = new Point(290, 8), Width = 105, DropDownStyle = ComboBoxStyle.DropDownList };
            cboRevFromMonth.SelectedIndexChanged += RevFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("Đến tháng:", 415, 12));
            cboRevToMonth = new ComboBox { Location = new Point(488, 8), Width = 105, DropDownStyle = ComboBoxStyle.DropDownList };
            cboRevToMonth.SelectedIndexChanged += RevFilter_Changed;

            Label lblNote = new Label
            {
                Text = "● Doanh thu ghi nhận theo ngày thanh toán thực tế trên phiếu thu (LichSuThanhToan)",
                Location = new Point(615, 12),
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 116, 139),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic)
            };
            pnlFilter.Controls.AddRange(new Control[] { cboRevYear, cboRevFromMonth, cboRevToMonth, lblNote });

            // 3 biểu đồ: 12 tháng, 4 quý, phân loại nguồn thu
            TableLayoutPanel tlpCharts = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 225,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(0, 6, 0, 6)
            };
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            pnlChartRevMonth = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 5, 0) };
            pnlChartRevMonth.Paint += pnlChartRevMonth_Paint;
            pnlChartRevMonth.Resize += (s, e) => pnlChartRevMonth.Invalidate();

            pnlChartRevQuarter = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(3, 0, 3, 0) };
            pnlChartRevQuarter.Paint += pnlChartRevQuarter_Paint;
            pnlChartRevQuarter.Resize += (s, e) => pnlChartRevQuarter.Invalidate();

            pnlChartRevSource = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(5, 0, 0, 0) };
            pnlChartRevSource.Paint += pnlChartRevSource_Paint;
            pnlChartRevSource.Resize += (s, e) => pnlChartRevSource.Invalidate();

            tlpCharts.Controls.Add(pnlChartRevMonth, 0, 0);
            tlpCharts.Controls.Add(pnlChartRevQuarter, 1, 0);
            tlpCharts.Controls.Add(pnlChartRevSource, 2, 0);

            // 2 bảng chi tiết: theo tháng (trái) và theo quý (phải)
            TableLayoutPanel tlpTables = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            tlpTables.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            tlpTables.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            tlpTables.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel pnlMonthTbl = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8, 4, 8, 6), Margin = new Padding(0, 0, 5, 0) };
            Label lblMT = new Label { Dock = DockStyle.Top, Height = 24, Text = "BẢNG DOANH THU CHI TIẾT THEO THÁNG", Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42) };
            dgvRevMonth = new DataGridView { Dock = DockStyle.Fill };
            pnlMonthTbl.Controls.Add(dgvRevMonth);
            pnlMonthTbl.Controls.Add(lblMT);

            Panel pnlQuarterTbl = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8, 4, 8, 6), Margin = new Padding(5, 0, 0, 0) };
            Label lblQT = new Label { Dock = DockStyle.Top, Height = 24, Text = "BẢNG TỔNG HỢP DOANH THU THEO QUÝ", Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42) };
            dgvRevQuarter = new DataGridView { Dock = DockStyle.Fill };
            pnlQuarterTbl.Controls.Add(dgvRevQuarter);
            pnlQuarterTbl.Controls.Add(lblQT);

            tlpTables.Controls.Add(pnlMonthTbl, 0, 0);
            tlpTables.Controls.Add(pnlQuarterTbl, 1, 0);

            pnlPageDoanhThu.Controls.Add(tlpTables);
            pnlPageDoanhThu.Controls.Add(tlpCharts);
            pnlPageDoanhThu.Controls.Add(pnlFilter);
            pnlPageDoanhThu.Controls.Add(tlpCards);
            pnlPageDoanhThu.Controls.Add(lblRevWarning);
        }

        // =====================================================================
        // PAGE 3: CÔNG NỢ SINH VIÊN & XUẤT EXCEL
        // =====================================================================
        private void BuildPageCongNo()
        {
            pnlPageCongNo = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Visible = false };
            lblDebtWarning = CreateWarningBanner();

            TableLayoutPanel tlpCards = CreateCardGrid(4, 80);
            tlpCards.Controls.Add(CreateKpiCard("SỐ SINH VIÊN CÒN NỢ", Color.FromArgb(239, 68, 68), out lblDebtCountVal, out lblDebtCountSub), 0, 0);
            tlpCards.Controls.Add(CreateKpiCard("TỔNG PHẢI THANH TOÁN", Color.FromArgb(37, 99, 235), out lblDebtMustPayVal, out lblDebtMustPaySub), 1, 0);
            tlpCards.Controls.Add(CreateKpiCard("TỔNG SỐ TIỀN CÒN NỢ", Color.FromArgb(220, 38, 38), out lblDebtRemainVal, out lblDebtRemainSub), 2, 0);
            tlpCards.Controls.Add(CreateKpiCard("TỔNG NỢ QUÁ HẠN", Color.FromArgb(217, 119, 6), out lblDebtOverdueVal, out lblDebtOverdueSub), 3, 0);

            // Thanh bộ lọc công nợ (2 dòng gọn)
            Panel pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 76,
                BackColor = Color.White,
                Padding = new Padding(10, 6, 10, 6)
            };

            pnlFilter.Controls.Add(CreateFilterLabel("Tìm SV:", 10, 11));
            txtDebtSearch = new TextBox { Location = new Point(64, 8), Width = 160 };
            txtDebtSearch.TextChanged += DebtFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("Phòng:", 236, 11));
            cboDebtRoom = new ComboBox { Location = new Point(286, 7), Width = 115, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDebtRoom.SelectedIndexChanged += DebtFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("Loại phí:", 413, 11));
            cboDebtFeeType = new ComboBox { Location = new Point(470, 7), Width = 135, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDebtFeeType.SelectedIndexChanged += DebtFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("TT thanh toán:", 617, 11));
            cboDebtPayStatus = new ComboBox { Location = new Point(710, 7), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDebtPayStatus.SelectedIndexChanged += DebtFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("Tình trạng hạn:", 10, 44));
            cboDebtOverdue = new ComboBox { Location = new Point(108, 40), Width = 125, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDebtOverdue.SelectedIndexChanged += DebtFilter_Changed;

            chkDebtPeriod = new CheckBox
            {
                Text = "Lọc theo kỳ HĐ từ:",
                Location = new Point(248, 42),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.75F)
            };
            chkDebtPeriod.CheckedChanged += chkDebtPeriod_CheckedChanged;

            dtpDebtFrom = new DateTimePicker
            {
                Location = new Point(382, 40),
                Width = 90,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/yyyy",
                ShowUpDown = true
            };
            dtpDebtFrom.ValueChanged += DebtFilter_Changed;

            pnlFilter.Controls.Add(CreateFilterLabel("đến:", 478, 44));
            dtpDebtTo = new DateTimePicker
            {
                Location = new Point(510, 40),
                Width = 90,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/yyyy",
                ShowUpDown = true
            };
            dtpDebtTo.ValueChanged += DebtFilter_Changed;

            btnDebtReset = CreateActionButton("Đặt lại bộ lọc", Color.FromArgb(100, 116, 139), 110);
            btnDebtReset.Location = new Point(615, 38);
            btnDebtReset.Height = 30;
            btnDebtReset.Click += btnDebtReset_Click;

            btnDebtExport = CreateActionButton("Xuất Excel danh sách nợ (.xlsx)", Color.FromArgb(22, 163, 74), 215);
            btnDebtExport.Location = new Point(735, 38);
            btnDebtExport.Height = 30;
            btnDebtExport.Click += btnDebtExport_Click;

            pnlFilter.Controls.AddRange(new Control[]
            {
                txtDebtSearch, cboDebtRoom, cboDebtFeeType, cboDebtPayStatus,
                cboDebtOverdue, chkDebtPeriod, dtpDebtFrom, dtpDebtTo, btnDebtReset, btnDebtExport
            });

            // 2 bảng: Tổng hợp theo sinh viên (trên) và Chi tiết từng khoản nợ của SV đang chọn (dưới)
            TableLayoutPanel tlpGrids = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(0, 6, 0, 0)
            };
            tlpGrids.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpGrids.RowStyles.Add(new RowStyle(SizeType.Percent, 62F));
            tlpGrids.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));

            Panel pnlMaster = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10, 4, 10, 6), Margin = new Padding(0, 0, 0, 4) };
            Panel pnlMasterHeader = new Panel { Dock = DockStyle.Top, Height = 30 };
            Label lblMasterTitle = new Label
            {
                Text = "DANH SÁCH TỔNG HỢP CÔNG NỢ THEO SINH VIÊN (Chọn 1 sinh viên để xem chi tiết từng khoản bên dưới)",
                Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true,
                Location = new Point(0, 6)
            };

            btnDebtNext = CreateActionButton("Sau ▶", Color.FromArgb(71, 85, 105), 64);
            btnDebtNext.Dock = DockStyle.Right;
            btnDebtNext.Click += btnDebtNext_Click;

            lblDebtPageInfo = new Label
            {
                Text = "Trang 1/1",
                Dock = DockStyle.Right,
                Width = 130,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold)
            };

            btnDebtPrev = CreateActionButton("◀ Trước", Color.FromArgb(71, 85, 105), 68);
            btnDebtPrev.Dock = DockStyle.Right;
            btnDebtPrev.Click += btnDebtPrev_Click;

            pnlMasterHeader.Controls.Add(lblMasterTitle);
            pnlMasterHeader.Controls.Add(btnDebtPrev);
            pnlMasterHeader.Controls.Add(lblDebtPageInfo);
            pnlMasterHeader.Controls.Add(btnDebtNext);

            dgvDebtStudents = new DataGridView { Dock = DockStyle.Fill };
            dgvDebtStudents.SelectionChanged += dgvDebtStudents_SelectionChanged;
            dgvDebtStudents.CellFormatting += dgvDebt_CellFormatting;
            pnlMaster.Controls.Add(dgvDebtStudents);
            pnlMaster.Controls.Add(pnlMasterHeader);

            Panel pnlDetail = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10, 4, 10, 6) };
            lblDebtDetailTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Text = "CHI TIẾT KHOẢN NỢ CỦA SINH VIÊN",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(37, 99, 235)
            };
            dgvDebtDetails = new DataGridView { Dock = DockStyle.Fill };
            dgvDebtDetails.CellFormatting += dgvDebt_CellFormatting;
            pnlDetail.Controls.Add(dgvDebtDetails);
            pnlDetail.Controls.Add(lblDebtDetailTitle);

            tlpGrids.Controls.Add(pnlMaster, 0, 0);
            tlpGrids.Controls.Add(pnlDetail, 0, 1);

            pnlPageCongNo.Controls.Add(tlpGrids);
            pnlPageCongNo.Controls.Add(pnlFilter);
            pnlPageCongNo.Controls.Add(tlpCards);
            pnlPageCongNo.Controls.Add(lblDebtWarning);
        }

        // =====================================================================
        // HÀM DỰNG CONTROL CHUẨN MODERN FLAT
        // =====================================================================

        private static Button CreateTabButton(string text, int x, int width, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, 2),
                Size = new Size(width, 34),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btn.FlatAppearance.BorderSize = 1;
            btn.Click += onClick;
            return btn;
        }

        private static Button CreateActionButton(string text, Color bg, int width)
        {
            Button btn = new Button
            {
                Text = text,
                Width = width,
                Height = 28,
                FlatStyle = FlatStyle.Flat,
                BackColor = bg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private static Label CreateFilterLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.FromArgb(71, 85, 105),
                Font = new Font("Segoe UI", 8.75F)
            };
        }

        private static Label CreateWarningBanner()
        {
            return new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 28,
                BackColor = Color.FromArgb(254, 243, 199),
                ForeColor = Color.FromArgb(146, 64, 14),
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 10, 0),
                Visible = false
            };
        }

        private static TableLayoutPanel CreateCardGrid(int columns, int height)
        {
            TableLayoutPanel tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = height,
                ColumnCount = columns,
                RowCount = 1,
                Padding = new Padding(0, 2, 0, 6)
            };
            float pct = 100F / columns;
            for (int i = 0; i < columns; i++)
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, pct));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            return tlp;
        }

        private static Panel CreateKpiCard(string title, Color accentColor, out Label lblVal, out Label lblSub)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(3, 0, 3, 0)
            };
            Panel accent = new Panel
            {
                Dock = DockStyle.Left,
                Width = 4,
                BackColor = accentColor
            };
            Label lblTitle = new Label
            {
                Text = title,
                Location = new Point(10, 6),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 7.75F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            lblVal = new Label
            {
                Text = "0",
                Location = new Point(10, 23),
                AutoSize = true,
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42)
            };
            lblSub = new Label
            {
                Text = "",
                Location = new Point(10, 52),
                AutoSize = true,
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            card.Controls.Add(lblSub);
            card.Controls.Add(lblVal);
            card.Controls.Add(lblTitle);
            card.Controls.Add(accent);
            return card;
        }

        // Khai báo controls
        private Panel pnlTopNav;
        private Button btnTabDashboard, btnTabPhong, btnTabDoanhThu, btnTabCongNo, btnRefreshAll;

        // Page 0: Dashboard
        private Panel pnlPageDashboard;
        private Label lblDashWarning;
        private Label lblDashSVVal, lblDashSVSub;
        private Label lblDashRoomVal, lblDashRoomSub;
        private Label lblDashEmptyBedVal, lblDashEmptyBedSub;
        private Label lblDashFullRoomVal, lblDashFullRoomSub;
        private Label lblDashRevVal, lblDashRevSub;
        private Label lblDashDebtSVVal, lblDashDebtSVSub;
        private Label lblDashDebtMoneyVal, lblDashDebtMoneySub;
        private Label lblDashOccupancyInfo, lblDashDebtSummary;
        private Panel pnlChartDashPie, pnlChartDashMonth, pnlChartDashQuarter;
        private ComboBox cboDashYear;
        private Button btnDashExport;
        private DataGridView dgvDashDebt;

        // Page 1: Phòng
        private Panel pnlPagePhong;
        private Label lblRoomWarning, lblRoomTableTitle;
        private Label lblRoomTotalVal, lblRoomTotalSub;
        private Label lblRoomBedsVal, lblRoomBedsSub;
        private Label lblRoomUsedBedsVal, lblRoomUsedBedsSub;
        private Label lblRoomEmptyVal, lblRoomEmptySub;
        private Label lblRoomAvailVal, lblRoomAvailSub;
        private Label lblRoomRateVal, lblRoomRateSub;
        private TextBox txtRoomSearch;
        private ComboBox cboRoomArea, cboRoomType, cboRoomStatus;
        private Button btnRoomReset;
        private Panel pnlChartRoomPie;
        private DataGridView dgvRoom;

        // Page 2: Doanh thu
        private Panel pnlPageDoanhThu;
        private Label lblRevWarning;
        private Label lblRevTotalVal, lblRevTotalSub;
        private Label lblRevRoomVal, lblRevRoomSub;
        private Label lblRevServiceVal, lblRevServiceSub;
        private Label lblRevOtherVal, lblRevOtherSub;
        private Label lblRevTxVal, lblRevTxSub;
        private Label lblRevRemainVal, lblRevRemainSub;
        private ComboBox cboRevYear, cboRevFromMonth, cboRevToMonth;
        private Panel pnlChartRevMonth, pnlChartRevQuarter, pnlChartRevSource;
        private DataGridView dgvRevMonth, dgvRevQuarter;

        // Page 3: Công nợ
        private Panel pnlPageCongNo;
        private Label lblDebtWarning, lblDebtDetailTitle, lblDebtPageInfo;
        private Label lblDebtCountVal, lblDebtCountSub;
        private Label lblDebtMustPayVal, lblDebtMustPaySub;
        private Label lblDebtRemainVal, lblDebtRemainSub;
        private Label lblDebtOverdueVal, lblDebtOverdueSub;
        private TextBox txtDebtSearch;
        private ComboBox cboDebtRoom, cboDebtFeeType, cboDebtPayStatus, cboDebtOverdue;
        private CheckBox chkDebtPeriod;
        private DateTimePicker dtpDebtFrom, dtpDebtTo;
        private Button btnDebtReset, btnDebtExport, btnDebtPrev, btnDebtNext;
        private DataGridView dgvDebtStudents, dgvDebtDetails;
    }
}

