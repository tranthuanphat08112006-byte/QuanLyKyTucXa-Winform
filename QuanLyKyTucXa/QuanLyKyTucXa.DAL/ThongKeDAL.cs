using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKyTucXa.DAL
{
    /// <summary>
    /// Truy vấn dữ liệu thô phục vụ module Quản lý thống kê.
    /// Chỉ ĐỌC dữ liệu từ các bảng hiện có của 4 module:
    ///   - Phong, DayNha            (Quản lý phòng)
    ///   - SinhVien                 (Quản lý sinh viên)
    ///   - HopDong                  (Quản lý lưu trú)
    ///   - HoaDon, LichSuThanhToan  (Quản lý dịch vụ &amp; tài chính)
    /// Không tạo bảng, không sửa cấu trúc, không ghi dữ liệu.
    /// Mọi phép tổng hợp nghiệp vụ được thực hiện ở ThongKeBLL.
    /// </summary>
    public class ThongKeDAL
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        public ThongKeDAL()
        {
            // Đảm bảo các cột/bảng của module Phòng và Tài chính đã được khởi tạo
            // (dùng lại cơ chế EnsureSchema sẵn có của 2 module, không tạo cấu trúc mới).
            try { new PhongDAL(); } catch { }
            try { new HoaDonDAL(); } catch { }
        }

        // =====================================================================
        // 1. PHÒNG & LƯU TRÚ
        // =====================================================================

        /// <summary>
        /// Danh sách phòng kèm số sinh viên đang lưu trú hợp lệ tại ngày thống kê.
        /// Lưu trú hợp lệ: HopDong.TrangThai = 'Đang hiệu lực' và NgayKetThuc &gt;= ngày thống kê.
        /// Đếm DISTINCT MaSV để không đếm trùng nếu 1 sinh viên có nhiều hợp đồng trong cùng phòng.
        /// </summary>
        public DataTable GetPhongVaSoNguoiDangO(DateTime ngayThongKe)
        {
            const string query = @"
SELECT
    p.MaPhong,
    ISNULL(p.TenPhong, N'Phòng ' + p.MaPhong) AS TenPhong,
    ISNULL(p.TenKhu, N'') AS TenKhu,
    ISNULL(p.LoaiPhong, N'') AS LoaiPhong,
    p.SoGiuongToiDa,
    p.SoGiuongTrong,
    ISNULL(p.TrangThaiPhong, N'') AS TrangThaiPhong,
    ISNULL(lt.SoNguoiDangO, 0) AS SoNguoiDangO
FROM Phong p
LEFT JOIN (
    SELECT MaPhong, COUNT(DISTINCT MaSV) AS SoNguoiDangO
    FROM HopDong
    WHERE TrangThai = N'Đang hiệu lực'
      AND NgayKetThuc >= @Ngay
    GROUP BY MaPhong
) lt ON lt.MaPhong = p.MaPhong
ORDER BY p.TenKhu, p.MaPhong";

            return dbHelper.ExecuteQuery(query, new[]
            {
                new SqlParameter("@Ngay", SqlDbType.Date) { Value = ngayThongKe.Date }
            });
        }

        /// <summary>
        /// Số sinh viên (DISTINCT) đang lưu trú hợp lệ tại ngày thống kê.
        /// </summary>
        public int CountSinhVienDangLuuTru(DateTime ngayThongKe)
        {
            const string query = @"
SELECT COUNT(DISTINCT MaSV) FROM HopDong
WHERE TrangThai = N'Đang hiệu lực' AND NgayKetThuc >= @Ngay";
            object result = dbHelper.ExecuteScalar(query, new[]
            {
                new SqlParameter("@Ngay", SqlDbType.Date) { Value = ngayThongKe.Date }
            });
            return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
        }

        /// <summary>
        /// Sinh viên có nhiều hơn 1 hợp đồng 'Đang hiệu lực' cùng lúc (dữ liệu lưu trú trùng lặp).
        /// </summary>
        public DataTable GetSinhVienTrungHopDongHieuLuc(DateTime ngayThongKe)
        {
            const string query = @"
SELECT MaSV, COUNT(*) AS SoHopDong,
       STUFF((SELECT CONCAT(N', ', h2.MaPhong) FROM HopDong h2
              WHERE h2.MaSV = h.MaSV AND h2.TrangThai = N'Đang hiệu lực' AND h2.NgayKetThuc >= @Ngay
              FOR XML PATH('')), 1, 2, N'') AS DanhSachPhong
FROM HopDong h
WHERE TrangThai = N'Đang hiệu lực' AND NgayKetThuc >= @Ngay
GROUP BY MaSV
HAVING COUNT(*) > 1";
            return dbHelper.ExecuteQuery(query, new[]
            {
                new SqlParameter("@Ngay", SqlDbType.Date) { Value = ngayThongKe.Date }
            });
        }

        /// <summary>
        /// Số hợp đồng đã quá NgayKetThuc nhưng vẫn mang trạng thái 'Đang hiệu lực'
        /// (chưa chạy chức năng cập nhật hợp đồng hết hạn của module Lưu trú).
        /// </summary>
        public int CountHopDongQuaHanChuaCapNhat(DateTime ngayThongKe)
        {
            const string query = @"
SELECT COUNT(*) FROM HopDong
WHERE TrangThai = N'Đang hiệu lực' AND NgayKetThuc < @Ngay";
            object result = dbHelper.ExecuteScalar(query, new[]
            {
                new SqlParameter("@Ngay", SqlDbType.Date) { Value = ngayThongKe.Date }
            });
            return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
        }

        // =====================================================================
        // 2. DOANH THU
        // =====================================================================

        /// <summary>
        /// Các phiếu thu (LichSuThanhToan) có NgayThanhToan trong [tuNgay, denNgay)
        /// kèm cơ cấu khoản thu của hóa đơn gốc để phân loại nguồn thu.
        /// Mỗi dòng là một giao dịch thu tiền thành công (module Tài chính chỉ ghi
        /// LichSuThanhToan khi giao dịch đã commit thành công).
        /// </summary>
        public DataTable GetPhieuThuTrongKhoang(DateTime tuNgay, DateTime denNgay)
        {
            const string query = @"
SELECT
    ls.MaThanhToan,
    ls.MaHoaDon,
    ls.NgayThanhToan,
    ls.SoTienThanhToan,
    ISNULL(hd.TienPhong, 0) AS TienPhong,
    ISNULL(hd.TienDien, 0) AS TienDien,
    ISNULL(hd.TienNuoc, 0) AS TienNuoc,
    ISNULL(hd.TienDichVuKhac, 0) AS TienDichVuKhac,
    ISNULL(hd.TongTien, 0) AS TongTien
FROM LichSuThanhToan ls
INNER JOIN HoaDon hd ON hd.MaHoaDon = ls.MaHoaDon
WHERE ls.NgayThanhToan >= @TuNgay AND ls.NgayThanhToan < @DenNgay";

            return dbHelper.ExecuteQuery(query, new[]
            {
                new SqlParameter("@TuNgay", SqlDbType.DateTime) { Value = tuNgay },
                new SqlParameter("@DenNgay", SqlDbType.DateTime) { Value = denNgay }
            });
        }

        /// <summary>
        /// Tổng phát sinh và còn phải thu của hóa đơn theo kỳ (Thang) trong một năm.
        /// </summary>
        public DataTable GetPhatSinhVaConNoTheoThang(int nam)
        {
            const string query = @"
SELECT
    Thang,
    COUNT(*) AS SoHoaDon,
    ISNULL(SUM(ISNULL(TongTien, 0)), 0) AS PhatSinh,
    ISNULL(SUM(CASE WHEN ISNULL(TongTien, 0) - ISNULL(DaThanhToan, 0) > 0
                    THEN ISNULL(TongTien, 0) - ISNULL(DaThanhToan, 0) ELSE 0 END), 0) AS ConPhaiThu
FROM HoaDon
WHERE Nam = @Nam
GROUP BY Thang";
            return dbHelper.ExecuteQuery(query, new[] { new SqlParameter("@Nam", nam) });
        }

        /// <summary>
        /// Hóa đơn có DaThanhToan không khớp với tổng phiếu thu trong LichSuThanhToan.
        /// Dùng để cảnh báo sai lệch dữ liệu tài chính.
        /// </summary>
        public DataTable GetHoaDonLechThanhToan()
        {
            const string query = @"
SELECT hd.MaHoaDon, hd.MaPhong, hd.Thang, hd.Nam,
       ISNULL(hd.DaThanhToan, 0) AS DaThanhToan,
       ISNULL(ls.TongPhieuThu, 0) AS TongPhieuThu
FROM HoaDon hd
LEFT JOIN (SELECT MaHoaDon, SUM(SoTienThanhToan) AS TongPhieuThu
           FROM LichSuThanhToan GROUP BY MaHoaDon) ls ON ls.MaHoaDon = hd.MaHoaDon
WHERE ISNULL(hd.DaThanhToan, 0) <> ISNULL(ls.TongPhieuThu, 0)";
            return dbHelper.ExecuteQuery(query);
        }

        /// <summary>
        /// Danh sách các năm có phát sinh hóa đơn hoặc phiếu thu.
        /// </summary>
        public List<int> GetDanhSachNamCoDuLieu()
        {
            const string query = @"
SELECT Nam FROM HoaDon
UNION
SELECT YEAR(NgayThanhToan) FROM LichSuThanhToan
ORDER BY Nam DESC";
            DataTable dt = dbHelper.ExecuteQuery(query);
            List<int> result = new List<int>();
            foreach (DataRow row in dt.Rows)
            {
                if (row[0] != DBNull.Value) result.Add(Convert.ToInt32(row[0]));
            }
            return result;
        }

        // =====================================================================
        // 3. CÔNG NỢ
        // =====================================================================

        /// <summary>
        /// Các hóa đơn còn nợ (TongTien - DaThanhToan &gt; 0) kèm danh sách sinh viên
        /// có hợp đồng lưu trú tại phòng trong kỳ hóa đơn (giao nhau với tháng của hóa đơn).
        /// Trả về 1 dòng cho mỗi cặp (hóa đơn, sinh viên). Hóa đơn không có sinh viên nào
        /// trong kỳ vẫn được trả về với MaSV = NULL để cảnh báo.
        /// Một truy vấn duy nhất (không N+1).
        /// </summary>
        public DataTable GetHoaDonConNoVaSinhVien(int? tuKy, int? denKy, string maPhong)
        {
            List<string> conditions = new List<string>
            {
                "ISNULL(hd.TongTien, 0) - ISNULL(hd.DaThanhToan, 0) > 0"
            };
            List<SqlParameter> parameters = new List<SqlParameter>();

            // Kỳ được mã hóa Nam*100 + Thang để so sánh khoảng
            if (tuKy.HasValue)
            {
                conditions.Add("(hd.Nam * 100 + hd.Thang) >= @TuKy");
                parameters.Add(new SqlParameter("@TuKy", tuKy.Value));
            }
            if (denKy.HasValue)
            {
                conditions.Add("(hd.Nam * 100 + hd.Thang) <= @DenKy");
                parameters.Add(new SqlParameter("@DenKy", denKy.Value));
            }
            if (!string.IsNullOrWhiteSpace(maPhong))
            {
                conditions.Add("hd.MaPhong = @MaPhong");
                parameters.Add(new SqlParameter("@MaPhong", maPhong.Trim()));
            }

            string query = @"
WITH HDNo AS (
    SELECT
        hd.MaHoaDon, hd.MaPhong, hd.Thang, hd.Nam,
        ISNULL(hd.TienPhong, 0) AS TienPhong,
        ISNULL(hd.TienDien, 0) AS TienDien,
        ISNULL(hd.TienNuoc, 0) AS TienNuoc,
        ISNULL(hd.TienDichVuKhac, 0) AS TienDichVuKhac,
        ISNULL(hd.TongTien, 0) AS TongTien,
        ISNULL(hd.DaThanhToan, 0) AS DaThanhToan,
        hd.NgayLap,
        DATEFROMPARTS(hd.Nam, hd.Thang, 1) AS DauKy,
        EOMONTH(DATEFROMPARTS(hd.Nam, hd.Thang, 1)) AS CuoiKy
    FROM HoaDon hd
    WHERE " + string.Join(" AND ", conditions) + @"
      AND hd.Thang BETWEEN 1 AND 12
),
SVTrongKy AS (
    SELECT DISTINCT h.MaHoaDon, hg.MaSV
    FROM HDNo h
    INNER JOIN HopDong hg
        ON hg.MaPhong = h.MaPhong
       AND hg.NgayBatDau <= h.CuoiKy
       AND hg.NgayKetThuc >= h.DauKy
)
SELECT
    h.MaHoaDon, h.MaPhong,
    ISNULL(p.TenPhong, N'Phòng ' + h.MaPhong) AS TenPhong,
    h.Thang, h.Nam, h.TienPhong, h.TienDien, h.TienNuoc, h.TienDichVuKhac,
    h.TongTien, h.DaThanhToan, h.NgayLap,
    s.MaSV,
    sv.HoTen,
    ISNULL(sv.SDT, '') AS SDT,
    ISNULL(cur.PhongDangO, N'') AS PhongDangO
FROM HDNo h
LEFT JOIN Phong p ON p.MaPhong = h.MaPhong
LEFT JOIN SVTrongKy s ON s.MaHoaDon = h.MaHoaDon
LEFT JOIN SinhVien sv ON sv.MaSV = s.MaSV
OUTER APPLY (
    SELECT TOP 1 ISNULL(p2.TenPhong, CONCAT(N'Phòng ', hc.MaPhong)) AS PhongDangO
    FROM HopDong hc
    LEFT JOIN Phong p2 ON p2.MaPhong = hc.MaPhong
    WHERE hc.MaSV = s.MaSV AND hc.TrangThai = N'Đang hiệu lực'
    ORDER BY hc.NgayBatDau DESC
) cur
ORDER BY h.Nam, h.Thang, h.MaHoaDon, s.MaSV";

            return dbHelper.ExecuteQuery(query, parameters.ToArray());
        }

        /// <summary>
        /// Danh sách phòng có hóa đơn còn nợ (phục vụ combobox lọc theo phòng).
        /// </summary>
        public DataTable GetPhongCoHoaDon()
        {
            const string query = @"
SELECT DISTINCT hd.MaPhong FROM HoaDon hd ORDER BY hd.MaPhong";
            return dbHelper.ExecuteQuery(query);
        }
    }
}

