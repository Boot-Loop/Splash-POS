CREATE TABLE [dbo].[Company]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Name] NVARCHAR(100) NOT NULL, 
    [Address] NVARCHAR(200) NULL, 
    [EMail] VARCHAR(100) NULL, 
    [Telephone] VARCHAR(100) NULL, 
    [BankDetails] VARCHAR(200) NULL, 
    [Comments] TEXT NULL
)
