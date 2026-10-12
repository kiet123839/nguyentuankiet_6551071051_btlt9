-- =============================================
-- CSDL QuanLyDiem (SQL Server)
-- =============================================
USE master;
GO

IF DB_ID(N'QuanLyDiem') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyDiem SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyDiem;
END
GO

CREATE DATABASE QuanLyDiem;
GO

USE QuanLyDiem;
GO

-- ---------- Bảng Lop ----------
CREATE TABLE Lop (
    MaLop  VARCHAR(10)   NOT NULL,
    TenLop NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_Lop PRIMARY KEY (MaLop)
);
GO

-- ---------- Bảng SinhVien ----------
CREATE TABLE SinhVien (
    MaSV     VARCHAR(10)   NOT NULL,
    HoTen    NVARCHAR(100) NOT NULL,
    NgaySinh DATETIME      NOT NULL,
    GioiTinh NVARCHAR(3)   NOT NULL,
    MaLop    VARCHAR(10)   NOT NULL,
    CONSTRAINT PK_SinhVien PRIMARY KEY (MaSV),
    CONSTRAINT FK_SinhVien_Lop FOREIGN KEY (MaLop) REFERENCES Lop (MaLop),
    CONSTRAINT CK_SinhVien_GioiTinh CHECK (GioiTinh IN (N'Nam', N'Nữ'))
);
GO

-- ---------- Bảng DiemMon ----------
CREATE TABLE DiemMon (
    MaDiem   INT IDENTITY(1,1) NOT NULL,
    MaSV     VARCHAR(10)   NOT NULL,
    TenMon   NVARCHAR(100) NOT NULL,
    SoTinChi INT           NOT NULL,
    Diem     DECIMAL(4,2)  NOT NULL,
    CONSTRAINT PK_DiemMon PRIMARY KEY (MaDiem),
    CONSTRAINT FK_DiemMon_SinhVien FOREIGN KEY (MaSV) REFERENCES SinhVien (MaSV),
    CONSTRAINT CK_DiemMon_SoTinChi CHECK (SoTinChi BETWEEN 1 AND 6),
    CONSTRAINT CK_DiemMon_Diem CHECK (Diem BETWEEN 0 AND 10)
);
GO

-- =============================================
-- Dữ liệu mẫu
-- =============================================
INSERT INTO Lop (MaLop, TenLop) VALUES
('CNTT01', N'Công nghệ thông tin K01'),
('KT01',   N'Kế toán K01');

INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, MaLop) VALUES
('SV001', N'Nguyễn Minh Anh', '20050315', N'Nữ',  'CNTT01'),
('SV002', N'Trần Quốc Bảo',   '20050722', N'Nam', 'CNTT01'),
('SV003', N'Lê Thị Cẩm Tú',   '20051109', N'Nữ',  'KT01');

INSERT INTO DiemMon (MaSV, TenMon, SoTinChi, Diem) VALUES
('SV001', N'Lập trình trực quan',     3, 8.50),
('SV001', N'Cơ sở dữ liệu',           3, 7.50),
('SV001', N'Tiếng Anh chuyên ngành',  2, 9.00),
('SV001', N'Toán rời rạc',            3, 6.50),
('SV002', N'Lập trình trực quan',     3, 9.00),
('SV002', N'Cơ sở dữ liệu',           3, 8.00),
('SV002', N'Tiếng Anh chuyên ngành',  2, 8.50);
-- SV003: chưa có điểm môn nào
GO

-- Kiểm tra nhanh
SELECT * FROM Lop;
SELECT * FROM SinhVien;
SELECT * FROM DiemMon;
GO
