CREATE DATABASE QuanLyKyTucXa;
GO
USE QuanLyKyTucXa;
GO
-- ==========================================
-- 1. BẢNG SINH VIÊN (Master Data)
-- ==========================================
CREATE TABLE SinhVien (
    MaSV VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    CCCD VARCHAR(20) UNIQUE NOT NULL,
    SDT VARCHAR(15),
    GioiTinh NVARCHAR(10)
);

-- ==========================================
-- 2. BẢNG PHÒNG (Master Data)
-- ==========================================
CREATE TABLE Phong (
    MaPhong VARCHAR(10) PRIMARY KEY, -- VD: A101, B205
    TenKhu NVARCHAR(50),             -- VD: Khu A, Khu B
    LoaiPhong NVARCHAR(20),          -- VD: Nam, Nữ, Giảng viên
    SoGiuongToiDa INT NOT NULL,      -- VD: 8
    SoGiuongTrong INT NOT NULL,      -- Số giường còn trống để xét duyệt hợp đồng
    DonGia DECIMAL(18, 0) NOT NULL   -- Giá tiền 1 giường/tháng
);

-- ==========================================
-- 3. BẢNG HỢP ĐỒNG (Liên kết Sinh Viên <-> Phòng)
-- ==========================================
CREATE TABLE HopDong (
    MaHopDong INT IDENTITY(1,1) PRIMARY KEY, -- Tự động tăng
    MaSV VARCHAR(20) NOT NULL,
    MaPhong VARCHAR(10) NOT NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Đang hiệu lực', -- VD: Đang hiệu lực, Đã thanh lý
    FOREIGN KEY (MaSV) REFERENCES SinhVien(MaSV),
    FOREIGN KEY (MaPhong) REFERENCES Phong(MaPhong)
);

-- ==========================================
-- 4. BẢNG ĐIỆN NƯỚC (Quản lý chỉ số hàng tháng theo Phòng)
-- ==========================================
CREATE TABLE DienNuoc (
    MaDienNuoc INT IDENTITY(1,1) PRIMARY KEY,
    MaPhong VARCHAR(10) NOT NULL,
    Thang INT NOT NULL,
    Nam INT NOT NULL,
    ChiSoDienCu INT DEFAULT 0,
    ChiSoDienMoi INT DEFAULT 0,
    ChiSoNuocCu INT DEFAULT 0,
    ChiSoNuocMoi INT DEFAULT 0,
    FOREIGN KEY (MaPhong) REFERENCES Phong(MaPhong),
    -- Đảm bảo 1 phòng trong 1 tháng chỉ có 1 bản ghi điện nước
    CONSTRAINT UC_DienNuoc_PhongThangNam UNIQUE (MaPhong, Thang, Nam) 
);

-- ==========================================
-- 5. BẢNG HÓA ĐƠN THU PHÍ (Tổng hợp tiền phòng + điện nước)
-- ==========================================
CREATE TABLE HoaDon (
    MaHoaDon INT IDENTITY(1,1) PRIMARY KEY,
    MaPhong VARCHAR(10) NOT NULL,
    Thang INT NOT NULL,
    Nam INT NOT NULL,
    TienPhong DECIMAL(18, 0) DEFAULT 0,  -- Tiền phòng tổng của các SV trong phòng
    TienDien DECIMAL(18, 0) DEFAULT 0,
    TienNuoc DECIMAL(18, 0) DEFAULT 0,
    TongTien DECIMAL(18, 0) DEFAULT 0,
    TrangThai NVARCHAR(50) DEFAULT N'Chưa thanh toán', -- Chưa thanh toán, Đã thanh toán
    FOREIGN KEY (MaPhong) REFERENCES Phong(MaPhong)
);
