USE [master]
GO
/****** Object:  Database [ServerMonitor]    Script Date: 17/05/2018 21:20:45 ******/
CREATE DATABASE [ServerMonitor]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ServerMonitor', FILENAME = N'e:\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\ServerMonitor.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ServerMonitor_log', FILENAME = N'e:\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\ServerMonitor_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ServerMonitor] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ServerMonitor].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ServerMonitor] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ServerMonitor] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ServerMonitor] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ServerMonitor] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ServerMonitor] SET ARITHABORT OFF 
GO
ALTER DATABASE [ServerMonitor] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ServerMonitor] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ServerMonitor] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ServerMonitor] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ServerMonitor] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ServerMonitor] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ServerMonitor] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ServerMonitor] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ServerMonitor] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ServerMonitor] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ServerMonitor] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ServerMonitor] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ServerMonitor] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ServerMonitor] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ServerMonitor] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ServerMonitor] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ServerMonitor] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ServerMonitor] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ServerMonitor] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ServerMonitor] SET  MULTI_USER 
GO
ALTER DATABASE [ServerMonitor] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ServerMonitor] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ServerMonitor] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ServerMonitor] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [ServerMonitor]
GO
/****** Object:  StoredProcedure [dbo].[InsertDiskStats]    Script Date: 17/05/2018 21:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertDiskStats]
	-- Add the parameters for the stored procedure here
	@serverguid uniqueidentifier,
	@driveletter nchar,
	@totalsize float,
	@freespace float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO server_disks(server_guid,
	                         stats_datetime,
							 driveletter,
							 total_size,
							 free_space)
	VALUES(@serverguid,
	       getdate(),
		   @driveletter,
		   @totalsize,
		   @freespace);
END

GO
/****** Object:  StoredProcedure [dbo].[InsertStats]    Script Date: 17/05/2018 21:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertStats]
	-- Add the parameters for the stored procedure here
	@serverguid uniqueidentifier,
	@cpuusage float,
	@memoryusage float,
	@memoryavailable float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO server_stats(server_guid,
	                         stats_datetime,
							 cpu_usage,
							 memory_usage,
							 memory_available)
	VALUES(@serverguid,
	       getdate(),
		   @cpuusage,
		   @memoryusage,
		   @memoryavailable);
							 
END

GO
/****** Object:  Table [dbo].[Monitor_CPU]    Script Date: 17/05/2018 21:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Monitor_CPU](
	[Name] [nvarchar](200) NOT NULL,
	[BusyTime] [int] NOT NULL,
	[ResfreshTime] [date] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Server_Disks]    Script Date: 17/05/2018 21:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Server_Disks](
	[Server_Guid] [uniqueidentifier] NOT NULL,
	[DriveLetter] [nchar](1) NOT NULL,
	[Total_Size] [float] NOT NULL,
	[Free_Space] [float] NOT NULL,
	[stats_datetime] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Server_Header]    Script Date: 17/05/2018 21:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Server_Header](
	[Server_GUID] [uniqueidentifier] NOT NULL,
	[OS] [nvarchar](200) NULL,
	[CPUType] [nvarchar](200) NULL,
	[LastBoot] [date] NULL,
	[ServerName] [nvarchar](200) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Server_Stats]    Script Date: 17/05/2018 21:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Server_Stats](
	[server_guid] [uniqueidentifier] NOT NULL,
	[Stats_Datetime] [datetime] NOT NULL,
	[CPU_Usage] [float] NULL,
	[Memory_Usage] [float] NULL,
	[Memory_Available] [float] NULL
) ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [ServerMonitor] SET  READ_WRITE 
GO
