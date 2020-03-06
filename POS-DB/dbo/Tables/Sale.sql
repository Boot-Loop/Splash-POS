CREATE TABLE [dbo].[Sale]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [User_ID] INT NULL, 
    [Customer_ID] INT NULL, 
    [Payment_ID] INT NOT NULL, 
    CONSTRAINT [FK_Sale_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Staff]([ID]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Sale_Customer] FOREIGN KEY ([Customer_ID]) REFERENCES [dbo].[Customer]([ID]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Sale_Payment] FOREIGN KEY ([Payment_ID]) REFERENCES [dbo].[Payment]([ID]) ON DELETE CASCADE
)
