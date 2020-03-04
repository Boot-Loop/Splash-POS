CREATE TABLE [dbo].[Stock]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Product_ID] INT NOT NULL, 
    [Warehouse_ID] INT NULL , 
    [Supplier_ID] INT NULL,
    [Quantity] INT NOT NULL DEFAULT 0, 
    [Date] DATETIME NULL, 
    CONSTRAINT [FK_Stock_Product] FOREIGN KEY ([Product_ID]) REFERENCES [dbo].[Product]([ID]) ON DELETE CASCADE, 
    CONSTRAINT [FK_Stock_Warehouse] FOREIGN KEY ([Warehouse_ID]) REFERENCES [dbo].[Warehouse]([ID]) ON DELETE SET DEFAULT, 
    CONSTRAINT [FK_Stock_Supplier] FOREIGN KEY ([Supplier_ID]) REFERENCES [dbo].[Supplier]([ID]) ON DELETE SET NULL
)
