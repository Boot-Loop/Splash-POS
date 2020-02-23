﻿CREATE TABLE [dbo].[User]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(20) NOT NULL, 
    [LastName] NVARCHAR(20) NULL, 
    [UserName] VARCHAR(20) NOT NULL UNIQUE, 
    [Password] VARCHAR(32) NOT NULL UNIQUE, 
    [EMail] VARCHAR(50) NULL, 
    [AccessLevel] TINYINT NOT NULL
)
