USE [master]
GO
/****** Object:  Database [POS-DB]    Script Date: 3/17/2020 3:50:01 AM ******/
CREATE DATABASE [POS-DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'POS-DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\POS-DB_Primary.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'POS-DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\POS-DB_Primary.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [POS-DB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [POS-DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [POS-DB] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [POS-DB] SET ANSI_NULLS ON 
GO
ALTER DATABASE [POS-DB] SET ANSI_PADDING ON 
GO
ALTER DATABASE [POS-DB] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [POS-DB] SET ARITHABORT ON 
GO
ALTER DATABASE [POS-DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [POS-DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [POS-DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [POS-DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [POS-DB] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [POS-DB] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [POS-DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [POS-DB] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [POS-DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [POS-DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [POS-DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [POS-DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [POS-DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [POS-DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [POS-DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [POS-DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [POS-DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [POS-DB] SET RECOVERY FULL 
GO
ALTER DATABASE [POS-DB] SET  MULTI_USER 
GO
ALTER DATABASE [POS-DB] SET PAGE_VERIFY NONE  
GO
ALTER DATABASE [POS-DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [POS-DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [POS-DB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [POS-DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [POS-DB] SET QUERY_STORE = OFF
GO
USE [POS-DB]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ProductGroup_ID] [int] NOT NULL,
	[Brand_ID] [int] NULL,
	[MeasurementUnit_ID] [int] NULL,
	[Code] [int] NOT NULL,
	[Description] [text] NULL,
	[PLU] [int] NULL,
	[Image] [image] NULL,
	[Color] [varchar](50) NULL,
	[Price] [decimal](24, 2) NOT NULL,
	[Cost] [decimal](24, 2) NOT NULL,
	[IsService] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductReturn]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductReturn](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Recipt_ID] [varchar](20) NOT NULL,
	[Product_ID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[RefundAmount] [decimal](24, 2) NOT NULL,
	[TransactionTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ProductReturnView]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProductReturnView] AS
	SELECT pr.Name AS ProductName, prr.Quantity AS Quantity, prr.RefundAmount AS RefundAmount,
	prr.TransactionTime AS TransactionTime
	FROM [dbo].[ProductReturn] AS prr
	LEFT JOIN [dbo].[Product] AS pr ON prr.Product_ID = pr.ID
GO
/****** Object:  Table [dbo].[Brand]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ProductView]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ProductView] AS
	SELECT pr.ID AS ID, pr.Name AS ProductName, br.Value AS Brand, pr.Code AS Code, pr.Description AS Description,
	pr.PLU AS PLU, pr.Image AS Image, pr.Color AS Color, pr.Price AS Price,
	pr.IsService AS IsService, pr.DateCreated AS DateCreated, pr.DateUpdated AS DateUpdated
	FROM [dbo].[Product] AS pr FULL OUTER JOIN [dbo].[Brand] AS br
	ON pr.Brand_ID = br.ID
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentMethod_ID] [int] NOT NULL,
	[SubTotal] [decimal](24, 2) NOT NULL,
	[Discount] [decimal](24, 2) NOT NULL,
	[Total] [decimal](24, 2) NOT NULL,
	[Paid] [decimal](24, 2) NOT NULL,
	[Balance] [decimal](24, 2) NOT NULL,
	[TransactionTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sale]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sale](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NULL,
	[Customer_ID] [int] NULL,
	[Payment_ID] [int] NOT NULL,
	[CartDiscount] [decimal](24, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[SaleDetailView]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SaleDetailView] AS
	SELECT sl.ID AS SaleID, py.SubTotal AS SubTotal, py.Discount AS Discount, py.Total AS Total,
	py.TransactionTime AS TransactionTime
	FROM [dbo].[Sale] AS sl
	LEFT JOIN [dbo].[Payment] AS py ON sl.Payment_ID = py.ID
GO
/****** Object:  Table [dbo].[SaleProduct]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleProduct](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Sale_ID] [int] NOT NULL,
	[Product_ID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Discount] [float] NULL,
	[Price] [float] NOT NULL,
	[SubTotal] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[SaleProductView]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SaleProductView] AS
	SELECT pr.Name AS ProductName, sp.Sale_ID AS Sale_ID, sp.Quantity AS Quantity,
	sp.Discount AS Discount, sp.Price AS Price, sp.SubTotal AS SubTotal
	FROM [dbo].[SaleProduct] AS sp
	LEFT JOIN [dbo].[Product] AS pr ON sp.Product_ID = pr.ID
GO
/****** Object:  Table [dbo].[Stock]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Product_ID] [int] NOT NULL,
	[Warehouse_ID] [int] NULL,
	[Supplier_ID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NULL,
	[Company_ID] [int] NULL,
	[Address] [nvarchar](200) NULL,
	[EMail] [varchar](100) NULL,
	[Telephone] [varchar](100) NULL,
	[Comments] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Warehouse]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warehouse](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Location] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[StockView]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StockView] AS
	SELECT st.ID AS ID, st.Product_ID AS Product_ID, st.Warehouse_ID AS Warehouse_ID,
	st.Supplier_ID AS Supplier_ID, st.Quantity AS Quantity, st.Date AS Date,
	pr.Name AS ProductName,
	wh.Name AS WarehouseName,
	sp.FirstName AS SupplierFirstName
	FROM [dbo].[Stock] AS st
	LEFT JOIN [dbo].[Product] AS pr ON st.Product_ID = pr.ID
	LEFT JOIN [dbo].[Warehouse] AS wh ON st.Warehouse_ID = wh.ID
	LEFT JOIN [dbo].[Supplier] AS sp ON st.Supplier_ID = sp.ID
GO
/****** Object:  Table [dbo].[Barcode]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Barcode](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Product_ID] [int] NOT NULL,
	[Value] [varchar](128) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](200) NULL,
	[EMail] [varchar](100) NULL,
	[Telephone] [varchar](100) NULL,
	[BankDetails] [varchar](200) NULL,
	[Comments] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NULL,
	[Company_ID] [int] NULL,
	[Address] [nvarchar](200) NULL,
	[EMail] [varchar](100) NULL,
	[Telephone] [varchar](100) NULL,
	[Comments] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeasurementUnit]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeasurementUnit](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](128) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](128) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductGroup]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[ParentGroup_ID] [int] NULL,
	[Color] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipt]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipt](
	[ID] [varchar](20) NOT NULL,
	[Sale_ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 3/17/2020 3:50:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NULL,
	[UserName] [varchar](100) NOT NULL,
	[Password] [varchar](128) NOT NULL,
	[EMail] [varchar](100) NULL,
	[AccessLevel] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[PaymentMethod] ON 

INSERT [dbo].[PaymentMethod] ([ID], [Type]) VALUES (1, N'Cash')
SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF
SET IDENTITY_INSERT [dbo].[ProductGroup] ON 

INSERT [dbo].[ProductGroup] ([ID], [Name], [ParentGroup_ID], [Color]) VALUES (1, N'Products', NULL, NULL)
SET IDENTITY_INSERT [dbo].[ProductGroup] OFF
SET IDENTITY_INSERT [dbo].[Staff] ON 

INSERT [dbo].[Staff] ([ID], [FirstName], [LastName], [UserName], [Password], [EMail], [AccessLevel]) VALUES (1, N'admin', N'admin', N'admin', N'admin', N'admin@admin.com', 10)
SET IDENTITY_INSERT [dbo].[Staff] OFF
/****** Object:  Index [UQ__Product__A25C5AA725F0217F]    Script Date: 3/17/2020 3:50:01 AM ******/
ALTER TABLE [dbo].[Product] ADD UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Staff__87909B15E783CE1F]    Script Date: 3/17/2020 3:50:01 AM ******/
ALTER TABLE [dbo].[Staff] ADD UNIQUE NONCLUSTERED 
(
	[Password] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Staff__C9F28456DC3B372C]    Script Date: 3/17/2020 3:50:01 AM ******/
ALTER TABLE [dbo].[Staff] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ((1)) FOR [PaymentMethod_ID]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ((0.00)) FOR [SubTotal]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ((0.00)) FOR [Discount]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ((0.00)) FOR [Total]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ((0.00)) FOR [Paid]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ((0.00)) FOR [Balance]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ((1)) FOR [ProductGroup_ID]
GO
ALTER TABLE [dbo].[Sale] ADD  DEFAULT ((0.00)) FOR [CartDiscount]
GO
ALTER TABLE [dbo].[SaleProduct] ADD  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[SaleProduct] ADD  DEFAULT ((0.00)) FOR [Discount]
GO
ALTER TABLE [dbo].[SaleProduct] ADD  DEFAULT ((0.00)) FOR [Price]
GO
ALTER TABLE [dbo].[SaleProduct] ADD  DEFAULT ((0.00)) FOR [SubTotal]
GO
ALTER TABLE [dbo].[Stock] ADD  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Barcode]  WITH CHECK ADD  CONSTRAINT [FK_BarCode_Product] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Product] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Barcode] CHECK CONSTRAINT [FK_BarCode_Product]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Company] FOREIGN KEY([Company_ID])
REFERENCES [dbo].[Company] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Company]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_PaymentMethod] FOREIGN KEY([PaymentMethod_ID])
REFERENCES [dbo].[PaymentMethod] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_PaymentMethod]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Brand] FOREIGN KEY([Brand_ID])
REFERENCES [dbo].[Brand] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Brand]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_MeasurementUnit] FOREIGN KEY([MeasurementUnit_ID])
REFERENCES [dbo].[MeasurementUnit] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_MeasurementUnit]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductGroup] FOREIGN KEY([ProductGroup_ID])
REFERENCES [dbo].[ProductGroup] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductGroup]
GO
ALTER TABLE [dbo].[ProductReturn]  WITH CHECK ADD  CONSTRAINT [FK_ProductReturn_Product] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Product] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReturn] CHECK CONSTRAINT [FK_ProductReturn_Product]
GO
ALTER TABLE [dbo].[ProductReturn]  WITH CHECK ADD  CONSTRAINT [FK_ProductReturn_Receipt] FOREIGN KEY([Recipt_ID])
REFERENCES [dbo].[Recipt] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductReturn] CHECK CONSTRAINT [FK_ProductReturn_Receipt]
GO
ALTER TABLE [dbo].[Recipt]  WITH CHECK ADD  CONSTRAINT [FK_Recipt_Sale] FOREIGN KEY([Sale_ID])
REFERENCES [dbo].[Sale] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Recipt] CHECK CONSTRAINT [FK_Recipt_Sale]
GO
ALTER TABLE [dbo].[Sale]  WITH CHECK ADD  CONSTRAINT [FK_Sale_Customer] FOREIGN KEY([Customer_ID])
REFERENCES [dbo].[Customer] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Sale] CHECK CONSTRAINT [FK_Sale_Customer]
GO
ALTER TABLE [dbo].[Sale]  WITH CHECK ADD  CONSTRAINT [FK_Sale_Payment] FOREIGN KEY([Payment_ID])
REFERENCES [dbo].[Payment] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sale] CHECK CONSTRAINT [FK_Sale_Payment]
GO
ALTER TABLE [dbo].[Sale]  WITH CHECK ADD  CONSTRAINT [FK_Sale_User] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Staff] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Sale] CHECK CONSTRAINT [FK_Sale_User]
GO
ALTER TABLE [dbo].[SaleProduct]  WITH CHECK ADD  CONSTRAINT [FK_SaleProduct_Product] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Product] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SaleProduct] CHECK CONSTRAINT [FK_SaleProduct_Product]
GO
ALTER TABLE [dbo].[SaleProduct]  WITH CHECK ADD  CONSTRAINT [FK_SaleProduct_Sale] FOREIGN KEY([Sale_ID])
REFERENCES [dbo].[Sale] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SaleProduct] CHECK CONSTRAINT [FK_SaleProduct_Sale]
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [FK_Stock_Product] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Product] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [FK_Stock_Product]
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [FK_Stock_Supplier] FOREIGN KEY([Supplier_ID])
REFERENCES [dbo].[Supplier] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [FK_Stock_Supplier]
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [FK_Stock_Warehouse] FOREIGN KEY([Warehouse_ID])
REFERENCES [dbo].[Warehouse] ([ID])
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [FK_Stock_Warehouse]
GO
ALTER TABLE [dbo].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_Company] FOREIGN KEY([Company_ID])
REFERENCES [dbo].[Company] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Supplier] CHECK CONSTRAINT [FK_Supplier_Company]
GO
USE [master]
GO
ALTER DATABASE [POS-DB] SET  READ_WRITE 
GO
