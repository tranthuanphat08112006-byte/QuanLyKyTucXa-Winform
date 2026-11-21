# Thiết kế tính năng: Tab "Quản lý vai trò" & Phân quyền chi tiết

- **Ngày tạo**: 2026-11-20
- **Phạm vi**: Module `Quản lý Tài khoản` (`UCTaiKhoan`) và phân quyền thực thi trên toàn bộ ứng dụng WinForms `QuanLyKyTucXa` (.NET Framework 4.8).

---

## 1. Mục tiêu & Lý do (Intent & Why)
Bổ sung tab **"Quản lý vai trò"** vào trong module **Quản lý Tài khoản** nhằm:
1. Cho phép Quản trị viên chủ động thêm, sửa, xóa các vai trò trong hệ thống thay vì bị giới hạn cứng 3 vai trò (`Admin`, `NhanVien`, `SinhVien`).
2. Cho phép cấu hình **phân quyền chi tiết theo ma trận Module × Hành động (`Xem / Thêm / Sửa / Xóa`)** cho từng vai trò.
3. Gán vai trò động cho từng tài khoản khi thêm/sửa tài khoản, đồng thời áp dụng phân quyền thực tế trên giao diện và nghiệp vụ khi người dùng đăng nhập.

---

## 2. Goals, Non-Goals & Anti-Goals

### Goals (Mục tiêu phải đạt)
- **Quản lý Vai trò (CRUD)**: Xem danh sách vai trò (kèm số lượng tài khoản đang dùng vai trò đó), tìm kiếm, thêm mới, sửa tên/mô tả, xóa vai trò.
- **Ma trận Phân quyền chi tiết**: Quản lý quyền `Xem`, `Thêm`, `Sửa`, `Xóa` trên 5 module chính:
  1. `SinhVien` — Quản lý Sinh viên
  2. `Phong` — Quản lý Phòng & Dãy nhà
  3. `HopDong` — Quản lý Hợp đồng
  4. `DichVuTaiChinh` — Dịch vụ & Tài chính
  5. `TaiKhoan` — Quản lý Tài khoản & Vai trò
- **Tích hợp vào Quản lý Tài khoản**:
  - `UCTaiKhoan` chia thành 2 tab rõ ràng: **Quản lý Tài khoản** và **Quản lý Vai trò** (dùng phong cách tab chuẩn hiện đại như `UCDichVuTaiChinh` hoặc `TabControl` đồng bộ).
  - Thêm bộ lọc theo **Vai trò** động (nạp từ bảng `VaiTro`) ở tab Quản lý Tài khoản.
  - Bổ sung chọn **Vai trò** (`ComboBox` nạp từ bảng `VaiTro`) trong `frmChiTietTaiKhoan` khi thêm/sửa tài khoản.
  - Hiển thị đúng `TenVaiTro` từ bảng `VaiTro` thay vì `CASE WHEN` cứng.
- **Áp dụng phân quyền thực tế khi đăng nhập**:
  - `UserSession` nạp ma trận quyền của vai trò người dùng hiện tại khi đăng nhập.
  - `MainForm` ẩn các nút menu của module không có quyền `Xem` và tự động mở module đầu tiên người dùng được phép xem.
  - Cả 5 UserControl (`UCSinhVien`, `UCPhong`, `UCHopDong`, `UCDichVuTaiChinh`, `UCTaiKhoan`) tự động bật/tắt (hoặc ẩn) các nút hành động tương ứng quyền `Thêm / Sửa / Xóa`.
- **An toàn hệ thống**:
  - Không cho xóa hoặc gỡ quyền của vai trò hệ thống cốt lõi `Admin` (Admin luôn có toàn quyền để tránh khóa trắng hệ thống).
  - Không cho xóa vai trò đang có tài khoản sử dụng (phải chuyển tài khoản sang vai trò khác trước).

### Non-Goals (Ngoài phạm vi)
- Không làm phân quyền đến mức từng dòng dữ liệu (row-level security, ví dụ sinh viên chỉ thấy hợp đồng của riêng mình).
- Không làm gán nhiều vai trò cho 1 tài khoản (mỗi tài khoản vẫn thuộc đúng 1 vai trò `TaiKhoan.VaiTro`).
- Không làm audit log lịch sử thay đổi quyền.

