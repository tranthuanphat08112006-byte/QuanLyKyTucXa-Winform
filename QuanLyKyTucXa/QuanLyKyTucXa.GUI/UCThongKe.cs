using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using QuanLyKyTucXa.BLL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.GUI
{
    public partial class UCThongKe : UserControl
    {
        private readonly ThongKeBLL thongKeBLL = new ThongKeBLL();
        private readonly PhongBLL phongBLL = new PhongBLL();

        private ThongKePhongTongQuanDTO duLieuPhong;
        private BaoCaoDoanhThuDTO duLieuDoanhThu;
        private KetQuaCongNoDTO duLieuCongNo;
        private BaoCaoDoanhThuDTO duLieuDoanhThuDash;
        private KetQuaCongNoDTO duLieuCongNoDash;

        private int tabHienTai = 0; // 0: Tổng quan, 1: Phòng, 2: Doanh thu, 3: Công nợ
        private bool dangNapBoLoc = false;

        // Phân trang bảng công nợ
        private int trangCongNo = 1;
        private const int KICH_THUOC_TRANG = 20;

        public UCThongKe()
        {
            InitializeComponent();
        }

        private void UCThongKe_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            BatDoubleBuffer(pnlChartDashPie);
            BatDoubleBuffer(pnlChartDashMonth);
            BatDoubleBuffer(pnlChartDashQuarter);
            BatDoubleBuffer(pnlChartRoomPie);
            BatDoubleBuffer(pnlChartRevMonth);
            BatDoubleBuffer(pnlChartRevQuarter);
            BatDoubleBuffer(pnlChartRevSource);

            UIHelper.FormatModernDataGridView(dgvDashDebt);
            UIHelper.FormatModernDataGridView(dgvRoom);
            UIHelper.FormatModernDataGridView(dgvRevMonth);
            UIHelper.FormatModernDataGridView(dgvRevQuarter);
            UIHelper.FormatModernDataGridView(dgvDebtStudents);
            UIHelper.FormatModernDataGridView(dgvDebtDetails);

            KhoiTaoBoLoc();
            ChuyenTab(0);
        }

        private static void BatDoubleBuffer(Control c)
        {
            if (c == null) return;
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(c, true, null);
        }

        public void RefreshAllData()
        {
            NapDanhSachNamVaPhong();
            TaiDuLieuTheoTab();
        }

        private void KhoiTaoBoLoc()
        {
            dangNapBoLoc = true;

            // Tháng từ / đến cho tab Doanh thu
            cboRevFromMonth.Items.Clear();
            cboRevToMonth.Items.Clear();
            for (int m = 1; m <= 12; m++)
            {
                cboRevFromMonth.Items.Add("Tháng " + m);
                cboRevToMonth.Items.Add("Tháng " + m);
            }
            cboRevFromMonth.SelectedIndex = 0;
            cboRevToMonth.SelectedIndex = 11;

            // Trạng thái phòng
            cboRoomStatus.Items.Clear();
            cboRoomStatus.Items.AddRange(new object[]
            {
                "Tất cả trạng thái",
                ThongKeBLL.TT_TRONG,
                ThongKeBLL.TT_CON_CHO,
                ThongKeBLL.TT_DA_DAY,
                ThongKeBLL.TT_BAT_THUONG
            });
            cboRoomStatus.SelectedIndex = 0;

            // Loại phí công nợ
            cboDebtFeeType.Items.Clear();
            cboDebtFeeType.Items.Add("Tất cả loại phí");
            foreach (string ten in LoaiPhi.Ten) cboDebtFeeType.Items.Add(ten);
            cboDebtFeeType.SelectedIndex = 0;

            // Trạng thái thanh toán công nợ
            cboDebtPayStatus.Items.Clear();
            cboDebtPayStatus.Items.AddRange(new object[]
            {
                "Tất cả",
                ThongKeBLL.TTTT_CHUA,
                ThongKeBLL.TTTT_MOT_PHAN
            });
            cboDebtPayStatus.SelectedIndex = 0;

            // Tình trạng hạn
            cboDebtOverdue.Items.Clear();
            cboDebtOverdue.Items.AddRange(new object[]
            {
                "Tất cả",
                ThongKeBLL.HAN_QUA_HAN,
                ThongKeBLL.HAN_CHUA_DEN
            });
            cboDebtOverdue.SelectedIndex = 0;

            dtpDebtFrom.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpDebtTo.Value = DateTime.Today;
            chkDebtPeriod.Checked = false;
            dtpDebtFrom.Enabled = false;
            dtpDebtTo.Enabled = false;

            NapDanhSachNamVaPhong();
            dangNapBoLoc = false;
        }

        private void NapDanhSachNamVaPhong()
        {
            bool prev = dangNapBoLoc;
            dangNapBoLoc = true;
            try
            {
                List<int> dsNam = thongKeBLL.GetDanhSachNam();
                CapNhatComboNam(cboDashYear, dsNam);
                CapNhatComboNam(cboRevYear, dsNam);

                string chonPhongNo = cboDebtRoom.SelectedItem?.ToString();
                cboDebtRoom.Items.Clear();
                cboDebtRoom.Items.Add("Tất cả phòng");
                var dtPhong = phongBLL.GetAllPhong();
                if (dtPhong != null)
                {
                    foreach (System.Data.DataRow r in dtPhong.Rows)
                    {
                        cboDebtRoom.Items.Add(r["MaPhong"].ToString());
                    }
                }
                int idx = !string.IsNullOrEmpty(chonPhongNo) ? cboDebtRoom.Items.IndexOf(chonPhongNo) : 0;
                cboDebtRoom.SelectedIndex = idx >= 0 ? idx : 0;
            }
            catch
            {
                if (cboDashYear.Items.Count == 0) { cboDashYear.Items.Add(DateTime.Today.Year); cboDashYear.SelectedIndex = 0; }
                if (cboRevYear.Items.Count == 0) { cboRevYear.Items.Add(DateTime.Today.Year); cboRevYear.SelectedIndex = 0; }
                if (cboDebtRoom.Items.Count == 0) { cboDebtRoom.Items.Add("Tất cả phòng"); cboDebtRoom.SelectedIndex = 0; }
            }
            finally
            {
                dangNapBoLoc = prev;
            }
        }

        private static void CapNhatComboNam(ComboBox cbo, List<int> dsNam)
        {
            object cur = cbo.SelectedItem;
            cbo.Items.Clear();
            foreach (int y in dsNam) cbo.Items.Add(y);
            if (cur != null && cbo.Items.Contains(cur)) cbo.SelectedItem = cur;
            else if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
        }

        // =====================================================================
        // ĐIỀU HƯỚNG TAB
        // =====================================================================

        private void ChuyenTab(int index)
        {
            tabHienTai = index;
            Button[] tabs = { btnTabDashboard, btnTabPhong, btnTabDoanhThu, btnTabCongNo };
            Panel[] pages = { pnlPageDashboard, pnlPagePhong, pnlPageDoanhThu, pnlPageCongNo };

            for (int i = 0; i < tabs.Length; i++)
            {
                bool active = (i == index);
                tabs[i].BackColor = active ? Color.FromArgb(37, 99, 235) : Color.White;
                tabs[i].ForeColor = active ? Color.White : Color.FromArgb(71, 85, 105);
                pages[i].Visible = active;
                if (active) pages[i].BringToFront();
            }

            TaiDuLieuTheoTab();
        }

        private void TaiDuLieuTheoTab()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (tabHienTai == 0) TaiTabTongQuan();
                else if (tabHienTai == 1) TaiTabPhong();
                else if (tabHienTai == 2) TaiTabDoanhThu();
                else if (tabHienTai == 3) TaiTabCongNo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu thống kê: " + ex.Message,
                    "Lỗi thống kê", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // =====================================================================
        // TAB 0: TỔNG QUAN DASHBOARD
        // =====================================================================

        private void TaiTabTongQuan()
        {
            int nam = LayNam(cboDashYear);
            DashboardThongKeDTO dash = thongKeBLL.GetDashboard(nam);
            duLieuPhong = dash.Phong;
            duLieuDoanhThuDash = dash.DoanhThu;
            duLieuCongNoDash = dash.CongNo;

            // 7 thẻ chỉ số tổng quan
            lblDashSVVal.Text = dash.Phong.SoSinhVienDangLuuTru.ToString("N0");
            lblDashSVSub.Text = $"Giường đang dùng: {dash.Phong.SoGiuongDangSuDung:N0}/{dash.Phong.TongSoGiuong:N0}";

            lblDashRoomVal.Text = dash.Phong.TongSoPhong.ToString("N0");
            lblDashRoomSub.Text = $"Trống: {dash.Phong.SoPhongTrong} | Còn chỗ: {dash.Phong.SoPhongConCho}";

            lblDashEmptyBedVal.Text = dash.Phong.SoGiuongTrong.ToString("N0");
            lblDashEmptyBedSub.Text = $"Tỷ lệ lấp đầy: {dash.Phong.TyLeLapDay:0.#}%";

            lblDashFullRoomVal.Text = dash.Phong.SoPhongDaDay.ToString("N0");
            lblDashFullRoomSub.Text = dash.Phong.SoPhongBatThuong > 0
                ? $"Bất thường: {dash.Phong.SoPhongBatThuong} phòng"
                : $"Trên tổng {dash.Phong.TongSoPhong} phòng";

            lblDashRevVal.Text = DinhDangTienRutGon(dash.DoanhThu.TongDoanhThu);
            lblDashRevSub.Text = $"Năm {nam} ({dash.DoanhThu.SoGiaoDichThanhCong} giao dịch)";

            lblDashDebtSVVal.Text = dash.CongNo.TongSoSinhVienNo.ToString("N0");
            lblDashDebtSVSub.Text = $"Quá hạn: {dash.CongNo.SoSinhVienQuaHan} sinh viên";

            lblDashDebtMoneyVal.Text = DinhDangTienRutGon(dash.CongNo.TongConNo);
            lblDashDebtMoneySub.Text = $"Quá hạn: {dash.CongNo.TongNoQuaHan:N0} đ";

            // Khu vực 2: tóm tắt phòng
            lblDashOccupancyInfo.Text =
                $"Tỷ lệ lấp đầy: {dash.Phong.TyLeLapDay:0.#}%   |   " +
                $"Đang sử dụng: {dash.Phong.SoGiuongDangSuDung:N0} giường   |   " +
                $"Còn trống: {dash.Phong.SoGiuongTrong:N0} giường";

            // Khu vực 4: tóm tắt công nợ + bảng
            lblDashDebtSummary.Text =
                $"Tổng SV nợ: {dash.CongNo.TongSoSinhVienNo:N0}   |   " +
                $"Quá hạn: {dash.CongNo.SoSinhVienQuaHan:N0} SV   |   " +
                $"Tổng còn nợ: {dash.CongNo.TongConNo:N0} đ";

            NapBangCongNo(dgvDashDebt, dash.CongNo.DanhSach.Take(15).ToList(), true);

            // Cảnh báo tổng hợp
            List<string> cb = new List<string>();
            cb.AddRange(dash.Phong.CanhBao);
            cb.AddRange(dash.DoanhThu.CanhBao);
            cb.AddRange(dash.CongNo.CanhBao);
            HienThiCanhBao(lblDashWarning, cb);

            pnlChartDashPie.Invalidate();
            pnlChartDashMonth.Invalidate();
            pnlChartDashQuarter.Invalidate();
        }

        // =====================================================================
        // TAB 1: THỐNG KÊ TÌNH TRẠNG PHÒNG
        // =====================================================================

        private void TaiTabPhong()
        {
            duLieuPhong = thongKeBLL.GetThongKePhong();

            // Cập nhật combobox dãy nhà & loại phòng từ dữ liệu thực tế
            bool prev = dangNapBoLoc;
            dangNapBoLoc = true;
            CapNhatComboDanhMuc(cboRoomArea, "Tất cả khu/dãy",
                duLieuPhong.ChiTiet.Select(p => p.TenKhu).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().OrderBy(s => s));
            CapNhatComboDanhMuc(cboRoomType, "Tất cả loại phòng",
                duLieuPhong.ChiTiet.Select(p => p.LoaiPhong).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().OrderBy(s => s));
            dangNapBoLoc = prev;

            // 8 chỉ số trên 6 thẻ
            lblRoomTotalVal.Text = duLieuPhong.TongSoPhong.ToString("N0");
            lblRoomTotalSub.Text = $"SV đang lưu trú: {duLieuPhong.SoSinhVienDangLuuTru:N0}";

            lblRoomBedsVal.Text = duLieuPhong.TongSoGiuong.ToString("N0");
            lblRoomBedsSub.Text = $"Đang dùng: {duLieuPhong.SoGiuongDangSuDung:N0} | Trống: {duLieuPhong.SoGiuongTrong:N0}";

            lblRoomUsedBedsVal.Text = duLieuPhong.SoGiuongDangSuDung.ToString("N0");
            lblRoomUsedBedsSub.Text = $"Giường còn trống: {duLieuPhong.SoGiuongTrong:N0}";

            lblRoomEmptyVal.Text = duLieuPhong.SoPhongTrong.ToString("N0");
            lblRoomEmptySub.Text = "Phòng 0 người đang ở";

            lblRoomAvailVal.Text = duLieuPhong.SoPhongConCho.ToString("N0");
            lblRoomAvailSub.Text = $"Đã đầy: {duLieuPhong.SoPhongDaDay:N0} phòng";

            lblRoomRateVal.Text = $"{duLieuPhong.TyLeLapDay:0.#}%";
            lblRoomRateSub.Text = duLieuPhong.SoPhongBatThuong > 0
                ? $"Cảnh báo bất thường: {duLieuPhong.SoPhongBatThuong} phòng"
                : "Dữ liệu sức chứa hợp lệ";

            HienThiCanhBao(lblRoomWarning, duLieuPhong.CanhBao);
            pnlChartRoomPie.Invalidate();
            ApDungLocPhong();
        }

        private static void CapNhatComboDanhMuc(ComboBox cbo, string macDinh, IEnumerable<string> items)
        {
            string cur = cbo.SelectedItem?.ToString();
            cbo.Items.Clear();
            cbo.Items.Add(macDinh);
            foreach (string s in items) cbo.Items.Add(s);
            int idx = !string.IsNullOrEmpty(cur) ? cbo.Items.IndexOf(cur) : 0;
            cbo.SelectedIndex = idx >= 0 ? idx : 0;
        }

        private void ApDungLocPhong()
        {
            if (duLieuPhong == null) return;

            string khu = cboRoomArea.SelectedIndex > 0 ? cboRoomArea.SelectedItem.ToString() : null;
            string loai = cboRoomType.SelectedIndex > 0 ? cboRoomType.SelectedItem.ToString() : null;
            string tt = cboRoomStatus.SelectedIndex > 0 ? cboRoomStatus.SelectedItem.ToString() : null;

            List<ThongKePhongChiTietDTO> ds = thongKeBLL.LocChiTietPhong(
                duLieuPhong.ChiTiet, txtRoomSearch.Text, khu, loai, tt);

            dgvRoom.DataSource = null;
            dgvRoom.Columns.Clear();
            dgvRoom.AutoGenerateColumns = false;

            dgvRoom.Columns.Add(TaoCot("MaPhong", "Mã phòng", 85, DataGridViewContentAlignment.MiddleCenter));
            dgvRoom.Columns.Add(TaoCot("TenPhong", "Tên phòng", 130, DataGridViewContentAlignment.MiddleLeft));
            dgvRoom.Columns.Add(TaoCot("TenKhu", "Khu / Dãy", 95, DataGridViewContentAlignment.MiddleCenter));
            dgvRoom.Columns.Add(TaoCot("LoaiPhong", "Loại phòng", 95, DataGridViewContentAlignment.MiddleCenter));
            dgvRoom.Columns.Add(TaoCot("SucChua", "Sức chứa", 85, DataGridViewContentAlignment.MiddleCenter, "N0"));
            dgvRoom.Columns.Add(TaoCot("SoNguoiDangO", "Số người đang ở", 115, DataGridViewContentAlignment.MiddleCenter, "N0"));
            dgvRoom.Columns.Add(TaoCot("SoGiuongTrong", "Số giường trống", 115, DataGridViewContentAlignment.MiddleCenter, "N0"));
            dgvRoom.Columns.Add(TaoCot("TyLeLapDay", "Lấp đầy (%)", 95, DataGridViewContentAlignment.MiddleRight, "0.#"));
            dgvRoom.Columns.Add(TaoCot("TrangThai", "Trạng thái", 105, DataGridViewContentAlignment.MiddleCenter));
            dgvRoom.Columns.Add(TaoCot("GhiChuBatThuong", "Ghi chú / Cảnh báo", 180, DataGridViewContentAlignment.MiddleLeft));

            dgvRoom.DataSource = ds;
            lblRoomTableTitle.Text = $"DANH SÁCH CHI TIẾT PHÒNG ({ds.Count}/{duLieuPhong.TongSoPhong} phòng)";
        }

        // =====================================================================
        // TAB 2: BÁO CÁO DOANH THU THEO THÁNG / QUÝ
        // =====================================================================

        private void TaiTabDoanhThu()
        {
            int nam = LayNam(cboRevYear);
            int tuThang = Math.Max(1, cboRevFromMonth.SelectedIndex + 1);
            int denThang = Math.Max(1, cboRevToMonth.SelectedIndex + 1);
            if (tuThang > denThang)
            {
                dangNapBoLoc = true;
                cboRevToMonth.SelectedIndex = tuThang - 1;
                dangNapBoLoc = false;
                denThang = tuThang;
            }

            duLieuDoanhThu = thongKeBLL.GetBaoCaoDoanhThu(nam, tuThang, denThang);

            lblRevTotalVal.Text = $"{duLieuDoanhThu.TongDoanhThu:N0} đ";
            lblRevTotalSub.Text = $"Năm {nam} (Tháng {tuThang} - Tháng {denThang})";

            lblRevRoomVal.Text = $"{duLieuDoanhThu.DoanhThuTienPhong:N0} đ";
            lblRevRoomSub.Text = TyTrongText(duLieuDoanhThu.DoanhThuTienPhong, duLieuDoanhThu.TongDoanhThu);

            lblRevServiceVal.Text = $"{duLieuDoanhThu.DoanhThuDichVu:N0} đ";
            lblRevServiceSub.Text = $"Điện: {duLieuDoanhThu.DoanhThuDien:N0} | Nước: {duLieuDoanhThu.DoanhThuNuoc:N0}";

            lblRevOtherVal.Text = $"{duLieuDoanhThu.DoanhThuKhac:N0} đ";
            lblRevOtherSub.Text = TyTrongText(duLieuDoanhThu.DoanhThuKhac, duLieuDoanhThu.TongDoanhThu);

            lblRevTxVal.Text = $"{duLieuDoanhThu.SoGiaoDichThanhCong:N0} GD";
            lblRevTxSub.Text = $"TB tháng: {duLieuDoanhThu.DoanhThuTrungBinhThang:N0} đ";

            lblRevRemainVal.Text = $"{duLieuDoanhThu.TongConPhaiThu:N0} đ";
            string maxMin = duLieuDoanhThu.ThangCaoNhat != null
                ? $"Cao nhất: T{duLieuDoanhThu.ThangCaoNhat.Ky} ({DinhDangTienRutGon(duLieuDoanhThu.ThangCaoNhat.TongThu)}) | Thấp nhất: T{duLieuDoanhThu.ThangThapNhat.Ky} ({DinhDangTienRutGon(duLieuDoanhThu.ThangThapNhat.TongThu)})"
                : "Chưa có giao dịch phát sinh";
            lblRevRemainSub.Text = maxMin;

            HienThiCanhBao(lblRevWarning, duLieuDoanhThu.CanhBao);

            NapBangDoanhThu(dgvRevMonth, duLieuDoanhThu.TheoThang, "Tháng");
            NapBangDoanhThu(dgvRevQuarter, duLieuDoanhThu.TheoQuy, "Quý");

            pnlChartRevMonth.Invalidate();
            pnlChartRevQuarter.Invalidate();
            pnlChartRevSource.Invalidate();
        }

        private static string TyTrongText(decimal phan, decimal tong)
        {
            if (tong <= 0) return "Chiếm 0% tổng thu";
            return $"Chiếm {Math.Round(phan * 100m / tong, 1):0.#}% tổng thu";
        }

        private static void NapBangDoanhThu(DataGridView dgv, List<DoanhThuKyDTO> ds, string nhanKy)
        {
            dgv.DataSource = null;
            dgv.Columns.Clear();
            dgv.AutoGenerateColumns = false;

            dgv.Columns.Add(TaoCot("TenKy", nhanKy, 85, DataGridViewContentAlignment.MiddleCenter));
            dgv.Columns.Add(TaoCot("SoGiaoDich", "Số GD", 65, DataGridViewContentAlignment.MiddleCenter, "N0"));
            dgv.Columns.Add(TaoCot("ThuTienPhong", "Thu tiền phòng", 115, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgv.Columns.Add(TaoCot("ThuTienDien", "Thu tiền điện", 110, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgv.Columns.Add(TaoCot("ThuTienNuoc", "Thu tiền nước", 110, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgv.Columns.Add(TaoCot("ThuDichVuKhac", "Thu khác", 95, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgv.Columns.Add(TaoCot("TongThu", "Tổng đã thu", 120, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgv.Columns.Add(TaoCot("ConPhaiThu", "Còn phải thu", 115, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgv.Columns.Add(TaoCot("TangTruong", "So kỳ trước (%)", 100, DataGridViewContentAlignment.MiddleRight, "+0.#;-0.#;0"));

            dgv.DataSource = ds;
        }

        // =====================================================================
        // TAB 3: CÔNG NỢ SINH VIÊN & XUẤT EXCEL
        // =====================================================================

        private CongNoFilterDTO TaoBoLocCongNo()
        {
            return new CongNoFilterDTO
            {
                TuKhoa = txtDebtSearch.Text.Trim(),
                MaPhong = cboDebtRoom.SelectedIndex > 0 ? cboDebtRoom.SelectedItem.ToString() : null,
                LoaiPhi = cboDebtFeeType.SelectedIndex - 1, // -1 = Tất cả
                TrangThaiThanhToan = cboDebtPayStatus.SelectedIndex > 0 ? cboDebtPayStatus.SelectedItem.ToString() : null,
                TinhTrangHan = cboDebtOverdue.SelectedIndex > 0 ? cboDebtOverdue.SelectedItem.ToString() : null,
                TuKy = chkDebtPeriod.Checked ? new DateTime(dtpDebtFrom.Value.Year, dtpDebtFrom.Value.Month, 1) : (DateTime?)null,
                DenKy = chkDebtPeriod.Checked ? new DateTime(dtpDebtTo.Value.Year, dtpDebtTo.Value.Month, 1) : (DateTime?)null
            };
        }

        private string MoTaBoLocCongNo(CongNoFilterDTO f)
        {
            List<string> p = new List<string>();
            if (!string.IsNullOrWhiteSpace(f.TuKhoa)) p.Add($"Từ khóa: '{f.TuKhoa}'");
            if (!string.IsNullOrWhiteSpace(f.MaPhong)) p.Add($"Phòng: {f.MaPhong}");
            if (f.LoaiPhi >= 0 && f.LoaiPhi < LoaiPhi.Ten.Length) p.Add($"Loại phí: {LoaiPhi.Ten[f.LoaiPhi]}");
            if (!string.IsNullOrWhiteSpace(f.TrangThaiThanhToan)) p.Add($"TT thanh toán: {f.TrangThaiThanhToan}");
            if (!string.IsNullOrWhiteSpace(f.TinhTrangHan)) p.Add($"Hạn: {f.TinhTrangHan}");
            if (f.TuKy.HasValue && f.DenKy.HasValue) p.Add($"Kỳ HĐ: {f.TuKy.Value:MM/yyyy} - {f.DenKy.Value:MM/yyyy}");
            return p.Count == 0 ? "Tất cả khoản nợ" : string.Join(" | ", p);
        }

        private void TaiTabCongNo()
        {
            CongNoFilterDTO filter = TaoBoLocCongNo();
            duLieuCongNo = thongKeBLL.GetCongNo(filter);

            lblDebtCountVal.Text = duLieuCongNo.TongSoSinhVienNo.ToString("N0");
            lblDebtCountSub.Text = $"Quá hạn: {duLieuCongNo.SoSinhVienQuaHan:N0} sinh viên";

            lblDebtMustPayVal.Text = $"{duLieuCongNo.TongPhaiDong:N0} đ";
            lblDebtMustPaySub.Text = $"Đã trả: {duLieuCongNo.TongDaThanhToan:N0} đ";

            lblDebtRemainVal.Text = $"{duLieuCongNo.TongConNo:N0} đ";
            lblDebtRemainSub.Text = $"Tổng tiền sinh viên còn nợ";

            lblDebtOverdueVal.Text = $"{duLieuCongNo.TongNoQuaHan:N0} đ";
            lblDebtOverdueSub.Text = $"Hạn = Ngày lập HĐ + {ThongKeBLL.SO_NGAY_HAN_THANH_TOAN} ngày";

            HienThiCanhBao(lblDebtWarning, duLieuCongNo.CanhBao);

            trangCongNo = 1;
            HienThiTrangCongNo();
        }

        private void HienThiTrangCongNo()
        {
            if (duLieuCongNo == null) return;
            int tong = duLieuCongNo.DanhSach.Count;
            int tongTrang = Math.Max(1, (int)Math.Ceiling(tong * 1.0 / KICH_THUOC_TRANG));
            if (trangCongNo < 1) trangCongNo = 1;
            if (trangCongNo > tongTrang) trangCongNo = tongTrang;

            List<CongNoSinhVienDTO> trang = duLieuCongNo.DanhSach
                .Skip((trangCongNo - 1) * KICH_THUOC_TRANG)
                .Take(KICH_THUOC_TRANG)
                .ToList();

            NapBangCongNo(dgvDebtStudents, trang, false);

            lblDebtPageInfo.Text = $"Trang {trangCongNo}/{tongTrang} ({tong} SV)";
            btnDebtPrev.Enabled = trangCongNo > 1;
            btnDebtNext.Enabled = trangCongNo < tongTrang;

            if (trang.Count > 0)
            {
                HienThiChiTietKhoanNo(trang[0]);
            }
            else
            {
                dgvDebtDetails.DataSource = null;
                lblDebtDetailTitle.Text = "CHI TIẾT KHOẢN NỢ CỦA SINH VIÊN (Không có dữ liệu công nợ theo bộ lọc hiện tại)";
            }
        }

        private static void NapBangCongNo(DataGridView dgv, List<CongNoSinhVienDTO> ds, bool rutGon)
        {
            dgv.DataSource = null;
            dgv.Columns.Clear();
            dgv.AutoGenerateColumns = false;

            dgv.Columns.Add(TaoCot("STT", "STT", 48, DataGridViewContentAlignment.MiddleCenter));
            dgv.Columns.Add(TaoCot("MaSV", "Mã SV", 85, DataGridViewContentAlignment.MiddleCenter));
            dgv.Columns.Add(TaoCot("HoTen", "Họ và tên", 145, DataGridViewContentAlignment.MiddleLeft));
            if (!rutGon)
                dgv.Columns.Add(TaoCot("SDT", "Số ĐT", 95, DataGridViewContentAlignment.MiddleCenter));
            dgv.Columns.Add(TaoCot("PhongDangO", "Phòng đang lưu trú", 115, DataGridViewContentAlignment.MiddleCenter));
            dgv.Columns.Add(TaoCot("LoaiPhiConNo", "Loại khoản phí còn nợ", 170, DataGridViewContentAlignment.MiddleLeft));
            dgv.Columns.Add(TaoCot("TongPhaiDong", "Phải thanh toán", 115, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgv.Columns.Add(TaoCot("DaThanhToan", "Đã thanh toán", 110, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgv.Columns.Add(TaoCot("ConNo", "Số tiền còn nợ", 115, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgv.Columns.Add(TaoCot("HanThanhToan", "Hạn TT", 95, DataGridViewContentAlignment.MiddleCenter, "dd/MM/yyyy"));
            dgv.Columns.Add(TaoCot("TrangThai", "Trạng thái thanh toán", 155, DataGridViewContentAlignment.MiddleLeft));

            dgv.DataSource = ds;
        }

        private void HienThiChiTietKhoanNo(CongNoSinhVienDTO sv)
        {
            if (sv == null) return;
            lblDebtDetailTitle.Text = $"CHI TIẾT CÁC KHOẢN NỢ CỦA: {sv.MaSV} - {sv.HoTen} ({sv.ChiTiet.Count} khoản, còn nợ {sv.ConNo:N0} đ)";

            dgvDebtDetails.DataSource = null;
            dgvDebtDetails.Columns.Clear();
            dgvDebtDetails.AutoGenerateColumns = false;

            dgvDebtDetails.Columns.Add(TaoCot("MaHoaDon", "Mã HĐ", 70, DataGridViewContentAlignment.MiddleCenter));
            dgvDebtDetails.Columns.Add(TaoCot("TenPhong", "Phòng", 110, DataGridViewContentAlignment.MiddleCenter));
            dgvDebtDetails.Columns.Add(TaoCot("KyHoaDon", "Kỳ HĐ", 80, DataGridViewContentAlignment.MiddleCenter));
            dgvDebtDetails.Columns.Add(TaoCot("SoSinhVienChiaSe", "Số SV ở", 75, DataGridViewContentAlignment.MiddleCenter));
            dgvDebtDetails.Columns.Add(TaoCot("LoaiPhiConNo", "Loại phí còn nợ", 190, DataGridViewContentAlignment.MiddleLeft));
            dgvDebtDetails.Columns.Add(TaoCot("TongPhaiDong", "Phần phải đóng", 120, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgvDebtDetails.Columns.Add(TaoCot("DaThanhToan", "Phần đã đóng", 115, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgvDebtDetails.Columns.Add(TaoCot("ConNo", "Còn nợ", 115, DataGridViewContentAlignment.MiddleRight, "N0"));
            dgvDebtDetails.Columns.Add(TaoCot("HanThanhToan", "Hạn thanh toán", 105, DataGridViewContentAlignment.MiddleCenter, "dd/MM/yyyy"));
            dgvDebtDetails.Columns.Add(TaoCot("TrangThai", "Trạng thái", 130, DataGridViewContentAlignment.MiddleCenter));

            dgvDebtDetails.DataSource = sv.ChiTiet;
        }

        private void XuatExcelCongNo(KetQuaCongNoDTO kq, string moTaBoLoc)
        {
            if (kq == null || kq.DanhSach.Count == 0)
            {
                MessageBox.Show("Không có sinh viên nợ phí nào trong danh sách hiện tại để xuất Excel!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Xuất danh sách sinh viên nợ đọng phí";
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = $"DanhSachSinhVienNoPhi_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        thongKeBLL.XuatExcelCongNo(kq, sfd.FileName, moTaBoLoc);
                        MessageBox.Show(
                            $"Đã xuất thành công {kq.TongSoSinhVienNo} sinh viên nợ phí (tổng nợ {kq.TongConNo:N0} đ) ra file:\n{sfd.FileName}",
                            "Xuất Excel thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất file Excel: " + ex.Message,
                            "Lỗi xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // =====================================================================
        // VẼ BIỂU ĐỒ GDI+
        // =====================================================================

        private void pnlChartDashPie_Paint(object sender, PaintEventArgs e)
        {
            VeBieuDoTronPhong(e.Graphics, pnlChartDashPie.ClientRectangle, duLieuPhong, "TÌNH TRẠNG PHÒNG");
        }

        private void pnlChartRoomPie_Paint(object sender, PaintEventArgs e)
        {
            VeBieuDoTronPhong(e.Graphics, pnlChartRoomPie.ClientRectangle, duLieuPhong, "TỶ LỆ TRẠNG THÁI PHÒNG");
        }

        private void pnlChartDashMonth_Paint(object sender, PaintEventArgs e)
        {
            VeBieuDoCotDoanhThu(e.Graphics, pnlChartDashMonth.ClientRectangle,
                duLieuDoanhThuDash?.TheoThang, "DOANH THU 12 THÁNG (VNĐ)", true);
        }

        private void pnlChartDashQuarter_Paint(object sender, PaintEventArgs e)
        {
            VeBieuDoCotDoanhThu(e.Graphics, pnlChartDashQuarter.ClientRectangle,
                duLieuDoanhThuDash?.TheoQuy, "DOANH THU 4 QUÝ", false);
        }

        private void pnlChartRevMonth_Paint(object sender, PaintEventArgs e)
        {
            VeBieuDoCotDoanhThu(e.Graphics, pnlChartRevMonth.ClientRectangle,
                duLieuDoanhThu?.TheoThang, "BIỂU ĐỒ DOANH THU THEO THÁNG (VNĐ)", true);
        }

        private void pnlChartRevQuarter_Paint(object sender, PaintEventArgs e)
        {
            VeBieuDoCotDoanhThu(e.Graphics, pnlChartRevQuarter.ClientRectangle,
                duLieuDoanhThu?.TheoQuy, "DOANH THU THEO QUÝ", false);
        }

        private void pnlChartRevSource_Paint(object sender, PaintEventArgs e)
        {
            VeBieuDoTronNguonThu(e.Graphics, pnlChartRevSource.ClientRectangle, duLieuDoanhThu);
        }

        private static void VeBieuDoTronPhong(Graphics g, Rectangle bounds, ThongKePhongTongQuanDTO data, string title)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            VeTieuDeKhung(g, bounds, title);

            if (data == null || data.TongSoPhong <= 0)
            {
                VeTrangThaiTrong(g, bounds, "Chưa có dữ liệu phòng trong hệ thống");
                return;
            }

            var items = new List<Tuple<string, int, Color>>
            {
                Tuple.Create("Trống", data.SoPhongTrong, Color.FromArgb(148, 163, 184)),
                Tuple.Create("Còn chỗ", data.SoPhongConCho, Color.FromArgb(37, 99, 235)),
                Tuple.Create("Đã đầy", data.SoPhongDaDay, Color.FromArgb(22, 163, 74))
            };
            if (data.SoPhongBatThuong > 0)
                items.Add(Tuple.Create("Bất thường", data.SoPhongBatThuong, Color.FromArgb(220, 38, 38)));

            int diameter = Math.Max(60, Math.Min(bounds.Height - 54, (int)(bounds.Width * 0.48)));
            Rectangle pieRect = new Rectangle(bounds.X + 18, bounds.Y + 36 + (bounds.Height - 48 - diameter) / 2, diameter, diameter);

            float start = -90f;
            int total = items.Sum(i => i.Item2);
            foreach (var it in items)
            {
                if (it.Item2 <= 0) continue;
                float sweep = (float)it.Item2 * 360f / total;
                using (SolidBrush br = new SolidBrush(it.Item3))
                {
                    g.FillPie(br, pieRect, start, sweep);
                }
                start += sweep;
            }

            // Lỗ donut ở giữa hiển thị tỷ lệ lấp đầy
            int hole = (int)(diameter * 0.56);
            Rectangle holeRect = new Rectangle(pieRect.X + (diameter - hole) / 2, pieRect.Y + (diameter - hole) / 2, hole, hole);
            g.FillEllipse(Brushes.White, holeRect);
            using (Font fBig = new Font("Segoe UI", 11F, FontStyle.Bold))
            using (Font fSub = new Font("Segoe UI", 7.5F))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString($"{data.TyLeLapDay:0.#}%\nlấp đầy", fBig, new SolidBrush(Color.FromArgb(15, 23, 42)), holeRect, sf);
            }

            // Chú thích bên phải
            int lx = pieRect.Right + 16;
            int ly = pieRect.Top + 6;
            using (Font f = new Font("Segoe UI", 8.75F))
            using (Font fBold = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold))
            {
                foreach (var it in items)
                {
                    double pct = total > 0 ? Math.Round(it.Item2 * 100.0 / total, 1) : 0;
                    using (SolidBrush br = new SolidBrush(it.Item3))
                    {
                        g.FillRectangle(br, lx, ly + 3, 12, 12);
                    }
                    g.DrawString($"{it.Item1}: {it.Item2} ({pct:0.#}%)", fBold,
                        new SolidBrush(Color.FromArgb(30, 41, 59)), lx + 18, ly);
                    ly += 24;
                }
                g.DrawString($"Giường: {data.SoGiuongDangSuDung}/{data.TongSoGiuong}", f,
                    new SolidBrush(Color.FromArgb(100, 116, 139)), lx, ly + 4);
            }
        }

        private static void VeBieuDoTronNguonThu(Graphics g, Rectangle bounds, BaoCaoDoanhThuDTO data)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            VeTieuDeKhung(g, bounds, "PHÂN LOẠI NGUỒN THU");

            if (data == null || data.TongDoanhThu <= 0)
            {
                VeTrangThaiTrong(g, bounds, "Chưa có doanh thu thực nhận");
                return;
            }

            var items = new List<Tuple<string, decimal, Color>>
            {
                Tuple.Create("Tiền phòng", data.DoanhThuTienPhong, Color.FromArgb(37, 99, 235)),
                Tuple.Create("Dịch vụ (Điện+Nước)", data.DoanhThuDichVu, Color.FromArgb(16, 185, 129)),
                Tuple.Create("Khoản thu khác", data.DoanhThuKhac, Color.FromArgb(245, 158, 11))
            };

            int diameter = Math.Max(60, Math.Min(bounds.Height - 54, (int)(bounds.Width * 0.44)));
            Rectangle pieRect = new Rectangle(bounds.X + 14, bounds.Y + 36 + (bounds.Height - 48 - diameter) / 2, diameter, diameter);

            decimal total = items.Sum(i => Math.Max(0, i.Item2));
            if (total <= 0)
            {
                VeTrangThaiTrong(g, bounds, "Chưa có doanh thu dương");
                return;
            }

            float start = -90f;
            foreach (var it in items)
            {
                if (it.Item2 <= 0) continue;
                float sweep = (float)(it.Item2 * 360m / total);
                using (SolidBrush br = new SolidBrush(it.Item3))
                {
                    g.FillPie(br, pieRect, start, sweep);
                }
                start += sweep;
            }

            int hole = (int)(diameter * 0.52);
            g.FillEllipse(Brushes.White, new Rectangle(pieRect.X + (diameter - hole) / 2, pieRect.Y + (diameter - hole) / 2, hole, hole));

            int lx = pieRect.Right + 12;
            int ly = pieRect.Top + 4;
            using (Font fBold = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold))
            using (Font fSub = new Font("Segoe UI", 8F))
            {
                foreach (var it in items)
                {
                    decimal pct = Math.Round(it.Item2 * 100m / total, 1);
                    using (SolidBrush br = new SolidBrush(it.Item3))
                    {
                        g.FillRectangle(br, lx, ly + 3, 11, 11);
                    }
                    g.DrawString($"{it.Item1} ({pct:0.#}%)", fBold, new SolidBrush(Color.FromArgb(30, 41, 59)), lx + 16, ly);
                    g.DrawString($"{it.Item2:N0} đ", fSub, new SolidBrush(Color.FromArgb(100, 116, 139)), lx + 16, ly + 15);
                    ly += 36;
                }
            }
        }

        private static void VeBieuDoCotDoanhThu(Graphics g, Rectangle bounds, List<DoanhThuKyDTO> ds, string title, bool laThang)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            VeTieuDeKhung(g, bounds, title);

            if (ds == null || ds.Count == 0 || ds.All(x => x.TongThu == 0))
            {
                VeTrangThaiTrong(g, bounds, "Chưa có giao dịch thanh toán trong kỳ đã chọn");
                return;
            }

            int padLeft = 56, padRight = 14, padTop = 38, padBottom = 28;
            Rectangle plot = new Rectangle(bounds.X + padLeft, bounds.Y + padTop,
                Math.Max(40, bounds.Width - padLeft - padRight),
                Math.Max(40, bounds.Height - padTop - padBottom));

            decimal maxVal = ds.Max(x => Math.Max(0, x.TongThu));
            if (maxVal <= 0) maxVal = 1;

            using (Pen gridPen = new Pen(Color.FromArgb(226, 232, 240), 1f) { DashStyle = DashStyle.Dot })
            using (Font fAxis = new Font("Segoe UI", 7.5F))
            using (Brush brAxis = new SolidBrush(Color.FromArgb(100, 116, 139)))
            {
                for (int i = 0; i <= 4; i++)
                {
                    int y = plot.Bottom - (int)(plot.Height * (i / 4.0));
                    g.DrawLine(gridPen, plot.Left, y, plot.Right, y);
                    decimal v = maxVal * i / 4m;
                    string lbl = DinhDangTienTruc(v);
                    SizeF sz = g.MeasureString(lbl, fAxis);
                    g.DrawString(lbl, fAxis, brAxis, plot.Left - sz.Width - 4, y - sz.Height / 2);
                }
            }

            int n = ds.Count;
            float slotW = (float)plot.Width / n;
            float barW = Math.Max(8f, Math.Min(38f, slotW * 0.58f));

            using (Font fLbl = new Font("Segoe UI", 7.5F))
            using (Font fVal = new Font("Segoe UI Semibold", 7.25F, FontStyle.Bold))
            using (Brush brText = new SolidBrush(Color.FromArgb(51, 65, 85)))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center })
            {
                for (int i = 0; i < n; i++)
                {
                    decimal val = Math.Max(0, ds[i].TongThu);
                    float h = (float)(val / maxVal) * (plot.Height - 14);
                    float x = plot.Left + i * slotW + (slotW - barW) / 2f;
                    float y = plot.Bottom - h;

                    Color barColor = laThang ? Color.FromArgb(37, 99, 235) : Color.FromArgb(14, 165, 233);
                    if (val > 0)
                    {
                        using (SolidBrush br = new SolidBrush(barColor))
                        {
                            g.FillRectangle(br, x, y, barW, Math.Max(2f, h));
                        }
                        string valText = DinhDangTienTruc(val);
                        g.DrawString(valText, fVal, brText, x + barW / 2f, y - 13, sf);
                    }

                    string xLbl = laThang ? ("T" + ds[i].Ky) : ("Q" + ds[i].Ky);
                    g.DrawString(xLbl, fLbl, brText, x + barW / 2f, plot.Bottom + 4, sf);
                }
            }
        }

        private static void VeTieuDeKhung(Graphics g, Rectangle bounds, string title)
        {
            using (Font f = new Font("Segoe UI Semibold", 9.25F, FontStyle.Bold))
            using (Brush br = new SolidBrush(Color.FromArgb(30, 41, 59)))
            {
                g.DrawString(title, f, br, bounds.X + 12, bounds.Y + 8);
            }
            using (Pen border = new Pen(Color.FromArgb(226, 232, 240)))
            {
                g.DrawRectangle(border, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }
        }

        private static void VeTrangThaiTrong(Graphics g, Rectangle bounds, string message)
        {
            using (Font f = new Font("Segoe UI", 9F, FontStyle.Italic))
            using (Brush br = new SolidBrush(Color.FromArgb(148, 163, 184)))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(message, f, br, new Rectangle(bounds.X, bounds.Y + 24, bounds.Width, bounds.Height - 24), sf);
            }
        }

        // =====================================================================
        // SỰ KIỆN GIAO DIỆN
        // =====================================================================

        private void btnTabDashboard_Click(object sender, EventArgs e) => ChuyenTab(0);
        private void btnTabPhong_Click(object sender, EventArgs e) => ChuyenTab(1);
        private void btnTabDoanhThu_Click(object sender, EventArgs e) => ChuyenTab(2);
        private void btnTabCongNo_Click(object sender, EventArgs e) => ChuyenTab(3);
        private void btnRefreshAll_Click(object sender, EventArgs e) => RefreshAllData();

        private void cboDashYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dangNapBoLoc) return;
            TaiTabTongQuan();
        }

        private void btnDashExport_Click(object sender, EventArgs e)
        {
            if (duLieuCongNoDash == null) duLieuCongNoDash = thongKeBLL.GetCongNo(new CongNoFilterDTO());
            XuatExcelCongNo(duLieuCongNoDash, "Tất cả sinh viên đang nợ phí (xuất từ Dashboard)");
        }

        private void RoomFilter_Changed(object sender, EventArgs e)
        {
            if (dangNapBoLoc) return;
            ApDungLocPhong();
        }

        private void btnRoomReset_Click(object sender, EventArgs e)
        {
            dangNapBoLoc = true;
            txtRoomSearch.Text = "";
            if (cboRoomArea.Items.Count > 0) cboRoomArea.SelectedIndex = 0;
            if (cboRoomType.Items.Count > 0) cboRoomType.SelectedIndex = 0;
            if (cboRoomStatus.Items.Count > 0) cboRoomStatus.SelectedIndex = 0;
            dangNapBoLoc = false;
            ApDungLocPhong();
        }

        private void RevFilter_Changed(object sender, EventArgs e)
        {
            if (dangNapBoLoc) return;
            TaiTabDoanhThu();
        }

        private void chkDebtPeriod_CheckedChanged(object sender, EventArgs e)
        {
            dtpDebtFrom.Enabled = chkDebtPeriod.Checked;
            dtpDebtTo.Enabled = chkDebtPeriod.Checked;
            if (!dangNapBoLoc) TaiTabCongNo();
        }

        private void DebtFilter_Changed(object sender, EventArgs e)
        {
            if (dangNapBoLoc) return;
            TaiTabCongNo();
        }

        private void btnDebtReset_Click(object sender, EventArgs e)
        {
            dangNapBoLoc = true;
            txtDebtSearch.Text = "";
            if (cboDebtRoom.Items.Count > 0) cboDebtRoom.SelectedIndex = 0;
            if (cboDebtFeeType.Items.Count > 0) cboDebtFeeType.SelectedIndex = 0;
            if (cboDebtPayStatus.Items.Count > 0) cboDebtPayStatus.SelectedIndex = 0;
            if (cboDebtOverdue.Items.Count > 0) cboDebtOverdue.SelectedIndex = 0;
            chkDebtPeriod.Checked = false;
            dtpDebtFrom.Enabled = false;
            dtpDebtTo.Enabled = false;
            dangNapBoLoc = false;
            TaiTabCongNo();
        }

        private void btnDebtExport_Click(object sender, EventArgs e)
        {
            CongNoFilterDTO f = TaoBoLocCongNo();
            if (duLieuCongNo == null) duLieuCongNo = thongKeBLL.GetCongNo(f);
            XuatExcelCongNo(duLieuCongNo, MoTaBoLocCongNo(f));
        }

        private void btnDebtPrev_Click(object sender, EventArgs e)
        {
            trangCongNo--;
            HienThiTrangCongNo();
        }

        private void btnDebtNext_Click(object sender, EventArgs e)
        {
            trangCongNo++;
            HienThiTrangCongNo();
        }

        private void dgvDebtStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDebtStudents.CurrentRow?.DataBoundItem is CongNoSinhVienDTO sv)
            {
                HienThiChiTietKhoanNo(sv);
            }
        }

        private void dgvRoom_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRoom.Columns[e.ColumnIndex].DataPropertyName == "TrangThai" && e.Value != null)
            {
                string v = e.Value.ToString();
                if (v == ThongKeBLL.TT_BAT_THUONG)
                {
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                    e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
                else if (v == ThongKeBLL.TT_DA_DAY)
                {
                    e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74);
                }
                else if (v == ThongKeBLL.TT_CON_CHO)
                {
                    e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);
                }
            }
        }

        private void dgvDebt_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv == null || e.RowIndex < 0) return;
            string prop = dgv.Columns[e.ColumnIndex].DataPropertyName;
            if (prop == "ConNo")
            {
                e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }
            else if (prop == "TrangThai" && e.Value != null && e.Value.ToString().Contains(ThongKeBLL.HAN_QUA_HAN))
            {
                e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
            }
        }

        // =====================================================================
        // TIỆN ÍCH ĐỊNH DẠNG
        // =====================================================================

        private static DataGridViewTextBoxColumn TaoCot(string prop, string header, int fillWeight,
            DataGridViewContentAlignment align, string format = null)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = "col_" + prop,
                DataPropertyName = prop,
                HeaderText = header,
                FillWeight = fillWeight,
                ReadOnly = true
            };
            col.DefaultCellStyle.Alignment = align;
            col.HeaderCell.Style.Alignment = align;
            if (!string.IsNullOrEmpty(format)) col.DefaultCellStyle.Format = format;
            return col;
        }

        private static int LayNam(ComboBox cbo)
        {
            if (cbo.SelectedItem != null && int.TryParse(cbo.SelectedItem.ToString(), out int y))
                return y;
            return DateTime.Today.Year;
        }

        private static void HienThiCanhBao(Label lbl, List<string> canhBao)
        {
            if (canhBao == null || canhBao.Count == 0)
            {
                lbl.Visible = false;
                lbl.Text = "";
            }
            else
            {
                lbl.Text = "⚠ " + string.Join("   |   ⚠ ", canhBao);
                lbl.Visible = true;
            }
        }

        private static string DinhDangTienRutGon(decimal v)
        {
            decimal abs = Math.Abs(v);
            if (abs >= 1_000_000_000m) return $"{v / 1_000_000_000m:0.##} tỷ";
            if (abs >= 100_000_000m) return $"{v / 1_000_000m:0.#} tr";
            return $"{v:N0} đ";
        }

        private static string DinhDangTienTruc(decimal v)
        {
            decimal abs = Math.Abs(v);
            if (abs >= 1_000_000_000m) return $"{v / 1_000_000_000m:0.#}B";
            if (abs >= 1_000_000m) return $"{v / 1_000_000m:0.#}M";
            if (abs >= 1_000m) return $"{v / 1_000m:0.#}K";
            return $"{v:0}";
        }
    }
}

