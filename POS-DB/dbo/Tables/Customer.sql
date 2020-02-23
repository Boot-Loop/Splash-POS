CREATE TABLE [dbo].[Customer]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [FirstName] NVARCHAR(20) NOT NULL, 
    [LastName] NVARCHAR(20) NULL, 
    [Telephone] VARCHAR(50) NULL, 
    [Address] VARCHAR(50) NULL, 
    [EMail] VARCHAR(50) NULL, 
    [Comments] TEXT NULL
)