### Anti-Goals (Tránh làm hỏng)
- Không làm mất hoặc hỏng dữ liệu tài khoản/vai trò đang có sẵn trên CSDL hiện tại.
- Không để Admin tự gỡ quyền truy cập module `TaiKhoan` của chính vai trò `Admin` dẫn đến không thể vào lại màn hình quản trị.
- Không quên đăng ký file `.cs` mới vào các file `.csproj` (vì dự án dùng .NET Framework 4.8 non-SDK-style).

---

## 3. Các hướng tiếp cận đã cân nhắc (Approaches)

### Hướng 1: Phân quyền theo Module (Truy cập / Không truy cập)
- Chỉ lưu danh sách module mỗi vai trò được phép vào (`VaiTro_Quyen(MaVaiTro, MaChucNang)`).
- *Ưu điểm*: Gọn nhẹ, chỉ cần sửa `MainForm`, không đụng vào 4 UserControl nghiệp vụ.
- *Nhược điểm*: Không phân tách được quyền xem với quyền thao tác (Thêm/Sửa/Xóa).

### Hướng 2: Phân quyền theo Ma trận Module × Hành động (`Xem / Thêm / Sửa / Xóa`) — **[ĐÃ CHỌN]**
- Lưu ma trận quyền chi tiết theo từng module và 4 hành động cơ bản (`PhanQuyen(MaVaiTro, MaModule, CoQuyenXem, CoQuyenThem, CoQuyenSua, CoQuyenXoa)`).
- *Ưu điểm*: Đáp ứng đúng yêu cầu "phân quyền chi tiết", linh hoạt tạo các vai trò thực tế (như *Kế toán*, *Nhân viên trực*, *Sinh viên chỉ xem*), hiệu ứng kiểm thử/demo rõ rệt trên toàn ứng dụng.
- *Nhược điểm*: Cần cập nhật thêm phần kiểm tra quyền nút bấm trên cả 5 UserControl.
- *Lý do chọn*: Người dùng đã xác nhận chọn Hướng 2 để có phân quyền chi tiết thực sự.

---

## 4. Ràng buộc kỹ thuật (Constraints)
- **Kiến trúc 3 tầng**: `QuanLyKyTucXa.DTO` -> `QuanLyKyTucXa.DAL` -> `QuanLyKyTucXa.BLL` -> `QuanLyKyTucXa.GUI`.
- **Khởi tạo schema tự động**: Dự án dùng `EnsureSchema()` trong DAL (`TaiKhoanDAL`) để tự động tạo/cập nhật bảng SQL Server nếu chưa có. Mọi thay đổi schema (`PhanQuyen`, gỡ `CK_TaiKhoan_VaiTro`, thêm `FK_TaiKhoan_VaiTro`, seed quyền mặc định) phải idempotent trong `EnsureSchema()`.
- **Ngôn ngữ & Framework**: C# (.NET Framework 4.8), WinForms, `System.Data.SqlClient`, `UIHelper.FormatModernDataGridView`.

---

## 5. Thiết kế chi tiết

### 5.1. Cơ sở dữ liệu (SQL Server)
1. **Bảng `VaiTro`** (đã có, giữ nguyên cấu trúc):
   - `MaVaiTro NVARCHAR(20) PRIMARY KEY`
   - `TenVaiTro NVARCHAR(50) NOT NULL`
   - `MoTa NVARCHAR(255) NULL`
   - Dữ liệu mặc định: `Admin`, `NhanVien`, `SinhVien`.
2. **Cập nhật bảng `TaiKhoan`**:
   - Gỡ bỏ ràng buộc cứng `CK_TaiKhoan_VaiTro` (nếu tồn tại).
   - Bổ sung khóa ngoại `FK_TaiKhoan_VaiTro FOREIGN KEY (VaiTro) REFERENCES VaiTro(MaVaiTro)` (nếu chưa tồn tại).
3. **Bảng mới `PhanQuyen`**:
   ```sql
   CREATE TABLE PhanQuyen (
       MaVaiTro NVARCHAR(20) NOT NULL,
       MaModule NVARCHAR(30) NOT NULL,
       CoQuyenXem BIT NOT NULL DEFAULT 0,
       CoQuyenThem BIT NOT NULL DEFAULT 0,
       CoQuyenSua BIT NOT NULL DEFAULT 0,
       CoQuyenXoa BIT NOT NULL DEFAULT 0,
       CONSTRAINT PK_PhanQuyen PRIMARY KEY (MaVaiTro, MaModule),
       CONSTRAINT FK_PhanQuyen_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro) ON DELETE CASCADE
   );
   ```
