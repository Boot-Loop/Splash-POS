CREATE VIEW [dbo].[ProductView] AS
	SELECT pr.ID AS ID, pr.Name AS ProductName, br.Value AS Brand, pr.Code AS Code, pr.Description AS Description,
	pr.PLU AS PLU, pr.Image AS Image, pr.Color AS Color, pr.Price AS Price,
	pr.IsService AS IsService, pr.DateCreated AS DateCreated, pr.DateUpdated AS DateUpdated
	FROM [dbo].[Product] AS pr FULL OUTER JOIN [dbo].[Brand] AS br
	ON pr.Brand_ID = br.ID