using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QuanLyKyTucXa.BLL.UTIL;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    /// <summary>
    /// Nghiệp vụ module Quản lý thống kê.
    /// Tổng hợp dữ liệu thật từ 4 module: Sinh viên, Phòng, Lưu trú (Hợp đồng), Dịch vụ &amp; Tài chính.
    /// </summary>
    public class ThongKeBLL
    {
        /// <summary>
        /// Hạn thanh toán của hóa đơn = Ngày lập + SO_NGAY_HAN_THANH_TOAN.
        /// Bảng HoaDon hiện chưa có cột hạn thanh toán, đây là quy ước nghiệp vụ của module thống kê.
        /// </summary>
        public const int SO_NGAY_HAN_THANH_TOAN = 15;

        public const string TT_TRONG = "Trống";
        public const string TT_CON_CHO = "Còn chỗ";
        public const string TT_DA_DAY = "Đã đầy";
        public const string TT_BAT_THUONG = "Bất thường";

        public const string TTTT_CHUA = "Chưa thanh toán";
        public const string TTTT_MOT_PHAN = "Thanh toán một phần";
        public const string HAN_QUA_HAN = "Quá hạn";
        public const string HAN_CHUA_DEN = "Chưa đến hạn";

        private readonly ThongKeDAL thongKeDAL = new ThongKeDAL();

        private static void KiemTraQuyen()
        {
            // Hệ thống phân quyền hiện tại: Admin + NhanVien đều được dùng module nghiệp vụ,
            // chỉ module Tài khoản giới hạn Admin. Thống kê yêu cầu đã đăng nhập.
            if (!UserSession.IsLoggedIn)
                throw new UnauthorizedAccessException("Bạn cần đăng nhập để sử dụng module Quản lý thống kê!");
        }

        // =====================================================================
        // 1. THỐNG KÊ TÌNH TRẠNG PHÒNG
        // =====================================================================

        public ThongKePhongTongQuanDTO GetThongKePhong()
        {
            return GetThongKePhong(DateTime.Today);
        }

        public ThongKePhongTongQuanDTO GetThongKePhong(DateTime ngayThongKe)
        {
            KiemTraQuyen();
            DataTable dt = thongKeDAL.GetPhongVaSoNguoiDangO(ngayThongKe);
            ThongKePhongTongQuanDTO kq = new ThongKePhongTongQuanDTO();
            int soPhongLechBoDem = 0;

            foreach (DataRow row in dt.Rows)
            {
                int sucChua = ToInt(row["SoGiuongToiDa"]);
                int giuongTrongModulePhong = ToInt(row["SoGiuongTrong"]);
                int soNguoiTheoPhong = Math.Max(0, sucChua - giuongTrongModulePhong);
                int soNguoiTheoHopDong = ToInt(row["SoNguoiDangO"]);

                // Kết hợp dữ liệu từ cả module Quản lý Phòng (SoGiuongToiDa - SoGiuongTrong)
                // và module Quản lý Lưu trú (HopDong đang hiệu lực):
                // Nếu phòng đã được khai báo số người đang ở trực tiếp tại module Phòng hoặc qua Hợp đồng,
                // lấy giá trị thực tế lớn hơn để đồng bộ số liệu giữa hai module.
                int soNguoi = Math.Max(soNguoiTheoPhong, soNguoiTheoHopDong);

                ThongKePhongChiTietDTO ct = new ThongKePhongChiTietDTO
                {
                    MaPhong = Convert.ToString(row["MaPhong"]),
                    TenPhong = Convert.ToString(row["TenPhong"]),
                    TenKhu = Convert.ToString(row["TenKhu"]),
                    LoaiPhong = Convert.ToString(row["LoaiPhong"]),
                    TinhTrangVanHanh = Convert.ToString(row["TrangThaiPhong"]),
                    SucChua = sucChua,
                    SoNguoiDangO = soNguoi,
                    SoNguoiTheoModulePhong = soNguoiTheoPhong
                };

                List<string> ghiChu = new List<string>();
                if (sucChua <= 0)
                {
                    ct.TrangThai = TT_BAT_THUONG;
                    ct.SoGiuongTrong = 0;
                    ghiChu.Add($"Sức chứa không hợp lệ ({sucChua})");
                }
                else if (soNguoi > sucChua)
                {
                    ct.TrangThai = TT_BAT_THUONG;
                    ct.SoGiuongTrong = 0;
                    ghiChu.Add($"Số người đang ở ({soNguoi}) vượt sức chứa ({sucChua})");
                }
                else
                {
                    ct.SoGiuongTrong = sucChua - soNguoi;
                    ct.TrangThai = soNguoi == 0 ? TT_TRONG : (soNguoi < sucChua ? TT_CON_CHO : TT_DA_DAY);
                }

                if (sucChua > 0 && soNguoiTheoPhong != soNguoiTheoHopDong)
                {
                    soPhongLechBoDem++;
                    ghiChu.Add($"Phòng: {soNguoiTheoPhong} SV | Hợp đồng: {soNguoiTheoHopDong} SV");
                }

                ct.GhiChuBatThuong = string.Join("; ", ghiChu);
                ct.TyLeLapDay = sucChua > 0 ? Math.Round(Math.Min(soNguoi, sucChua) * 100.0 / sucChua, 1) : 0;
                kq.ChiTiet.Add(ct);
            }

            kq.TongSoPhong = kq.ChiTiet.Count;
            kq.TongSoGiuong = kq.ChiTiet.Sum(p => Math.Max(0, p.SucChua));
            kq.SoGiuongDangSuDung = kq.ChiTiet.Sum(p => Math.Min(p.SoNguoiDangO, Math.Max(0, p.SucChua)));
            kq.SoGiuongTrong = kq.TongSoGiuong - kq.SoGiuongDangSuDung;
            kq.SoPhongTrong = kq.ChiTiet.Count(p => p.TrangThai == TT_TRONG);
            kq.SoPhongConCho = kq.ChiTiet.Count(p => p.TrangThai == TT_CON_CHO);
            kq.SoPhongDaDay = kq.ChiTiet.Count(p => p.TrangThai == TT_DA_DAY);
            kq.SoPhongBatThuong = kq.ChiTiet.Count(p => p.TrangThai == TT_BAT_THUONG);
            kq.TyLeLapDay = kq.TongSoGiuong > 0 ? Math.Round(kq.SoGiuongDangSuDung * 100.0 / kq.TongSoGiuong, 1) : 0;
            int svTheoHopDong = thongKeDAL.CountSinhVienDangLuuTru(ngayThongKe);
            kq.SoSinhVienDangLuuTru = Math.Max(svTheoHopDong, kq.SoGiuongDangSuDung);

            // ---- Cảnh báo dữ liệu bất thường ----
            if (kq.SoPhongBatThuong > 0)
            {
                int vuot = kq.ChiTiet.Where(p => p.SucChua > 0 && p.SoNguoiDangO > p.SucChua).Sum(p => p.SoNguoiDangO - p.SucChua);
                kq.CanhBao.Add($"Có {kq.SoPhongBatThuong} phòng dữ liệu bất thường (sức chứa không hợp lệ hoặc vượt sức chứa" +
                               (vuot > 0 ? $", tổng {vuot} sinh viên vượt sức chứa không được tính vào giường đang dùng" : "") + ").");
            }
            if (soPhongLechBoDem > 0)
                kq.CanhBao.Add($"Lưu ý: Có {soPhongLechBoDem} phòng đã khai báo số người ở trực tiếp tại module Phòng nhưng chưa lập đủ Hợp đồng lưu trú tương ứng.");

            DataTable dtTrung = thongKeDAL.GetSinhVienTrungHopDongHieuLuc(ngayThongKe);
            if (dtTrung.Rows.Count > 0)
            {
                List<string> ds = dtTrung.AsEnumerable().Take(5)
                    .Select(r => $"{r["MaSV"]} ({r["DanhSachPhong"]})").ToList();
                kq.CanhBao.Add($"Có {dtTrung.Rows.Count} sinh viên đồng thời có nhiều hợp đồng đang hiệu lực: {string.Join(", ", ds)}" +
                               (dtTrung.Rows.Count > 5 ? "..." : "") + ". Tổng số sinh viên lưu trú chỉ đếm 1 lần/sinh viên.");
            }

            int hdQuaHan = thongKeDAL.CountHopDongQuaHanChuaCapNhat(ngayThongKe);
            if (hdQuaHan > 0)
                kq.CanhBao.Add($"Có {hdQuaHan} hợp đồng đã quá ngày kết thúc nhưng vẫn ở trạng thái 'Đang hiệu lực' (không được tính là đang lưu trú). Hãy chạy cập nhật hợp đồng hết hạn trong module Hợp đồng.");

            return kq;
        }

        /// <summary>
        /// Lọc danh sách chi tiết phòng theo từ khóa (mã/tên phòng), dãy nhà, loại phòng và trạng thái.
        /// </summary>
        public List<ThongKePhongChiTietDTO> LocChiTietPhong(IEnumerable<ThongKePhongChiTietDTO> nguon,
            string tuKhoa, string tenKhu, string loaiPhong, string trangThai)
        {
            if (nguon == null) return new List<ThongKePhongChiTietDTO>();
            IEnumerable<ThongKePhongChiTietDTO> q = nguon;

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string k = tuKhoa.Trim();
                q = q.Where(p => Contains(p.MaPhong, k) || Contains(p.TenPhong, k));
            }
            if (!string.IsNullOrWhiteSpace(tenKhu))
                q = q.Where(p => string.Equals(p.TenKhu, tenKhu, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(loaiPhong))
                q = q.Where(p => string.Equals(p.LoaiPhong, loaiPhong, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(trangThai))
                q = q.Where(p => string.Equals(p.TrangThai, trangThai, StringComparison.OrdinalIgnoreCase));

            return q.ToList();
        }

        // =====================================================================
        // 2. BÁO CÁO DOANH THU THEO THÁNG / QUÝ
        // =====================================================================

        public List<int> GetDanhSachNam()
        {
            List<int> ds;
            try { ds = thongKeDAL.GetDanhSachNamCoDuLieu(); }
            catch { ds = new List<int>(); }
            if (!ds.Contains(DateTime.Today.Year)) ds.Add(DateTime.Today.Year);
            return ds.Distinct().OrderByDescending(y => y).ToList();
        }

        /// <summary>
        /// Báo cáo doanh thu thực nhận trong năm, giới hạn từ tháng đến tháng.
        /// - Nguồn: LichSuThanhToan (mỗi dòng = 1 giao dịch thu tiền thành công).
        /// - Kỳ ghi nhận: theo NgayThanhToan.
        /// - Phân loại nguồn thu: phân bổ số tiền của từng phiếu thu theo tỷ trọng
        ///   TienPhong / TienDien / TienNuoc / TienDichVuKhac của hóa đơn gốc.
        /// </summary>
        public BaoCaoDoanhThuDTO GetBaoCaoDoanhThu(int nam, int tuThang = 1, int denThang = 12)
        {
            KiemTraQuyen();
            if (nam < 2000 || nam > 2100)
                throw new ArgumentException("Năm thống kê không hợp lệ!");
            if (tuThang < 1 || tuThang > 12 || denThang < 1 || denThang > 12)
                throw new ArgumentException("Tháng thống kê phải nằm trong khoảng 1 - 12!");
            if (tuThang > denThang)
                throw new ArgumentException("Tháng bắt đầu không được lớn hơn tháng kết thúc!");

            DateTime tuNgay = new DateTime(nam, tuThang, 1);
            DateTime denNgay = new DateTime(nam, denThang, 1).AddMonths(1);

            DataTable dtThu = thongKeDAL.GetPhieuThuTrongKhoang(tuNgay, denNgay);
            DataTable dtPhatSinh = thongKeDAL.GetPhatSinhVaConNoTheoThang(nam);

            // Khởi tạo 12 tháng
            Dictionary<int, DoanhThuKyDTO> thang = new Dictionary<int, DoanhThuKyDTO>();
            for (int t = tuThang; t <= denThang; t++)
            {
                thang[t] = new DoanhThuKyDTO { Nam = nam, Ky = t, TenKy = "Tháng " + t };
            }

            int soGiaoDichAm = 0;
            foreach (DataRow r in dtThu.Rows)
            {
                DateTime ngay = Convert.ToDateTime(r["NgayThanhToan"]);
                DoanhThuKyDTO ky;
                if (!thang.TryGetValue(ngay.Month, out ky)) continue;

                decimal soTien = ToDecimal(r["SoTienThanhToan"]);
                decimal[] coCau = CoCauHoaDon(r);
                decimal[] phanBo = PhanBoTheoTyTrong(soTien, coCau);

                ky.ThuTienPhong += phanBo[LoaiPhi.Phong];
                ky.ThuTienDien += phanBo[LoaiPhi.Dien];
                ky.ThuTienNuoc += phanBo[LoaiPhi.Nuoc];
                ky.ThuDichVuKhac += phanBo[LoaiPhi.Khac];
                ky.TongThu += soTien;
                if (soTien > 0) ky.SoGiaoDich++;
                else soGiaoDichAm++;
            }

            foreach (DataRow r in dtPhatSinh.Rows)
            {
                int t = ToInt(r["Thang"]);
                DoanhThuKyDTO ky;
                if (!thang.TryGetValue(t, out ky)) continue;
                ky.PhatSinh = ToDecimal(r["PhatSinh"]);
                ky.ConPhaiThu = ToDecimal(r["ConPhaiThu"]);
            }

            BaoCaoDoanhThuDTO bc = new BaoCaoDoanhThuDTO
            {
                Nam = nam,
                TuThang = tuThang,
                DenThang = denThang,
                TheoThang = thang.Values.OrderBy(k => k.Ky).ToList()
            };
            TinhTangTruong(bc.TheoThang);

            // Theo quý (chỉ các tháng nằm trong khoảng lọc)
            for (int q = 1; q <= 4; q++)
            {
                List<DoanhThuKyDTO> cacThang = bc.TheoThang.Where(k => (k.Ky - 1) / 3 + 1 == q).ToList();
                if (cacThang.Count == 0) continue;
                bc.TheoQuy.Add(new DoanhThuKyDTO
                {
                    Nam = nam,
                    Ky = q,
                    TenKy = "Quý " + SoLaMa(q),
                    SoGiaoDich = cacThang.Sum(k => k.SoGiaoDich),
                    ThuTienPhong = cacThang.Sum(k => k.ThuTienPhong),
                    ThuTienDien = cacThang.Sum(k => k.ThuTienDien),
                    ThuTienNuoc = cacThang.Sum(k => k.ThuTienNuoc),
                    ThuDichVuKhac = cacThang.Sum(k => k.ThuDichVuKhac),
                    TongThu = cacThang.Sum(k => k.TongThu),
                    PhatSinh = cacThang.Sum(k => k.PhatSinh),
                    ConPhaiThu = cacThang.Sum(k => k.ConPhaiThu)
                });
            }
            TinhTangTruong(bc.TheoQuy);

            bc.TongDoanhThu = bc.TheoThang.Sum(k => k.TongThu);
            bc.DoanhThuTienPhong = bc.TheoThang.Sum(k => k.ThuTienPhong);
            bc.DoanhThuDien = bc.TheoThang.Sum(k => k.ThuTienDien);
            bc.DoanhThuNuoc = bc.TheoThang.Sum(k => k.ThuTienNuoc);
            bc.DoanhThuDichVu = bc.DoanhThuDien + bc.DoanhThuNuoc;
            bc.DoanhThuKhac = bc.TheoThang.Sum(k => k.ThuDichVuKhac);
            bc.SoGiaoDichThanhCong = bc.TheoThang.Sum(k => k.SoGiaoDich);
            bc.TongPhatSinh = bc.TheoThang.Sum(k => k.PhatSinh);
            bc.TongConPhaiThu = bc.TheoThang.Sum(k => k.ConPhaiThu);

            // Trung bình theo số tháng đã diễn ra trong khoảng lọc (không chia cho các tháng tương lai)
            int thangCuoiDaQua = denThang;
            if (nam == DateTime.Today.Year) thangCuoiDaQua = Math.Min(denThang, DateTime.Today.Month);
            else if (nam > DateTime.Today.Year) thangCuoiDaQua = tuThang - 1;
            int soThangTinh = Math.Max(0, thangCuoiDaQua - tuThang + 1);
            bc.DoanhThuTrungBinhThang = soThangTinh > 0 ? Math.Round(bc.TongDoanhThu / soThangTinh, 0) : 0;

            List<DoanhThuKyDTO> thangDaQua = bc.TheoThang.Where(k => k.Ky <= thangCuoiDaQua).ToList();
            if (bc.TongDoanhThu != 0 && thangDaQua.Count > 0)
            {
                bc.ThangCaoNhat = thangDaQua.OrderByDescending(k => k.TongThu).ThenBy(k => k.Ky).First();
                bc.ThangThapNhat = thangDaQua.OrderBy(k => k.TongThu).ThenBy(k => k.Ky).First();
            }

            // Cảnh báo sai lệch dữ liệu tài chính
            if (soGiaoDichAm > 0)
                bc.CanhBao.Add($"Có {soGiaoDichAm} phiếu thu có số tiền ≤ 0 (điều chỉnh/hoàn tiền) đã được trừ vào doanh thu và không tính là giao dịch thành công.");
            try
            {
                DataTable dtLech = thongKeDAL.GetHoaDonLechThanhToan();
                if (dtLech.Rows.Count > 0)
                {
                    bc.CanhBao.Add($"Có {dtLech.Rows.Count} hóa đơn có 'Đã thanh toán' không khớp tổng phiếu thu (VD: HĐ #{dtLech.Rows[0]["MaHoaDon"]}). " +
                                   "Doanh thu thực nhận chỉ tính theo phiếu thu có ngày thanh toán.");
                }
            }
            catch { }

            return bc;
        }

        // =====================================================================
        // 3. CÔNG NỢ SINH VIÊN
        // =====================================================================

        /// <summary>
        /// Tổng hợp công nợ theo sinh viên.
        /// Hóa đơn trong hệ thống được lập theo PHÒNG/THÁNG, vì vậy nghĩa vụ của từng sinh viên
        /// = phần chia đều hóa đơn cho các sinh viên có hợp đồng lưu trú tại phòng trong tháng đó.
        /// Số đã thanh toán của hóa đơn cũng được chia đều tương ứng.
        /// </summary>
        public KetQuaCongNoDTO GetCongNo(CongNoFilterDTO filter)
        {
            return GetCongNo(filter, DateTime.Today);
        }

        public KetQuaCongNoDTO GetCongNo(CongNoFilterDTO filter, DateTime ngayThongKe)
        {
            KiemTraQuyen();
            if (filter == null) filter = new CongNoFilterDTO();

            int? tuKy = filter.TuKy.HasValue ? filter.TuKy.Value.Year * 100 + filter.TuKy.Value.Month : (int?)null;
            int? denKy = filter.DenKy.HasValue ? filter.DenKy.Value.Year * 100 + filter.DenKy.Value.Month : (int?)null;
            if (tuKy.HasValue && denKy.HasValue && tuKy.Value > denKy.Value)
                throw new ArgumentException("Kỳ bắt đầu không được lớn hơn kỳ kết thúc!");
            if (filter.LoaiPhi < -1 || filter.LoaiPhi > 3)
                throw new ArgumentException("Loại phí không hợp lệ!");

            DataTable dt = thongKeDAL.GetHoaDonConNoVaSinhVien(tuKy, denKy, filter.MaPhong);
            KetQuaCongNoDTO kq = new KetQuaCongNoDTO();
            List<CongNoChiTietDTO> tatCaChiTiet = new List<CongNoChiTietDTO>();
            Dictionary<string, DataRow> thongTinSV = new Dictionary<string, DataRow>();

            foreach (var nhomHD in dt.AsEnumerable().GroupBy(r => ToInt(r["MaHoaDon"])))
            {
                DataRow first = nhomHD.First();
                decimal tongTien = Math.Max(0, ToDecimal(first["TongTien"]));
                decimal daTT = Math.Min(Math.Max(0, ToDecimal(first["DaThanhToan"])), tongTien);

                decimal[] phaiDong = PhanBoTheoTyTrong(tongTien, CoCauHoaDon(first));
                decimal[] daDong = PhanBoTheoTyTrong(daTT, phaiDong);

                DateTime ngayLap = first["NgayLap"] == DBNull.Value ? new DateTime(ToInt(first["Nam"]), ToInt(first["Thang"]), 1) : Convert.ToDateTime(first["NgayLap"]);
                DateTime han = ngayLap.Date.AddDays(SO_NGAY_HAN_THANH_TOAN);
                bool quaHan = ngayThongKe.Date > han;

                List<DataRow> dsSV = nhomHD.Where(r => r["MaSV"] != DBNull.Value).ToList();
                if (dsSV.Count == 0)
                {
                    kq.SoHoaDonKhongXacDinhSV++;
                    kq.TienNoKhongXacDinhSV += tongTien - daTT;
                    continue;
                }

                int n = dsSV.Count;
                decimal[][] phaiDongSV = new decimal[4][];
                decimal[][] daDongSV = new decimal[4][];
                for (int k = 0; k < 4; k++)
                {
                    phaiDongSV[k] = ChiaDeu(phaiDong[k], n);
                    daDongSV[k] = ChiaDeu(daDong[k], n);
                }

                for (int i = 0; i < n; i++)
                {
                    DataRow svRow = dsSV[i];
                    string maSV = Convert.ToString(svRow["MaSV"]);
                    if (!thongTinSV.ContainsKey(maSV)) thongTinSV[maSV] = svRow;

                    CongNoChiTietDTO ct = new CongNoChiTietDTO
                    {
                        MaHoaDon = nhomHD.Key,
                        MaSV = maSV,
                        MaPhong = Convert.ToString(first["MaPhong"]),
                        TenPhong = Convert.ToString(first["TenPhong"]),
                        Thang = ToInt(first["Thang"]),
                        Nam = ToInt(first["Nam"]),
                        SoSinhVienChiaSe = n,
                        NgayLap = ngayLap,
                        HanThanhToan = han,
                        QuaHan = quaHan
                    };
                    for (int k = 0; k < 4; k++)
                    {
                        ct.PhaiDongTheoLoai[k] = phaiDongSV[k][i];
                        ct.DaDongTheoLoai[k] = daDongSV[k][i];
                    }

                    if (filter.LoaiPhi >= 0)
                    {
                        ct.TongPhaiDong = ct.PhaiDongTheoLoai[filter.LoaiPhi];
                        ct.DaThanhToan = ct.DaDongTheoLoai[filter.LoaiPhi];
                    }
                    else
                    {
                        ct.TongPhaiDong = ct.PhaiDongTheoLoai.Sum();
                        ct.DaThanhToan = ct.DaDongTheoLoai.Sum();
                    }

                    if (ct.ConNo <= 0) continue; // phần này đã trả đủ (hoặc loại phí này không phát sinh)

                    ct.LoaiPhiConNo = string.Join(", ", Enumerable.Range(0, 4)
                        .Where(k => (filter.LoaiPhi < 0 || filter.LoaiPhi == k) && ct.PhaiDongTheoLoai[k] - ct.DaDongTheoLoai[k] > 0)
                        .Select(k => LoaiPhi.Ten[k]));
                    ct.TrangThai = ct.DaThanhToan > 0 ? TTTT_MOT_PHAN : TTTT_CHUA;
                    tatCaChiTiet.Add(ct);
                }
            }

            // ---- Lọc ở cấp từng khoản nợ ----
            IEnumerable<CongNoChiTietDTO> q = tatCaChiTiet;
            if (!string.IsNullOrWhiteSpace(filter.TrangThaiThanhToan) && filter.TrangThaiThanhToan != "Tất cả")
                q = q.Where(c => c.TrangThai == filter.TrangThaiThanhToan);
            if (filter.TinhTrangHan == HAN_QUA_HAN) q = q.Where(c => c.QuaHan);
            else if (filter.TinhTrangHan == HAN_CHUA_DEN) q = q.Where(c => !c.QuaHan);

            // ---- Tổng hợp theo sinh viên ----
            List<CongNoSinhVienDTO> ds = new List<CongNoSinhVienDTO>();
            foreach (var nhomSV in q.GroupBy(c => c.MaSV))
            {
                DataRow sv = thongTinSV[nhomSV.Key];
                List<CongNoChiTietDTO> items = nhomSV.OrderBy(c => c.Nam).ThenBy(c => c.Thang).ToList();
                decimal daTra = items.Sum(c => c.DaThanhToan);

                CongNoSinhVienDTO item = new CongNoSinhVienDTO
                {
                    MaSV = nhomSV.Key,
                    HoTen = sv["HoTen"] == DBNull.Value ? "(Không rõ)" : Convert.ToString(sv["HoTen"]),
                    SDT = Convert.ToString(sv["SDT"]),
                    PhongDangO = string.IsNullOrWhiteSpace(Convert.ToString(sv["PhongDangO"])) ? "Đã kết thúc lưu trú" : Convert.ToString(sv["PhongDangO"]),
                    SoKhoanNo = items.Count,
                    TongPhaiDong = items.Sum(c => c.TongPhaiDong),
                    DaThanhToan = daTra,
                    ConNo = items.Sum(c => c.ConNo),
                    HanThanhToan = items.Min(c => c.HanThanhToan),
                    CoKhoanQuaHan = items.Any(c => c.QuaHan),
                    SoTienQuaHan = items.Where(c => c.QuaHan).Sum(c => c.ConNo),
                    ChiTiet = items,
                    LoaiPhiConNo = string.Join(", ", LoaiPhi.Ten.Where(t => items.Any(c => (c.LoaiPhiConNo ?? "").Contains(t))))
                };
                item.TrangThai = (daTra > 0 ? TTTT_MOT_PHAN : TTTT_CHUA) + (item.CoKhoanQuaHan ? " - " + HAN_QUA_HAN : "");
                ds.Add(item);
            }

            // ---- Tìm kiếm theo mã SV / họ tên ----
            if (!string.IsNullOrWhiteSpace(filter.TuKhoa))
            {
                string k = filter.TuKhoa.Trim();
                ds = ds.Where(s => Contains(s.MaSV, k) || Contains(s.HoTen, k)).ToList();
            }

            ds = ds.OrderByDescending(s => s.CoKhoanQuaHan).ThenByDescending(s => s.ConNo).ThenBy(s => s.MaSV).ToList();
            for (int i = 0; i < ds.Count; i++) ds[i].STT = i + 1;

            kq.DanhSach = ds;
            kq.TongSoSinhVienNo = ds.Count;
            kq.SoSinhVienQuaHan = ds.Count(s => s.CoKhoanQuaHan);
            kq.TongPhaiDong = ds.Sum(s => s.TongPhaiDong);
            kq.TongDaThanhToan = ds.Sum(s => s.DaThanhToan);
            kq.TongConNo = ds.Sum(s => s.ConNo);
            kq.TongNoQuaHan = ds.Sum(s => s.SoTienQuaHan);

            if (kq.SoHoaDonKhongXacDinhSV > 0)
            {
                kq.CanhBao.Add($"Có {kq.SoHoaDonKhongXacDinhSV} hóa đơn còn nợ {kq.TienNoKhongXacDinhSV:N0} đ nhưng phòng không có sinh viên lưu trú trong kỳ, " +
                               "không thể quy nợ cho sinh viên (xem chi tiết ở module Dịch vụ & Tài chính).");
            }

            return kq;
        }

        /// <summary>
        /// Xuất danh sách sinh viên nợ phí (theo đúng kết quả đã lọc) ra file .xlsx.
        /// </summary>
        public void XuatExcelCongNo(KetQuaCongNoDTO kq, string filePath, string moTaBoLoc)
        {
            KiemTraQuyen();
            if (kq == null) throw new ArgumentNullException(nameof(kq));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Vui lòng chọn nơi lưu file!");
            if (!filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase)) filePath += ".xlsx";

            DateTime now = DateTime.Now;
            XlsxWriter wb = new XlsxWriter();

            // ---------------- Sheet 1: Tổng hợp theo sinh viên ----------------
            string[] headers = { "STT", "Mã sinh viên", "Họ và tên", "Số điện thoại", "Phòng đang lưu trú", "Loại khoản phí còn nợ",
                                 "Tổng phải thanh toán", "Đã thanh toán", "Số tiền còn nợ", "Hạn thanh toán", "Trạng thái thanh toán" };
            double[] widths = { 6, 14, 26, 14, 20, 30, 20, 18, 18, 15, 30 };
            int soCot = headers.Length;

            XlsxSheet s1 = wb.AddSheet("Cong no sinh vien");
            for (int c = 0; c < soCot; c++) s1.SetColumnWidth(c + 1, widths[c]);

            s1.SetCell(1, 1, "DANH SÁCH SINH VIÊN NỢ ĐỌNG PHÍ", XlsxStyle.Title);
            s1.Merge(1, 1, 1, soCot);
            s1.SetCell(2, 1, "Ngày xuất báo cáo: " + now.ToString("dd/MM/yyyy HH:mm") +
                             (UserSession.IsLoggedIn ? "   -   Người xuất: " + UserSession.CurrentUser.HoTen : ""), XlsxStyle.Subtitle);
            s1.Merge(2, 1, 2, soCot);
            s1.SetCell(3, 1, "Bộ lọc: " + (string.IsNullOrWhiteSpace(moTaBoLoc) ? "Không áp dụng" : moTaBoLoc), XlsxStyle.Subtitle);
            s1.Merge(3, 1, 3, soCot);

            int row = 5;
            for (int c = 0; c < soCot; c++) s1.SetCell(row, c + 1, headers[c], XlsxStyle.Header);
            s1.FreezeRowsAbove(row + 1);

            foreach (CongNoSinhVienDTO sv in kq.DanhSach)
            {
                row++;
                s1.SetCell(row, 1, sv.STT, XlsxStyle.Integer);
                s1.SetCell(row, 2, sv.MaSV, XlsxStyle.TextCenter);
                s1.SetCell(row, 3, sv.HoTen, XlsxStyle.Text);
                s1.SetCell(row, 4, sv.SDT, XlsxStyle.TextCenter);
                s1.SetCell(row, 5, sv.PhongDangO, XlsxStyle.Text);
                s1.SetCell(row, 6, sv.LoaiPhiConNo, XlsxStyle.Text);
                s1.SetCell(row, 7, sv.TongPhaiDong, XlsxStyle.Money);
                s1.SetCell(row, 8, sv.DaThanhToan, XlsxStyle.Money);
                s1.SetCell(row, 9, sv.ConNo, XlsxStyle.Money);
                s1.SetCell(row, 10, sv.HanThanhToan, XlsxStyle.Date);
                s1.SetCell(row, 11, sv.TrangThai, XlsxStyle.Text);
            }

            row += 2;
            s1.SetCell(row, 1, "Tổng số sinh viên còn nợ", XlsxStyle.TotalLabel);
            for (int c = 2; c <= 8; c++) s1.SetCell(row, c, null, XlsxStyle.TotalLabel);
            s1.Merge(row, 1, row, 8);
            s1.SetCell(row, 9, kq.TongSoSinhVienNo, XlsxStyle.TotalInteger);
            row++;
            s1.SetCell(row, 1, "Số sinh viên có khoản quá hạn", XlsxStyle.TotalLabel);
            for (int c = 2; c <= 8; c++) s1.SetCell(row, c, null, XlsxStyle.TotalLabel);
            s1.Merge(row, 1, row, 8);
            s1.SetCell(row, 9, kq.SoSinhVienQuaHan, XlsxStyle.TotalInteger);
            row++;
            s1.SetCell(row, 1, "Tổng số tiền còn nợ", XlsxStyle.TotalLabel);
            for (int c = 2; c <= 8; c++) s1.SetCell(row, c, null, XlsxStyle.TotalLabel);
            s1.Merge(row, 1, row, 8);
            s1.SetCell(row, 9, kq.TongConNo, XlsxStyle.TotalMoney);
            row++;
            s1.SetCell(row, 1, "Trong đó nợ quá hạn", XlsxStyle.TotalLabel);
            for (int c = 2; c <= 8; c++) s1.SetCell(row, c, null, XlsxStyle.TotalLabel);
            s1.Merge(row, 1, row, 8);
            s1.SetCell(row, 9, kq.TongNoQuaHan, XlsxStyle.TotalMoney);

            row += 2;
            s1.SetCell(row, 1, $"Ghi chú: Hóa đơn được lập theo phòng/tháng; nghĩa vụ của mỗi sinh viên = phần chia đều hóa đơn cho số sinh viên lưu trú tại phòng trong tháng. " +
                               $"Hạn thanh toán = ngày lập hóa đơn + {SO_NGAY_HAN_THANH_TOAN} ngày.", XlsxStyle.Subtitle);
            s1.Merge(row, 1, row, soCot);
            foreach (string cb in kq.CanhBao)
            {
                row++;
                s1.SetCell(row, 1, "Lưu ý: " + cb, XlsxStyle.Subtitle);
                s1.Merge(row, 1, row, soCot);
            }

            // ---------------- Sheet 2: Chi tiết từng khoản nợ ----------------
            string[] h2 = { "STT", "Mã sinh viên", "Họ và tên", "Mã hóa đơn", "Phòng", "Kỳ hóa đơn", "Số SV chia sẻ",
                            "Loại phí còn nợ", "Phải thanh toán", "Đã thanh toán", "Còn nợ", "Hạn thanh toán", "Tình trạng" };
            double[] w2 = { 6, 14, 26, 12, 16, 12, 12, 30, 18, 18, 18, 15, 28 };
            XlsxSheet s2 = wb.AddSheet("Chi tiet khoan no");
            for (int c = 0; c < h2.Length; c++) s2.SetColumnWidth(c + 1, w2[c]);
            s2.SetCell(1, 1, "CHI TIẾT CÁC KHOẢN NỢ ĐỌNG PHÍ", XlsxStyle.Title);
            s2.Merge(1, 1, 1, h2.Length);
            s2.SetCell(2, 1, "Ngày xuất báo cáo: " + now.ToString("dd/MM/yyyy HH:mm"), XlsxStyle.Subtitle);
            s2.Merge(2, 1, 2, h2.Length);

            int r2 = 4;
            for (int c = 0; c < h2.Length; c++) s2.SetCell(r2, c + 1, h2[c], XlsxStyle.Header);
            s2.FreezeRowsAbove(r2 + 1);
            int stt = 0;
            foreach (CongNoSinhVienDTO sv in kq.DanhSach)
            {
                foreach (CongNoChiTietDTO ct in sv.ChiTiet)
                {
                    r2++; stt++;
                    s2.SetCell(r2, 1, stt, XlsxStyle.Integer);
                    s2.SetCell(r2, 2, sv.MaSV, XlsxStyle.TextCenter);
                    s2.SetCell(r2, 3, sv.HoTen, XlsxStyle.Text);
                    s2.SetCell(r2, 4, "#" + ct.MaHoaDon, XlsxStyle.TextCenter);
                    s2.SetCell(r2, 5, ct.TenPhong, XlsxStyle.Text);
                    s2.SetCell(r2, 6, ct.KyHoaDon, XlsxStyle.TextCenter);
                    s2.SetCell(r2, 7, ct.SoSinhVienChiaSe, XlsxStyle.Integer);
                    s2.SetCell(r2, 8, ct.LoaiPhiConNo, XlsxStyle.Text);
                    s2.SetCell(r2, 9, ct.TongPhaiDong, XlsxStyle.Money);
                    s2.SetCell(r2, 10, ct.DaThanhToan, XlsxStyle.Money);
                    s2.SetCell(r2, 11, ct.ConNo, XlsxStyle.Money);
                    s2.SetCell(r2, 12, ct.HanThanhToan, XlsxStyle.Date);
                    s2.SetCell(r2, 13, ct.TrangThai + (ct.QuaHan ? " - " + HAN_QUA_HAN : " - " + HAN_CHUA_DEN), XlsxStyle.Text);
                }
            }
            r2 += 2;
            s2.SetCell(r2, 1, "Tổng cộng", XlsxStyle.TotalLabel);
            for (int c = 2; c <= 8; c++) s2.SetCell(r2, c, null, XlsxStyle.TotalLabel);
            s2.Merge(r2, 1, r2, 8);
            s2.SetCell(r2, 9, kq.TongPhaiDong, XlsxStyle.TotalMoney);
            s2.SetCell(r2, 10, kq.TongDaThanhToan, XlsxStyle.TotalMoney);
            s2.SetCell(r2, 11, kq.TongConNo, XlsxStyle.TotalMoney);

            wb.Save(filePath);
        }

        // =====================================================================
        // 4. DASHBOARD
        // =====================================================================

        public DashboardThongKeDTO GetDashboard(int nam)
        {
            KiemTraQuyen();
            return new DashboardThongKeDTO
            {
                Phong = GetThongKePhong(),
                DoanhThu = GetBaoCaoDoanhThu(nam, 1, 12),
                CongNo = GetCongNo(new CongNoFilterDTO())
            };
        }

        // =====================================================================
        // HÀM HỖ TRỢ TÍNH TOÁN
        // =====================================================================

        /// <summary>
        /// Cơ cấu khoản thu của hóa đơn [Phòng, Điện, Nước, Khác].
        /// Nếu TongTien lớn hơn tổng các thành phần, phần chênh lệch được tính vào "Khác".
        /// Nếu không có thành phần nào, toàn bộ được tính vào "Khác".
        /// </summary>
        private static decimal[] CoCauHoaDon(DataRow r)
        {
            decimal phong = Math.Max(0, ToDecimal(r["TienPhong"]));
            decimal dien = Math.Max(0, ToDecimal(r["TienDien"]));
            decimal nuoc = Math.Max(0, ToDecimal(r["TienNuoc"]));
            decimal khac = Math.Max(0, ToDecimal(r["TienDichVuKhac"]));
            decimal tong = ToDecimal(r["TongTien"]);
            decimal chenh = tong - (phong + dien + nuoc + khac);
            if (chenh > 0) khac += chenh;
            if (phong + dien + nuoc + khac <= 0) khac = 1; // trọng số mặc định
            return new[] { phong, dien, nuoc, khac };
        }

        /// <summary>
        /// Phân bổ một số tiền (VNĐ, số nguyên) theo tỷ trọng bằng phương pháp phần dư lớn nhất.
        /// Đảm bảo: tổng các phần = số tiền gốc (không lệch 1 đồng) và mỗi phần không vượt trọng số
        /// khi số tiền &lt;= tổng trọng số.
        /// </summary>
        public static decimal[] PhanBoTheoTyTrong(decimal soTien, decimal[] trongSo)
        {
            int n = trongSo.Length;
            decimal[] kq = new decimal[n];
            if (n == 0) return kq;

            bool am = soTien < 0;
            decimal tong = Math.Round(Math.Abs(soTien), 0, MidpointRounding.AwayFromZero);
            decimal W = trongSo.Sum(w => Math.Max(0, w));
            if (tong == 0) return kq;
            if (W <= 0)
            {
                kq[n - 1] = am ? -tong : tong;
                return kq;
            }

            decimal[] du = new decimal[n];
            decimal daChia = 0;
            for (int i = 0; i < n; i++)
            {
                decimal raw = tong * Math.Max(0, trongSo[i]) / W;
                kq[i] = Math.Floor(raw);
                du[i] = raw - kq[i];
                daChia += kq[i];
            }

            int conLai = (int)(tong - daChia);
            foreach (int i in Enumerable.Range(0, n).OrderByDescending(i => du[i]).ThenBy(i => i).Take(conLai))
            {
                kq[i] += 1;
            }

            if (am) for (int i = 0; i < n; i++) kq[i] = -kq[i];
            return kq;
        }

        /// <summary>Chia đều số tiền cho n sinh viên (tổng các phần = số tiền gốc).</summary>
        public static decimal[] ChiaDeu(decimal soTien, int n)
        {
            if (n <= 0) return new decimal[0];
            return PhanBoTheoTyTrong(soTien, Enumerable.Repeat(1m, n).ToArray());
        }

        private static void TinhTangTruong(List<DoanhThuKyDTO> ds)
        {
            for (int i = 0; i < ds.Count; i++)
            {
                if (i == 0 || ds[i - 1].TongThu == 0) { ds[i].TangTruong = null; continue; }
                ds[i].TangTruong = Math.Round((double)((ds[i].TongThu - ds[i - 1].TongThu) / ds[i - 1].TongThu * 100), 1);
            }
        }

        private static string SoLaMa(int q)
        {
            switch (q)
            {
                case 1: return "I";
                case 2: return "II";
                case 3: return "III";
                default: return "IV";
            }
        }

        private static bool Contains(string source, string keyword)
        {
            return !string.IsNullOrEmpty(source) &&
                   source.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private static int ToInt(object v)
        {
            return v == null || v == DBNull.Value ? 0 : Convert.ToInt32(v);
        }

        private static decimal ToDecimal(object v)
        {
            return v == null || v == DBNull.Value ? 0 : Convert.ToDecimal(v);
        }
    }
}

