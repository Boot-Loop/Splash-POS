CREATE TABLE [dbo].[Sales]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [User_ID] INT NOT NULL, 
    [Customer_ID] INT NULL, 
    [PaymentMethod_ID] INT NOT NULL, 
    [Paid] DECIMAL(20, 2) NOT NULL, 
    [Balance] DECIMAL(20, 2) NOT NULL, 
    [TransactionTime] DATETIME NOT NULL, 
    CONSTRAINT [FK_Sales_User] FOREIGN KEY ([User_ID]) REFERENCES [Sales]([ID]), 
    CONSTRAINT [FK_Sales_Client] FOREIGN KEY ([Customer_ID]) REFERENCES [Customer]([ID]), 
    CONSTRAINT [FK_Sales_PaymentMethod] FOREIGN KEY ([PaymentMethod_ID]) REFERENCES [PaymentMethod]([ID])
)
