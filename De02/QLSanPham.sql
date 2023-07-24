/*
Created		24/07/2023
Modified		24/07/2023
Project		
Model			
Company		
Author		
Version		
Database		MS SQL 2005 
*/
CREATE DATABASE QLSanPham
GO
USE  QLSanPham
GO

Drop table [SanPham] 
go
Drop table [LoaiSP] 
go


Create table [LoaiSP]
(
	[MaLoai] Char(2) NOT NULL,
	[TenLoai] Nvarchar(30) NULL,
Primary Key ([MaLoai])
) 
go

Create table [SanPham]
(
	[MaSP] Char(6) NOT NULL,
	[TenSP] Nvarchar(30) NULL,
	[Ngaynhap] Datetime NULL,
	[MaLoai] Char(2) NOT NULL,
Primary Key ([MaSP])
) 
go


Alter table [SanPham] add  foreign key([MaLoai]) references [LoaiSP] ([MaLoai])  on update no action on delete no action 
go


Set quoted_identifier on
go


Set quoted_identifier off
go


