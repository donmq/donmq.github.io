USE [master]
GO
/****** Object:  Database [Top11]    Script Date: 11/12/2024 04:22:50 PM ******/
CREATE DATABASE [Top11]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Top11', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\Top11.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Top11_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\Top11_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Top11] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Top11].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Top11] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Top11] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Top11] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Top11] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Top11] SET ARITHABORT OFF 
GO
ALTER DATABASE [Top11] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Top11] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Top11] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Top11] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Top11] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Top11] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Top11] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Top11] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Top11] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Top11] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Top11] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Top11] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Top11] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Top11] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Top11] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Top11] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Top11] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Top11] SET RECOVERY FULL 
GO
ALTER DATABASE [Top11] SET  MULTI_USER 
GO
ALTER DATABASE [Top11] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Top11] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Top11] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Top11] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Top11] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Top11] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Top11', N'ON'
GO
ALTER DATABASE [Top11] SET QUERY_STORE = ON
GO
ALTER DATABASE [Top11] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Top11]
GO
/****** Object:  User [sa]    Script Date: 11/12/2024 04:22:51 PM ******/
CREATE USER [sa] FOR LOGIN [sa1] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Attributes]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attributes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Attributes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compares]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compares](
	[InforID] [int] NOT NULL,
	[PlanID] [int] NOT NULL,
	[ChatLuong] [int] NULL,
	[CanPha] [int] NULL,
	[KemNguoi] [int] NULL,
	[ChayCho] [int] NULL,
	[DanhDau] [int] NULL,
	[DungCam] [int] NULL,
	[ChuyenBong] [int] NULL,
	[ReBong] [int] NULL,
	[TatCanh] [int] NULL,
	[SutManh] [int] NULL,
	[DutDiem] [int] NULL,
	[TheLuc] [int] NULL,
	[SucManh] [int] NULL,
	[XongXao] [int] NULL,
	[TocDo] [int] NULL,
	[SangTao] [int] NULL,
 CONSTRAINT [PK_Compares] PRIMARY KEY CLUSTERED 
(
	[InforID] ASC,
	[PlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExerciseAttributes]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExerciseAttributes](
	[ExerciseID] [int] NOT NULL,
	[AttributeID] [int] NOT NULL,
 CONSTRAINT [PK_ExerciseAttributes] PRIMARY KEY CLUSTERED 
(
	[ExerciseID] ASC,
	[AttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exercises]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exercises](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Difficult_Level ] [int] NULL,
	[Class] [varchar](10) NULL,
 CONSTRAINT [PK_Exercises] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Information]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Information](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[YearOld] [nvarchar](50) NULL,
	[Quality] [int] NULL,
 CONSTRAINT [PK_Information] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MainAttributes]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MainAttributes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [int] NULL,
 CONSTRAINT [PK_MainAttributes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerValue]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerValue](
	[Q] [int] IDENTITY(1,1) NOT NULL,
	[18Year] [decimal](5, 2) NULL,
	[19Year] [decimal](5, 2) NULL,
	[20Year] [decimal](5, 2) NULL,
	[21Year] [decimal](5, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Position]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Position](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PositionInformation]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PositionInformation](
	[InforID] [int] NOT NULL,
	[PositionID] [int] NOT NULL,
 CONSTRAINT [PK_PositionInformation] PRIMARY KEY CLUSTERED 
(
	[InforID] ASC,
	[PositionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QualityAfter]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QualityAfter](
	[ID] [int] NOT NULL,
	[InforID] [int] NOT NULL,
	[PlanID] [int] NOT NULL,
	[ExerciseID] [int] NOT NULL,
	[Average] [int] NULL,
	[CanPha] [int] NULL,
	[KemNguoi] [int] NULL,
	[ChayCho] [int] NULL,
	[DanhDau] [int] NULL,
	[DungCam] [int] NULL,
	[ChuyenBong] [int] NULL,
	[ReBong] [int] NULL,
	[TatCanh] [int] NULL,
	[SutManh] [int] NULL,
	[DutDiem] [int] NULL,
	[TheLuc] [int] NULL,
	[SucManh] [int] NULL,
	[XongXao] [int] NULL,
	[TocDo] [int] NULL,
	[SangTao] [int] NULL,
 CONSTRAINT [PK_QualityAfter] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[InforID] ASC,
	[PlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QualityBefore]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QualityBefore](
	[InforID] [int] NOT NULL,
	[CanPha] [int] NULL,
	[KemNguoi] [int] NULL,
	[ChayCho] [int] NULL,
	[DanhDau] [int] NULL,
	[DungCam] [int] NULL,
	[ChuyenBong] [int] NULL,
	[ReBong] [int] NULL,
	[TatCanh] [int] NULL,
	[SutManh] [int] NULL,
	[DutDiem] [int] NULL,
	[TheLuc] [int] NULL,
	[SucManh] [int] NULL,
	[XongXao] [int] NULL,
	[TocDo] [int] NULL,
	[SangTao] [int] NULL,
 CONSTRAINT [PK_QualityBefore] PRIMARY KEY CLUSTERED 
(
	[InforID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeAttributes]    Script Date: 11/12/2024 04:22:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeAttributes](
	[PositionID] [int] NOT NULL,
	[AttributeID] [int] NOT NULL,
	[Disable] [bit] NOT NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Attributes] ON 

INSERT [dbo].[Attributes] ([ID], [Name]) VALUES (1, N'ATK')
INSERT [dbo].[Attributes] ([ID], [Name]) VALUES (2, NULL)
SET IDENTITY_INSERT [dbo].[Attributes] OFF
GO
INSERT [dbo].[Compares] ([InforID], [PlanID], [ChatLuong], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)
INSERT [dbo].[Compares] ([InforID], [PlanID], [ChatLuong], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (1, 2, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100)
INSERT [dbo].[Compares] ([InforID], [PlanID], [ChatLuong], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (33, 1, 180, 64, 85, 213, 134, 72, 295, 257, 309, 195, 237, 231, 61, 27, 248, 275)
INSERT [dbo].[Compares] ([InforID], [PlanID], [ChatLuong], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (33, 2, 180, 72, 108, 192, 130, 89, 295, 300, 299, 185, 231, 200, 76, 44, 217, 275)
INSERT [dbo].[Compares] ([InforID], [PlanID], [ChatLuong], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (33, 3, 180, 66, 100, 207, 130, 72, 295, 271, 301, 187, 203, 232, 60, 40, 275, 275)
INSERT [dbo].[Compares] ([InforID], [PlanID], [ChatLuong], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (33, 4, 180, 66, 84, 207, 127, 72, 295, 271, 296, 182, 229, 232, 60, 40, 275, 275)
INSERT [dbo].[Compares] ([InforID], [PlanID], [ChatLuong], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (33, 5, 180, 66, 83, 207, 127, 72, 295, 271, 293, 179, 230, 232, 60, 43, 280, 275)
INSERT [dbo].[Compares] ([InforID], [PlanID], [ChatLuong], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (33, 6, 180, 69, 80, 212, 124, 72, 300, 270, 290, 176, 230, 235, 59, 44, 275, 275)
GO
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (1, 1)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (1, 7)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (1, 10)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (2, 6)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (2, 9)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (2, 14)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (3, 2)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (3, 4)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (3, 8)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (3, 9)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (4, 9)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (4, 10)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (4, 12)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (5, 6)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (5, 7)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (5, 11)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (5, 14)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (6, 4)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (6, 8)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (6, 9)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (6, 10)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (7, 6)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (7, 8)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (7, 10)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (7, 15)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (8, 3)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (8, 5)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (8, 15)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (9, 3)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (9, 4)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (9, 6)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (9, 15)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (10, 2)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (10, 3)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (11, 1)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (11, 2)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (11, 5)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (11, 7)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (11, 12)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (12, 2)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (12, 4)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (12, 5)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (12, 8)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (13, 1)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (13, 2)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (13, 3)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (13, 5)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (13, 13)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (15, 4)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (15, 7)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (15, 15)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (16, 1)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (16, 3)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (16, 6)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (16, 11)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (16, 13)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (17, 6)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (17, 7)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (17, 11)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (18, 3)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (18, 6)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (18, 8)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (18, 14)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (18, 15)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (19, 3)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (19, 11)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (19, 14)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (20, 2)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (20, 5)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (20, 7)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (20, 12)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (20, 13)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (21, 3)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (21, 6)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (21, 10)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (21, 15)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (22, 4)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (22, 11)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (22, 13)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (23, 11)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (23, 12)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (23, 14)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (24, 13)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (24, 14)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (25, 11)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (25, 14)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (26, 5)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (26, 12)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (26, 14)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (27, 5)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (27, 13)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (27, 14)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (28, 11)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (28, 12)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (29, 7)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (29, 11)
INSERT [dbo].[ExerciseAttributes] ([ExerciseID], [AttributeID]) VALUES (29, 14)
GO
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (1, N'Dứt điểm đối mặt', 2, N'TC')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (2, N'Chuyền, chạy và sút', 2, N'TC')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (3, N'Phản công đá phạt', 3, N'TC')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (4, N'Kỹ Thuật Sút', 3, N'TC')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (5, N'Rê vượt ngại vật', 4, N'TC')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (6, N'Lối chơi biên', 4, N'TC')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (7, N'Phản công nhanh', 5, N'TC')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (8, N'Phân tích video', 1, N'PT')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (9, N'Sử dụng đầu', 2, N'PT')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (10, N'Một hàng hậu vệ', 3, N'PT')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (11, N'Ngăn C.Thủ tấn công', 3, N'PT')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (12, N'Chặn đường chuyền', 3, N'PT')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (13, N'Lối chơi áp sát', 4, N'PT')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (14, N'Huấn luyện thủ môn', 4, N'PT')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (15, N'Kiểm soát bóng', 1, N'KSB')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (16, N'Chơi bóng ma', 2, N'KSB')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (17, N'Chơi đỡ bước một', 2, N'KSB')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (18, N'Đảo cánh nhanh', 3, N'KSB')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (19, N'Theo đúng tuyến', 3, N'KSB')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (20, N'Lối chơi chạm bóng', 3, N'KSB')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (21, N'Chuyền trước khi sút', 4, N'KSB')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (22, N'Khởi động', 1, N'TL')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (23, N'Giãn cơ', 2, N'TL')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (24, N'Chạy thang ngang', 2, N'TL')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (25, N'Chạy dài', 3, N'TL')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (26, N'Chạy con thoi', 4, N'TL')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (27, N'Nhảy rào', 4, N'TL')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (28, N'Thể dục', 5, N'TL')
INSERT [dbo].[Exercises] ([ID], [Name], [Difficult_Level ], [Class]) VALUES (29, N'Chạy nước rút', 5, N'TL')
GO
SET IDENTITY_INSERT [dbo].[Information] ON 

INSERT [dbo].[Information] ([ID], [Name], [YearOld], [Quality]) VALUES (1, N'Kylian Mpapé', N'18', 0)
INSERT [dbo].[Information] ([ID], [Name], [YearOld], [Quality]) VALUES (33, N'Vini', N'18', 0)
INSERT [dbo].[Information] ([ID], [Name], [YearOld], [Quality]) VALUES (34, N'DMC 9*', N'18', NULL)
INSERT [dbo].[Information] ([ID], [Name], [YearOld], [Quality]) VALUES (35, N'MR 9*', N'18', NULL)
INSERT [dbo].[Information] ([ID], [Name], [YearOld], [Quality]) VALUES (36, N'ML 9*', N'18', NULL)
INSERT [dbo].[Information] ([ID], [Name], [YearOld], [Quality]) VALUES (39, N'Ronaldo', N'18', 0)
INSERT [dbo].[Information] ([ID], [Name], [YearOld], [Quality]) VALUES (40, N'Ronaldo', N'18', NULL)
INSERT [dbo].[Information] ([ID], [Name], [YearOld], [Quality]) VALUES (42, N'Vini', N'18', 0)
SET IDENTITY_INSERT [dbo].[Information] OFF
GO
SET IDENTITY_INSERT [dbo].[MainAttributes] ON 

INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (1, N'canPha', 1)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (2, N'kemNguoi', 1)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (3, N'chayCho', 1)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (4, N'danhDau', 1)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (5, N'dungCam', 1)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (6, N'chuyenBong', 2)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (7, N'reBong', 2)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (8, N'tatCanh', 2)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (9, N'sutManh', 2)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (10, N'dutDiem', 2)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (11, N'theLuc', 3)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (12, N'sucManh', 3)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (13, N'xongXao', 3)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (14, N'tocDo', 3)
INSERT [dbo].[MainAttributes] ([ID], [Name], [Type]) VALUES (15, N'sangTao', 3)
SET IDENTITY_INSERT [dbo].[MainAttributes] OFF
GO
SET IDENTITY_INSERT [dbo].[PlayerValue] ON 

INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (150, CAST(110.60 AS Decimal(5, 2)), CAST(110.20 AS Decimal(5, 2)), CAST(109.90 AS Decimal(5, 2)), CAST(109.50 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (151, CAST(110.70 AS Decimal(5, 2)), CAST(110.40 AS Decimal(5, 2)), CAST(110.00 AS Decimal(5, 2)), CAST(109.70 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (152, CAST(110.80 AS Decimal(5, 2)), CAST(110.50 AS Decimal(5, 2)), CAST(110.10 AS Decimal(5, 2)), CAST(109.80 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (153, CAST(111.00 AS Decimal(5, 2)), CAST(110.60 AS Decimal(5, 2)), CAST(110.20 AS Decimal(5, 2)), CAST(109.90 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (154, CAST(111.10 AS Decimal(5, 2)), CAST(110.70 AS Decimal(5, 2)), CAST(110.40 AS Decimal(5, 2)), CAST(110.00 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (155, CAST(111.20 AS Decimal(5, 2)), CAST(110.80 AS Decimal(5, 2)), CAST(110.50 AS Decimal(5, 2)), CAST(110.10 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (156, CAST(111.30 AS Decimal(5, 2)), CAST(111.00 AS Decimal(5, 2)), CAST(110.60 AS Decimal(5, 2)), CAST(110.20 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (157, CAST(111.40 AS Decimal(5, 2)), CAST(111.10 AS Decimal(5, 2)), CAST(110.70 AS Decimal(5, 2)), CAST(110.40 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (158, CAST(111.50 AS Decimal(5, 2)), CAST(111.20 AS Decimal(5, 2)), CAST(110.80 AS Decimal(5, 2)), CAST(110.50 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (159, CAST(111.60 AS Decimal(5, 2)), CAST(111.30 AS Decimal(5, 2)), CAST(111.00 AS Decimal(5, 2)), CAST(110.60 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (160, CAST(111.80 AS Decimal(5, 2)), CAST(111.40 AS Decimal(5, 2)), CAST(111.10 AS Decimal(5, 2)), CAST(110.70 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (161, CAST(111.90 AS Decimal(5, 2)), CAST(111.50 AS Decimal(5, 2)), CAST(111.20 AS Decimal(5, 2)), CAST(110.80 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (162, CAST(112.00 AS Decimal(5, 2)), CAST(111.60 AS Decimal(5, 2)), CAST(111.30 AS Decimal(5, 2)), CAST(111.00 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (163, CAST(112.20 AS Decimal(5, 2)), CAST(111.80 AS Decimal(5, 2)), CAST(111.40 AS Decimal(5, 2)), CAST(111.10 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (164, CAST(112.40 AS Decimal(5, 2)), CAST(111.90 AS Decimal(5, 2)), CAST(111.50 AS Decimal(5, 2)), CAST(111.20 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (165, CAST(112.60 AS Decimal(5, 2)), CAST(112.00 AS Decimal(5, 2)), CAST(111.60 AS Decimal(5, 2)), CAST(111.30 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (166, CAST(112.80 AS Decimal(5, 2)), CAST(112.20 AS Decimal(5, 2)), CAST(111.80 AS Decimal(5, 2)), CAST(111.40 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (167, CAST(113.00 AS Decimal(5, 2)), CAST(112.40 AS Decimal(5, 2)), CAST(111.90 AS Decimal(5, 2)), CAST(111.50 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (168, CAST(113.20 AS Decimal(5, 2)), CAST(112.60 AS Decimal(5, 2)), CAST(112.00 AS Decimal(5, 2)), CAST(111.60 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (169, CAST(113.40 AS Decimal(5, 2)), CAST(112.80 AS Decimal(5, 2)), CAST(112.20 AS Decimal(5, 2)), CAST(111.80 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (170, CAST(113.60 AS Decimal(5, 2)), CAST(113.00 AS Decimal(5, 2)), CAST(112.40 AS Decimal(5, 2)), CAST(111.90 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (171, CAST(113.80 AS Decimal(5, 2)), CAST(113.20 AS Decimal(5, 2)), CAST(112.60 AS Decimal(5, 2)), CAST(112.00 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (172, CAST(114.00 AS Decimal(5, 2)), CAST(113.40 AS Decimal(5, 2)), CAST(112.80 AS Decimal(5, 2)), CAST(112.20 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (173, CAST(114.20 AS Decimal(5, 2)), CAST(113.60 AS Decimal(5, 2)), CAST(113.00 AS Decimal(5, 2)), CAST(112.40 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (174, CAST(114.40 AS Decimal(5, 2)), CAST(113.80 AS Decimal(5, 2)), CAST(113.20 AS Decimal(5, 2)), CAST(112.60 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (175, CAST(114.60 AS Decimal(5, 2)), CAST(114.00 AS Decimal(5, 2)), CAST(113.40 AS Decimal(5, 2)), CAST(112.80 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (176, CAST(114.80 AS Decimal(5, 2)), CAST(114.20 AS Decimal(5, 2)), CAST(113.60 AS Decimal(5, 2)), CAST(113.00 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (177, CAST(115.00 AS Decimal(5, 2)), CAST(114.40 AS Decimal(5, 2)), CAST(113.80 AS Decimal(5, 2)), CAST(113.20 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (178, CAST(115.20 AS Decimal(5, 2)), CAST(114.60 AS Decimal(5, 2)), CAST(114.00 AS Decimal(5, 2)), CAST(113.40 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (179, CAST(115.40 AS Decimal(5, 2)), CAST(114.80 AS Decimal(5, 2)), CAST(114.20 AS Decimal(5, 2)), CAST(113.60 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (180, CAST(115.60 AS Decimal(5, 2)), CAST(115.00 AS Decimal(5, 2)), CAST(114.40 AS Decimal(5, 2)), CAST(113.80 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (181, CAST(115.80 AS Decimal(5, 2)), CAST(115.20 AS Decimal(5, 2)), CAST(114.60 AS Decimal(5, 2)), CAST(114.00 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (182, CAST(116.00 AS Decimal(5, 2)), CAST(115.40 AS Decimal(5, 2)), CAST(114.80 AS Decimal(5, 2)), CAST(114.20 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (183, CAST(116.20 AS Decimal(5, 2)), CAST(115.60 AS Decimal(5, 2)), CAST(115.00 AS Decimal(5, 2)), CAST(114.40 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (184, CAST(116.40 AS Decimal(5, 2)), CAST(115.80 AS Decimal(5, 2)), CAST(115.20 AS Decimal(5, 2)), CAST(114.60 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (185, CAST(116.60 AS Decimal(5, 2)), CAST(116.00 AS Decimal(5, 2)), CAST(115.40 AS Decimal(5, 2)), CAST(114.80 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (186, CAST(116.80 AS Decimal(5, 2)), CAST(116.20 AS Decimal(5, 2)), CAST(115.60 AS Decimal(5, 2)), CAST(115.00 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (187, CAST(117.00 AS Decimal(5, 2)), CAST(116.40 AS Decimal(5, 2)), CAST(115.80 AS Decimal(5, 2)), CAST(115.20 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (188, CAST(117.20 AS Decimal(5, 2)), CAST(116.60 AS Decimal(5, 2)), CAST(116.00 AS Decimal(5, 2)), CAST(115.40 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (189, CAST(117.40 AS Decimal(5, 2)), CAST(116.80 AS Decimal(5, 2)), CAST(116.20 AS Decimal(5, 2)), CAST(115.60 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (190, CAST(117.60 AS Decimal(5, 2)), CAST(117.00 AS Decimal(5, 2)), CAST(116.40 AS Decimal(5, 2)), CAST(115.80 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (191, CAST(117.80 AS Decimal(5, 2)), CAST(117.20 AS Decimal(5, 2)), CAST(116.60 AS Decimal(5, 2)), CAST(116.00 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (192, CAST(118.00 AS Decimal(5, 2)), CAST(117.40 AS Decimal(5, 2)), CAST(116.80 AS Decimal(5, 2)), CAST(116.20 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (193, CAST(118.20 AS Decimal(5, 2)), CAST(117.60 AS Decimal(5, 2)), CAST(117.00 AS Decimal(5, 2)), CAST(116.40 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (194, CAST(118.40 AS Decimal(5, 2)), CAST(117.80 AS Decimal(5, 2)), CAST(117.20 AS Decimal(5, 2)), CAST(116.60 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (195, CAST(118.60 AS Decimal(5, 2)), CAST(118.00 AS Decimal(5, 2)), CAST(117.40 AS Decimal(5, 2)), CAST(116.80 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (196, CAST(118.80 AS Decimal(5, 2)), CAST(118.20 AS Decimal(5, 2)), CAST(117.60 AS Decimal(5, 2)), CAST(117.00 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (197, CAST(119.00 AS Decimal(5, 2)), CAST(118.40 AS Decimal(5, 2)), CAST(117.80 AS Decimal(5, 2)), CAST(117.20 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (198, CAST(119.20 AS Decimal(5, 2)), CAST(118.60 AS Decimal(5, 2)), CAST(118.00 AS Decimal(5, 2)), CAST(117.40 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (199, CAST(119.40 AS Decimal(5, 2)), CAST(118.80 AS Decimal(5, 2)), CAST(118.20 AS Decimal(5, 2)), CAST(117.60 AS Decimal(5, 2)))
INSERT [dbo].[PlayerValue] ([Q], [18Year], [19Year], [20Year], [21Year]) VALUES (200, CAST(119.60 AS Decimal(5, 2)), CAST(119.00 AS Decimal(5, 2)), CAST(118.40 AS Decimal(5, 2)), CAST(117.80 AS Decimal(5, 2)))
SET IDENTITY_INSERT [dbo].[PlayerValue] OFF
GO
SET IDENTITY_INSERT [dbo].[Position] ON 

INSERT [dbo].[Position] ([ID], [Name]) VALUES (1, N'ST')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (2, N'AML')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (3, N'AMC')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (4, N'AMR')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (5, N'ML')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (6, N'MC')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (7, N'MR')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (8, N'DMC')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (9, N'DL')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (10, N'DC')
INSERT [dbo].[Position] ([ID], [Name]) VALUES (11, N'DR')
SET IDENTITY_INSERT [dbo].[Position] OFF
GO
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (1, 2)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (1, 3)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (1, 4)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (33, 2)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (33, 5)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (34, 8)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (34, 10)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (35, 4)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (35, 6)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (35, 7)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (36, 2)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (36, 5)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (36, 6)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (39, 1)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (39, 3)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (40, 1)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (40, 3)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (42, 2)
INSERT [dbo].[PositionInformation] ([InforID], [PositionID]) VALUES (42, 5)
GO
INSERT [dbo].[QualityAfter] ([ID], [InforID], [PlanID], [ExerciseID], [Average], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (1, 42, 1, 2, 180, 50, 61, 193, 95, 72, 291, 216, 230, 115, 180, 123, 26, 27, 135, 275)
GO
INSERT [dbo].[QualityBefore] ([InforID], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (1, 43, 60, 86, 151, 75, 278, 247, 272, 313, 295, 223, 38, 1, 254, 234)
INSERT [dbo].[QualityBefore] ([InforID], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (33, 50, 61, 193, 95, 72, 297, 216, 230, 121, 180, 123, 26, 27, 141, 275)
INSERT [dbo].[QualityBefore] ([InforID], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (34, 220, 197, 173, 243, 182, 262, 88, 66, 100, 121, 192, 210, 150, 148, 202)
INSERT [dbo].[QualityBefore] ([InforID], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (35, 63, 186, 207, 45, 183, 205, 256, 227, 206, 192, 209, 93, 81, 239, 145)
INSERT [dbo].[QualityBefore] ([InforID], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (36, 50, 88, 219, 17, 135, 231, 262, 267, 222, 250, 218, 64, 42, 305, 159)
INSERT [dbo].[QualityBefore] ([InforID], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (39, 1, 47, 218, 188, 29, 257, 244, 31, 390, 410, 114, 221, 11, 236, 204)
INSERT [dbo].[QualityBefore] ([InforID], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (40, 1, 47, 218, 188, 29, 257, 244, 31, 390, 410, 114, 221, 11, 236, 204)
INSERT [dbo].[QualityBefore] ([InforID], [CanPha], [KemNguoi], [ChayCho], [DanhDau], [DungCam], [ChuyenBong], [ReBong], [TatCanh], [SutManh], [DutDiem], [TheLuc], [SucManh], [XongXao], [TocDo], [SangTao]) VALUES (42, 50, 61, 193, 95, 72, 297, 216, 230, 121, 180, 123, 26, 27, 141, 275)
GO
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 1, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 2, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 3, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 4, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 5, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 6, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 7, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 8, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 9, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 10, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 11, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 12, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 13, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 14, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (1, 15, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 1, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 2, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 3, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 4, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 5, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 6, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 7, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 8, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 9, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 10, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 12, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 13, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 14, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (2, 15, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 1, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 2, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 3, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 4, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 5, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 6, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 7, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 8, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 9, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 10, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 12, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 13, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 14, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (3, 15, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 1, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 2, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 3, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 4, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 5, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 6, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 7, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 8, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 9, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 10, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 12, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 13, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 14, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (4, 15, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 1, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 2, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 3, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 4, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 5, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 6, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 7, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 8, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 9, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 10, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 12, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 13, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 14, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (5, 15, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 1, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 2, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 3, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 4, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 5, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 6, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 7, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 8, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 9, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 10, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 12, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 13, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 14, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (6, 15, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 1, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 2, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 3, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 4, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 5, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 6, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 7, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 8, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 9, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 10, 0)
GO
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 12, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 13, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 14, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (7, 15, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 1, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 2, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 3, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 4, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 5, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 6, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 7, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 8, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 9, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 10, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 12, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 13, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 14, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (8, 15, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 1, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 2, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 3, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 4, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 5, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 6, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 7, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 8, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 9, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 10, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 12, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 13, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 14, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (9, 15, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 1, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 2, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 3, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 4, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 5, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 6, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 7, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 8, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 9, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 10, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 12, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 13, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 14, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (10, 15, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 1, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 2, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 3, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 4, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 5, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 6, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 7, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 8, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 9, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 10, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 11, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 12, 0)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 13, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 14, 1)
INSERT [dbo].[TypeAttributes] ([PositionID], [AttributeID], [Disable]) VALUES (11, 15, 0)
GO
USE [master]
GO
ALTER DATABASE [Top11] SET  READ_WRITE 
GO
