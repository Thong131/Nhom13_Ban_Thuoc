use master
Create Database QLThuocSo1VN;
use QLThuocSo1VN;
go

DROP DATABASE QlNhaThuoc;


CREATE TABLE [dbo].[DanhMuc](
	[maDM] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[tenDM] [nvarchar](100) NOT NULL,
);



CREATE TABLE NhaCungCap (
    MaNhaCungCap INT PRIMARY KEY IDENTITY(1,1),   
    TenNhaCungCap NVARCHAR(100) NOT NULL,        
    SDT CHAR(10),                             
    DiaChi NVARCHAR(255),                         
	Email NVARCHAR(100)
);

CREATE TABLE [dbo].[Benh](
	[maBenh] [int] IDENTITY(1,1) NOT NULL Primary key,
	[tenBenh] [nvarchar](700) NOT NULL,
	[moTa] [nvarchar](900) NOT NULL,
);

CREATE TABLE KhuyenMai
(
  MaKhuyenMai INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
  GiaTri int not null,
  ThoiGianBatDau datetime not null default getdate(),
  ThoiGianKetThuc datetime not null default getdate(),
  TrangThai bit not null default 0,
  NgayTao datetime not null default getdate(),
  DieuKienApDung int default 0,
  SoLuong int not null default 1
);


CREATE TABLE [dbo].[SanPham](
	[maSP] [int] IDENTITY(1,1) NOT NULL Primary key,
	[tenSP] [nvarchar](700) NOT NULL,
	[maBenh] [int] NOT NULL,
	[MaNhaCungCap] INT NOT NULL,
	[thanhPhan] [nvarchar](700) NOT NULL,
	[giaTien] [float] NOT NULL,
	[donVi] [float] NOT NULL,
	[hansuDung] [int] NULL,
	[chitietSP] [nvarchar](1000) NULL,
	[maDM] [int] NOT NULL,
	MaKhuyenMai INT NOT NULL,
	[soLuong] [int] NULL,
	[hinhAnh1] [nvarchar](700) NULL,
	[hinhAnh2] [nvarchar](700) NULL,
	[hinhAnh3] [nvarchar](700) NULL,
	[hinhAnh4] [nvarchar](700) NULL,
	FOREIGN KEY (MaNhaCungCap) REFERENCES NhaCungCap(MaNhaCungCap),
	FOREIGN KEY ([maBenh]) REFERENCES Benh([maBenh]),
	FOREIGN KEY (MaKhuyenMai) REFERENCES KhuyenMai(MaKhuyenMai),
	FOREIGN KEY (maDM) REFERENCES DanhMuc(maDM)
);

CREATE TABLE [dbo].[PhanQuyen](
	[roleID] [int] IDENTITY(1,1) NOT NULL Primary key,
	[roleName] [nvarchar](20) NOT NULL,
);
GO

CREATE TABLE [dbo].[NguoiDung](
    [maNguoiDung] [int] IDENTITY(1,1) NOT NULL Primary key,
	[username] [nvarchar](200) NOT NULL,
	[hoTen] [nvarchar](200) NOT NULL,
	[email] [nvarchar](200) NOT NULL,
	[sdt] [nvarchar](200) NOT NULL,
	[matkhau] [nvarchar](200) NOT NULL,
	[roleID] [int] NOT NULL,
	FOREIGN KEY (roleID) REFERENCES PhanQuyen(roleID)
);

CREATE TABLE [dbo].[DonHang](
	[maDH] [nvarchar](255) NOT NULL Primary key,
	[username] [nvarchar](200) NOT NULL,
	[diachi] [nvarchar](700) NOT NULL,
	[tongTien] [float] NOT NULL,
	[soLuong] [int] NOT NULL,
	[trangThai] [nvarchar](700) NOT NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
	  [maNguoiDung] [int],
	  FOREIGN KEY ([maNguoiDung]) REFERENCES [NguoiDung]([maNguoiDung])     
	,
);

CREATE TABLE ThanhToan
(
    MaThanhToan INT IDENTITY(1,1) PRIMARY KEY, 
    maDH [nvarchar](255) NOT NULL,                      
    PhuongThucThanhToan NVARCHAR(50) NOT NULL,   
	NgayThanhToan DATETIME DEFAULT GETDATE(),    
    TongTien FLOAT NOT NULL,                     
    TrangThaiThanhToan bit not null default 0, 
    FOREIGN KEY (maDH) REFERENCES DonHang(maDH)     
);

CREATE TABLE BinhLuan
(
  MaBinhLuan INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
  maSP INT NOT NULL,
  MaNguoiDung INT NOT NULL,
  NoiDung NVARCHAR(MAX) NOT NULL,
  NgayBinhLuan DATETIME NOT NULL DEFAULT GETDATE(),
  FOREIGN KEY (maSP) REFERENCES SanPham(maSP),
  FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);
CREATE TABLE DanhGia
(
  MaDanhGia INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
  MaSanPham INT NOT NULL,
  MaNguoiDung INT NOT NULL,
  NoiDung NVARCHAR(MAX) NOT NULL,
  NgayBinhLuan DATETIME NOT NULL DEFAULT GETDATE(),
  FOREIGN KEY (MaSanPham) REFERENCES SanPham(maSP),
  FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);



CREATE TABLE [dbo].[HinhAnh](
	[id] [int] IDENTITY(1,1) NOT NULL Primary key,
	[urlHinh] [nvarchar](2000) NOT NULL,
	[maSP] [int] NULL,
	FOREIGN KEY (maSP) REFERENCES SanPham(maSP)
);


