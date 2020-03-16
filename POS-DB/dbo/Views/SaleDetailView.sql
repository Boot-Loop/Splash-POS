CREATE VIEW [dbo].[SaleDetailView] AS
	SELECT sl.ID AS SaleID, py.SubTotal AS SubTotal, py.Discount AS Discount, py.Total AS Total,
	py.TransactionTime AS TransactionTime
	FROM [dbo].[Sale] AS sl
	LEFT JOIN [dbo].[Payment] AS py ON sl.Payment_ID = py.ID
	