CREATE TABLE [dbo].[Sales]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [User_ID] INT NOT NULL, 
    [Customer_ID] INT NULL, 
    [Payment_ID] INT NOT NULL, 
    CONSTRAINT [FK_Sales_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Sales]([ID]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Sales_Client] FOREIGN KEY ([Customer_ID]) REFERENCES [dbo].[Customer]([ID]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Sales_Payment] FOREIGN KEY ([Payment_ID]) REFERENCES [dbo].[Payment]([ID]) ON DELETE SET NULL
)
