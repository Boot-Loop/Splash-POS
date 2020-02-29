CREATE TABLE [dbo].[Product]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Name] NVARCHAR(100) NOT NULL, 
    [ProductGroup_ID] INT NULL, 
    [Brand_ID] INT NULL, 
    [MeasurementUnit_ID] INT NULL, 
    [Code] INT NOT NULL,
    [Description] TEXT NULL, 
    [PLU] INT NULL, 
    [Image] IMAGE NULL, 
    [Color] VARCHAR(50) NULL, 
    [Price] DECIMAL(24, 2) NOT NULL, 
    [IsService] BIT NOT NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateUpdated] DATETIME NOT NULL, 
    CONSTRAINT [FK_Product_ProductGroup] FOREIGN KEY ([ProductGroup_ID]) REFERENCES [dbo].[ProductGroup]([ID]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Product_Brand] FOREIGN KEY ([Brand_ID]) REFERENCES [dbo].[Brand]([ID]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Product_MeasurementUnit] FOREIGN KEY ([MeasurementUnit_ID]) REFERENCES [dbo].[MeasurementUnit]([ID]) ON DELETE SET NULL
)
