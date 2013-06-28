IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = N'PK_KeyTypeConfiguration')
BEGIN
	ALTER TABLE [dbo].[KeyTypeConfiguration] ADD  CONSTRAINT [PK_KeyTypeConfiguration] PRIMARY KEY CLUSTERED ([KeyTypeConfigurationId] ASC) ON [PRIMARY]
END
IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_KeyTypeConfiguration_HQID_PartNumber')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [IX_KeyTypeConfiguration_HQID_PartNumber] ON [dbo].[KeyTypeConfiguration]
	([HeadQuarterId] ASC, [LicensablePartNumber] ASC) ON [PRIMARY]
END
IF OBJECT_ID('dbo.TempKeyId', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[TempKeyId](
        [KeyId] [bigint] NOT NULL,
        [KeyState] [tinyint] NULL,
        [KeyType] [int] NULL,
     CONSTRAINT [PK_KeyIds] PRIMARY KEY CLUSTERED 
     (
        [KeyId] ASC
     )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyState]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[KeyState](
        [KeyStateId] [tinyint] NOT NULL,
        [KeyState] [nvarchar](20) NOT NULL,
     CONSTRAINT [PK_KeyState] PRIMARY KEY CLUSTERED  
     (
        [KeyStateId] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
   ) ON [PRIMARY]
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (1, N'Fulfilled')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (2, N'Consumed')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (3, N'Bound')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (4, N'NotifiedBound')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (5, N'Returned')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (7, N'ReportedBound')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (8, N'ReportedReturn')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (9, N'ActivationEnabled')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (10, N'ActivationDenied')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (11, N'Assigned')
 INSERT [dbo].[KeyState] ([KeyStateId], [KeyState]) VALUES (12, N'Retrieved')
END
IF COLUMNPROPERTY( OBJECT_ID('dbo.FulfillmentInfo'),'Tags','PRECISION') IS NULL 
BEGIN
      ALTER TABLE FulfillmentInfo  
      ADD [Tags] [nvarchar](200) NULL
END
IF COLUMNPROPERTY( OBJECT_ID('dbo.ProductKeyInfo'),'Tags','PRECISION') IS NULL 
BEGIN
      ALTER TABLE ProductKeyInfo  
      ADD [Tags] [nvarchar](200) NULL,
          [Description] [nvarchar](500) NULL
END