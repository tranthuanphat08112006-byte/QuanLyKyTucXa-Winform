using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using QuanLyKyTucXa.DAL;
using QuanLyKyTucXa.DTO;

namespace QuanLyKyTucXa.BLL
{
    public class VaiTroBLL
    {
        private readonly VaiTroDAL vaiTroDAL = new VaiTroDAL();

        private void CheckPermission(string action)
        {
            if (!UserSession.IsLoggedIn)
                throw new UnauthorizedAccessException("Phiên đăng nhập đã hết hạn hoặc chưa đăng nhập. Vui lòng đăng nhập lại!");

            bool allowed = false;
            switch (action)
            {
                case "View": allowed = UserSession.CanView(ModuleSystem.TaiKhoan); break;
                case "Add": allowed = UserSession.CanAdd(ModuleSystem.TaiKhoan); break;
                case "Edit": allowed = UserSession.CanEdit(ModuleSystem.TaiKhoan); break;
                case "Delete": allowed = UserSession.CanDelete(ModuleSystem.TaiKhoan); break;
            }

            if (!allowed)
                throw new UnauthorizedAccessException("Bạn không có quyền thực hiện thao tác này trên module Quản lý Tài khoản & Vai trò!");
        }

        public DataTable GetAllVaiTro()
        {
            CheckPermission("View");
            return vaiTroDAL.GetAllVaiTro();
        }

        public DataTable FilterVaiTro(string keyword)
        {
            CheckPermission("View");
            return vaiTroDAL.FilterVaiTro(keyword ?? "");
        }

        public VaiTroDTO GetVaiTroById(string maVaiTro)
        {
            CheckPermission("View");
            return vaiTroDAL.GetVaiTroById(maVaiTro);
        }

        /// <summary>
        /// Lấy danh sách phân quyền của một vai trò (dùng cả khi đăng nhập và khi hiển thị ma trận quyền)
        /// </summary>
        public List<PhanQuyenDTO> GetPhanQuyenByVaiTro(string maVaiTro)
        {
            if (string.IsNullOrWhiteSpace(maVaiTro))
                return new List<PhanQuyenDTO>();

            return vaiTroDAL.GetPhanQuyenByVaiTro(maVaiTro.Trim());
        }

        public bool InsertVaiTro(VaiTroDTO vt, List<PhanQuyenDTO> permissions = null)
        {
            CheckPermission("Add");

            if (vt == null)
                throw new Exception("Dữ liệu vai trò không hợp lệ!");

            if (string.IsNullOrWhiteSpace(vt.MaVaiTro))
                throw new Exception("Mã vai trò không được để trống!");

            string ma = vt.MaVaiTro.Trim();
            if (ma.Length < 2 || ma.Length > 20)
                throw new Exception("Mã vai trò phải có độ dài từ 2 đến 20 ký tự!");

            if (!Regex.IsMatch(ma, @"^[a-zA-Z0-9_]+$"))
                throw new Exception("Mã vai trò chỉ được chứa chữ cái không dấu, số và dấu gạch dưới (_), không chứa khoảng trắng!");

            if (vaiTroDAL.CheckMaVaiTroExists(ma))
                throw new Exception($"Mã vai trò '{ma}' đã tồn tại trong hệ thống!");

            if (string.IsNullOrWhiteSpace(vt.TenVaiTro))
                throw new Exception("Tên vai trò không được để trống!");

            if (vt.TenVaiTro.Trim().Length > 50)
                throw new Exception("Tên vai trò không được vượt quá 50 ký tự!");

            vt.MaVaiTro = ma;
            vt.TenVaiTro = vt.TenVaiTro.Trim();
            vt.MoTa = vt.MoTa?.Trim() ?? "";

            List<PhanQuyenDTO> normalized = NormalizePermissions(ma, permissions);
            return vaiTroDAL.InsertVaiTro(vt, normalized);
        }

