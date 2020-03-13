using System;
using System.Data.SqlClient;

using Core.DB.Models;
using CoreApp = Core.Application;

namespace Core.DB.Access
{
	public class SaleAccess : DataAccess
	{

		private static readonly SaleAccess instance = new SaleAccess();

		private SaleAccess() { }

		public static SaleAccess singleton {
			get { return instance; }
		}

		public int addPayment(PaymentModel model) {
			int ID;
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Payment (PaymentMethod_ID, SubTotal, Discount, Total, Paid, Balance, TransactionTime) VALUES (@PaymentMethod_ID, @SubTotal, @Discount, @Total, @Paid, @Balance, @TransactionTime); SELECT SCOPE_IDENTITY()";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@PaymentMethod_ID", System.Data.SqlDbType.Int).Value		= model.PaymentMethodID.value;
					command.Parameters.Add("@SubTotal", System.Data.SqlDbType.Float).Value				= model.SubTotal.value;
					command.Parameters.Add("@Discount", System.Data.SqlDbType.Float).Value				= model.Discount.value;
					command.Parameters.Add("@Total", System.Data.SqlDbType.Float).Value					= model.Total.value;
					command.Parameters.Add("@Paid", System.Data.SqlDbType.Float).Value					= model.Paid.value;
					command.Parameters.Add("@Balance", System.Data.SqlDbType.Float).Value				= model.Balance.value;
					command.Parameters.Add("@TransactionTime", System.Data.SqlDbType.DateTime).Value	= model.TransactionTime.value;
					try {
						connection.Open();
						ID = Convert.ToInt32(command.ExecuteScalar());
						CoreApp.logger.log("Successfully payment added to database");
						return ID;
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public void addPaymentMethod(PaymentMethodModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.PaymentMethod (Type) VALUES (@Type)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Type", System.Data.SqlDbType.VarChar, 128).Value = model.Type.value;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully payment_method added to database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public int addSale(SaleModel model) {
			int ID;
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Sale (User_ID, Customer_ID, Payment_ID, CartDiscount) VALUES (@User_ID, @Customer_ID, @Payment_ID, @CartDiscount); SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int).Value			= !model.UserID.isNull() ? model.UserID.value : (object)DBNull.Value;
					command.Parameters.Add("@Customer_ID", System.Data.SqlDbType.Int).Value		= !model.CustomerID.isNull() ? model.CustomerID.value : (object)DBNull.Value;
					command.Parameters.Add("@Payment_ID", System.Data.SqlDbType.Int).Value		= model.PaymentID.value;
					command.Parameters.Add("@CartDiscount", System.Data.SqlDbType.Float).Value	= model.CartDiscount.value;
					try {
						connection.Open();
						ID = Convert.ToInt32(command.ExecuteScalar());
						CoreApp.logger.log("Successfully sale added to database");
						return ID;
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public void addSaleProduct(SaleProductModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.SaleProduct (Sale_ID, Product_ID, Quantity, Discount, Price) VALUES (@Sale_ID, @Product_ID, @Quantity, @Discount, @Price)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Sale_ID", System.Data.SqlDbType.Int).Value		= model.SaleID.value;
					command.Parameters.Add("@Product_ID", System.Data.SqlDbType.Int).Value	= model.ProductID.value;
					command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value	= model.Qunatity.value;
					command.Parameters.Add("@Discount", System.Data.SqlDbType.Float).Value	= !model.Discount.isNull() ? model.Discount.value : (object)DBNull.Value;
					command.Parameters.Add("@Price", System.Data.SqlDbType.Float).Value		= model.Price.value;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully saleproduct added to database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
	}
}
