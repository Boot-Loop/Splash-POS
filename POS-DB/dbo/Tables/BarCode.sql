CREATE TABLE [dbo].[Barcode]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Product_ID] INT NOT NULL, 
    [Value] VARCHAR(128) NOT NULL, 
    CONSTRAINT [FK_BarCode_Product] FOREIGN KEY ([Product_ID]) REFERENCES [dbo].[Product]([ID]) ON DELETE CASCADE
)
