CREATE TABLE [dbo].[Payment]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [PaymentMethod_ID] INT NULL, 
    [Amount] DECIMAL(24, 2) NOT NULL DEFAULT 0.00, 
    [TransactionTime] DATETIME NULL, 
    CONSTRAINT [FK_Payment_PaymentMethod] FOREIGN KEY ([PaymentMethod_ID]) REFERENCES [dbo].[PaymentMethod]([ID]) ON DELETE SET NULL
)
