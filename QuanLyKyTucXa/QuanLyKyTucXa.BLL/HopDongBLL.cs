using System;
using System.Data;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    public class HopDongBLL
    {
        private readonly HopDongDAL hopDongDAL = new HopDongDAL();

        public DataTable GetAllHopDong()
        {
            return hopDongDAL.GetAllHopDong();
        }

        public DataTable FilterHopDong(string keyword, string trangThai)
        {
            return hopDongDAL.FilterHopDong(keyword ?? string.Empty, trangThai ?? "Tất cả trạng thái");
        }

        /// <summary>
        /// Lập hợp đồng mới kèm các chốt chặn kiểm tra nghiệp vụ chặt chẽ.
        /// </summary>
        public bool LapHopDongMoi(HopDongDTO hd)
        {
            if (hd == null)
                throw new Exception("Thông tin hợp đồng không hợp lệ!");

            if (string.IsNullOrWhiteSpace(hd.MaSV))
                throw new Exception("Vui lòng chọn sinh viên cần lập hợp đồng!");

            if (string.IsNullOrWhiteSpace(hd.MaPhong))
                throw new Exception("Vui lòng chọn phòng cần xếp cho sinh viên!");

            hd.MaSV = hd.MaSV.Trim();
            hd.MaPhong = hd.MaPhong.Trim();

            // 1. Kiểm tra thời hạn hợp đồng tối thiểu 30 ngày
            if ((hd.NgayKetThuc.Date - hd.NgayBatDau.Date).TotalDays < 30)
                throw new Exception("Ngày kết thúc hợp đồng phải lớn hơn ngày bắt đầu ít nhất 30 ngày!");

            // 2. Sinh viên không được có hợp đồng nào đang ở trạng thái 'Đang hiệu lực'
            if (hopDongDAL.KiemTraSinhVienCoHopDongHieuLuc(hd.MaSV))
                throw new Exception($"Sinh viên '{hd.MaSV}' hiện đã có hợp đồng đang hiệu lực! Không thể lập thêm hợp đồng mới.");

            // 3 & 4. Kiểm tra phòng còn trống, không sửa chữa và đối chiếu giới tính sinh viên với loại phòng
            ValidatePhongVaGioiTinh(hd.MaSV, hd.MaPhong);

            hd.TrangThai = "Đang hiệu lực";
            return hopDongDAL.LapHopDongMoi(hd);
        }

        /// <summary>
        /// Thanh lý hợp đồng đang hiệu lực và trả lại 1 giường trống cho phòng.
        /// </summary>
        public bool ThanhLyHopDong(string maHopDong, string maPhong)
        {
            if (string.IsNullOrWhiteSpace(maHopDong))
                throw new Exception("Vui lòng chọn hợp đồng cần thanh lý!");

            if (string.IsNullOrWhiteSpace(maPhong))
                throw new Exception("Không xác định được mã phòng của hợp đồng cần thanh lý!");

            DataRow rowHD = hopDongDAL.GetThongTinHopDong(maHopDong.Trim());
            if (rowHD == null)
                throw new Exception("Không tìm thấy hợp đồng trong hệ thống!");

            string trangThaiHD = rowHD["TrangThai"]?.ToString()?.Trim() ?? string.Empty;
            if (!string.Equals(trangThaiHD, "Đang hiệu lực", StringComparison.OrdinalIgnoreCase))
                throw new Exception($"Chỉ có thể thanh lý hợp đồng đang hiệu lực (trạng thái hiện tại: '{trangThaiHD}').");

            return hopDongDAL.ThanhLyHopDong(maHopDong.Trim(), maPhong.Trim());
        }

        /// <summary>
        /// Chuyển phòng cho sinh viên: Thanh lý hợp đồng cũ (trạng thái 'Đã chuyển phòng') + Lập hợp đồng mới.
        /// </summary>
        public bool ChuyenPhong(string maHopDongCu, string maPhongCu, HopDongDTO hdMoi)
        {
            if (string.IsNullOrWhiteSpace(maHopDongCu))
                throw new Exception("Vui lòng chọn hợp đồng hiện tại cần chuyển phòng!");

            if (string.IsNullOrWhiteSpace(maPhongCu))
                throw new Exception("Không xác định được phòng hiện tại của sinh viên!");

            if (hdMoi == null || string.IsNullOrWhiteSpace(hdMoi.MaPhong))
                throw new Exception("Vui lòng chọn phòng mới muốn chuyển đến!");

            maHopDongCu = maHopDongCu.Trim();
            maPhongCu = maPhongCu.Trim();
            hdMoi.MaPhong = hdMoi.MaPhong.Trim();

            // 1. Mã phòng mới phải khác mã phòng cũ
            if (string.Equals(hdMoi.MaPhong, maPhongCu, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Phòng mới phải khác phòng hiện tại!");

            // Kiểm tra hợp đồng cũ tồn tại và đang hiệu lực
            DataRow rowHDCu = hopDongDAL.GetThongTinHopDong(maHopDongCu);
            if (rowHDCu == null)
                throw new Exception("Không tìm thấy hợp đồng cũ trong hệ thống!");

            string trangThaiCu = rowHDCu["TrangThai"]?.ToString()?.Trim() ?? string.Empty;
            if (!string.Equals(trangThaiCu, "Đang hiệu lực", StringComparison.OrdinalIgnoreCase))
                throw new Exception($"Chỉ có thể chuyển phòng cho hợp đồng đang hiệu lực (trạng thái hiện tại: '{trangThaiCu}').");

            // Tự đồng bộ MaSV từ hợp đồng cũ nếu chưa truyền
            string maSVCu = rowHDCu["MaSV"]?.ToString()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(hdMoi.MaSV))
            {
                hdMoi.MaSV = maSVCu;
            }
            else
            {
                hdMoi.MaSV = hdMoi.MaSV.Trim();
            }

            if (hdMoi.NgayKetThuc.Date <= hdMoi.NgayBatDau.Date)
                throw new Exception("Ngày kết thúc hợp đồng mới phải lớn hơn ngày bắt đầu!");

            // 2. Phòng mới phải thỏa mãn đủ điều kiện còn giường trống, không sửa chữa và đúng giới tính
            ValidatePhongVaGioiTinh(hdMoi.MaSV, hdMoi.MaPhong);

            hdMoi.TrangThai = "Đang hiệu lực";
            return hopDongDAL.ChuyenPhong(maHopDongCu, maPhongCu, hdMoi);
        }

        /// <summary>
        /// Quét và cập nhật các hợp đồng quá hạn sang 'Hết hạn', đồng thời hoàn trả giường trống cho phòng.
        /// </summary>
        public int CapNhatHopDongHetHan()
        {
            return hopDongDAL.CapNhatHopDongHetHan();
        }

        private void ValidatePhongVaGioiTinh(string maSV, string maPhong)
        {
            string gioiTinhSV = hopDongDAL.GetGioiTinhSinhVien(maSV);
            if (gioiTinhSV == null)
                throw new Exception($"Không tìm thấy sinh viên có mã '{maSV}' trong hệ thống!");

            DataRow rowPhong = hopDongDAL.GetThongTinPhong(maPhong);
            if (rowPhong == null)
                throw new Exception($"Không tìm thấy phòng có mã '{maPhong}' trong hệ thống!");

            string trangThaiPhong = rowPhong["TrangThaiPhong"] == DBNull.Value
                ? string.Empty
                : rowPhong["TrangThaiPhong"].ToString().Trim();

            if (string.Equals(trangThaiPhong, "Đang sửa chữa", StringComparison.OrdinalIgnoreCase)
                || string.Equals(trangThaiPhong, "Bảo trì", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Phòng '{maPhong}' hiện đang trong trạng thái '{trangThaiPhong}', không thể xếp sinh viên vào!");
            }

            int soGiuongTrong = Convert.ToInt32(rowPhong["SoGiuongTrong"]);
            if (soGiuongTrong <= 0)
                throw new Exception($"Phòng '{maPhong}' đã hết giường trống (còn 0 giường), vui lòng chọn phòng khác!");

            string loaiPhong = rowPhong["LoaiPhong"] == DBNull.Value
                ? string.Empty
                : rowPhong["LoaiPhong"].ToString().Trim();

            bool quyDinhNam = string.Equals(loaiPhong, "Nam", StringComparison.OrdinalIgnoreCase);
            bool quyDinhNu = string.Equals(loaiPhong, "Nữ", StringComparison.OrdinalIgnoreCase);

            if ((quyDinhNam || quyDinhNu) && !string.Equals(gioiTinhSV, loaiPhong, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Sinh viên '{maSV}' có giới tính '{gioiTinhSV}', không phù hợp với phòng '{maPhong}' (chỉ dành cho {loaiPhong})!");
            }
        }
    }
}

