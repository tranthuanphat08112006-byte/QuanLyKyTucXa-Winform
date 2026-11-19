-- ====================================================================
-- Script cập nhật CSDL cho Module Quản lý Dịch vụ & Tài chính
-- ====================================================================

USE QuanLyKyTucXa;
GO

-- 1. Bảng CauHinhDichVu: lưu trữ đơn giá điện, nước, dịch vụ khác
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CauHinhDichVu')
BEGIN
    CREATE TABLE CauHinhDichVu (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        MaDichVu NVARCHAR(50) NOT NULL UNIQUE,
        TenDichVu NVARCHAR(100) NOT NULL,
        DonGia DECIMAL(18,2) NOT NULL DEFAULT 0,
        DonViTinh NVARCHAR(20) NOT NULL,
        NgayCapNhat DATETIME DEFAULT GETDATE(),
        GhiChu NVARCHAR(255) NULL
    );

    -- Dữ liệu mẫu ban đầu
    INSERT INTO CauHinhDichVu (MaDichVu, TenDichVu, DonGia, DonViTinh, GhiChu)
    VALUES 
    (N'DIEN', N'Điện sinh hoạt', 3500, N'kWh', N'Đơn giá điện'),
    (N'NUOC', N'Nước sinh hoạt', 15000, N'm3', N'Đơn giá nước'),
    (N'RAC_VESINH', N'Rác & Vệ sinh', 30000, N'Tháng', N'Phí vệ sinh định kỳ');
END
GO

-- 2. Cập nhật bảng HoaDon nếu thiếu các cột chi tiết và công nợ
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'TienPhong')
    ALTER TABLE HoaDon ADD TienPhong DECIMAL(18,2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'SoDienTieuThu')
    ALTER TABLE HoaDon ADD SoDienTieuThu INT DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'DonGiaDien')
    ALTER TABLE HoaDon ADD DonGiaDien DECIMAL(18,2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'TienDien')
    ALTER TABLE HoaDon ADD TienDien DECIMAL(18,2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'SoNuocTieuThu')
    ALTER TABLE HoaDon ADD SoNuocTieuThu INT DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'DonGiaNuoc')
    ALTER TABLE HoaDon ADD DonGiaNuoc DECIMAL(18,2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'TienNuoc')
    ALTER TABLE HoaDon ADD TienNuoc DECIMAL(18,2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'TienDichVuKhac')
    ALTER TABLE HoaDon ADD TienDichVuKhac DECIMAL(18,2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'DaThanhToan')
    ALTER TABLE HoaDon ADD DaThanhToan DECIMAL(18,2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'ConLai')
    ALTER TABLE HoaDon ADD ConLai DECIMAL(18,2) DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HoaDon') AND name = 'GhiChu')
    ALTER TABLE HoaDon ADD GhiChu NVARCHAR(255) NULL;
GO

-- 3. Bảng LichSuThanhToan: ghi nhận mỗi lần thanh toán của hóa đơn
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LichSuThanhToan')
BEGIN
    CREATE TABLE LichSuThanhToan (
        MaThanhToan INT IDENTITY(1,1) PRIMARY KEY,
        MaHoaDon INT NOT NULL,
        NgayThanhToan DATETIME NOT NULL DEFAULT GETDATE(),
        SoTienThanhToan DECIMAL(18,2) NOT NULL,
        HinhThucThanhToan NVARCHAR(50) NOT NULL,
        NguoiThu NVARCHAR(100) NULL,
        GhiChu NVARCHAR(255) NULL,
        CONSTRAINT FK_LichSuThanhToan_HoaDon FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon) ON DELETE CASCADE
    );
END
GO
