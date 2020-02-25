CREATE TABLE [dbo].[Supplier]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [FirstName] NVARCHAR(100) NULL, 
    [LastName] NVARCHAR(100) NULL, 
    [Company_ID] INT NULL, 
    [Address] NVARCHAR(200) NULL, 
    [EMail] VARCHAR(100) NULL,
    [Telephone] VARCHAR(100) NULL, 
    [Comments] TEXT NULL, 
    CONSTRAINT [FK_Supplier_Company] FOREIGN KEY ([Company_ID]) REFERENCES [Company]([ID]) ON DELETE SET NULL
)
