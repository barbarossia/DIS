﻿/*
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
