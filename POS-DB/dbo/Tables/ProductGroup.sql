CREATE TABLE [dbo].[ProductGroup]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Name] VARCHAR(20) NOT NULL, 
    [ParentGroup_ID] INT NULL, 
    [Color] VARCHAR(20) NULL 
)
