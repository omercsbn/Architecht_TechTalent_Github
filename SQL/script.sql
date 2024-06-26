USE [master]
GO
/****** Object:  Database [FinanceManagement]    Script Date: 26.06.2024 18:44:45 ******/
CREATE DATABASE [FinanceManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FinanceManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\FinanceManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FinanceManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\FinanceManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [FinanceManagement] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FinanceManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FinanceManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FinanceManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FinanceManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FinanceManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FinanceManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [FinanceManagement] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [FinanceManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FinanceManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FinanceManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FinanceManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FinanceManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FinanceManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FinanceManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FinanceManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FinanceManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [FinanceManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FinanceManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FinanceManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FinanceManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FinanceManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FinanceManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FinanceManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FinanceManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FinanceManagement] SET  MULTI_USER 
GO
ALTER DATABASE [FinanceManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FinanceManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FinanceManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FinanceManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FinanceManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FinanceManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [FinanceManagement] SET QUERY_STORE = ON
GO
ALTER DATABASE [FinanceManagement] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [FinanceManagement]
GO
/****** Object:  Table [dbo].[account_types]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account_types](
	[id] [int] NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[interest_rate] [decimal](5, 2) NULL,
	[deposit_term] [int] NULL,
	[minimum_deposit] [decimal](10, 2) NULL,
	[interest_period] [nvarchar](50) NULL,
	[daily_withdrawal_limit] [decimal](10, 2) NULL,
	[withdrawal_fee] [decimal](10, 2) NULL,
	[minimum_balance] [decimal](10, 2) NULL,
	[special_advantages] [text] NULL,
	[atm_usage_limit] [int] NULL,
	[linked_check_account] [tinyint] NULL,
	[transaction_fee] [decimal](10, 2) NULL,
	[exchange_rate] [decimal](10, 2) NULL,
	[foreign_currency_options] [text] NULL,
	[retirement_contributions] [decimal](10, 2) NULL,
	[early_withdrawal_penalty] [decimal](10, 2) NULL,
	[tax_benefits] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[accountID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [uniqueidentifier] NOT NULL,
	[depositOption] [char](36) NULL,
	[accountName] [nvarchar](100) NOT NULL,
	[accountType] [nvarchar](20) NOT NULL,
	[balance] [decimal](10, 2) NULL,
	[availableBalance] [decimal](10, 2) NOT NULL,
	[currency] [nvarchar](3) NOT NULL,
	[openDate] [datetime2](7) NULL,
	[withdrawalAccountId] [int] NULL,
	[updatedAt] [datetime2](7) NULL,
	[Iban] [varchar](26) NULL,
PRIMARY KEY CLUSTERED 
(
	[accountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[altins]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[altins](
	[altin_id] [int] NOT NULL,
	[gram_altin_miktari] [decimal](10, 2) NOT NULL,
	[alis_fiyati] [decimal](10, 2) NOT NULL,
	[satis_fiyati] [decimal](10, 2) NOT NULL,
	[tarih] [date] NOT NULL,
	[createdAt] [datetime] NOT NULL,
	[updatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[altin_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[currency_rates]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[currency_rates](
	[id] [int] NOT NULL,
	[CurrencyCode] [nvarchar](10) NOT NULL,
	[SaleRate] [decimal](10, 4) NOT NULL,
	[PurchaseRate] [decimal](10, 4) NOT NULL,
	[ChangeAmount] [decimal](10, 4) NOT NULL,
	[ChangeRate] [decimal](10, 4) NOT NULL,
	[ChangeDirection] [nvarchar](20) NOT NULL,
	[createdAt] [datetime] NOT NULL,
	[updatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[currencyrates]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[currencyrates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyCode] [varchar](10) NOT NULL,
	[SaleRate] [decimal](10, 4) NOT NULL,
	[PurchaseRate] [decimal](10, 4) NOT NULL,
	[ChangeAmount] [decimal](10, 4) NOT NULL,
	[ChangeRate] [decimal](10, 4) NOT NULL,
	[ChangeDirection] [varchar](20) NOT NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[firms]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[firms](
	[id] [int] NOT NULL,
	[company_name] [nvarchar](50) NOT NULL,
	[subscription_id] [int] NOT NULL,
	[payment_value] [int] NOT NULL,
	[unique_company_id] [nvarchar](6) NOT NULL,
	[interest_rate_overdue_payment] [int] NOT NULL,
	[payment_status] [nvarchar](7) NOT NULL,
	[subscription_start_date] [date] NOT NULL,
	[subscription_end_date] [date] NOT NULL,
	[createdAt] [datetime] NOT NULL,
	[updatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[golds]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[golds](
	[altin_id] [int] NOT NULL,
	[gram_altin_miktari] [decimal](10, 2) NOT NULL,
	[alis_fiyati] [decimal](10, 2) NOT NULL,
	[satis_fiyati] [decimal](10, 2) NOT NULL,
	[tarih] [date] NOT NULL,
	[createdAt] [datetime] NOT NULL,
	[updatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[altin_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[invoices]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoices](
	[invoiceID] [int] IDENTITY(1,1) NOT NULL,
	[senderAccountID] [varchar](255) NOT NULL,
	[receiverAccountID] [varchar](255) NOT NULL,
	[receiverName] [varchar](255) NOT NULL,
	[transactionType] [varchar](255) NOT NULL,
	[description] [text] NULL,
	[amount] [decimal](10, 2) NOT NULL,
	[invoiceDate] [date] NOT NULL,
	[dueDate] [date] NOT NULL,
	[paid] [bit] NOT NULL,
	[date] [datetime] NOT NULL,
	[updatedAt] [datetime] NULL,
	[userID] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[invoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[settings]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[settings](
	[ayar_id] [int] NOT NULL,
	[ayar_adi] [nvarchar](100) NULL,
	[ayar_degeri] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ayar_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[SenderAccountID] [int] NOT NULL,
	[ReceiverAccountID] [int] NOT NULL,
	[Amount] [decimal](10, 2) NOT NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[TransactionDate] [datetime] NOT NULL,
	[IsHide] [bit] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 26.06.2024 18:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Name] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[Address] [nvarchar](200) NULL,
	[Phone] [nvarchar](20) NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[updatedAt] [datetime] NULL,
	[surname] [nvarchar](255) NOT NULL,
	[role] [nvarchar](5) NULL,
	[createdAt] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_currencyrates_CurrencyCode]    Script Date: 26.06.2024 18:44:45 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_currencyrates_CurrencyCode] ON [dbo].[currencyrates]
(
	[CurrencyCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0.00)) FOR [availableBalance]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (getdate()) FOR [openDate]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[altins] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[altins] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[currency_rates] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[currency_rates] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[currencyrates] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[currencyrates] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[firms] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[golds] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[golds] ADD  DEFAULT (getdate()) FOR [updatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ('User') FOR [role]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD CHECK  (([accountType]='Gold' OR [accountType]='Deposit' OR [accountType]='Checking'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([role]='Admin' OR [role]='User'))
GO
USE [master]
GO
ALTER DATABASE [FinanceManagement] SET  READ_WRITE 
GO
