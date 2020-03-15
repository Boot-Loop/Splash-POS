CREATE TABLE [dbo].[ProductReturn]
(
    [ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Recipt_ID] VARCHAR(20) NOT NULL, 
    [Product_ID] INT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [RefundAmount] DECIMAL(24, 2) NOT NULL, 
    CONSTRAINT [FK_ProductReturn_Receipt] FOREIGN KEY ([Recipt_ID]) REFERENCES [Recipt]([ID]) ON DELETE CASCADE, 
    CONSTRAINT [FK_ProductReturn_Product] FOREIGN KEY ([Product_ID]) REFERENCES [Product]([ID]) ON DELETE CASCADE
)
