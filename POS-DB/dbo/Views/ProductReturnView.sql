CREATE VIEW [dbo].[ProductReturnView] AS
	SELECT pr.Name AS ProductName, prr.Quantity AS Quantity, prr.RefundAmount AS RefundAmount,
	prr.TransactionTime AS TransactionTime
	FROM [dbo].[ProductReturn] AS prr
	LEFT JOIN [dbo].[Product] AS pr ON prr.Product_ID = pr.ID
	