CREATE TABLE [dbo].[ChiTietDonHang](
	[id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[maDH] [nvarchar](255) NOT NULL,
	[maSP] [int] NOT NULL,
	[soLuong] [int] NOT NULL,
	[tongTien] [int] NOT NULL,
	FOREIGN KEY ([maSP]) REFERENCES SanPham(maSP),
		FOREIGN KEY (maDH) REFERENCES DonHang(maDH)

);

CREATE TABLE [dbo].[ChiTietGioHang](
	[id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[maGioHang] [int] NULL,
	[soLuongSP] [int] NULL,
	[maSP] [int] NULL,
	[tongTien] [int] NULL,
	FOREIGN KEY (maSP) REFERENCES SanPham(maSP)

);

CREATE TABLE [dbo].[GioHang](
	[maGioHang] [int] IDENTITY(1,1) NOT NULL Primary key,
	[maNguoiDung] [int] NULL,
	[soLuong] [int] NULL,
	FOREIGN KEY (maNguoiDung) REFERENCES NguoiDung(maNguoiDung)
);


INSERT INTO DanhMuc (tenDM) VALUES
('Thuốc cảm cúm'),
('Thuốc kháng sinh'),
('Thuốc bổ sung vitamin'),
('Thuốc giảm đau');

INSERT INTO NhaCungCap (TenNhaCungCap, SDT, DiaChi, Email) VALUES
('Công ty Dược A', '0123456789', '123 Đường A, TP.HCM', 'contact@duoca.com'),
('Công ty Dược B', '0987654321', '456 Đường B, Hà Nội', 'contact@duocb.com');

INSERT INTO Benh (tenBenh, moTa) VALUES
('Cảm cúm', 'Bệnh do virus gây ra với các triệu chứng như sốt, ho, đau đầu'),
('Viêm phổi', 'Bệnh nhiễm trùng ở phổi gây ra bởi vi khuẩn hoặc virus');

INSERT INTO KhuyenMai (GiaTri, ThoiGianBatDau, ThoiGianKetThuc, TrangThai, NgayTao, DieuKienApDung, SoLuong) VALUES
(10, '2024-10-01', '2024-10-15', 1, GETDATE(), 100000, 50),
(20, '2024-11-01', '2024-11-30', 1, GETDATE(), 200000, 30);

INSERT INTO SanPham (tenSP, maBenh, MaNhaCungCap, thanhPhan, giaTien, donVi, hansuDung, chitietSP, maDM, MaKhuyenMai, soLuong, hinhAnh1, hinhAnh2, hinhAnh3, hinhAnh4) VALUES
('Thuốc giảm đau A', 1, 1, 'Paracetamol', 50000, 1, 24, 'Thuốc dùng để giảm đau', 1, 1, 100, 'img1.png', 'img2.png', NULL, NULL),
('Thuốc kháng sinh B', 2, 2, 'Amoxicillin', 100000, 1, 12, 'Thuốc kháng sinh phổ rộng', 2, 2, 200, 'img3.png', 'img4.png', NULL, NULL);

INSERT INTO PhanQuyen (roleName) VALUES
('Admin'),
('Khách hàng');

INSERT INTO NguoiDung (username, hoTen, email, sdt, matkhau, roleID) VALUES
('user01', 'Nguyen Van A', 'user01@example.com', '0912345678', 'password123', 1),
('user02', 'Le Thi B', 'user02@example.com', '0987654321', 'password456', 2);

INSERT INTO DonHang (maDH, username, diachi, tongTien, soLuong, trangThai, createdAt, updatedAt, maNguoiDung) VALUES
('DH001', 'user01', '123 Đường ABC, TP.HCM', 150000, 3, 'Đang xử lý', GETDATE(), NULL, 1),
('DH002', 'user02', '456 Đường XYZ, Hà Nội', 200000, 2, 'Hoàn thành', GETDATE(), NULL, 2);

INSERT INTO ThanhToan (maDH, PhuongThucThanhToan, TongTien, TrangThaiThanhToan) VALUES
('DH001', 'MoMo', 150000, 1),
('DH002', 'ZaloPay', 200000, 1);

INSERT INTO BinhLuan (maSP, MaNguoiDung, NoiDung, NgayBinhLuan) VALUES
(1, 1, 'Sản phẩm rất tốt, chất lượng', GETDATE()),
(2, 2, 'Thuốc hiệu quả sau khi sử dụng', GETDATE());

INSERT INTO DanhGia (MaSanPham, MaNguoiDung, NoiDung, NgayBinhLuan) VALUES
(1, 1, 'Đánh giá 5 sao', GETDATE()),
(2, 2, 'Đánh giá 4 sao', GETDATE());

INSERT INTO HinhAnh (urlHinh, maSP) VALUES
('img1.png', 1),
('img2.png', 2);

INSERT INTO ChiTietDonHang (maDH, maSP, soLuong, tongTien) VALUES
('DH001', 1, 2, 100000),
('DH002', 2, 1, 100000);

INSERT INTO ChiTietGioHang (maGioHang, soLuongSP, maSP, tongTien) VALUES
(1, 2, 1, 100000),
(2, 1, 2, 100000);

INSERT INTO GioHang (maNguoiDung, soLuong) VALUES
(1, 2),
(2, 1);

