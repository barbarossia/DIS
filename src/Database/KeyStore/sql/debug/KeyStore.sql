/*
Deployment script for OemKeyStore
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "OemKeyStore"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
USE [master]
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL
    AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Creating $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)]
    ON 
    PRIMARY(NAME = [KeyStore], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', SIZE = 51456 KB, FILEGROWTH = 1024 KB)
    LOG ON (NAME = [KeyStore_log], FILENAME = '$(DefaultLogPath)$(DatabaseName)_log.ldf', SIZE = 12352 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %) COLLATE SQL_Latin1_General_CP1_CI_AS
GO
EXECUTE sp_dbcmptlevel [$(DatabaseName)], 100;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CLOSE OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
USE [$(DatabaseName)]
GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
PRINT N'Creating [dbo].[Category]...';


GO
SET QUOTED_IDENTIFIER ON;

SET ANSI_NULLS OFF;


GO
CREATE TABLE [dbo].[Category] (
    [CategoryID]   INT           IDENTITY (1, 1) NOT NULL,
    [CategoryName] NVARCHAR (64) NOT NULL
);


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Creating PK_Categories...';


GO
ALTER TABLE [dbo].[Category]
    ADD CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[CategoryLog]...';


GO
SET QUOTED_IDENTIFIER ON;

SET ANSI_NULLS OFF;


GO
CREATE TABLE [dbo].[CategoryLog] (
    [CategoryLogID] INT IDENTITY (1, 1) NOT NULL,
    [CategoryID]    INT NOT NULL,
    [LogID]         INT NOT NULL
);


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Creating PK_CategoryLog...';


GO
ALTER TABLE [dbo].[CategoryLog]
    ADD CONSTRAINT [PK_CategoryLog] PRIMARY KEY CLUSTERED ([CategoryLogID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[ComputerBuildReport]...';


GO
CREATE TABLE [dbo].[ComputerBuildReport] (
    [MSReportUniqueID]       UNIQUEIDENTIFIER NULL,
    [CustomerReportUniqueID] UNIQUEIDENTIFIER NOT NULL,
    [MSReceivedDateUTC]      DATETIME         NULL,
    [SoldToCustomerID]       NVARCHAR (10)    COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
    [ReceivedFromCustomerID] NVARCHAR (10)    COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
    [CBRAckFileTotal]        INT              NULL,
    [CBRAckFileNumber]       INT              NULL,
    [CBRStatus]              INT              NOT NULL,
    [CreatedDateUTC]         DATETIME         NOT NULL,
    [ModifiedDateUTC]        DATETIME         NOT NULL
);


GO
PRINT N'Creating PK_ComputerBuildReport...';


GO
ALTER TABLE [dbo].[ComputerBuildReport]
    ADD CONSTRAINT [PK_ComputerBuildReport] PRIMARY KEY CLUSTERED ([CustomerReportUniqueID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[ComputerBuildReportKey]...';


GO
CREATE TABLE [dbo].[ComputerBuildReportKey] (
    [CustomerReportUniqueID] UNIQUEIDENTIFIER NOT NULL,
    [ProductKeyID]           BIGINT           NOT NULL,
    [HardwareHash]           NVARCHAR (512)   NOT NULL,
    [OEMOptionalInfo]        XML              NULL,
    [ReasonCode]             NVARCHAR (2)     NULL,
    [ReasonCodeDescription]  NVARCHAR (160)   NULL
);


GO
PRINT N'Creating PK_ComputerBuildReportKey...';


GO
ALTER TABLE [dbo].[ComputerBuildReportKey]
    ADD CONSTRAINT [PK_ComputerBuildReportKey] PRIMARY KEY CLUSTERED ([CustomerReportUniqueID] ASC, [ProductKeyID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Configuration]...';


GO
CREATE TABLE [dbo].[Configuration] (
    [ConfigurationID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (50) NOT NULL,
    [Value]           XML           NOT NULL,
    [Type]            NVARCHAR (50) NOT NULL
);


GO
PRINT N'Creating PK_Configuration...';


GO
ALTER TABLE [dbo].[Configuration]
    ADD CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([ConfigurationID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[DuplicatedComputerBuildReport]...';


GO
CREATE TABLE [dbo].[DuplicatedComputerBuildReport] (
    [CustomerReportUniqueID] UNIQUEIDENTIFIER NOT NULL,
    [IsExported]             BIT              NOT NULL
);


GO
PRINT N'Creating PK_DuplicatedComputerBuildReport...';


GO
ALTER TABLE [dbo].[DuplicatedComputerBuildReport]
    ADD CONSTRAINT [PK_DuplicatedComputerBuildReport] PRIMARY KEY CLUSTERED ([CustomerReportUniqueID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[DuplicatedComputerBuildReport].[IX_DuplicatedComputerBuildReport_IsExported]...';


GO
CREATE NONCLUSTERED INDEX [IX_DuplicatedComputerBuildReport_IsExported]
    ON [dbo].[DuplicatedComputerBuildReport]([IsExported] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[DuplicatedKey]...';


GO
CREATE TABLE [dbo].[DuplicatedKey] (
    [DuplicatedKeyID] INT           IDENTITY (1, 1) NOT NULL,
    [ProductKeyID]    BIGINT        NOT NULL,
    [ProductKey]      NVARCHAR (29) NOT NULL,
    [Handled]         BIT           NOT NULL,
    [OperationID]     INT           NULL
);


GO
PRINT N'Creating PK_DuplicatedKey...';


GO
ALTER TABLE [dbo].[DuplicatedKey]
    ADD CONSTRAINT [PK_DuplicatedKey] PRIMARY KEY CLUSTERED ([DuplicatedKeyID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[DuplicatedKey].[IX_DuplicatedKey_Handled]...';


GO
CREATE NONCLUSTERED INDEX [IX_DuplicatedKey_Handled]
    ON [dbo].[DuplicatedKey]([Handled] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[FulfillmentInfo]...';


GO
CREATE TABLE [dbo].[FulfillmentInfo] (
    [FulfillmentNumber] CHAR (10)        NOT NULL,
    [SoldToCustomerID]  CHAR (10)        COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
    [OrderUniqueID]     UNIQUEIDENTIFIER NOT NULL,
    [FulfillmentStatus] TINYINT          NOT NULL,
    [ResponseData]      NVARCHAR (MAX)   NULL,
    [CreatedDateUTC]    DATETIME         NOT NULL,
    [ModifiedDateUTC]   DATETIME         NOT NULL
);


GO
PRINT N'Creating PK_FulfillmentInfo...';


GO
ALTER TABLE [dbo].[FulfillmentInfo]
    ADD CONSTRAINT [PK_FulfillmentInfo] PRIMARY KEY CLUSTERED ([FulfillmentNumber] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[HeadQuarter]...';


GO
CREATE TABLE [dbo].[HeadQuarter] (
    [HeadQuarterID]     INT            IDENTITY (1, 1) NOT NULL,
    [DisplayName]       NVARCHAR (20)  NULL,
    [CertSubject]       NVARCHAR (128) NULL,
    [CertThumbPrint]    NVARCHAR (128) NULL,
    [ServiceHostUrl]    NVARCHAR (200) NULL,
    [UserName]          NVARCHAR (10)  COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
    [AccessKey]         NVARCHAR (50)  NULL,
    [Description]       NVARCHAR (50)  NULL,
    [IsCentralizedMode] BIT            NOT NULL,
    [IsCarbonCopy]      BIT            NOT NULL
);


GO
PRINT N'Creating PK_HeadQuarter...';


GO
ALTER TABLE [dbo].[HeadQuarter]
    ADD CONSTRAINT [PK_HeadQuarter] PRIMARY KEY CLUSTERED ([HeadQuarterID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[KeyExportLog]...';


GO
CREATE TABLE [dbo].[KeyExportLog] (
    [ExportLogID] INT            IDENTITY (1, 1) NOT NULL,
    [ExportTo]    NVARCHAR (20)  NOT NULL,
    [ExportType]  NVARCHAR (20)  NOT NULL,
    [KeyCount]    INT            NOT NULL,
    [FileName]    NVARCHAR (300) NOT NULL,
    [IsEncrypted] BIT            NOT NULL,
    [FileContent] XML            NOT NULL,
    [CreateBy]    NVARCHAR (50)  NOT NULL,
    [CreateDate]  DATETIME       NOT NULL
);


GO
PRINT N'Creating PK_KeyExportLog...';


GO
ALTER TABLE [dbo].[KeyExportLog]
    ADD CONSTRAINT [PK_KeyExportLog] PRIMARY KEY CLUSTERED ([ExportLogID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[KeyHistory]...';


GO
SET QUOTED_IDENTIFIER ON;

SET ANSI_NULLS OFF;


GO
CREATE TABLE [dbo].[KeyHistory] (
    [ProductKeyID]      BIGINT   NOT NULL,
    [ProductKeyStateID] TINYINT  NOT NULL,
    [StateChangeDate]   DATETIME NOT NULL
);


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Creating PK_KeyHistory...';


GO
ALTER TABLE [dbo].[KeyHistory]
    ADD CONSTRAINT [PK_KeyHistory] PRIMARY KEY CLUSTERED ([ProductKeyID] ASC, [ProductKeyStateID] ASC, [StateChangeDate] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[KeyInfoEx]...';


GO
CREATE TABLE [dbo].[KeyInfoEx] (
    [ProductKeyID]     BIGINT NOT NULL,
    [KeyType]          INT    NULL,
    [SSID]             INT    NULL,
    [HQID]             INT    NULL,
    [IsInProgress]     BIT    NOT NULL,
    [ShouldCarbonCopy] BIT    NULL
);


GO
PRINT N'Creating PK_KeyInfoEx...';


GO
ALTER TABLE [dbo].[KeyInfoEx]
    ADD CONSTRAINT [PK_KeyInfoEx] PRIMARY KEY CLUSTERED ([ProductKeyID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[KeyInfoEx].[IX_KeyInfoEx_HQID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KeyInfoEx_HQID]
    ON [dbo].[KeyInfoEx]([HQID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[KeyInfoEx].[IX_KeyInfoEx_IsInProgress]...';


GO
CREATE NONCLUSTERED INDEX [IX_KeyInfoEx_IsInProgress]
    ON [dbo].[KeyInfoEx]([IsInProgress] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[KeyInfoEx].[IX_KeyInfoEx_KeyType]...';


GO
CREATE NONCLUSTERED INDEX [IX_KeyInfoEx_KeyType]
    ON [dbo].[KeyInfoEx]([KeyType] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[KeyInfoEx].[IX_KeyInfoEx_SSID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KeyInfoEx_SSID]
    ON [dbo].[KeyInfoEx]([SSID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[KeyOperationHistory]...';


GO
CREATE TABLE [dbo].[KeyOperationHistory] (
    [OperationID]  INT            IDENTITY (1, 1) NOT NULL,
    [ProductKeyID] BIGINT         NOT NULL,
    [ProductKey]   NVARCHAR (29)  NOT NULL,
    [HardwareHash] NVARCHAR (512) NULL,
    [KeyStateFrom] TINYINT        NOT NULL,
    [KeyStateTo]   TINYINT        NOT NULL,
    [Operator]     NVARCHAR (20)  NOT NULL,
    [Message]      NVARCHAR (200) NOT NULL,
    [CreatedDate]  DATETIME       NULL
);


GO
PRINT N'Creating PK_KeyOperationHistory...';


GO
ALTER TABLE [dbo].[KeyOperationHistory]
    ADD CONSTRAINT [PK_KeyOperationHistory] PRIMARY KEY CLUSTERED ([OperationID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[KeyOperationHistory].[IX_KeyOperationHistory_ProductKey]...';


GO
CREATE NONCLUSTERED INDEX [IX_KeyOperationHistory_ProductKey]
    ON [dbo].[KeyOperationHistory]([ProductKey] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[KeySyncNotification]...';


GO
CREATE TABLE [dbo].[KeySyncNotification] (
    [ProductKeyID]      BIGINT   NOT NULL,
    [ProductKeyStateID] TINYINT  NOT NULL,
    [CreateDate]        DATETIME NOT NULL
);


GO
PRINT N'Creating PK_KeySyncNotification...';


GO
ALTER TABLE [dbo].[KeySyncNotification]
    ADD CONSTRAINT [PK_KeySyncNotification] PRIMARY KEY CLUSTERED ([ProductKeyID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[KeyTypeConfiguration]...';


GO
CREATE TABLE [dbo].[KeyTypeConfiguration] (
    [KeyTypeConfigurationId] INT           IDENTITY (1, 1) NOT NULL,
    [HeadQuarterId]          INT           NULL,
    [LicensablePartNumber]   NVARCHAR (16) NOT NULL,
    [Maximum]                INT           NULL,
    [Minimum]                INT           NULL,
    [KeyType]                INT           NULL
);


GO
PRINT N'Creating [dbo].[Log]...';


GO
SET QUOTED_IDENTIFIER ON;

SET ANSI_NULLS OFF;


GO
CREATE TABLE [dbo].[Log] (
    [LogID]            INT             IDENTITY (1, 1) NOT NULL,
    [EventID]          INT             NULL,
    [Priority]         INT             NOT NULL,
    [Severity]         NVARCHAR (32)   NOT NULL,
    [Title]            NVARCHAR (256)  NOT NULL,
    [Timestamp]        DATETIME        NOT NULL,
    [MachineName]      NVARCHAR (32)   NOT NULL,
    [AppDomainName]    NVARCHAR (512)  NOT NULL,
    [ProcessID]        NVARCHAR (256)  NOT NULL,
    [ProcessName]      NVARCHAR (512)  NOT NULL,
    [ThreadName]       NVARCHAR (512)  NULL,
    [Win32ThreadId]    NVARCHAR (128)  NULL,
    [Message]          NVARCHAR (1500) NULL,
    [FormattedMessage] NTEXT           NULL
);


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Creating PK_Log...';


GO
ALTER TABLE [dbo].[Log]
    ADD CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([LogID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Log].[IX_Log_Severity]...';


GO
CREATE NONCLUSTERED INDEX [IX_Log_Severity]
    ON [dbo].[Log]([Severity] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[Log].[IX_Log_Timestamp]...';


GO
CREATE NONCLUSTERED INDEX [IX_Log_Timestamp]
    ON [dbo].[Log]([Timestamp] DESC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[Log].[IX_Log_Title]...';


GO
CREATE NONCLUSTERED INDEX [IX_Log_Title]
    ON [dbo].[Log]([Title] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo]...';


GO
CREATE TABLE [dbo].[ProductKeyInfo] (
    [ProductKeyID]               BIGINT           NOT NULL,
    [ProductKey]                 NVARCHAR (50)    NULL,
    [ProductKeyStateID]          TINYINT          NOT NULL,
    [ProductKeyState]            NVARCHAR (20)    NULL,
    [HardwareID]                 NVARCHAR (512)   NULL,
    [OEMPartNumber]              NVARCHAR (35)    NULL,
    [SoldToCustomerName]         NVARCHAR (80)    NULL,
    [OrderUniqueID]              UNIQUEIDENTIFIER NULL,
    [SoldToCustomerID]           CHAR (10)        COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
    [CallOffReferenceNumber]     NVARCHAR (35)    NULL,
    [OEMPONumber]                NVARCHAR (35)    NULL,
    [MSOrderNumber]              NVARCHAR (10)    NULL,
    [LicensablePartNumber]       NVARCHAR (16)    NULL,
    [Quantity]                   INT              NULL,
    [SKUID]                      NVARCHAR (50)    NULL,
    [ReturnReasonCode]           NVARCHAR (10)    NULL,
    [CreatedDate]                DATETIME         NULL,
    [ModifiedDate]               DATETIME         NULL,
    [MSOrderLineNumber]          INT              NULL,
    [OEMPODateUTC]               DATETIME         NULL,
    [ShipToCustomerID]           CHAR (10)        COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
    [ShipToCustomerName]         NVARCHAR (80)    NULL,
    [LicensableName]             NVARCHAR (40)    NULL,
    [OEMPOLineNumber]            NVARCHAR (6)     NULL,
    [CallOffLineNumber]          NVARCHAR (6)     NULL,
    [FulfillmentResendIndicator] BIT              NULL,
    [FulfillmentNumber]          CHAR (10)        NULL,
    [FulfilledDateUTC]           DATETIME         NULL,
    [FulfillmentCreateDateUTC]   DATETIME         NULL,
    [EndItemPartNumber]          NVARCHAR (18)    NULL,
    [ZPC_MODEL_SKU]              NVARCHAR (64)    NULL,
    [ZMANUF_GEO_LOC]             NVARCHAR (10)    NULL,
    [ZPGM_ELIG_VALUES]           NVARCHAR (48)    NULL,
    [ZOEM_EXT_ID]                NVARCHAR (16)    NULL,
    [ZCHANNEL_REL_ID]            NVARCHAR (32)    NULL,
    [TrackingInfo]               NVARCHAR (1024)  NULL
);


GO
PRINT N'Creating PK_ProductKeyInfo...';


GO
ALTER TABLE [dbo].[ProductKeyInfo]
    ADD CONSTRAINT [PK_ProductKeyInfo] PRIMARY KEY CLUSTERED ([ProductKeyID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_FulfilledDateUTC]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_FulfilledDateUTC]
    ON [dbo].[ProductKeyInfo]([FulfilledDateUTC] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_LicensablePartNumber]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_LicensablePartNumber]
    ON [dbo].[ProductKeyInfo]([LicensablePartNumber] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_MSOrderNumber]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_MSOrderNumber]
    ON [dbo].[ProductKeyInfo]([MSOrderNumber] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_OEMPartNumber]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_OEMPartNumber]
    ON [dbo].[ProductKeyInfo]([OEMPartNumber] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_OEMPONumber]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_OEMPONumber]
    ON [dbo].[ProductKeyInfo]([OEMPONumber] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_ProductKey]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductKeyInfo_ProductKey]
    ON [dbo].[ProductKeyInfo]([ProductKey] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_ProductKeyStateID]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ProductKeyStateID]
    ON [dbo].[ProductKeyInfo]([ProductKeyStateID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_ZCHANNEL_REL_ID]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZCHANNEL_REL_ID]
    ON [dbo].[ProductKeyInfo]([ZCHANNEL_REL_ID] ASC) WHERE ([ZCHANNEL_REL_ID] IS NOT NULL) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_ZMANUF_GEO_LOC]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZMANUF_GEO_LOC]
    ON [dbo].[ProductKeyInfo]([ZMANUF_GEO_LOC] ASC) WHERE ([ZMANUF_GEO_LOC] IS NOT NULL) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_ZOEM_EXT_ID]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZOEM_EXT_ID]
    ON [dbo].[ProductKeyInfo]([ZOEM_EXT_ID] ASC) WHERE ([ZOEM_EXT_ID] IS NOT NULL) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_ZPC_MODEL_SKU]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZPC_MODEL_SKU]
    ON [dbo].[ProductKeyInfo]([ZPC_MODEL_SKU] ASC) WHERE ([ZPC_MODEL_SKU] IS NOT NULL) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ProductKeyInfo].[IX_ProductKeyInfo_ZPGM_ELIG_VALUES]...';


GO
CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZPGM_ELIG_VALUES]
    ON [dbo].[ProductKeyInfo]([ZPGM_ELIG_VALUES] ASC) WHERE ([ZPGM_ELIG_VALUES] IS NOT NULL) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[ReturnReport]...';


GO
CREATE TABLE [dbo].[ReturnReport] (
    [CustomerReturnUniqueID] UNIQUEIDENTIFIER NOT NULL,
    [ReturnUniqueID]         UNIQUEIDENTIFIER NULL,
    [MSReturnNumber]         NCHAR (10)       NULL,
    [ReturnDateUTC]          DATETIME         NULL,
    [OEMRMADateUTC]          DATETIME         NULL,
    [OEMRMANumber]           NVARCHAR (35)    NOT NULL,
    [SoldToCustomerName]     NVARCHAR (40)    NULL,
    [OEMRMADate]             DATETIME         NOT NULL,
    [SoldToCustomerID]       NVARCHAR (10)    NOT NULL,
    [ReturnNoCredit]         BIT              NOT NULL,
    [ReportStatus]           INT              NOT NULL
);


GO
PRINT N'Creating PK_ReturnReport...';


GO
ALTER TABLE [dbo].[ReturnReport]
    ADD CONSTRAINT [PK_ReturnReport] PRIMARY KEY CLUSTERED ([CustomerReturnUniqueID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[ReturnReportKey]...';


GO
CREATE TABLE [dbo].[ReturnReportKey] (
    [CustomerReturnUniqueID]      UNIQUEIDENTIFIER NOT NULL,
    [OEMRMALineNumber]            INT              NOT NULL,
    [ReturnTypeID]                NCHAR (3)        NOT NULL,
    [ProductKeyID]                BIGINT           NOT NULL,
    [MSReturnLineNumber]          INT              NULL,
    [LicensablePartNumber]        NVARCHAR (16)    NULL,
    [ReturnReasonCode]            NVARCHAR (2)     NULL,
    [ReturnReasonCodeDescription] NVARCHAR (40)    NULL,
    [PreProductKeyStateID]        TINYINT          NOT NULL
);


GO
PRINT N'Creating PK_ReturnReportKey...';


GO
ALTER TABLE [dbo].[ReturnReportKey]
    ADD CONSTRAINT [PK_ReturnReportKey] PRIMARY KEY CLUSTERED ([CustomerReturnUniqueID] ASC, [ProductKeyID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Role]...';


GO
CREATE TABLE [dbo].[Role] (
    [RoleID]      INT            IDENTITY (1, 1) NOT NULL,
    [RoleName]    NVARCHAR (20)  NOT NULL,
    [Description] NVARCHAR (200) NULL
);


GO
PRINT N'Creating PK_Role...';


GO
ALTER TABLE [dbo].[Role]
    ADD CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([RoleID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[Subsidiary]...';


GO
CREATE TABLE [dbo].[Subsidiary] (
    [SSID]           INT            IDENTITY (1, 1) NOT NULL,
    [DisplayName]    NVARCHAR (20)  NULL,
    [ServiceHostUrl] NVARCHAR (200) NULL,
    [UserName]       NVARCHAR (10)  COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
    [AccessKey]      NVARCHAR (50)  NULL,
    [Type]           NVARCHAR (20)  NOT NULL,
    [Description]    NVARCHAR (50)  NULL
);


GO
PRINT N'Creating PK_Subsidiary...';


GO
ALTER TABLE [dbo].[Subsidiary]
    ADD CONSTRAINT [PK_Subsidiary] PRIMARY KEY CLUSTERED ([SSID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[User]...';


GO
CREATE TABLE [dbo].[User] (
    [UserID]      INT            IDENTITY (1, 1) NOT NULL,
    [Password]    NVARCHAR (128) NOT NULL,
    [PasswordRev] INT            NOT NULL,
    [Salt]        CHAR (10)      NULL,
    [LoginID]     NVARCHAR (20)  NOT NULL,
    [Department]  NVARCHAR (50)  NULL,
    [Phone]       NVARCHAR (20)  NULL,
    [Email]       NVARCHAR (50)  NULL,
    [CreateDate]  DATETIME       NOT NULL,
    [UpdateDate]  DATETIME       NOT NULL,
    [FirstName]   NVARCHAR (20)  NULL,
    [SecondName]  NVARCHAR (20)  NULL,
    [Position]    NVARCHAR (20)  NULL,
    [Language]    NVARCHAR (15)  NULL
);


GO
PRINT N'Creating PK_User...';


GO
ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[User].[IX_User_LoginID]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_LoginID]
    ON [dbo].[User]([LoginID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[UserHeadQuarter]...';


GO
CREATE TABLE [dbo].[UserHeadQuarter] (
    [UserID]        INT NOT NULL,
    [HeadQuarterID] INT NOT NULL,
    [IsDefault]     BIT NOT NULL
);


GO
PRINT N'Creating PK_UserHeadQuarter...';


GO
ALTER TABLE [dbo].[UserHeadQuarter]
    ADD CONSTRAINT [PK_UserHeadQuarter] PRIMARY KEY CLUSTERED ([UserID] ASC, [HeadQuarterID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating [dbo].[UserRole]...';


GO
CREATE TABLE [dbo].[UserRole] (
    [UserID] INT NOT NULL,
    [RoleID] INT NOT NULL
);


GO
PRINT N'Creating PK_UserRole...';


GO
ALTER TABLE [dbo].[UserRole]
    ADD CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([UserID] ASC, [RoleID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


GO
PRINT N'Creating DF_User_CreateDate...';


GO
ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [DF_User_CreateDate] DEFAULT (((1900)-(1))-(1)) FOR [CreateDate];


GO
PRINT N'Creating DF_User_UpdateDate...';


GO
ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [DF_User_UpdateDate] DEFAULT (((1900)-(1))-(1)) FOR [UpdateDate];


GO
PRINT N'Creating FK_CategoryLog_Category...';


GO
ALTER TABLE [dbo].[CategoryLog] WITH NOCHECK
    ADD CONSTRAINT [FK_CategoryLog_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([CategoryID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_CategoryLog_Log...';


GO
ALTER TABLE [dbo].[CategoryLog] WITH NOCHECK
    ADD CONSTRAINT [FK_CategoryLog_Log] FOREIGN KEY ([LogID]) REFERENCES [dbo].[Log] ([LogID]) ON DELETE CASCADE ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_ComputerBuildReportKey_ComputerBuildReport...';


GO
ALTER TABLE [dbo].[ComputerBuildReportKey] WITH NOCHECK
    ADD CONSTRAINT [FK_ComputerBuildReportKey_ComputerBuildReport] FOREIGN KEY ([CustomerReportUniqueID]) REFERENCES [dbo].[ComputerBuildReport] ([CustomerReportUniqueID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_ComputerBuildReportKey_ProductKeyInfo...';


GO
ALTER TABLE [dbo].[ComputerBuildReportKey] WITH NOCHECK
    ADD CONSTRAINT [FK_ComputerBuildReportKey_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_DuplicatedComputerBuildReport_ComputerBuildReport...';


GO
ALTER TABLE [dbo].[DuplicatedComputerBuildReport] WITH NOCHECK
    ADD CONSTRAINT [FK_DuplicatedComputerBuildReport_ComputerBuildReport] FOREIGN KEY ([CustomerReportUniqueID]) REFERENCES [dbo].[ComputerBuildReport] ([CustomerReportUniqueID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_DuplicatedKey_KeyOperationHistory...';


GO
ALTER TABLE [dbo].[DuplicatedKey] WITH NOCHECK
    ADD CONSTRAINT [FK_DuplicatedKey_KeyOperationHistory] FOREIGN KEY ([OperationID]) REFERENCES [dbo].[KeyOperationHistory] ([OperationID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_DuplicatedKey_ProductKeyInfo...';


GO
ALTER TABLE [dbo].[DuplicatedKey] WITH NOCHECK
    ADD CONSTRAINT [FK_DuplicatedKey_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_KeyHistory_ProductKeyInfo...';


GO
ALTER TABLE [dbo].[KeyHistory] WITH NOCHECK
    ADD CONSTRAINT [FK_KeyHistory_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_KeyInfoEx_HeadQuarter...';


GO
ALTER TABLE [dbo].[KeyInfoEx] WITH NOCHECK
    ADD CONSTRAINT [FK_KeyInfoEx_HeadQuarter] FOREIGN KEY ([HQID]) REFERENCES [dbo].[HeadQuarter] ([HeadQuarterID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_KeyInfoEx_ProductKeyInfo...';


GO
ALTER TABLE [dbo].[KeyInfoEx] WITH NOCHECK
    ADD CONSTRAINT [FK_KeyInfoEx_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_KeyInfoEx_Subsidiary...';


GO
ALTER TABLE [dbo].[KeyInfoEx] WITH NOCHECK
    ADD CONSTRAINT [FK_KeyInfoEx_Subsidiary] FOREIGN KEY ([SSID]) REFERENCES [dbo].[Subsidiary] ([SSID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_KeyOperationHistory_ProductKeyInfo...';


GO
ALTER TABLE [dbo].[KeyOperationHistory] WITH NOCHECK
    ADD CONSTRAINT [FK_KeyOperationHistory_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_KeySyncNotification_ProductKeyInfo...';


GO
ALTER TABLE [dbo].[KeySyncNotification] WITH NOCHECK
    ADD CONSTRAINT [FK_KeySyncNotification_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_ReturnReportKey_ProductKeyInfo...';


GO
ALTER TABLE [dbo].[ReturnReportKey] WITH NOCHECK
    ADD CONSTRAINT [FK_ReturnReportKey_ProductKeyInfo] FOREIGN KEY ([ProductKeyID]) REFERENCES [dbo].[ProductKeyInfo] ([ProductKeyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_ReturnReportKey_ReturnReport...';


GO
ALTER TABLE [dbo].[ReturnReportKey] WITH NOCHECK
    ADD CONSTRAINT [FK_ReturnReportKey_ReturnReport] FOREIGN KEY ([CustomerReturnUniqueID]) REFERENCES [dbo].[ReturnReport] ([CustomerReturnUniqueID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_UserHeadQuarter_HeadQuarter...';


GO
ALTER TABLE [dbo].[UserHeadQuarter] WITH NOCHECK
    ADD CONSTRAINT [FK_UserHeadQuarter_HeadQuarter] FOREIGN KEY ([HeadQuarterID]) REFERENCES [dbo].[HeadQuarter] ([HeadQuarterID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_UserHeadQuarter_User...';


GO
ALTER TABLE [dbo].[UserHeadQuarter] WITH NOCHECK
    ADD CONSTRAINT [FK_UserHeadQuarter_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_UserRole_Role...';


GO
ALTER TABLE [dbo].[UserRole] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role] ([RoleID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_UserRole_User...';


GO
ALTER TABLE [dbo].[UserRole] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating [dbo].[ClearLogs]...';


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER OFF;


GO
CREATE PROCEDURE [dbo].[ClearLogs]
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM CategoryLog
	DELETE FROM [Log]
	DELETE FROM Category
END
GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Creating [dbo].[InsertCategoryLog]...';


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER OFF;


GO
CREATE PROCEDURE [dbo].[InsertCategoryLog]
	@CategoryID INT,
	@LogID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CatLogID INT
	SELECT @CatLogID FROM CategoryLog WHERE CategoryID=@CategoryID and LogID = @LogID
	IF @CatLogID IS NULL
	BEGIN
		INSERT INTO CategoryLog (CategoryID, LogID) VALUES(@CategoryID, @LogID)
		RETURN @@IDENTITY
	END
	ELSE RETURN @CatLogID
END
GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Creating [dbo].[WriteLog]...';


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER OFF;


GO
CREATE PROCEDURE [dbo].[WriteLog]
(
	@eventID int, 
	@priority int, 
	@severity nvarchar(32), 
	@title nvarchar(256), 
	@timestamp datetime,
	@machineName nvarchar(32), 
	@AppDomainName nvarchar(512),
	@ProcessID nvarchar(256),
	@ProcessName nvarchar(512),
	@ThreadName nvarchar(512),
	@Win32ThreadId nvarchar(128),
	@message nvarchar(1500),
	@formattedmessage ntext,
	@LogId int OUTPUT
)
AS 

	INSERT INTO [Log] (
		EventID,
		Priority,
		Severity,
		Title,
		[Timestamp],
		MachineName,
		AppDomainName,
		ProcessID,
		ProcessName,
		ThreadName,
		Win32ThreadId,
		Message,
		FormattedMessage
	)
	VALUES (
		@eventID, 
		@priority, 
		@severity, 
		@title, 
		@timestamp,
		@machineName, 
		@AppDomainName,
		@ProcessID,
		@ProcessName,
		@ThreadName,
		@Win32ThreadId,
		@message,
		@formattedmessage)

	SET @LogId = @@IDENTITY
	RETURN @LogId
GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Creating [dbo].[AddCategory]...';


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER OFF;


GO
CREATE PROCEDURE [dbo].[AddCategory]
	-- Add the parameters for the function here
	@categoryName nvarchar(64),
	@logID int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @CatID INT
	SELECT @CatID = CategoryID FROM Category WHERE CategoryName = @categoryName
	IF @CatID IS NULL
	BEGIN
		INSERT INTO Category (CategoryName) VALUES(@categoryName)
		SELECT @CatID = @@IDENTITY
	END

	EXEC InsertCategoryLog @CatID, @logID 

	RETURN @CatID
END
GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
-- Refactoring step to update target server with deployed transaction logs
CREATE TABLE  [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
GO
sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
GO

GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- =============================================
-- Script Template
-- =============================================

IF NOT EXISTS(SELECT TOP 1 1 FROM Role where RoleName='Manager')
BEGIN
    Insert into Role values('Manager',null)
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Role where RoleName='Operator')
BEGIN
    Insert into Role values('Operator',null)
END

IF NOT EXISTS(SELECT TOP 1 1 FROM Configuration)
BEGIN
    insert Configuration(Name,Value,[Type]) values(N'IsAutoDiagnostic', '<boolean>false</boolean>', N'System.Boolean')
    insert Configuration(Name,Value,[Type]) values(N'FulfillmentInterval', '<int>600000</int>', N'System.Int32')
    insert Configuration(Name,Value,[Type]) values(N'ReportInterval', '<int>600000</int>', N'System.Int32')
    insert Configuration(Name,Value,[Type]) values(N'IsAutoFulfillmentOn', '<boolean>false</boolean>', N'System.Boolean')
    insert Configuration(Name,Value,[Type]) values(N'IsAutoReportOn', '<boolean>false</boolean>', N'System.Boolean')
    insert Configuration(Name,Value,[Type]) values(N'OldTimeline', '<int>180</int>', N'System.Int32')
    insert Configuration(Name,Value,[Type]) values(N'IsEncryptExportedFile', '<boolean>true</boolean>', N'System.Boolean')
    insert Configuration(Name,Value,[Type]) values(N'CertificateSubject', 
	'<DisCert xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <Subject></Subject>
    <ThumbPrint></ThumbPrint>
    </DisCert>',
    N'DIS.Data.DataContract.DisCert')

    insert Configuration(Name,Value,[Type]) values(N'InternalServiceConfig', '<ServiceConfig xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <UserName></UserName>
  <UserKey></UserKey>
  <ServiceHostUrl>https://localhost:87/KeyBinding.svc</ServiceHostUrl>
</ServiceConfig>', N'DIS.Data.DataContract.ServiceConfig')

insert Configuration(Name,Value,[Type]) values(N'MsServiceConfig', '<ServiceConfig xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <UserName>1234560000</UserName>
  <UserKey>40bd001563085fc35165329ea1ff5c5ecbdbbeef</UserKey>
  <ServiceHostUrl>https://localhost/KeyBinding.svc</ServiceHostUrl>
</ServiceConfig>', N'DIS.Data.DataContract.ServiceConfig')

insert Configuration(Name,Value,[Type]) values(N'ProxySetting', '<ProxySetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <ProxyType>Default</ProxyType>
  <BypassProxyOnLocal>false</BypassProxyOnLocal>
  <ServiceConfig>
    <ServiceHostUrl>http://proxyUrl</ServiceHostUrl>
    <UserName>username</UserName>
    <UserKey>password</UserKey>
  </ServiceConfig>
</ProxySetting>', N'DIS.Data.DataContract.ProxySetting')
insert Configuration(Name,Value,[Type]) values(N'IsMsServiceEnabled', '<boolean>false</boolean>', N'System.Boolean')
END

IF NOT EXISTS (SELECT TOP 1 1 FROM [Category] where CategoryName = 'System')
BEGIN 
insert into [Category] (CategoryName) values ('System');
END

IF NOT EXISTS (SELECT TOP 1 1 FROM [Category] where CategoryName = 'Operation')
BEGIN 
insert into [Category] (CategoryName) values ('Operation');
END

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where LoginID='Admin')
BEGIN
    Insert into [User](LoginID,[Password], [PasswordRev],CreateDate,UpdateDate) 
    values('Admin','40bd001563085fc35165329ea1ff5c5ecbdbbeef', 1,GETDATE(),GETDATE())
    Insert into UserRole values(1,1)
END


--:r .\ProductMasterPopulate.sql

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[CategoryLog] WITH CHECK CHECK CONSTRAINT [FK_CategoryLog_Category];

ALTER TABLE [dbo].[CategoryLog] WITH CHECK CHECK CONSTRAINT [FK_CategoryLog_Log];

ALTER TABLE [dbo].[ComputerBuildReportKey] WITH CHECK CHECK CONSTRAINT [FK_ComputerBuildReportKey_ComputerBuildReport];

ALTER TABLE [dbo].[ComputerBuildReportKey] WITH CHECK CHECK CONSTRAINT [FK_ComputerBuildReportKey_ProductKeyInfo];

ALTER TABLE [dbo].[DuplicatedComputerBuildReport] WITH CHECK CHECK CONSTRAINT [FK_DuplicatedComputerBuildReport_ComputerBuildReport];

ALTER TABLE [dbo].[DuplicatedKey] WITH CHECK CHECK CONSTRAINT [FK_DuplicatedKey_KeyOperationHistory];

ALTER TABLE [dbo].[DuplicatedKey] WITH CHECK CHECK CONSTRAINT [FK_DuplicatedKey_ProductKeyInfo];

ALTER TABLE [dbo].[KeyHistory] WITH CHECK CHECK CONSTRAINT [FK_KeyHistory_ProductKeyInfo];

ALTER TABLE [dbo].[KeyInfoEx] WITH CHECK CHECK CONSTRAINT [FK_KeyInfoEx_HeadQuarter];

ALTER TABLE [dbo].[KeyInfoEx] WITH CHECK CHECK CONSTRAINT [FK_KeyInfoEx_ProductKeyInfo];

ALTER TABLE [dbo].[KeyInfoEx] WITH CHECK CHECK CONSTRAINT [FK_KeyInfoEx_Subsidiary];

ALTER TABLE [dbo].[KeyOperationHistory] WITH CHECK CHECK CONSTRAINT [FK_KeyOperationHistory_ProductKeyInfo];

ALTER TABLE [dbo].[KeySyncNotification] WITH CHECK CHECK CONSTRAINT [FK_KeySyncNotification_ProductKeyInfo];

ALTER TABLE [dbo].[ReturnReportKey] WITH CHECK CHECK CONSTRAINT [FK_ReturnReportKey_ProductKeyInfo];

ALTER TABLE [dbo].[ReturnReportKey] WITH CHECK CHECK CONSTRAINT [FK_ReturnReportKey_ReturnReport];

ALTER TABLE [dbo].[UserHeadQuarter] WITH CHECK CHECK CONSTRAINT [FK_UserHeadQuarter_HeadQuarter];

ALTER TABLE [dbo].[UserHeadQuarter] WITH CHECK CHECK CONSTRAINT [FK_UserHeadQuarter_User];

ALTER TABLE [dbo].[UserRole] WITH CHECK CHECK CONSTRAINT [FK_UserRole_Role];

ALTER TABLE [dbo].[UserRole] WITH CHECK CHECK CONSTRAINT [FK_UserRole_User];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        DECLARE @VarDecimalSupported AS BIT;
        SELECT @VarDecimalSupported = 0;
        IF ((ServerProperty(N'EngineEdition') = 3)
            AND (((@@microsoftversion / power(2, 24) = 9)
                  AND (@@microsoftversion & 0xffff >= 3024))
                 OR ((@@microsoftversion / power(2, 24) = 10)
                     AND (@@microsoftversion & 0xffff >= 1600))))
            SELECT @VarDecimalSupported = 1;
        IF (@VarDecimalSupported > 0)
            BEGIN
                EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
            END
    END


GO