4. **Dữ liệu quyền mặc định (Seed nếu chưa có)**:
   - `Admin`: Toàn quyền (`1, 1, 1, 1`) trên cả 5 module.
   - `NhanVien`: Toàn quyền (`1, 1, 1, 1`) trên `SinhVien`, `Phong`, `HopDong`, `DichVuTaiChinh`; không có quyền (`0, 0, 0, 0`) trên `TaiKhoan`.
   - `SinhVien`: Chỉ `Xem` (`1, 0, 0, 0`) trên `SinhVien`, `Phong`, `HopDong`, `DichVuTaiChinh`; không có quyền trên `TaiKhoan`.

### 5.2. Tầng DTO (`QuanLyKyTucXa.DTO`)
- **`ModuleSystem`** (hằng số mã module chuẩn): `SinhVien`, `Phong`, `HopDong`, `DichVuTaiChinh`, `TaiKhoan` kèm tên hiển thị tiếng Việt.
- **`PhanQuyenDTO`**: `MaVaiTro`, `MaModule`, `TenModule`, `CoQuyenXem`, `CoQuyenThem`, `CoQuyenSua`, `CoQuyenXoa`.
- **`VaiTroDTO`**: Bổ sung thêm `SoTaiKhoan` (hiển thị) nếu cần.
- **`TaiKhoanDTO`**: Bổ sung `TenVaiTro` (lấy từ `JOIN VaiTro`), `VaiTroHienThi` ưu tiên trả về `TenVaiTro` nếu có, fallback về mapping cũ.
- **`UserSession`**:
  - Lưu `Dictionary<string, PhanQuyenDTO> Permissions` khi đăng nhập.
  - Cung cấp các helper: `CanView(string module)`, `CanAdd(string module)`, `CanEdit(string module)`, `CanDelete(string module)`.
  - Nếu `IsAdmin == true`, các hàm `Can*` luôn trả về `true` (bảo đảm Admin không bao giờ bị khóa khỏi hệ thống).

### 5.3. Tầng DAL & BLL
- **`VaiTroDAL` & `VaiTroBLL`**:
  - `GetAllVaiTro()` (kèm `COUNT` số tài khoản thuộc vai trò), `FilterVaiTro(string keyword)`.
  - `InsertVaiTro(VaiTroDTO, List<PhanQuyenDTO>)`, `UpdateVaiTro(VaiTroDTO, List<PhanQuyenDTO>)`, `DeleteVaiTro(string maVaiTro)`.
  - `GetPhanQuyenByVaiTro(string maVaiTro)` (trả đủ 5 dòng cho 5 module), `SavePhanQuyen(string maVaiTro, List<PhanQuyenDTO>)`.
  - **Validate BLL**:
    - `MaVaiTro`: bắt buộc, 2–20 ký tự, chỉ chữ/số/gạch dưới, không trùng.
    - `TenVaiTro`: bắt buộc.
    - Không cho xóa vai trò `Admin` hoặc vai trò đang có tài khoản sử dụng (`SoTaiKhoan > 0`).
    - Khi sửa vai trò `Admin`, không cho tắt các quyền của module `TaiKhoan` (hoặc khóa không cho sửa quyền của `Admin`).
    - Quy tắc logic quyền: Nếu `Them` / `Sua` / `Xoa` = `true` thì tự động buộc `Xem` = `true`.
    - Kiểm tra quyền `CanView/CanAdd/CanEdit/CanDelete(ModuleSystem.TaiKhoan)` ở BLL.
- **`TaiKhoanDAL` & `TaiKhoanBLL`**:
  - Cập nhật `GetAllTaiKhoan()`, `FilterTaiKhoan()`, `GetTaiKhoanByUsername()`, `GetTaiKhoanById()` để `LEFT JOIN VaiTro` lấy `TenVaiTro`.
  - Cập nhật `CheckAdminPermission()` hoặc bổ sung kiểm tra quyền theo `ModuleSystem.TaiKhoan` để vai trò tùy chỉnh được cấp quyền `TaiKhoan` cũng thao tác được hợp lệ.

