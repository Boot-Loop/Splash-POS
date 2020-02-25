CREATE TABLE [dbo].[Customer]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [FirstName] NVARCHAR(100) NOT NULL, 
    [LastName] NVARCHAR(100) NULL, 
    [Company_ID] INT NULL, 
    [Address] NVARCHAR(200) NULL, 
    [EMail] VARCHAR(100) NULL, 
    [Telephone] VARCHAR(100) NULL, 
    [Comments] TEXT NULL, 
    CONSTRAINT [FK_Customer_Company] FOREIGN KEY ([Company_ID]) REFERENCES [dbo].[Company]([ID]) ON DELETE CASCADE
)
