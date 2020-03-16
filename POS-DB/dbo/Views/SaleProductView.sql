CREATE VIEW [dbo].[SaleProductView] AS
	SELECT pr.Name AS ProductName, sp.Sale_ID AS Sale_ID, sp.Quantity AS Quantity,
	sp.Discount AS Discount, sp.Price AS Price, sp.SubTotal AS SubTotal
	FROM [dbo].[SaleProduct] AS sp
	LEFT JOIN [dbo].[Product] AS pr ON sp.Product_ID = pr.ID
	