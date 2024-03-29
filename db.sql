USE [master]
GO
/****** Object:  Database [WahoS8]    Script Date: 17/7/2023 3:47:24 PM ******/
CREATE DATABASE [WahoS8]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WahoS8', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.DUC\MSSQL\DATA\WahoS8.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WahoS8_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.DUC\MSSQL\DATA\WahoS8_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [WahoS8] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WahoS8].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WahoS8] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WahoS8] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WahoS8] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WahoS8] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WahoS8] SET ARITHABORT OFF 
GO
ALTER DATABASE [WahoS8] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WahoS8] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WahoS8] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WahoS8] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WahoS8] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WahoS8] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WahoS8] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WahoS8] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WahoS8] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WahoS8] SET  ENABLE_BROKER 
GO
ALTER DATABASE [WahoS8] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WahoS8] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WahoS8] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WahoS8] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WahoS8] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WahoS8] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WahoS8] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WahoS8] SET RECOVERY FULL 
GO
ALTER DATABASE [WahoS8] SET  MULTI_USER 
GO
ALTER DATABASE [WahoS8] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WahoS8] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WahoS8] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WahoS8] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WahoS8] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WahoS8] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'WahoS8', N'ON'
GO
ALTER DATABASE [WahoS8] SET QUERY_STORE = OFF
GO
USE [WahoS8]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[billID] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NULL,
	[active] [bit] NULL,
	[descriptions] [nvarchar](100) NULL,
	[billStatus] [nvarchar](50) NULL,
	[total] [decimal](10, 2) NULL,
	[wahoID] [int] NULL,
	[userName] [nvarchar](50) NULL,
	[customerID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[billID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillDetails]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillDetails](
	[billID] [int] NOT NULL,
	[productID] [int] NOT NULL,
	[quantity] [int] NULL,
	[discount] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[billID] ASC,
	[productID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[categoryID] [int] IDENTITY(1,1) NOT NULL,
	[categoryName] [nvarchar](100) NOT NULL,
	[haveDate] [bit] NULL,
	[description] [nvarchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[categoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[customerID] [int] IDENTITY(1,1) NOT NULL,
	[customerName] [nvarchar](150) NULL,
	[phone] [nvarchar](50) NULL,
	[Dob] [date] NULL,
	[adress] [nvarchar](50) NULL,
	[typeOfCustomer] [bit] NULL,
	[taxCode] [nvarchar](50) NULL,
	[email] [nvarchar](50) NULL,
	[description] [nvarchar](50) NULL,
	[active] [bit] NULL,
	[wahoID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[customerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[userName] [nvarchar](50) NOT NULL,
	[employeeName] [nvarchar](150) NULL,
	[title] [nvarchar](50) NULL,
	[Dob] [date] NULL,
	[hireDate] [date] NULL,
	[address] [nvarchar](150) NULL,
	[phone] [nvarchar](50) NULL,
	[note] [nvarchar](250) NULL,
	[password] [nvarchar](50) NULL,
	[Role] [int] NULL,
	[email] [varchar](255) NULL,
	[active] [bit] NULL,
	[wahoID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[userName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventorySheetDetail]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventorySheetDetail](
	[inventorySheetID] [int] NOT NULL,
	[productID] [int] NOT NULL,
	[curNWareHouse] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[inventorySheetID] ASC,
	[productID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventorySheets]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventorySheets](
	[inventorySheetID] [int] IDENTITY(1,1) NOT NULL,
	[description] [nvarchar](100) NULL,
	[date] [date] NULL,
	[active] [bit] NULL,
	[wahoID] [int] NULL,
	[userName] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[inventorySheetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[locationID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[locationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OderDetails]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OderDetails](
	[oderID] [int] NOT NULL,
	[productID] [int] NOT NULL,
	[quantity] [int] NULL,
	[discount] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[oderID] ASC,
	[productID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Oders]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Oders](
	[oderID] [int] IDENTITY(1,1) NOT NULL,
	[oderState] [nvarchar](50) NULL,
	[region] [nvarchar](150) NULL,
	[cod] [nchar](10) NULL,
	[orderDate] [date] NULL,
	[estimatedDate] [date] NULL,
	[total] [decimal](10, 2) NULL,
	[deposit] [decimal](10, 2) NULL,
	[active] [bit] NULL,
	[userName] [nvarchar](50) NULL,
	[customerID] [int] NULL,
	[shipperID] [int] NULL,
	[wahoID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[oderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[productID] [int] IDENTITY(1,1) NOT NULL,
	[productName] [nvarchar](150) NOT NULL,
	[importPrice] [int] NULL,
	[unitPrice] [int] NULL,
	[quantity] [int] NULL,
	[haveDate] [bit] NOT NULL,
	[dateOfManufacture] [date] NULL,
	[expiry] [date] NULL,
	[trademark] [nvarchar](50) NULL,
	[weight] [int] NULL,
	[unit] [nvarchar](50) NULL,
	[inventoryLevelMin] [int] NULL,
	[inventoryLevelMax] [int] NULL,
	[description] [nvarchar](150) NULL,
	[active] [bit] NOT NULL,
	[locationID] [int] NULL,
	[subCategoryID] [int] NULL,
	[wahoID] [int] NULL,
	[supplierID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[productID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnOrderProduct]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnOrderProduct](
	[productID] [int] NOT NULL,
	[returnOrderID] [int] NOT NULL,
	[quantity] [int] NULL,
	[discount] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[returnOrderID] ASC,
	[productID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnOrders]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnOrders](
	[returnOrderID] [int] IDENTITY(1,1) NOT NULL,
	[state] [bit] NULL,
	[date] [date] NULL,
	[description] [nvarchar](50) NULL,
	[active] [bit] NOT NULL,
	[payCustomer] [decimal](10, 2) NULL,
	[paidCustomer] [decimal](10, 2) NULL,
	[billID] [int] NULL,
	[userName] [nvarchar](50) NULL,
	[customerID] [int] NULL,
	[wahoID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[returnOrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shippers]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shippers](
	[shipperID] [int] IDENTITY(1,1) NOT NULL,
	[shipperName] [nvarchar](50) NULL,
	[phone] [nvarchar](50) NULL,
	[wahoID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[shipperID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubCategories]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCategories](
	[subCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[subCategoryName] [nvarchar](250) NULL,
	[description] [nvarchar](250) NULL,
	[categoryID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[subCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[supplierID] [int] IDENTITY(1,1) NOT NULL,
	[companyName] [nvarchar](150) NULL,
	[address] [nvarchar](150) NULL,
	[phone] [nvarchar](50) NULL,
	[taxCode] [nvarchar](50) NULL,
	[branch] [nvarchar](50) NULL,
	[description] [nvarchar](50) NULL,
	[active] [bit] NULL,
	[wahoID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[supplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WahoInformation]    Script Date: 17/7/2023 3:47:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WahoInformation](
	[wahoID] [int] IDENTITY(1,1) NOT NULL,
	[wahoName] [nvarchar](250) NULL,
	[address] [nvarchar](250) NULL,
	[phone] [nvarchar](50) NULL,
	[email] [nvarchar](150) NULL,
	[active] [bit] NULL,
	[categoryID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[wahoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Bill] ON 

INSERT [dbo].[Bill] ([billID], [date], [active], [descriptions], [billStatus], [total], [wahoID], [userName], [customerID]) VALUES (1, CAST(N'2023-06-20' AS Date), 1, NULL, N'done', CAST(1000000.00 AS Decimal(10, 2)), 1, N'ductv305', 1)
SET IDENTITY_INSERT [dbo].[Bill] OFF
GO
INSERT [dbo].[BillDetails] ([billID], [productID], [quantity], [discount]) VALUES (1, 1, 5, 0.1)
INSERT [dbo].[BillDetails] ([billID], [productID], [quantity], [discount]) VALUES (1, 3, 2, 0.1)
INSERT [dbo].[BillDetails] ([billID], [productID], [quantity], [discount]) VALUES (1, 4, 2, 0.1)
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([categoryID], [categoryName], [haveDate], [description]) VALUES (1, N'thời trang', 0, NULL)
INSERT [dbo].[Categories] ([categoryID], [categoryName], [haveDate], [description]) VALUES (2, N'thực phẩm', 1, NULL)
INSERT [dbo].[Categories] ([categoryID], [categoryName], [haveDate], [description]) VALUES (3, N'Ô tô', 0, NULL)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 

INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (1, N'Phan Quan', N'phone', NULL, N'adress', NULL, N'taxCode', N'email', N'description', NULL, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (2, N'Trần Đức update', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'tranduc@gmail.com', N'', 0, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (3, N'Trần Đức 2', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'tranduc@gmail.com', N'', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (4, N'Trần Đức 3', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'tranduc@gmail.com', NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (5, N'Trần Đức 4', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'tranduc@gmail.com', NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (6, N'Tùng Lâm', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'lamnt@gmail.com', NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (7, N'Tùng Lâm 2', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'lamnt@gmail.com', NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (8, N'Tùng Lâm 3', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'lamnt@gmail.com', NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (9, N'Tùng Lâm 4', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'lamnt@gmail.com', NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (10, N'thu', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'thunt@gmail.com', NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (11, N'thu 3', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'thunt@gmail.com', NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (12, N'thu 4', N'0998888888', CAST(N'2001-01-01' AS Date), N'Hà Nội', 1, N'099999', N'thunt@gmail.com', N'update', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (13, N'Tùng Lâm', N'0998888888', CAST(N'2001-01-01' AS Date), N'', 0, N'', N'', N'', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (14, N'aduc00005', N'0354897794', CAST(N'2001-01-01' AS Date), N'đường tỉnh lộ 84,xã yên bài,huyện ba vì,hà đươ', 0, N'152888', N'aduc00005@gmail.com', N'', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (15, N'Trần Đức', N'0998888888', CAST(N'2001-01-01' AS Date), NULL, NULL, NULL, NULL, NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (16, N'Trần Đức', N'0998888888', CAST(N'2001-01-01' AS Date), NULL, NULL, NULL, NULL, NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (17, N'Trần Đức', N'0998888888', CAST(N'2001-01-01' AS Date), NULL, NULL, NULL, NULL, NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (18, N'Trần Đức', N'0998888888', CAST(N'2001-01-01' AS Date), NULL, NULL, NULL, NULL, NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (19, N'Nguyễn Phi', N'0354897794', CAST(N'2023-03-27' AS Date), N'đường tỉnh lộ 84,xã yên bài,huyện ba vì,hà đươ', 1, N'152888', N'aduc00005@gmail.com', N'', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (20, N'aduc00005', N'0354897794', CAST(N'2023-03-27' AS Date), N'đường tỉnh lộ 84,xã yên bài,huyện ba vì,hà đươ', 1, N'152888', N'aduc00005@gmail.com', N'', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (21, N'tùng lâm 200', N'0354897794,0354897794', CAST(N'2023-03-27' AS Date), N'đường tỉnh lộ 84,xã yên bài,huyện ba vì,hà đươ', 1, N'099999', N'aduc00005@gmail.com', N'', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (22, N'Trần Đức 4', N'0998888888', NULL, NULL, NULL, NULL, NULL, NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (23, N'tùng 201', N'0955555585,0955555585', CAST(N'2023-03-27' AS Date), N'đường tỉnh lộ 84,xã yên bài,huyện ba vì,hà đươ', 0, N'099999', N'aduc00005@gmail.com', N'', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (24, N'tùng 201', N'0955555585,0955555585', NULL, NULL, NULL, NULL, NULL, NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (25, N'tùng 201', N'0955555585,0955555585', NULL, NULL, NULL, NULL, NULL, NULL, 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (26, N'Trần Đức 4', N'0998888888', NULL, NULL, NULL, NULL, NULL, NULL, 1, 2)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (27, N'Trần Đức 4', N'0998888888', NULL, NULL, NULL, NULL, NULL, NULL, 1, 2)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (28, N'Trần Đức 4', N'0998888888', NULL, NULL, NULL, NULL, NULL, NULL, 1, 2)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (29, N'Trần Đức 4', N'0998888888', NULL, NULL, NULL, NULL, NULL, NULL, 1, 2)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (30, N'Trần Đức 4', N'0998888888', NULL, NULL, NULL, NULL, NULL, NULL, 1, 2)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (31, N'thu 3', N'0998888888', NULL, NULL, NULL, NULL, NULL, NULL, 1, 2)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (34, N'Trần Văn Đức', N'0989373658', CAST(N'2023-05-28' AS Date), N'hà nội', 0, N'00333', N'testregister1@gmail.com', N'', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (35, N'Nguyễn Tùng Lâm', N'0988899999', CAST(N'2023-05-29' AS Date), N'hà nội', 1, N'00333', N'lamnt2@gmail.com', N'', 1, 1)
INSERT [dbo].[Customers] ([customerID], [customerName], [phone], [Dob], [adress], [typeOfCustomer], [taxCode], [email], [description], [active], [wahoID]) VALUES (36, N'Nguyễn Chi Tùng', N'0123456789', CAST(N'2023-05-29' AS Date), N'đường tỉnh lộ 84,xã yên bài,huyện ba vì,hà đươ', 1, N'099999', N'aChiTung@gmail.com', N'', 1, 1)
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
INSERT [dbo].[Employees] ([userName], [employeeName], [title], [Dob], [hireDate], [address], [phone], [note], [password], [Role], [email], [active], [wahoID]) VALUES (N'ductv305', N'Trần Văn Đức', N'Admin', CAST(N'2001-05-30' AS Date), CAST(N'2023-06-06' AS Date), N'ha noi', N'0989373658', NULL, N'123456', 1, N'aduc00005@gmail.com', 1, 1)
INSERT [dbo].[Employees] ([userName], [employeeName], [title], [Dob], [hireDate], [address], [phone], [note], [password], [Role], [email], [active], [wahoID]) VALUES (N'lamnt5', N'nguyễn tùng lâm', N'Thu Ngân', CAST(N'2023-06-07' AS Date), CAST(N'2023-06-10' AS Date), N'Hà Nội Việt Nam', N'0998888888', N'', N'wahoEmployee', 2, N'ductv30501@gmail.com', 1, 1)
INSERT [dbo].[Employees] ([userName], [employeeName], [title], [Dob], [hireDate], [address], [phone], [note], [password], [Role], [email], [active], [wahoID]) VALUES (N'PostEmployeeVM', N'nguyễn tùng lâm update', N'Nhân viên', CAST(N'2023-07-12' AS Date), CAST(N'2023-07-12' AS Date), N'Hà Nội Việt Nam', N'0998888888', N'update', N'wahoEmployee', 1, N'aduc00005@gmail.com', 1, 1)
INSERT [dbo].[Employees] ([userName], [employeeName], [title], [Dob], [hireDate], [address], [phone], [note], [password], [Role], [email], [active], [wahoID]) VALUES (N'test12345', N'test', N'test', CAST(N'2023-07-10' AS Date), NULL, N'hà nội', N'09888888', NULL, N'test12345', 1, N'test@gmail.com', 1, 2)
INSERT [dbo].[Employees] ([userName], [employeeName], [title], [Dob], [hireDate], [address], [phone], [note], [password], [Role], [email], [active], [wahoID]) VALUES (N'testresgister4', N'Nhân viên quèn', N'Nhân Viên Quèn', CAST(N'2023-05-29' AS Date), CAST(N'2023-06-06' AS Date), N'Hà Nội Việt Nam', N'0998888888', N'', N'wahoEmployee', 3, N'testresgister4@gmail.com', 1, 1)
INSERT [dbo].[Employees] ([userName], [employeeName], [title], [Dob], [hireDate], [address], [phone], [note], [password], [Role], [email], [active], [wahoID]) VALUES (N'testresgister434', N'nguyễn tùng lâm', N'131123', CAST(N'2023-07-07' AS Date), CAST(N'2023-07-15' AS Date), N'Hà Nội Việt Nam', N'0998888888', N'1231231', N'wahoEmployee', 2, N'testresgister434@gmail.com', 1, 1)
GO
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (1, 1, 90)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (1, 3, 90)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (1, 4, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (1, 5, 110)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (2, 1, 99)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (2, 3, 111)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (2, 4, 89)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (2, 5, 110)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 1, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 3, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 4, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 5, 110)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 6, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 7, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 8, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 9, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 10, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 11, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 14, 1000)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 16, 100)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 20, 200)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 21, 200)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 22, 200)
INSERT [dbo].[InventorySheetDetail] ([inventorySheetID], [productID], [curNWareHouse]) VALUES (9, 23, 200)
GO
SET IDENTITY_INSERT [dbo].[InventorySheets] ON 

INSERT [dbo].[InventorySheets] ([inventorySheetID], [description], [date], [active], [wahoID], [userName]) VALUES (1, N'update', CAST(N'2023-06-18' AS Date), 1, 1, N'ductv305')
INSERT [dbo].[InventorySheets] ([inventorySheetID], [description], [date], [active], [wahoID], [userName]) VALUES (2, NULL, CAST(N'2023-06-15' AS Date), 1, 1, N'ductv305')
INSERT [dbo].[InventorySheets] ([inventorySheetID], [description], [date], [active], [wahoID], [userName]) VALUES (3, NULL, CAST(N'2023-06-14' AS Date), 1, 1, N'ductv305')
INSERT [dbo].[InventorySheets] ([inventorySheetID], [description], [date], [active], [wahoID], [userName]) VALUES (5, NULL, CAST(N'2023-06-10' AS Date), 1, 1, N'ductv305')
INSERT [dbo].[InventorySheets] ([inventorySheetID], [description], [date], [active], [wahoID], [userName]) VALUES (6, NULL, CAST(N'2023-06-09' AS Date), 1, 1, N'ductv305')
INSERT [dbo].[InventorySheets] ([inventorySheetID], [description], [date], [active], [wahoID], [userName]) VALUES (7, NULL, CAST(N'2023-06-08' AS Date), 0, 1, N'ductv305')
INSERT [dbo].[InventorySheets] ([inventorySheetID], [description], [date], [active], [wahoID], [userName]) VALUES (9, NULL, CAST(N'2023-06-19' AS Date), 1, 1, N'ductv305')
SET IDENTITY_INSERT [dbo].[InventorySheets] OFF
GO
SET IDENTITY_INSERT [dbo].[Location] ON 

INSERT [dbo].[Location] ([locationID], [name]) VALUES (1, N'A01')
INSERT [dbo].[Location] ([locationID], [name]) VALUES (2, N'A02')
INSERT [dbo].[Location] ([locationID], [name]) VALUES (3, N'A03')
INSERT [dbo].[Location] ([locationID], [name]) VALUES (4, N'A04')
INSERT [dbo].[Location] ([locationID], [name]) VALUES (5, N'A05')
INSERT [dbo].[Location] ([locationID], [name]) VALUES (6, N'B01')
INSERT [dbo].[Location] ([locationID], [name]) VALUES (7, N'B02')
INSERT [dbo].[Location] ([locationID], [name]) VALUES (8, N'B03')
INSERT [dbo].[Location] ([locationID], [name]) VALUES (9, N'B04')
INSERT [dbo].[Location] ([locationID], [name]) VALUES (10, N'B05')
SET IDENTITY_INSERT [dbo].[Location] OFF
GO
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (10, 1, 1, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (10, 3, 1, 0.2)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (10, 4, 2, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (11, 5, 1, 0)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (11, 6, 1, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (12, 4, 1, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (12, 11, 1, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (13, 3, 1, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (13, 8, 1, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (13, 11, 2, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (14, 8, 1, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (15, 1, 10, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (15, 3, 10, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (16, 8, 10, 0.1)
INSERT [dbo].[OderDetails] ([oderID], [productID], [quantity], [discount]) VALUES (16, 14, 10, 0.1)
GO
SET IDENTITY_INSERT [dbo].[Oders] ON 

INSERT [dbo].[Oders] ([oderID], [oderState], [region], [cod], [orderDate], [estimatedDate], [total], [deposit], [active], [userName], [customerID], [shipperID], [wahoID]) VALUES (10, N'pending', N'Hà Nội', N'0999885   ', CAST(N'2023-03-13' AS Date), CAST(N'2023-03-20' AS Date), CAST(100000.00 AS Decimal(10, 2)), CAST(20000.00 AS Decimal(10, 2)), 1, N'ductv305', 14, 1, 1)
INSERT [dbo].[Oders] ([oderID], [oderState], [region], [cod], [orderDate], [estimatedDate], [total], [deposit], [active], [userName], [customerID], [shipperID], [wahoID]) VALUES (11, N'pending', N'Hải Phòng', N'0212345   ', CAST(N'2023-03-13' AS Date), CAST(N'2023-03-20' AS Date), CAST(100000.00 AS Decimal(10, 2)), CAST(20000.00 AS Decimal(10, 2)), 1, N'ductv305', 14, 1, 1)
INSERT [dbo].[Oders] ([oderID], [oderState], [region], [cod], [orderDate], [estimatedDate], [total], [deposit], [active], [userName], [customerID], [shipperID], [wahoID]) VALUES (12, N'notDelivery', N'Hà Nội', N'0231      ', CAST(N'2023-06-19' AS Date), CAST(N'2023-06-22' AS Date), CAST(220000.00 AS Decimal(10, 2)), CAST(50000.00 AS Decimal(10, 2)), 1, N'ductv305', 34, 1, 1)
INSERT [dbo].[Oders] ([oderID], [oderState], [region], [cod], [orderDate], [estimatedDate], [total], [deposit], [active], [userName], [customerID], [shipperID], [wahoID]) VALUES (13, N'notDelivery', N'Hà Nội', N'0231      ', CAST(N'2023-06-19' AS Date), CAST(N'2023-06-24' AS Date), CAST(500000.00 AS Decimal(10, 2)), CAST(40000.00 AS Decimal(10, 2)), 1, N'ductv305', 35, 1, 1)
INSERT [dbo].[Oders] ([oderID], [oderState], [region], [cod], [orderDate], [estimatedDate], [total], [deposit], [active], [userName], [customerID], [shipperID], [wahoID]) VALUES (14, N'notDelivery', N'4800', N'100abc    ', CAST(N'2023-06-19' AS Date), CAST(N'2023-06-25' AS Date), CAST(35000.00 AS Decimal(10, 2)), CAST(100000.00 AS Decimal(10, 2)), 0, N'ductv305', 36, 1, 1)
INSERT [dbo].[Oders] ([oderID], [oderState], [region], [cod], [orderDate], [estimatedDate], [total], [deposit], [active], [userName], [customerID], [shipperID], [wahoID]) VALUES (15, N'notDelivery', N'Hà Nội', N'0123      ', CAST(N'2023-06-20' AS Date), CAST(N'2023-06-24' AS Date), CAST(2200000.00 AS Decimal(10, 2)), CAST(500000.00 AS Decimal(10, 2)), 1, N'ductv305', 14, 1, 1)
INSERT [dbo].[Oders] ([oderID], [oderState], [region], [cod], [orderDate], [estimatedDate], [total], [deposit], [active], [userName], [customerID], [shipperID], [wahoID]) VALUES (16, N'notDelivery', N'4800', N'100abc    ', CAST(N'2023-07-08' AS Date), CAST(N'2023-07-12' AS Date), CAST(3050000.00 AS Decimal(10, 2)), CAST(100000.00 AS Decimal(10, 2)), 1, N'ductv305', 14, 1, 1)
SET IDENTITY_INSERT [dbo].[Oders] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (1, N'Phông trơn', 50000, 150000, 90, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (3, N'Phông xanh dương', 50000, 150000, 90, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (4, N'Phông xanh lá', 50000, 150000, 100, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (5, N'Phông Be', 50000, 150000, 110, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (6, N'Phông Trắng', 50000, 150000, 100, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (7, N'Phông Đen', 50000, 150000, 100, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (8, N'Phông Hoa', 50000, 150000, 90, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (9, N'Phông Logo', 50000, 150000, 100, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (10, N'Phông Hồng', 50000, 150000, 100, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (11, N'Phông Xám', 50000, 150000, 100, 0, NULL, NULL, N'SWE', 100, N'Chiếc', 5, 10000, NULL, 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (14, N'test create', 100000, 200000, 990, 1, CAST(N'2023-05-30' AS Date), CAST(N'2023-07-01' AS Date), N'test update', 100, N'chiếc', 100, 1000, N'', 1, 5, 1, 1, 2)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (15, N'test create 2', 100000, 200000, 1000, 1, CAST(N'2023-05-30' AS Date), CAST(N'2023-07-01' AS Date), N'test', 100, N'chiếc', 10, 1000, N'te', 0, 7, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (16, N'test create 2 update', 100000, 200000, 100, 0, NULL, NULL, N'test', 100, N'chiếc', 10, 1000, N'', 1, 1, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (20, N'Áo Khoác hồng', 300000, 300000, 200, 0, NULL, NULL, N'Local Brand', 400, N'Chiếc', 5, 300, N'áo 2 lớp gió', 1, 1, 2, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (21, N'Áo Khoác hồng', 300000, 300000, 200, 0, CAST(N'2023-01-31' AS Date), CAST(N'2023-02-11' AS Date), N'Local Brand', 400, N'Chiếc', 5, 300, N'áo 2 lớp gió', 1, 2, 2, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (22, N'áo Tshirt ong mật ver 2', 100000, 200000, 200, 1, CAST(N'2023-02-01' AS Date), CAST(N'2023-04-30' AS Date), N'Swe', 200, N'chiếc', 5, 200, N's', 1, 3, 1, 1, 1)
INSERT [dbo].[Products] ([productID], [productName], [importPrice], [unitPrice], [quantity], [haveDate], [dateOfManufacture], [expiry], [trademark], [weight], [unit], [inventoryLevelMin], [inventoryLevelMax], [description], [active], [locationID], [subCategoryID], [wahoID], [supplierID]) VALUES (23, N'áo Tshirt ong mật ver 2', 1000, 200000, 200, 1, CAST(N'2023-01-31' AS Date), CAST(N'2023-02-11' AS Date), N'Swe', 200, N'chiếc', 5, 5, N's', 1, 4, 2, 1, 1)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
INSERT [dbo].[ReturnOrderProduct] ([productID], [returnOrderID], [quantity], [discount]) VALUES (1, 1, 1, 0.1)
INSERT [dbo].[ReturnOrderProduct] ([productID], [returnOrderID], [quantity], [discount]) VALUES (1, 2, 1, 0.1)
INSERT [dbo].[ReturnOrderProduct] ([productID], [returnOrderID], [quantity], [discount]) VALUES (4, 5, 1, 0.1)
INSERT [dbo].[ReturnOrderProduct] ([productID], [returnOrderID], [quantity], [discount]) VALUES (11, 6, 1, 0.1)
INSERT [dbo].[ReturnOrderProduct] ([productID], [returnOrderID], [quantity], [discount]) VALUES (8, 7, 1, 0.1)
INSERT [dbo].[ReturnOrderProduct] ([productID], [returnOrderID], [quantity], [discount]) VALUES (14, 8, 1, 0.1)
GO
SET IDENTITY_INSERT [dbo].[ReturnOrders] ON 

INSERT [dbo].[ReturnOrders] ([returnOrderID], [state], [date], [description], [active], [payCustomer], [paidCustomer], [billID], [userName], [customerID], [wahoID]) VALUES (1, 1, CAST(N'2023-06-20' AS Date), NULL, 1, CAST(1000000.00 AS Decimal(10, 2)), CAST(50000.00 AS Decimal(10, 2)), 1, N'ductv305', 1, 1)
INSERT [dbo].[ReturnOrders] ([returnOrderID], [state], [date], [description], [active], [payCustomer], [paidCustomer], [billID], [userName], [customerID], [wahoID]) VALUES (2, 1, CAST(N'2023-06-20' AS Date), N'', 1, CAST(1000000.00 AS Decimal(10, 2)), CAST(50000.00 AS Decimal(10, 2)), NULL, N'ductv305', 1, 1)
INSERT [dbo].[ReturnOrders] ([returnOrderID], [state], [date], [description], [active], [payCustomer], [paidCustomer], [billID], [userName], [customerID], [wahoID]) VALUES (5, 1, CAST(N'2023-06-20' AS Date), N'', 0, CAST(135000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), NULL, N'ductv305', 34, 1)
INSERT [dbo].[ReturnOrders] ([returnOrderID], [state], [date], [description], [active], [payCustomer], [paidCustomer], [billID], [userName], [customerID], [wahoID]) VALUES (6, 1, CAST(N'2023-06-21' AS Date), N'', 1, CAST(135000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), 12, N'ductv305', 34, 1)
INSERT [dbo].[ReturnOrders] ([returnOrderID], [state], [date], [description], [active], [payCustomer], [paidCustomer], [billID], [userName], [customerID], [wahoID]) VALUES (7, 1, CAST(N'2023-07-08' AS Date), N'', 1, CAST(135000.00 AS Decimal(10, 2)), CAST(135000.00 AS Decimal(10, 2)), 16, N'ductv305', 14, 1)
INSERT [dbo].[ReturnOrders] ([returnOrderID], [state], [date], [description], [active], [payCustomer], [paidCustomer], [billID], [userName], [customerID], [wahoID]) VALUES (8, 1, CAST(N'2023-07-08' AS Date), N'', 1, CAST(180000.00 AS Decimal(10, 2)), CAST(180000.00 AS Decimal(10, 2)), 16, N'ductv305', 14, 1)
SET IDENTITY_INSERT [dbo].[ReturnOrders] OFF
GO
SET IDENTITY_INSERT [dbo].[Shippers] ON 

INSERT [dbo].[Shippers] ([shipperID], [shipperName], [phone], [wahoID]) VALUES (1, N'lâm', N'0123456789', 1)
INSERT [dbo].[Shippers] ([shipperID], [shipperName], [phone], [wahoID]) VALUES (2, N'phi', N'0123659784', 1)
INSERT [dbo].[Shippers] ([shipperID], [shipperName], [phone], [wahoID]) VALUES (3, N'tùng', N'08889955555', 1)
SET IDENTITY_INSERT [dbo].[Shippers] OFF
GO
SET IDENTITY_INSERT [dbo].[SubCategories] ON 

INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (1, N'Áo', NULL, 1)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (2, N'Quần', NULL, 1)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (3, N'Mũ', NULL, 1)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (4, N'Giày', NULL, 1)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (5, N'Dép', NULL, 1)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (6, N'Tất', NULL, 1)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (7, N'Kính', NULL, 1)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (8, N'Nhẫn', NULL, 1)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (9, N'Rau Xanh', NULL, 2)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (10, N'Thịt Bò', NULL, 2)
INSERT [dbo].[SubCategories] ([subCategoryID], [subCategoryName], [description], [categoryID]) VALUES (11, N'Thịt Lợn', NULL, 2)
SET IDENTITY_INSERT [dbo].[SubCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[Suppliers] ON 

INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (1, N'Swe', N'TP HCM', N'0988888888', N'9999', N'Swe', NULL, 1, 1)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (2, N'21St Urban', N'Hà Nội', N'0555555555', N'8888', N'Urban', NULL, 1, 1)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (3, N'SSSTuter', N'Hà Nội ', N'0666666666', N'6666', N'SSStuter', NULL, 1, 1)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (4, N'Boo', N'Hà Nội', N'0999999999', N'5555', N'Boo', NULL, 1, 1)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (5, N'H2T', N'Hà Nội', N'0333333333', N'3333', N'H2T', NULL, 1, 1)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (6, N'Min', N'Hải Phòng', N'0123456789', N'4567', N'Min', N'', 1, 1)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (8, N'Big', N'Hải Dương', N'0987654321', N'9876', N'Big', NULL, 1, 2)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (9, N'VuonNha', N'Thái Nguyên', N'0223344555', N'1222', N'VuonNha', NULL, 1, 2)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (10, N'Thái Nguyên', N'Đà Nẵng', N'0223344555', N'8999', N'Thái xanh', N'xanh', 1, 2)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (11, N'Thái Nguyên', N'Dak Lak', N'0223344555', N'7899', N'Dak', N'sạch', 1, 2)
INSERT [dbo].[Suppliers] ([supplierID], [companyName], [address], [phone], [taxCode], [branch], [description], [active], [wahoID]) VALUES (12, N'test update', N'test', N'0989898988', N'1234', N'hà nội', N'', 0, 1)
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
GO
SET IDENTITY_INSERT [dbo].[WahoInformation] ON 

INSERT [dbo].[WahoInformation] ([wahoID], [wahoName], [address], [phone], [email], [active], [categoryID]) VALUES (1, N'Clos', N'ha dong ha noi', N'0999999999', N'clos@gmail.com', 1, 1)
INSERT [dbo].[WahoInformation] ([wahoID], [wahoName], [address], [phone], [email], [active], [categoryID]) VALUES (2, N'TPXanh', N'Hải Phòng', N'0123654978', N'tpxanh@gmail.com', 1, 2)
INSERT [dbo].[WahoInformation] ([wahoID], [wahoName], [address], [phone], [email], [active], [categoryID]) VALUES (4, N'Clos 2', N'ha noi', N'09999999', N'aduc00005@gmail.com', 1, 1)
SET IDENTITY_INSERT [dbo].[WahoInformation] OFF
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([customerID])
REFERENCES [dbo].[Customers] ([customerID])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([userName])
REFERENCES [dbo].[Employees] ([userName])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([wahoID])
REFERENCES [dbo].[WahoInformation] ([wahoID])
GO
ALTER TABLE [dbo].[BillDetails]  WITH CHECK ADD FOREIGN KEY([billID])
REFERENCES [dbo].[Bill] ([billID])
GO
ALTER TABLE [dbo].[BillDetails]  WITH CHECK ADD FOREIGN KEY([productID])
REFERENCES [dbo].[Products] ([productID])
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD FOREIGN KEY([wahoID])
REFERENCES [dbo].[WahoInformation] ([wahoID])
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD FOREIGN KEY([wahoID])
REFERENCES [dbo].[WahoInformation] ([wahoID])
GO
ALTER TABLE [dbo].[InventorySheetDetail]  WITH CHECK ADD FOREIGN KEY([inventorySheetID])
REFERENCES [dbo].[InventorySheets] ([inventorySheetID])
GO
ALTER TABLE [dbo].[InventorySheetDetail]  WITH CHECK ADD FOREIGN KEY([productID])
REFERENCES [dbo].[Products] ([productID])
GO
ALTER TABLE [dbo].[InventorySheets]  WITH CHECK ADD FOREIGN KEY([userName])
REFERENCES [dbo].[Employees] ([userName])
GO
ALTER TABLE [dbo].[InventorySheets]  WITH CHECK ADD FOREIGN KEY([wahoID])
REFERENCES [dbo].[WahoInformation] ([wahoID])
GO
ALTER TABLE [dbo].[OderDetails]  WITH CHECK ADD FOREIGN KEY([oderID])
REFERENCES [dbo].[Oders] ([oderID])
GO
ALTER TABLE [dbo].[OderDetails]  WITH CHECK ADD FOREIGN KEY([productID])
REFERENCES [dbo].[Products] ([productID])
GO
ALTER TABLE [dbo].[Oders]  WITH CHECK ADD FOREIGN KEY([customerID])
REFERENCES [dbo].[Customers] ([customerID])
GO
ALTER TABLE [dbo].[Oders]  WITH CHECK ADD FOREIGN KEY([shipperID])
REFERENCES [dbo].[Shippers] ([shipperID])
GO
ALTER TABLE [dbo].[Oders]  WITH CHECK ADD FOREIGN KEY([userName])
REFERENCES [dbo].[Employees] ([userName])
GO
ALTER TABLE [dbo].[Oders]  WITH CHECK ADD FOREIGN KEY([wahoID])
REFERENCES [dbo].[WahoInformation] ([wahoID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([locationID])
REFERENCES [dbo].[Location] ([locationID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([subCategoryID])
REFERENCES [dbo].[SubCategories] ([subCategoryID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([supplierID])
REFERENCES [dbo].[Suppliers] ([supplierID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([wahoID])
REFERENCES [dbo].[WahoInformation] ([wahoID])
GO
ALTER TABLE [dbo].[ReturnOrderProduct]  WITH CHECK ADD FOREIGN KEY([productID])
REFERENCES [dbo].[Products] ([productID])
GO
ALTER TABLE [dbo].[ReturnOrderProduct]  WITH CHECK ADD FOREIGN KEY([returnOrderID])
REFERENCES [dbo].[ReturnOrders] ([returnOrderID])
GO
ALTER TABLE [dbo].[ReturnOrders]  WITH CHECK ADD FOREIGN KEY([customerID])
REFERENCES [dbo].[Customers] ([customerID])
GO
ALTER TABLE [dbo].[ReturnOrders]  WITH CHECK ADD FOREIGN KEY([userName])
REFERENCES [dbo].[Employees] ([userName])
GO
ALTER TABLE [dbo].[ReturnOrders]  WITH CHECK ADD FOREIGN KEY([wahoID])
REFERENCES [dbo].[WahoInformation] ([wahoID])
GO
ALTER TABLE [dbo].[Shippers]  WITH CHECK ADD FOREIGN KEY([wahoID])
REFERENCES [dbo].[WahoInformation] ([wahoID])
GO
ALTER TABLE [dbo].[SubCategories]  WITH CHECK ADD FOREIGN KEY([categoryID])
REFERENCES [dbo].[Categories] ([categoryID])
GO
ALTER TABLE [dbo].[Suppliers]  WITH CHECK ADD FOREIGN KEY([wahoID])
REFERENCES [dbo].[WahoInformation] ([wahoID])
GO
ALTER TABLE [dbo].[WahoInformation]  WITH CHECK ADD FOREIGN KEY([categoryID])
REFERENCES [dbo].[Categories] ([categoryID])
GO
USE [master]
GO
ALTER DATABASE [WahoS8] SET  READ_WRITE 
GO
