USE [ServerMonitor]
GO
/****** Object:  StoredProcedure [dbo].[InsertDiskStats]    Script Date: 05/09/2018 11:40:38 ******/
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
/****** Object:  StoredProcedure [dbo].[InsertStats]    Script Date: 05/09/2018 11:40:38 ******/
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
/****** Object:  Table [dbo].[Monitor_CPU]    Script Date: 05/09/2018 11:40:38 ******/
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
/****** Object:  Table [dbo].[Server_Disks]    Script Date: 05/09/2018 11:40:38 ******/
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
/****** Object:  Table [dbo].[Server_Header]    Script Date: 05/09/2018 11:40:38 ******/
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
/****** Object:  Table [dbo].[Server_Stats]    Script Date: 05/09/2018 11:40:38 ******/
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
