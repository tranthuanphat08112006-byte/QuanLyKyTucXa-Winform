# Tab "Quản lý vai trò" & Phân quyền chi tiết Implementation Plan

**Goal:** Thêm tab "Quản lý vai trò" kèm ma trận phân quyền chi tiết (Module × Xem/Thêm/Sửa/Xóa) vào module `Quản lý Tài khoản` (`UCTaiKhoan`), cho phép gán vai trò động cho tài khoản và thực thi phân quyền trên toàn ứng dụng WinForms.

**Architecture:** Kiến trúc 3 tầng hiện có (`DTO` → `DAL` → `BLL` → `GUI`). Bổ sung bảng `PhanQuyen` và khóa ngoại `FK_TaiKhoan_VaiTro` qua `EnsureSchema()` idempotent; thêm `PhanQuyenDTO`, `ModuleSystem`, `VaiTroDAL`, `VaiTroBLL`, `frmChiTietVaiTro`; nâng cấp `UCTaiKhoan` (2 tab: Tài khoản & Vai trò), `frmChiTietTaiKhoan` (chọn vai trò), `UserSession`, `frmDangNhap`, `MainForm`, và 5 UserControl (`UCSinhVien`, `UCPhong`, `UCHopDong`, `UCDichVuTaiChinh`, `UCTaiKhoan`).

**Tech Stack:** C# (.NET Framework 4.7.2/4.8), WinForms, SQL Server (`System.Data.SqlClient`), MSBuild.

**Spec:** [2026-11-20-quan-ly-vai-tro-phan-quyen-design.md](../specs/2026-11-20-quan-ly-vai-tro-phan-quyen-design.md)

## Global Constraints
- Không phá vỡ dữ liệu hiện có; mọi thay đổi DDL trong `EnsureSchema()` phải idempotent.
- Mọi file `.cs` mới phải được khai báo trong file `.csproj` tương ứng (non-SDK-style `.csproj`).
- Vai trò `Admin` luôn mặc định toàn quyền, không được xóa, không được tắt quyền trên module `TaiKhoan`.
- Không được xóa vai trò đang có tài khoản sử dụng (`SoTaiKhoan > 0`).
- Quy tắc ma trận quyền: `Them`/`Sua`/`Xoa` = true buộc `Xem` = true; bỏ `Xem` buộc bỏ cả 3 quyền còn lại.

## Review Focus
1. Xóa ràng buộc `CK_TaiKhoan_VaiTro` và thêm `FK_TaiKhoan_VaiTro` an toàn ngay cả khi CSDL đã có dữ liệu `TaiKhoan` cũ.
2. Nạp danh sách vai trò vào `ComboBox` ở `UCTaiKhoan` và `frmChiTietTaiKhoan` mà không gây vòng lặp event (`isFilterLoading`).
3. Khi `UserSession` kiểm tra quyền trên vai trò mới tạo (chưa có đủ 5 dòng trong `PhanQuyen`), mặc định trả về `false` an toàn, không `NullReferenceException` hay `KeyNotFoundException`.
4. Trên `MainForm`, nếu user không có quyền xem `SinhVien` (màn hình mặc định), phải tự động mở màn hình đầu tiên user có quyền `Xem` (hoặc hiển thị thông báo không có quyền nếu cả 5 module đều `false`).
5. Khi `UpdateActionButtonsState()` chạy trên `UCHopDong` và `UCDichVuTaiChinh`, không được vô tình bật lại các nút mà user không có quyền `Them`/`Sua`/`Xoa`.

---

### Task 1: DTO Layer (`ModuleSystem`, `PhanQuyenDTO`, `VaiTroDTO`, `TaiKhoanDTO`, `UserSession`)
**Files:**
- Create: `QuanLyKyTucXa.DTO/ModuleSystem.cs`
- Create: `QuanLyKyTucXa.DTO/PhanQuyenDTO.cs`
- Modify: `QuanLyKyTucXa.DTO/VaiTroDTO.cs`
- Modify: `QuanLyKyTucXa.DTO/TaiKhoanDTO.cs`
- Modify: `QuanLyKyTucXa.DTO/UserSession.cs`
- Modify: `QuanLyKyTucXa.DTO/QuanLyKyTucXa.DTO.csproj`

