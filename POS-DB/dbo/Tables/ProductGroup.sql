CREATE TABLE [dbo].[ProductGroup]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(20) NOT NULL, 
    [ParentGroup_ID] INT NOT NULL, 
    [Color] VARCHAR(20) NULL, 
    CONSTRAINT [FK_ProductGroup_ProductGroup] FOREIGN KEY ([ID]) REFERENCES [ProductGroup]([ID]) ON DELETE CASCADE
)
