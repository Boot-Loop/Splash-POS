CREATE VIEW [dbo].[ProductView] AS
	SELECT pr.ID AS ID, pr.Name AS ProductName, gr.Name AS GroupName, br.Value AS BrandName,
	mu.Value AS MeasurementUnit, pr.Code AS Code, pr.Description AS Description,
	pr.PLU AS PLU, pr.Image AS Image, pr.Color AS Color, pr.Price AS Price,
	pr.IsService AS IsService, pr.DateCreated AS DateCreated, pr.DateUpdated AS DateUpdated
	FROM [dbo].[Product] AS pr, [dbo].[ProductGroup] AS gr, [dbo].[Brand] AS br,
	[dbo].[MeasurementUnit] AS mu
	WHERE pr.ProductGroup_ID = gr.ID AND pr.Brand_ID = br.ID AND pr.MeasurementUnit_ID = mu.ID;