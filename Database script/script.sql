USE [master]
GO
/****** Object:  Database [VoyagoDatabase]    Script Date: 11/30/2022 4:15:50 PM ******/
CREATE DATABASE [VoyagoDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'VoyagoDatabase', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\VoyagoDatabase.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'VoyagoDatabase_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\VoyagoDatabase_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [VoyagoDatabase] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VoyagoDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VoyagoDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [VoyagoDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [VoyagoDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [VoyagoDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [VoyagoDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [VoyagoDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [VoyagoDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [VoyagoDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [VoyagoDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [VoyagoDatabase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [VoyagoDatabase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [VoyagoDatabase] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [VoyagoDatabase] SET QUERY_STORE = OFF
GO
USE [VoyagoDatabase]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 11/30/2022 4:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[EmailAddress] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](25) NULL,
	[PasswordHash] [varchar](128) NOT NULL,
	[PasswordSalt] [varchar](10) NOT NULL,
	[City] [nvarchar](30) NULL,
	[ZipCode] [nvarchar](15) NULL,
	[Street] [nvarchar](60) NULL,
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/30/2022 4:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[ProductNumber] [nvarchar](25) NOT NULL,
	[ProductModelId] [int] NULL,
	[ProductModel] [nvarchar](50) NULL,
	[ListPrice] [money] NOT NULL,
	[Color] [nvarchar](15) NULL,
	[Size] [nvarchar](5) NULL,
	[Weight] [decimal](8, 2) NULL,
	[DiscountPct] [smallmoney] NULL,
	[ProductCategoryID] [int] NULL,
	[ProductCategoryName] [nvarchar](50) NULL,
	[ProductSubcategoryID] [int] NULL,
	[ProductSubCategoryName] [nvarchar](50) NULL,
	[Description] [nvarchar](400) NULL,
	[Quantity] [smallint] NULL,
	[Rating] [int] NULL,
	[PhotoID] [int] NULL,
	[ThumbNailPhoto] [varbinary](max) NULL,
	[LargePhoto] [varbinary](max) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesOrder]    Script Date: 11/30/2022 4:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesOrder](
	[ProductName] [nvarchar](50) NOT NULL,
	[Total] [money] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[ShipDate] [datetime] NULL,
	[UnitPrice] [money] NOT NULL,
	[Quantity] [smallint] NOT NULL,
	[ContactID] [int] NOT NULL,
	[LineTotal] [numeric](38, 6) NOT NULL,
	[ProductID] [int] NULL,
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[OrderNumber] [int] NULL,
 CONSTRAINT [PK_SalesOrder] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShoppingCartItem]    Script Date: 11/30/2022 4:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShoppingCartItem](
	[ShoppingCartItemID] [int] IDENTITY(1,1) NOT NULL,
	[ShoppingCartID] [nvarchar](50) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ShoppingCartItem] PRIMARY KEY CLUSTERED 
(
	[ShoppingCartItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SalesOrder]  WITH CHECK ADD  CONSTRAINT [FK_SalesOrder_Contact_ContactID] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Contact] ([ContactID])
GO
ALTER TABLE [dbo].[SalesOrder] CHECK CONSTRAINT [FK_SalesOrder_Contact_ContactID]
GO
ALTER TABLE [dbo].[SalesOrder]  WITH CHECK ADD  CONSTRAINT [FK_SalesOrder_Product_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[SalesOrder] CHECK CONSTRAINT [FK_SalesOrder_Product_ProductID]
GO
ALTER TABLE [dbo].[ShoppingCartItem]  WITH CHECK ADD  CONSTRAINT [FK_ShoppingCartItem_Product_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ShoppingCartItem] CHECK CONSTRAINT [FK_ShoppingCartItem_Product_ProductID]
GO
USE [master]
GO
ALTER DATABASE [VoyagoDatabase] SET  READ_WRITE 
GO