        public bool UpdateVaiTro(VaiTroDTO vt)
        {
            CheckPermission("Edit");

            if (vt == null || string.IsNullOrWhiteSpace(vt.MaVaiTro))
                throw new Exception("Thông tin vai trò không hợp lệ!");

            if (string.IsNullOrWhiteSpace(vt.TenVaiTro))
                throw new Exception("Tên vai trò không được để trống!");

            if (vt.TenVaiTro.Trim().Length > 50)
                throw new Exception("Tên vai trò không được vượt quá 50 ký tự!");

            if (!vaiTroDAL.CheckMaVaiTroExists(vt.MaVaiTro.Trim()))
                throw new Exception("Không tìm thấy vai trò cần cập nhật!");

            vt.MaVaiTro = vt.MaVaiTro.Trim();
            vt.TenVaiTro = vt.TenVaiTro.Trim();
            vt.MoTa = vt.MoTa?.Trim() ?? "";

            return vaiTroDAL.UpdateVaiTro(vt);
        }

        public bool DeleteVaiTro(string maVaiTro)
        {
            CheckPermission("Delete");

            if (string.IsNullOrWhiteSpace(maVaiTro))
                throw new Exception("Vui lòng chọn vai trò cần xóa!");

            string ma = maVaiTro.Trim();
            if (string.Equals(ma, "Admin", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Không thể xóa vai trò hệ thống cốt lõi 'Admin'!");

            if (!vaiTroDAL.CheckMaVaiTroExists(ma))
                throw new Exception("Vai trò không tồn tại hoặc đã bị xóa trước đó!");

            int countUsers = vaiTroDAL.CountTaiKhoanByVaiTro(ma);
            if (countUsers > 0)
                throw new Exception($"Không thể xóa vai trò này vì đang có {countUsers} tài khoản sử dụng. Vui lòng chuyển các tài khoản đó sang vai trò khác trước!");

            return vaiTroDAL.DeleteVaiTro(ma);
        }

        public bool SavePhanQuyen(string maVaiTro, List<PhanQuyenDTO> permissions)
        {
            CheckPermission("Edit");

            if (string.IsNullOrWhiteSpace(maVaiTro))
                throw new Exception("Vui lòng chọn vai trò cần phân quyền!");

            string ma = maVaiTro.Trim();
            if (string.Equals(ma, "Admin", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Vai trò Quản trị viên (Admin) luôn mặc định có toàn quyền hệ thống và không thể chỉnh sửa!");

            if (!vaiTroDAL.CheckMaVaiTroExists(ma))
                throw new Exception("Vai trò không tồn tại trong hệ thống!");

            List<PhanQuyenDTO> normalized = NormalizePermissions(ma, permissions);
            bool saved = vaiTroDAL.SavePhanQuyen(ma, normalized);

            // Nếu người dùng hiện tại thuộc đúng vai trò vừa cập nhật quyền thì đồng bộ lại phiên ngay
            if (saved && UserSession.CurrentUser != null &&
                string.Equals(UserSession.CurrentUser.VaiTro, ma, StringComparison.OrdinalIgnoreCase))
            {
                UserSession.UpdatePermissions(normalized);
            }

            return saved;
        }

        private List<PhanQuyenDTO> NormalizePermissions(string maVaiTro, List<PhanQuyenDTO> input)
        {
            var map = new Dictionary<string, PhanQuyenDTO>(StringComparer.OrdinalIgnoreCase);
            if (input != null)
            {
                foreach (var p in input)
                {
                    if (p != null && !string.IsNullOrWhiteSpace(p.MaModule))
                    {
                        bool them = p.CoQuyenThem;
                        bool sua = p.CoQuyenSua;
                        bool xoa = p.CoQuyenXoa;
                        bool xem = p.CoQuyenXem || them || sua || xoa;

                        map[p.MaModule.Trim()] = new PhanQuyenDTO(maVaiTro, p.MaModule.Trim(), xem, them, sua, xoa);
                    }
                }
            }

            var result = new List<PhanQuyenDTO>();
            foreach (var mod in ModuleSystem.AllModules)
            {
                if (map.TryGetValue(mod.Key, out PhanQuyenDTO item))
                    result.Add(item);
                else
                    result.Add(new PhanQuyenDTO(maVaiTro, mod.Key, false, false, false, false));
            }
            return result;
        }
    }
}
