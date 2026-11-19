using System;
using System.Data;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    public class ThanhToanBLL
    {
        private readonly ThanhToanDAL thanhToanDAL = new ThanhToanDAL();
        private readonly HoaDonDAL hoaDonDAL = new HoaDonDAL();

        /// <summary>
        /// Thực hiện thanh toán hóa đơn kèm kiểm tra nghiệp vụ chặt chẽ:
        /// - Không thanh toán số tiền <= 0
        /// - Không thanh toán hóa đơn đã thanh toán đầy đủ
        /// - Không thanh toán vượt quá số tiền còn nợ
        /// - Tự động cập nhật số tiền đã trả, còn nợ và trạng thái hóa đơn
        /// </summary>
        public bool ThanhToanHoaDon(int maHoaDon, decimal soTienThanhToan, string phuongThuc, string nguoiNop, string ghiChu)
        {
            if (maHoaDon <= 0)
                throw new Exception("Vui lòng chọn hóa đơn cần thanh toán!");

            if (soTienThanhToan <= 0)
                throw new Exception("Số tiền thanh toán phải lớn hơn 0 VNĐ!");

            DataRow rowHD = hoaDonDAL.GetHoaDonById(maHoaDon);
            if (rowHD == null)
                throw new Exception($"Không tìm thấy hóa đơn #{maHoaDon} trong hệ thống!");

            decimal tongTien = Convert.ToDecimal(rowHD["TongTien"]);
            decimal daThanhToan = Convert.ToDecimal(rowHD["DaThanhToan"]);
            decimal conNo = tongTien - daThanhToan;

            if (conNo <= 0)
                throw new Exception($"Hóa đơn #{maHoaDon} đã được thanh toán đầy đủ!");

            if (soTienThanhToan > conNo)
                throw new Exception($"Số tiền thanh toán ({soTienThanhToan:N0} đ) vượt quá số tiền còn nợ ({conNo:N0} đ) của hóa đơn #{maHoaDon}!");

            return thanhToanDAL.ThanhToanHoaDon(maHoaDon, soTienThanhToan, phuongThuc, nguoiNop, ghiChu);
        }

        public DataTable GetLichSuThanhToanByHoaDon(int maHoaDon)
        {
            if (maHoaDon <= 0)
                throw new Exception("Mã hóa đơn không hợp lệ!");

            return thanhToanDAL.GetLichSuThanhToanByHoaDon(maHoaDon);
        }

        public DataTable GetAllLichSuThanhToan(int? maHoaDon = null, string keyword = "")
        {
            return thanhToanDAL.GetAllLichSuThanhToan(maHoaDon, keyword);
        }
    }
}

