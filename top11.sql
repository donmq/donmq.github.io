-- Database export via SQLPro (https://www.sqlprostudio.com/)
-- Exported by admin at 18-12-2024 23:22.
-- WARNING: This file may contain descructive statements such as DROPs.
-- Please ensure that you are running the script at the proper location.


-- BEGIN TABLE dbo.Attributes
IF OBJECT_ID('dbo.Attributes', 'U') IS NOT NULL
DROP TABLE dbo.Attributes;
GO

CREATE TABLE dbo.Attributes (
	ID int NOT NULL IDENTITY(1,1),
	Name nvarchar(50) NULL
);
GO

ALTER TABLE dbo.Attributes ADD CONSTRAINT PK_Attributes PRIMARY KEY (ID);
GO

-- Inserting 2 rows into dbo.Attributes
SET IDENTITY_INSERT dbo.Attributes ON

-- Insert batch #1
INSERT INTO dbo.Attributes (ID, Name) VALUES
(1, 'ATK'),
(2, NULL);

SET IDENTITY_INSERT dbo.Attributes OFF

-- END TABLE dbo.Attributes

-- BEGIN TABLE dbo.Compares
IF OBJECT_ID('dbo.Compares', 'U') IS NOT NULL
DROP TABLE dbo.Compares;
GO

CREATE TABLE dbo.Compares (
	InforID int NOT NULL,
	PlanID int NOT NULL,
	ChatLuong int NULL,
	CanPha int NULL,
	KemNguoi int NULL,
	ChayCho int NULL,
	DanhDau int NULL,
	DungCam int NULL,
	ChuyenBong int NULL,
	ReBong int NULL,
	TatCanh int NULL,
	SutManh int NULL,
	DutDiem int NULL,
	TheLuc int NULL,
	SucManh int NULL,
	XongXao int NULL,
	TocDo int NULL,
	SangTao int NULL
);
GO

ALTER TABLE dbo.Compares ADD CONSTRAINT PK_Compares PRIMARY KEY (InforID, PlanID);
GO

-- Inserting 8 rows into dbo.Compares
-- Insert batch #1
INSERT INTO dbo.Compares (InforID, PlanID, ChatLuong, CanPha, KemNguoi, ChayCho, DanhDau, DungCam, ChuyenBong, ReBong, TatCanh, SutManh, DutDiem, TheLuc, SucManh, XongXao, TocDo, SangTao) VALUES
(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1),
(1, 2, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100),
(33, 1, 180, 64, 85, 213, 134, 72, 295, 257, 309, 195, 237, 231, 61, 27, 248, 275),
(33, 2, 180, 72, 108, 192, 130, 89, 295, 300, 299, 185, 231, 200, 76, 44, 217, 275),
(33, 3, 180, 66, 100, 207, 130, 72, 295, 271, 301, 187, 203, 232, 60, 40, 275, 275),
(33, 4, 180, 66, 84, 207, 127, 72, 295, 271, 296, 182, 229, 232, 60, 40, 275, 275),
(33, 5, 180, 66, 83, 207, 127, 72, 295, 271, 293, 179, 230, 232, 60, 43, 280, 275),
(33, 6, 180, 69, 80, 212, 124, 72, 300, 270, 290, 176, 230, 235, 59, 44, 275, 275);

-- END TABLE dbo.Compares

-- BEGIN TABLE dbo.ExerciseAttributes
IF OBJECT_ID('dbo.ExerciseAttributes', 'U') IS NOT NULL
DROP TABLE dbo.ExerciseAttributes;
GO

CREATE TABLE dbo.ExerciseAttributes (
	ExerciseID int NOT NULL,
	AttributeID int NOT NULL
);
GO

ALTER TABLE dbo.ExerciseAttributes ADD CONSTRAINT PK_ExerciseAttributes PRIMARY KEY (ExerciseID, AttributeID);
GO

-- Inserting 97 rows into dbo.ExerciseAttributes
-- Insert batch #1
INSERT INTO dbo.ExerciseAttributes (ExerciseID, AttributeID) VALUES
(1, 1),
(1, 7),
(1, 10),
(2, 6),
(2, 9),
(2, 14),
(3, 2),
(3, 4),
(3, 8),
(3, 9),
(4, 9),
(4, 10),
(4, 12),
(5, 6),
(5, 7),
(5, 11),
(5, 14),
(6, 4),
(6, 8),
(6, 9),
(6, 10),
(7, 6),
(7, 8),
(7, 10),
(7, 15),
(8, 3),
(8, 5),
(8, 15),
(9, 3),
(9, 4),
(9, 6),
(9, 15),
(10, 2),
(10, 3),
(11, 1),
(11, 2),
(11, 5),
(11, 7),
(11, 12),
(12, 2),
(12, 4),
(12, 5),
(12, 8),
(13, 1),
(13, 2),
(13, 3),
(13, 5),
(13, 13),
(15, 4),
(15, 7),
(15, 15),
(16, 1),
(16, 3),
(16, 6),
(16, 11),
(16, 13),
(17, 6),
(17, 7),
(17, 11),
(18, 3),
(18, 6),
(18, 8),
(18, 14),
(18, 15),
(19, 3),
(19, 11),
(19, 14),
(20, 2),
(20, 5),
(20, 7),
(20, 12),
(20, 13),
(21, 3),
(21, 6),
(21, 10),
(21, 15),
(22, 4),
(22, 11),
(22, 13),
(23, 11),
(23, 12),
(23, 14),
(24, 13),
(24, 14),
(25, 11),
(25, 14),
(26, 5),
(26, 12),
(26, 14),
(27, 5),
(27, 13),
(27, 14),
(28, 11),
(28, 12),
(29, 7),
(29, 11),
(29, 14);

-- END TABLE dbo.ExerciseAttributes

-- BEGIN TABLE dbo.Exercises
IF OBJECT_ID('dbo.Exercises', 'U') IS NOT NULL
DROP TABLE dbo.Exercises;
GO

CREATE TABLE dbo.Exercises (
	ID int NOT NULL,
	Name nvarchar(50) NULL,
	[Difficult_Level ] int NULL,
	Class varchar(10) NULL
);
GO

ALTER TABLE dbo.Exercises ADD CONSTRAINT PK_Exercises PRIMARY KEY (ID);
GO

-- Inserting 29 rows into dbo.Exercises
-- Insert batch #1
INSERT INTO dbo.Exercises (ID, Name, [Difficult_Level ], Class) VALUES
(1, 'Dứt điểm đối mặt', 2, 'TC'),
(2, 'Chuyền, chạy và sút', 2, 'TC'),
(3, 'Phản công đá phạt', 3, 'TC'),
(4, 'Kỹ Thuật Sút', 3, 'TC'),
(5, 'Rê vượt ngại vật', 4, 'TC'),
(6, 'Lối chơi biên', 4, 'TC'),
(7, 'Phản công nhanh', 5, 'TC'),
(8, 'Phân tích video', 1, 'PT'),
(9, 'Sử dụng đầu', 2, 'PT'),
(10, 'Một hàng hậu vệ', 3, 'PT'),
(11, 'Ngăn C.Thủ tấn công', 3, 'PT'),
(12, 'Chặn đường chuyền', 3, 'PT'),
(13, 'Lối chơi áp sát', 4, 'PT'),
(14, 'Huấn luyện thủ môn', 4, 'PT'),
(15, 'Kiểm soát bóng', 1, 'KSB'),
(16, 'Chơi bóng ma', 2, 'KSB'),
(17, 'Chơi đỡ bước một', 2, 'KSB'),
(18, 'Đảo cánh nhanh', 3, 'KSB'),
(19, 'Theo đúng tuyến', 3, 'KSB'),
(20, 'Lối chơi chạm bóng', 3, 'KSB'),
(21, 'Chuyền trước khi sút', 4, 'KSB'),
(22, 'Khởi động', 1, 'TL'),
(23, 'Giãn cơ', 2, 'TL'),
(24, 'Chạy thang ngang', 2, 'TL'),
(25, 'Chạy dài', 3, 'TL'),
(26, 'Chạy con thoi', 4, 'TL'),
(27, 'Nhảy rào', 4, 'TL'),
(28, 'Thể dục', 5, 'TL'),
(29, 'Chạy nước rút', 5, 'TL');

-- END TABLE dbo.Exercises

-- BEGIN TABLE dbo.Information
IF OBJECT_ID('dbo.Information', 'U') IS NOT NULL
DROP TABLE dbo.Information;
GO

CREATE TABLE dbo.Information (
	ID int NOT NULL IDENTITY(1,1),
	Name nvarchar(50) NULL,
	YearOld nvarchar(50) NULL,
	Quality int NULL
);
GO

ALTER TABLE dbo.Information ADD CONSTRAINT PK_Information PRIMARY KEY (ID);
GO

-- Inserting 9 rows into dbo.Information
SET IDENTITY_INSERT dbo.Information ON

-- Insert batch #1
INSERT INTO dbo.Information (ID, Name, YearOld, Quality) VALUES
(1, 'Kylian Mpapé', '18', 0),
(33, 'Vini', '18', 0),
(34, 'DMC 9*', '18', NULL),
(35, 'MR 9*', '18', NULL),
(36, 'ML 9*', '18', NULL),
(39, 'Ronaldo', '18', 0),
(40, 'Ronaldo', '18', NULL),
(42, 'Vini', '18', 0),
(43, 'ST', '18', NULL);

SET IDENTITY_INSERT dbo.Information OFF

-- END TABLE dbo.Information

-- BEGIN TABLE dbo.MainAttributes
IF OBJECT_ID('dbo.MainAttributes', 'U') IS NOT NULL
DROP TABLE dbo.MainAttributes;
GO

CREATE TABLE dbo.MainAttributes (
	ID int NOT NULL IDENTITY(1,1),
	Name nvarchar(50) NULL,
	[Type] int NULL
);
GO

ALTER TABLE dbo.MainAttributes ADD CONSTRAINT PK_MainAttributes PRIMARY KEY (ID);
GO

-- Inserting 15 rows into dbo.MainAttributes
SET IDENTITY_INSERT dbo.MainAttributes ON

-- Insert batch #1
INSERT INTO dbo.MainAttributes (ID, Name, [Type]) VALUES
(1, 'canPha', 1),
(2, 'kemNguoi', 1),
(3, 'chayCho', 1),
(4, 'danhDau', 1),
(5, 'dungCam', 1),
(6, 'chuyenBong', 2),
(7, 'reBong', 2),
(8, 'tatCanh', 2),
(9, 'sutManh', 2),
(10, 'dutDiem', 2),
(11, 'theLuc', 3),
(12, 'sucManh', 3),
(13, 'xongXao', 3),
(14, 'tocDo', 3),
(15, 'sangTao', 3);

SET IDENTITY_INSERT dbo.MainAttributes OFF

-- END TABLE dbo.MainAttributes

-- BEGIN TABLE dbo.PlayerValue
IF OBJECT_ID('dbo.PlayerValue', 'U') IS NOT NULL
DROP TABLE dbo.PlayerValue;
GO

CREATE TABLE dbo.PlayerValue (
	Q int NOT NULL IDENTITY(1,1),
	[18Year] decimal(5,2) NULL,
	[19Year] decimal(5,2) NULL,
	[20Year] decimal(5,2) NULL,
	[21Year] decimal(5,2) NULL
);
GO

-- Inserting 51 rows into dbo.PlayerValue
SET IDENTITY_INSERT dbo.PlayerValue ON

-- Insert batch #1
INSERT INTO dbo.PlayerValue (Q, [18Year], [19Year], [20Year], [21Year]) VALUES
(150, 110.6, 110.2, 109.9, 109.5),
(151, 110.7, 110.4, 110, 109.7),
(152, 110.8, 110.5, 110.1, 109.8),
(153, 111, 110.6, 110.2, 109.9),
(154, 111.1, 110.7, 110.4, 110),
(155, 111.2, 110.8, 110.5, 110.1),
(156, 111.3, 111, 110.6, 110.2),
(157, 111.4, 111.1, 110.7, 110.4),
(158, 111.5, 111.2, 110.8, 110.5),
(159, 111.6, 111.3, 111, 110.6),
(160, 111.8, 111.4, 111.1, 110.7),
(161, 111.9, 111.5, 111.2, 110.8),
(162, 112, 111.6, 111.3, 111),
(163, 112.2, 111.8, 111.4, 111.1),
(164, 112.4, 111.9, 111.5, 111.2),
(165, 112.6, 112, 111.6, 111.3),
(166, 112.8, 112.2, 111.8, 111.4),
(167, 113, 112.4, 111.9, 111.5),
(168, 113.2, 112.6, 112, 111.6),
(169, 113.4, 112.8, 112.2, 111.8),
(170, 113.6, 113, 112.4, 111.9),
(171, 113.8, 113.2, 112.6, 112),
(172, 114, 113.4, 112.8, 112.2),
(173, 114.2, 113.6, 113, 112.4),
(174, 114.4, 113.8, 113.2, 112.6),
(175, 114.6, 114, 113.4, 112.8),
(176, 114.8, 114.2, 113.6, 113),
(177, 115, 114.4, 113.8, 113.2),
(178, 115.2, 114.6, 114, 113.4),
(179, 115.4, 114.8, 114.2, 113.6),
(180, 115.6, 115, 114.4, 113.8),
(181, 115.8, 115.2, 114.6, 114),
(182, 116, 115.4, 114.8, 114.2),
(183, 116.2, 115.6, 115, 114.4),
(184, 116.4, 115.8, 115.2, 114.6),
(185, 116.6, 116, 115.4, 114.8),
(186, 116.8, 116.2, 115.6, 115),
(187, 117, 116.4, 115.8, 115.2),
(188, 117.2, 116.6, 116, 115.4),
(189, 117.4, 116.8, 116.2, 115.6),
(190, 117.6, 117, 116.4, 115.8),
(191, 117.8, 117.2, 116.6, 116),
(192, 118, 117.4, 116.8, 116.2),
(193, 118.2, 117.6, 117, 116.4),
(194, 118.4, 117.8, 117.2, 116.6),
(195, 118.6, 118, 117.4, 116.8),
(196, 118.8, 118.2, 117.6, 117),
(197, 119, 118.4, 117.8, 117.2),
(198, 119.2, 118.6, 118, 117.4),
(199, 119.4, 118.8, 118.2, 117.6),
(200, 119.6, 119, 118.4, 117.8);

SET IDENTITY_INSERT dbo.PlayerValue OFF

-- END TABLE dbo.PlayerValue

-- BEGIN TABLE dbo.Position
IF OBJECT_ID('dbo.[Position]', 'U') IS NOT NULL
DROP TABLE dbo.[Position];
GO

CREATE TABLE dbo.[Position] (
	ID int NOT NULL IDENTITY(1,1),
	Name nvarchar(50) NULL
);
GO

ALTER TABLE dbo.[Position] ADD CONSTRAINT PK_Position PRIMARY KEY (ID);
GO

-- Inserting 11 rows into dbo.[Position]
SET IDENTITY_INSERT dbo.[Position] ON

-- Insert batch #1
INSERT INTO dbo.[Position] (ID, Name) VALUES
(1, 'ST'),
(2, 'AML'),
(3, 'AMC'),
(4, 'AMR'),
(5, 'ML'),
(6, 'MC'),
(7, 'MR'),
(8, 'DMC'),
(9, 'DL'),
(10, 'DC'),
(11, 'DR');

SET IDENTITY_INSERT dbo.[Position] OFF

-- END TABLE dbo.Position

-- BEGIN TABLE dbo.PositionInformation
IF OBJECT_ID('dbo.PositionInformation', 'U') IS NOT NULL
DROP TABLE dbo.PositionInformation;
GO

CREATE TABLE dbo.PositionInformation (
	InforID int NOT NULL,
	PositionID int NOT NULL
);
GO

ALTER TABLE dbo.PositionInformation ADD CONSTRAINT PK_PositionInformation PRIMARY KEY (InforID, PositionID);
GO

-- Inserting 22 rows into dbo.PositionInformation
-- Insert batch #1
INSERT INTO dbo.PositionInformation (InforID, PositionID) VALUES
(1, 2),
(1, 3),
(1, 4),
(33, 2),
(33, 5),
(34, 8),
(34, 10),
(35, 4),
(35, 6),
(35, 7),
(36, 2),
(36, 5),
(36, 6),
(39, 1),
(39, 3),
(40, 1),
(40, 3),
(42, 2),
(42, 5),
(43, 1),
(43, 3),
(43, 6);

-- END TABLE dbo.PositionInformation

-- BEGIN TABLE dbo.QualityAfter
IF OBJECT_ID('dbo.QualityAfter', 'U') IS NOT NULL
DROP TABLE dbo.QualityAfter;
GO

CREATE TABLE dbo.QualityAfter (
	ID int NOT NULL,
	InforID int NOT NULL,
	PlanID int NOT NULL,
	ExerciseID int NOT NULL,
	Average int NULL,
	CanPha int NULL,
	KemNguoi int NULL,
	ChayCho int NULL,
	DanhDau int NULL,
	DungCam int NULL,
	ChuyenBong int NULL,
	ReBong int NULL,
	TatCanh int NULL,
	SutManh int NULL,
	DutDiem int NULL,
	TheLuc int NULL,
	SucManh int NULL,
	XongXao int NULL,
	TocDo int NULL,
	SangTao int NULL
);
GO

ALTER TABLE dbo.QualityAfter ADD CONSTRAINT PK_QualityAfter PRIMARY KEY (ID, InforID, PlanID);
GO

-- Inserting 1 row into dbo.QualityAfter
-- Insert batch #1
INSERT INTO dbo.QualityAfter (ID, InforID, PlanID, ExerciseID, Average, CanPha, KemNguoi, ChayCho, DanhDau, DungCam, ChuyenBong, ReBong, TatCanh, SutManh, DutDiem, TheLuc, SucManh, XongXao, TocDo, SangTao) VALUES
(1, 42, 1, 2, 180, 50, 61, 193, 95, 72, 291, 216, 230, 115, 180, 123, 26, 27, 135, 275);

-- END TABLE dbo.QualityAfter

-- BEGIN TABLE dbo.QualityBefore
IF OBJECT_ID('dbo.QualityBefore', 'U') IS NOT NULL
DROP TABLE dbo.QualityBefore;
GO

CREATE TABLE dbo.QualityBefore (
	InforID int NOT NULL,
	CanPha int NULL,
	KemNguoi int NULL,
	ChayCho int NULL,
	DanhDau int NULL,
	DungCam int NULL,
	ChuyenBong int NULL,
	ReBong int NULL,
	TatCanh int NULL,
	SutManh int NULL,
	DutDiem int NULL,
	TheLuc int NULL,
	SucManh int NULL,
	XongXao int NULL,
	TocDo int NULL,
	SangTao int NULL
);
GO

ALTER TABLE dbo.QualityBefore ADD CONSTRAINT PK_QualityBefore PRIMARY KEY (InforID);
GO

-- Inserting 9 rows into dbo.QualityBefore
-- Insert batch #1
INSERT INTO dbo.QualityBefore (InforID, CanPha, KemNguoi, ChayCho, DanhDau, DungCam, ChuyenBong, ReBong, TatCanh, SutManh, DutDiem, TheLuc, SucManh, XongXao, TocDo, SangTao) VALUES
(1, 43, 60, 86, 151, 75, 278, 247, 272, 313, 295, 223, 38, 1, 254, 234),
(33, 50, 61, 193, 95, 72, 297, 216, 230, 121, 180, 123, 26, 27, 141, 275),
(34, 220, 197, 173, 243, 182, 262, 88, 66, 100, 121, 192, 210, 150, 148, 202),
(35, 63, 186, 207, 45, 183, 205, 256, 227, 206, 192, 209, 93, 81, 239, 145),
(36, 50, 88, 219, 17, 135, 231, 262, 267, 222, 250, 218, 64, 42, 305, 159),
(39, 1, 47, 218, 188, 29, 257, 244, 31, 390, 410, 114, 221, 11, 236, 204),
(40, 1, 47, 218, 188, 29, 257, 244, 31, 390, 410, 114, 221, 11, 236, 204),
(42, 50, 61, 193, 95, 72, 297, 216, 230, 121, 180, 123, 26, 27, 141, 275),
(43, 87, 101, 250, 211, 132, 187, 246, 21, 327, 366, 163, 202, 36, 254, 131);

-- END TABLE dbo.QualityBefore

-- BEGIN TABLE dbo.TypeAttributes
IF OBJECT_ID('dbo.TypeAttributes', 'U') IS NOT NULL
DROP TABLE dbo.TypeAttributes;
GO

CREATE TABLE dbo.TypeAttributes (
	PositionID int NOT NULL,
	AttributeID int NOT NULL,
	[Disable] bit NOT NULL
);
GO

-- Inserting 165 rows into dbo.TypeAttributes
-- Insert batch #1
INSERT INTO dbo.TypeAttributes (PositionID, AttributeID, [Disable]) VALUES
(1, 1, 0),
(1, 2, 0),
(1, 3, 1),
(1, 4, 1),
(1, 5, 0),
(1, 6, 1),
(1, 7, 1),
(1, 8, 0),
(1, 9, 1),
(1, 10, 1),
(1, 11, 0),
(1, 12, 1),
(1, 13, 0),
(1, 14, 1),
(1, 15, 1),
(2, 1, 0),
(2, 2, 0),
(2, 3, 0),
(2, 4, 0),
(2, 5, 0),
(2, 6, 1),
(2, 7, 1),
(2, 8, 1),
(2, 9, 1),
(2, 10, 1),
(2, 11, 1),
(2, 12, 0),
(2, 13, 0),
(2, 14, 1),
(2, 15, 1),
(3, 1, 0),
(3, 2, 0),
(3, 3, 0),
(3, 4, 1),
(3, 5, 0),
(3, 6, 1),
(3, 7, 1),
(3, 8, 0),
(3, 9, 1),
(3, 10, 1),
(3, 11, 1),
(3, 12, 0),
(3, 13, 0),
(3, 14, 1),
(3, 15, 1),
(4, 1, 0),
(4, 2, 0),
(4, 3, 0),
(4, 4, 0),
(4, 5, 0),
(4, 6, 1),
(4, 7, 1),
(4, 8, 1),
(4, 9, 1),
(4, 10, 1),
(4, 11, 1),
(4, 12, 0),
(4, 13, 0),
(4, 14, 1),
(4, 15, 1),
(5, 1, 0),
(5, 2, 0),
(5, 3, 1),
(5, 4, 0),
(5, 5, 0),
(5, 6, 1),
(5, 7, 1),
(5, 8, 1),
(5, 9, 0),
(5, 10, 0),
(5, 11, 1),
(5, 12, 0),
(5, 13, 0),
(5, 14, 1),
(5, 15, 1),
(6, 1, 1),
(6, 2, 1),
(6, 3, 1),
(6, 4, 0),
(6, 5, 1),
(6, 6, 1),
(6, 7, 1),
(6, 8, 0),
(6, 9, 1),
(6, 10, 0),
(6, 11, 1),
(6, 12, 0),
(6, 13, 0),
(6, 14, 1),
(6, 15, 1),
(7, 1, 0),
(7, 2, 0),
(7, 3, 1),
(7, 4, 0),
(7, 5, 0),
(7, 6, 1),
(7, 7, 1),
(7, 8, 1),
(7, 9, 0),
(7, 10, 0),
(7, 11, 1),
(7, 12, 0),
(7, 13, 0),
(7, 14, 1),
(7, 15, 1),
(8, 1, 1),
(8, 2, 1),
(8, 3, 1),
(8, 4, 1),
(8, 5, 1),
(8, 6, 1),
(8, 7, 0),
(8, 8, 0),
(8, 9, 0),
(8, 10, 0),
(8, 11, 1),
(8, 12, 1),
(8, 13, 1),
(8, 14, 0),
(8, 15, 1),
(9, 1, 1),
(9, 2, 1),
(9, 3, 1),
(9, 4, 0),
(9, 5, 1),
(9, 6, 0),
(9, 7, 0),
(9, 8, 1),
(9, 9, 0),
(9, 10, 0),
(9, 11, 1),
(9, 12, 0),
(9, 13, 1),
(9, 14, 1),
(9, 15, 0),
(10, 1, 1),
(10, 2, 1),
(10, 3, 1),
(10, 4, 1),
(10, 5, 1),
(10, 6, 0),
(10, 7, 0),
(10, 8, 0),
(10, 9, 0),
(10, 10, 0),
(10, 11, 1),
(10, 12, 1),
(10, 13, 1),
(10, 14, 0),
(10, 15, 0),
(11, 1, 1),
(11, 2, 1),
(11, 3, 1),
(11, 4, 0),
(11, 5, 1),
(11, 6, 0),
(11, 7, 0),
(11, 8, 1),
(11, 9, 0),
(11, 10, 0),
(11, 11, 1),
(11, 12, 0),
(11, 13, 1),
(11, 14, 1),
(11, 15, 0);

-- END TABLE dbo.TypeAttributes

