CREATE TABLE [dbo].[SaleProduct]
(
	[Sale_ID] INT NOT NULL, 
    [Product_ID] INT NOT NULL, 
    [Quantity] INT NOT NULL DEFAULT 0, 
    [Price] DECIMAL(24, 2) NOT NULL DEFAULT 0.00, 
    CONSTRAINT [FK_SaleProduct_Sale] FOREIGN KEY ([Sale_ID]) REFERENCES [dbo].[Sale]([ID]) ON DELETE CASCADE, 
    CONSTRAINT [FK_SaleProduct_Product] FOREIGN KEY ([Product_ID]) REFERENCES [dbo].[Product]([ID]) ON DELETE CASCADE 
)
