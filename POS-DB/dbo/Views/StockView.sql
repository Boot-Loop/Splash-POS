CREATE VIEW [dbo].[StockView] AS
	SELECT st.ID AS ID, st.Product_ID AS Product_ID, st.Warehouse_ID AS Warehouse_ID,
	st.Supplier_ID AS Supplier_ID, st.Quantity AS Quantity, st.UnitPrice AS UnitPrice, st.Date AS Date,
	pr.Name AS ProductName,
	wh.Name AS WarehouseName,
	sp.FirstName AS SupplierFirstName
	FROM [dbo].[Stock] AS st
	LEFT JOIN [dbo].[Product] AS pr ON st.Product_ID = pr.ID
	LEFT JOIN [dbo].[Warehouse] AS wh ON st.Warehouse_ID = wh.ID
	LEFT JOIN [dbo].[Supplier] AS sp ON st.Supplier_ID = sp.ID