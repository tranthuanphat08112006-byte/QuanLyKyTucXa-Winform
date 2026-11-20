using System;
using System.Collections.Generic;

namespace QuanLyKyTucXa.DTO
{
    // =====================================================================
    // 1. THỐNG KÊ TÌNH TRẠNG PHÒNG
    // =====================================================================

    /// <summary>
    /// Chi tiết tình trạng sử dụng của một phòng.
    /// SoNguoiDangO được tính từ hợp đồng lưu trú hợp lệ (module Quản lý lưu trú),
    /// không lấy từ bộ đếm SoGiuongTrong của bảng Phong.
    /// </summary>
    public class ThongKePhongChiTietDTO
    {
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }
        public string TenKhu { get; set; }
        public string LoaiPhong { get; set; }
        public int SucChua { get; set; }
        public int SoNguoiDangO { get; set; }
        public int SoGiuongTrong { get; set; }
        /// <summary>Trống / Còn chỗ / Đã đầy / Bất thường</summary>
        public string TrangThai { get; set; }
        /// <summary>Tình trạng vận hành ghi trong module Phòng (VD: Bảo trì, Đang sửa chữa)</summary>
        public string TinhTrangVanHanh { get; set; }
        /// <summary>Số người đang ở theo bộ đếm của module Phòng (SoGiuongToiDa - SoGiuongTrong)</summary>
        public int SoNguoiTheoModulePhong { get; set; }
        public double TyLeLapDay { get; set; }
        public string GhiChuBatThuong { get; set; }
        public bool LaBatThuong => !string.IsNullOrEmpty(GhiChuBatThuong);
    }

    public class ThongKePhongTongQuanDTO
    {
        public int TongSoPhong { get; set; }
        public int TongSoGiuong { get; set; }
        public int SoGiuongDangSuDung { get; set; }
        public int SoGiuongTrong { get; set; }
        public int SoPhongTrong { get; set; }
        public int SoPhongConCho { get; set; }
        public int SoPhongDaDay { get; set; }
        public int SoPhongBatThuong { get; set; }
        public int SoSinhVienDangLuuTru { get; set; }
        public double TyLeLapDay { get; set; }
        public List<ThongKePhongChiTietDTO> ChiTiet { get; set; } = new List<ThongKePhongChiTietDTO>();
        /// <summary>Các cảnh báo dữ liệu bất thường (không âm thầm bỏ qua)</summary>
        public List<string> CanhBao { get; set; } = new List<string>();
    }

    // =====================================================================
    // 2. BÁO CÁO DOANH THU
    // =====================================================================

    /// <summary>
    /// Doanh thu của một kỳ (tháng hoặc quý).
    /// Doanh thu thực nhận = tổng các phiếu thu trong LichSuThanhToan có NgayThanhToan thuộc kỳ.
    /// </summary>
    public class DoanhThuKyDTO
    {
        public int Nam { get; set; }
        /// <summary>Số tháng (1..12) hoặc số quý (1..4)</summary>
        public int Ky { get; set; }
        public string TenKy { get; set; }
        public int SoGiaoDich { get; set; }
        public decimal ThuTienPhong { get; set; }
        public decimal ThuTienDien { get; set; }
        public decimal ThuTienNuoc { get; set; }
        public decimal ThuDichVuKhac { get; set; }
        public decimal TongThu { get; set; }
        /// <summary>Tổng giá trị hóa đơn phát sinh cho kỳ (theo Tháng/Năm của hóa đơn)</summary>
        public decimal PhatSinh { get; set; }
        /// <summary>Số tiền còn phải thu của các hóa đơn thuộc kỳ</summary>
        public decimal ConPhaiThu { get; set; }
        /// <summary>% tăng/giảm so với kỳ liền trước (null nếu kỳ trước = 0)</summary>
        public double? TangTruong { get; set; }
    }

    public class BaoCaoDoanhThuDTO
    {
        public int Nam { get; set; }
        public int TuThang { get; set; }
        public int DenThang { get; set; }
        public decimal TongDoanhThu { get; set; }
        public decimal DoanhThuTienPhong { get; set; }
        /// <summary>Dịch vụ = Điện + Nước</summary>
        public decimal DoanhThuDichVu { get; set; }
        public decimal DoanhThuDien { get; set; }
        public decimal DoanhThuNuoc { get; set; }
        /// <summary>Khoản thu khác = TienDichVuKhac (vệ sinh, rác, phụ phí...)</summary>
        public decimal DoanhThuKhac { get; set; }
        public int SoGiaoDichThanhCong { get; set; }
        public decimal TongPhatSinh { get; set; }
        public decimal TongConPhaiThu { get; set; }
        public decimal DoanhThuTrungBinhThang { get; set; }
        public DoanhThuKyDTO ThangCaoNhat { get; set; }
        public DoanhThuKyDTO ThangThapNhat { get; set; }
        public List<DoanhThuKyDTO> TheoThang { get; set; } = new List<DoanhThuKyDTO>();
        public List<DoanhThuKyDTO> TheoQuy { get; set; } = new List<DoanhThuKyDTO>();
        public List<string> CanhBao { get; set; } = new List<string>();
    }

    // =====================================================================
    // 3. CÔNG NỢ SINH VIÊN
    // =====================================================================

    public static class LoaiPhi
    {
        public const int Phong = 0;
        public const int Dien = 1;
        public const int Nuoc = 2;
        public const int Khac = 3;
        public static readonly string[] Ten = { "Tiền phòng", "Tiền điện", "Tiền nước", "Dịch vụ khác" };
    }

    /// <summary>
    /// Phần nghĩa vụ của 1 sinh viên trong 1 hóa đơn phòng còn nợ.
    /// </summary>
    public class CongNoChiTietDTO
    {
        public int MaHoaDon { get; set; }
        public string MaSV { get; set; }
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public string KyHoaDon => $"{Thang:D2}/{Nam}";
        public int SoSinhVienChiaSe { get; set; }
        public DateTime NgayLap { get; set; }
        public DateTime HanThanhToan { get; set; }
        /// <summary>Số tiền phải đóng theo loại phí (index theo LoaiPhi)</summary>
        public decimal[] PhaiDongTheoLoai { get; set; } = new decimal[4];
        /// <summary>Số tiền đã đóng theo loại phí (index theo LoaiPhi)</summary>
        public decimal[] DaDongTheoLoai { get; set; } = new decimal[4];

        public decimal TongPhaiDong { get; set; }
        public decimal DaThanhToan { get; set; }
        public decimal ConNo => Math.Max(0, TongPhaiDong - DaThanhToan);
        public string LoaiPhiConNo { get; set; }
        public bool QuaHan { get; set; }
        public string TrangThai { get; set; }
    }

    /// <summary>
    /// Công nợ tổng hợp theo sinh viên.
    /// </summary>
    public class CongNoSinhVienDTO
    {
        public int STT { get; set; }
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public string SDT { get; set; }
        public string PhongDangO { get; set; }
        public string LoaiPhiConNo { get; set; }
        public int SoKhoanNo { get; set; }
        public decimal TongPhaiDong { get; set; }
        public decimal DaThanhToan { get; set; }
        public decimal ConNo { get; set; }
        /// <summary>Hạn thanh toán sớm nhất trong các khoản còn nợ</summary>
        public DateTime HanThanhToan { get; set; }
        public bool CoKhoanQuaHan { get; set; }
        public decimal SoTienQuaHan { get; set; }
        public string TrangThai { get; set; }
        public List<CongNoChiTietDTO> ChiTiet { get; set; } = new List<CongNoChiTietDTO>();
    }

    /// <summary>
    /// Bộ lọc danh sách công nợ.
    /// </summary>
    public class CongNoFilterDTO
    {
        public string TuKhoa { get; set; }
        public string MaPhong { get; set; }
        /// <summary>-1 = tất cả, còn lại theo LoaiPhi</summary>
        public int LoaiPhi { get; set; } = -1;
        /// <summary>Tất cả / Chưa thanh toán / Thanh toán một phần</summary>
        public string TrangThaiThanhToan { get; set; }
        /// <summary>Tất cả / Quá hạn / Chưa đến hạn</summary>
        public string TinhTrangHan { get; set; }
        /// <summary>Kỳ hóa đơn từ (chỉ dùng Tháng/Năm)</summary>
        public DateTime? TuKy { get; set; }
        /// <summary>Kỳ hóa đơn đến (chỉ dùng Tháng/Năm)</summary>
        public DateTime? DenKy { get; set; }
    }

    public class KetQuaCongNoDTO
    {
        public List<CongNoSinhVienDTO> DanhSach { get; set; } = new List<CongNoSinhVienDTO>();
        public int TongSoSinhVienNo { get; set; }
        public int SoSinhVienQuaHan { get; set; }
        public decimal TongPhaiDong { get; set; }
        public decimal TongDaThanhToan { get; set; }
        public decimal TongConNo { get; set; }
        public decimal TongNoQuaHan { get; set; }
        /// <summary>Hóa đơn còn nợ nhưng không xác định được sinh viên lưu trú trong kỳ</summary>
        public int SoHoaDonKhongXacDinhSV { get; set; }
        public decimal TienNoKhongXacDinhSV { get; set; }
        public List<string> CanhBao { get; set; } = new List<string>();
    }

    // =====================================================================
    // 4. DASHBOARD
    // =====================================================================

    public class DashboardThongKeDTO
    {
        public ThongKePhongTongQuanDTO Phong { get; set; }
        public BaoCaoDoanhThuDTO DoanhThu { get; set; }
        public KetQuaCongNoDTO CongNo { get; set; }
    }
}