### 5.4. Tầng Giao diện (`QuanLyKyTucXa.GUI`)
1. **Cập nhật `UCTaiKhoan`** — bổ sung thanh chuyển tab hiện đại (tương tự `UCDichVuTaiChinh`) gồm 2 tab:
   - **Tab 1: Quản lý Tài khoản**
     - Giữ nguyên lưới tài khoản, bổ sung thêm `ComboBox` lọc theo **Vai trò** (nạp động từ bảng `VaiTro`).
   - **Tab 2: Quản lý Vai trò & Phân quyền**
     - **Bên trái (~48%)**: Danh sách vai trò (`dgvVaiTro`: Mã vai trò, Tên vai trò, Mô tả, Số tài khoản) + ô tìm kiếm + các nút `Thêm vai trò`, `Sửa`, `Xóa`.
     - **Bên phải (~52%)**: Panel **"Phân quyền chi tiết cho vai trò đang chọn"** gồm bảng ma trận `dgvPhanQuyen` (5 dòng module × 4 cột checkbox `Xem`, `Thêm`, `Sửa`, `Xóa`) + nút **"Lưu phân quyền"**.
     - Khi click chọn 1 dòng ở `dgvVaiTro`, ma trận bên phải tự nạp quyền của vai trò đó. Với vai trò `Admin`, ma trận hiển thị toàn quyền ở chế độ chỉ đọc (có ghi chú "Quản trị viên mặc định có toàn quyền hệ thống").
     - Auto-sync checkbox trong ma trận: tick `Thêm/Sửa/Xóa` → tự tick `Xem`; bỏ tick `Xem` → tự bỏ tick `Thêm/Sửa/Xóa`.
   - **Thẻ thống kê phía trên (`tlpStats`)**: Giữ nguyên 4 thẻ gọn đẹp (hoặc cập nhật nhãn thẻ thứ 4 hiển thị số Vai trò / Quản trị viên cho đồng bộ).
2. **Form mới `frmChiTietVaiTro`**:
   - Dùng cho Thêm mới / Sửa thông tin vai trò (`MaVaiTro`, `TenVaiTro`, `MoTa`), thiết kế đồng bộ chuẩn Modern Flat Design với `frmChiTietTaiKhoan`. Khi thêm mới, khởi tạo sẵn quyền mặc định (chỉ Xem hoặc trống) để Admin tick tiếp ở ma trận (hoặc cho phép tick ngay).
3. **Cập nhật `frmChiTietTaiKhoan`**:
   - Bổ sung `ComboBox` chọn **Vai trò** (`cmbVaiTro`), nạp danh sách từ `VaiTroBLL.GetAllVaiTro()`, hiển thị `TenVaiTro`, giá trị là `MaVaiTro`.
   - Khi sửa tài khoản, tự chọn đúng vai trò hiện tại của tài khoản.
4. **Áp dụng phân quyền trên toàn app**:
   - `frmDangNhap`: Sau khi `Authenticate` thành công, nạp `List<PhanQuyenDTO>` của vai trò đó vào `UserSession.Login(user, permissions)`.
   - `MainForm`: Kiểm tra `UserSession.CanView(...)` cho từng nút menu (`btnSinhVien`, `btnPhong`, `btnHopDong`, `btnDichVuTaiChinh`, `btnTaiKhoan`). Tự động mở module đầu tiên user có quyền `Xem` (nếu không có quyền nào thì thông báo rõ ràng).
   - 5 UserControl (`UCSinhVien`, `UCPhong`, `UCHopDong`, `UCDichVuTaiChinh`, `UCTaiKhoan`): Thêm hàm `ApplyPermissions()` gọi lúc `Load` (và sau các hàm `UpdateActionButtonsState()`) để ẩn/khóa các nút `Thêm`, `Sửa`, `Xóa` tương ứng quyền `CanAdd`, `CanEdit`, `CanDelete`.

---

## 6. Quyết định mặc định do người thiết kế đề xuất
- **Khóa chỉnh sửa quyền của vai trò `Admin`**: `Admin` luôn mặc định toàn quyền và chỉ đọc trên ma trận quyền để đảm bảo không bao giờ tự khóa khỏi hệ thống.
- **Vị trí ma trận phân quyền**: Đặt trực tiếp bên phải danh sách vai trò trong Tab "Quản lý vai trò" (layout 2 cột Master–Detail) để Quản trị viên chọn vai trò nào là thấy và chỉnh quyền ngay, không cần mở nhiều popup chồng nhau.
- **Lưu quyền tức thì**: Nếu tài khoản đang đăng nhập thuộc vai trò vừa được cập nhật quyền (trường hợp vai trò tùy chỉnh được cấp quyền `TaiKhoan`), có thể nạp lại `UserSession` hoặc áp dụng ngay lần đăng nhập tiếp theo.