- [ ] **Step 1:** Tạo `ModuleSystem.cs` định nghĩa hằng số 5 module (`SinhVien`, `Phong`, `HopDong`, `DichVuTaiChinh`, `TaiKhoan`) và danh sách `AllModules` kèm tên hiển thị tiếng Việt.
- [ ] **Step 2:** Tạo `PhanQuyenDTO.cs` với các thuộc tính `MaVaiTro`, `MaModule`, `TenModule`, `CoQuyenXem`, `CoQuyenThem`, `CoQuyenSua`, `CoQuyenXoa`.
- [ ] **Step 3:** Cập nhật `VaiTroDTO.cs` thêm `SoTaiKhoan` và `ToString()`.
- [ ] **Step 4:** Cập nhật `TaiKhoanDTO.cs` thêm `TenVaiTro` để `VaiTroHienThi` ưu tiên hiển thị tên từ bảng `VaiTro`.
- [ ] **Step 5:** Cập nhật `UserSession.cs` lưu `Permissions`, bổ sung `Login(TaiKhoanDTO, IEnumerable<PhanQuyenDTO>)`, `UpdatePermissions(...)`, `CanView`, `CanAdd`, `CanEdit`, `CanDelete`.
- [ ] **Step 6:** Đăng ký `ModuleSystem.cs` và `PhanQuyenDTO.cs` trong `QuanLyKyTucXa.DTO.csproj`.

### Task 2: DAL Layer (`TaiKhoanDAL` Schema & Joins + `VaiTroDAL`)
**Files:**
- Modify: `QuanLyKyTucXa.DAL/TaiKhoanDAL.cs`
- Create: `QuanLyKyTucXa.DAL/VaiTroDAL.cs`
- Modify: `QuanLyKyTucXa.DAL/QuanLyKyTucXa.DAL.csproj`

- [ ] **Step 1:** Cập nhật `TaiKhoanDAL.EnsureSchema()`: gỡ `CK_TaiKhoan_VaiTro`, thêm `FK_TaiKhoan_VaiTro`, tạo bảng `PhanQuyen`, seed quyền mặc định cho `Admin`, `NhanVien`, `SinhVien`.
- [ ] **Step 2:** Cập nhật `TaiKhoanDAL` (`GetTaiKhoanByUsername`, `GetTaiKhoanById`, `GetAllTaiKhoan`, `FilterTaiKhoan`) `LEFT JOIN VaiTro` lấy `TenVaiTro` và lọc theo `MaVaiTro` / `TenVaiTro` động.
- [ ] **Step 3:** Tạo `VaiTroDAL.cs` với: `GetAllVaiTro()`, `FilterVaiTro(string keyword)`, `GetVaiTroById(string maVaiTro)`, `CheckMaVaiTroExists(string maVaiTro)`, `CountTaiKhoanByVaiTro(string maVaiTro)`, `InsertVaiTro(VaiTroDTO, List<PhanQuyenDTO>)`, `UpdateVaiTro(VaiTroDTO)`, `DeleteVaiTro(string maVaiTro)`, `GetPhanQuyenByVaiTro(string maVaiTro)`, `SavePhanQuyen(string maVaiTro, List<PhanQuyenDTO>)`.
- [ ] **Step 4:** Đăng ký `VaiTroDAL.cs` trong `QuanLyKyTucXa.DAL.csproj`.

### Task 3: BLL Layer (`VaiTroBLL` & `TaiKhoanBLL`)
**Files:**
- Create: `QuanLyKyTucXa.BLL/VaiTroBLL.cs`
- Modify: `QuanLyKyTucXa.BLL/TaiKhoanBLL.cs`
- Modify: `QuanLyKyTucXa.BLL/QuanLyKyTucXa.BLL.csproj`

- [ ] **Step 1:** Tạo `VaiTroBLL.cs` với đầy đủ kiểm tra phân quyền (`ModuleSystem.TaiKhoan`), validation (`MaVaiTro`, `TenVaiTro`, không xóa `Admin`, không xóa vai trò có tài khoản, tự đồng bộ `Xem` khi có `Them`/`Sua`/`Xoa`, khóa sửa quyền của `Admin`).
- [ ] **Step 2:** Cập nhật `TaiKhoanBLL.cs` để kiểm tra quyền linh hoạt qua `UserSession.CanView/CanAdd/CanEdit/CanDelete(ModuleSystem.TaiKhoan)` (giữ `Admin` luôn có quyền) và validate `VaiTro` tồn tại.
- [ ] **Step 3:** Đăng ký `VaiTroBLL.cs` trong `QuanLyKyTucXa.BLL.csproj`.

### Task 4: GUI — `frmChiTietVaiTro`, `frmChiTietTaiKhoan`, và `UCTaiKhoan` (2 tab)
**Files:**
- Create: `QuanLyKyTucXa.GUI/frmChiTietVaiTro.cs`
- Create: `QuanLyKyTucXa.GUI/frmChiTietVaiTro.Designer.cs`
- Modify: `QuanLyKyTucXa.GUI/frmChiTietTaiKhoan.cs`
- Modify: `QuanLyKyTucXa.GUI/frmChiTietTaiKhoan.Designer.cs`
- Modify: `QuanLyKyTucXa.GUI/UCTaiKhoan.cs`
- Modify: `QuanLyKyTucXa.GUI/UCTaiKhoan.Designer.cs`
- Modify: `QuanLyKyTucXa.GUI/QuanLyKyTucXa.GUI.csproj`

- [ ] **Step 1:** Tạo `frmChiTietVaiTro` (Thêm/Sửa `MaVaiTro`, `TenVaiTro`, `MoTa`) theo chuẩn Modern Flat Design của dự án.
- [ ] **Step 2:** Cập nhật `frmChiTietTaiKhoan` thêm `ComboBox cmbVaiTro` nạp từ `VaiTroBLL.GetAllVaiTro()`, lưu đúng `MaVaiTro` được chọn.
- [ ] **Step 3:** Nâng cấp `UCTaiKhoan` có 2 tab (`Quản lý Tài khoản` và `Quản lý Vai trò`):
  - Tab Tài khoản: thêm lọc `cmbLocVaiTro` động.
  - Tab Vai trò: layout 2 cột (trái: danh sách vai trò + tìm kiếm + Thêm/Sửa/Xóa; phải: ma trận checkbox 5 module × 4 quyền + Lưu phân quyền + auto-sync checkbox + khóa read-only cho `Admin`).
  - Áp dụng quyền `CanAdd/CanEdit/CanDelete(ModuleSystem.TaiKhoan)` lên các nút trên cả 2 tab.
- [ ] **Step 4:** Đăng ký `frmChiTietVaiTro.cs` và `frmChiTietVaiTro.Designer.cs` trong `QuanLyKyTucXa.GUI.csproj`.

### Task 5: GUI — Thực thi phân quyền trên `frmDangNhap`, `MainForm`, và 4 UserControl nghiệp vụ
**Files:**
- Modify: `QuanLyKyTucXa.GUI/frmDangNhap.cs`
- Modify: `QuanLyKyTucXa.GUI/MainForm.cs`
- Modify: `QuanLyKyTucXa.GUI/UCSinhVien.cs`
- Modify: `QuanLyKyTucXa.GUI/UCPhong.cs`
- Modify: `QuanLyKyTucXa.GUI/UCHopDong.cs`
- Modify: `QuanLyKyTucXa.GUI/UCDichVuTaiChinh.cs`

- [ ] **Step 1:** Cập nhật `frmDangNhap.cs` nạp ma trận `PhanQuyen` qua `VaiTroBLL.GetPhanQuyenByVaiTro(user.VaiTro)` và truyền vào `UserSession.Login(user, permissions)`.
- [ ] **Step 2:** Cập nhật `MainForm.cs` ẩn/hiện 5 nút menu theo `UserSession.CanView(...)`, kiểm tra quyền khi click, và tự động mở module đầu tiên user có quyền xem.
- [ ] **Step 3:** Cập nhật `UCSinhVien.cs`, `UCPhong.cs`, `UCHopDong.cs`, `UCDichVuTaiChinh.cs` bổ sung `ApplyPermissions()` để ẩn/khóa các nút Thêm/Sửa/Xóa tương ứng quyền của `UserSession`.

### Task 6: Build & Verify
- [ ] **Step 1:** Chạy `MSBuild.exe QuanLyKyTucXa.sln /p:Configuration=Debug /t:Build /v:minimal` và xác nhận 0 lỗi biên dịch.
- [ ] **Step 2:** Kiểm tra lại toàn bộ các ràng buộc trong `Review Focus` và báo cáo kết quả cho người dùng.
