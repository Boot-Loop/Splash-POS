using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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

		public List<ProductReturnWithNameModel> getReturnProductDetails(string from, string to) {
			SqlCommand command = new SqlCommand($"SELECT * FROM dbo.ProductReturnView WHERE TransactionTime >= '{from}' AND TransactionTime < '{to}'");
			return excuteObject<ProductReturnWithNameModel>(command).ToList();
		}
		public List<SaleDetailModel> getSaleDetails(string from, string to) {
			SqlCommand command = new SqlCommand($"SELECT * FROM dbo.SaleDetailView WHERE TransactionTime >= '{from}' AND TransactionTime < '{to}'");
			return excuteObject<SaleDetailModel>(command).ToList();
		}
		public List<SaleProductWithNameModel> getSaleProductsWithName(int sale_id) {
			SqlCommand command = new SqlCommand($"SELECT * FROM dbo.SaleProductView WHERE Sale_ID=@SaleID");
			command.Parameters.Add("@SaleID", System.Data.SqlDbType.Int).Value = sale_id;
			return excuteObject<SaleProductWithNameModel>(command).ToList();
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
				string command_text = "INSERT INTO dbo.SaleProduct (Sale_ID, Product_ID, Quantity, Discount, Price, SubTotal) VALUES (@Sale_ID, @Product_ID, @Quantity, @Discount, @Price, @SubTotal)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Sale_ID", System.Data.SqlDbType.Int).Value		= model.SaleID.value;
					command.Parameters.Add("@Product_ID", System.Data.SqlDbType.Int).Value	= model.ProductID.value;
					command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value	= model.Qunatity.value;
					command.Parameters.Add("@Discount", System.Data.SqlDbType.Float).Value	= !model.Discount.isNull() ? model.Discount.value : (object)DBNull.Value;
					command.Parameters.Add("@Price", System.Data.SqlDbType.Float).Value		= model.Price.value;
					command.Parameters.Add("@SubTotal", System.Data.SqlDbType.Float).Value = model.SubTotal.value;
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
		public ReciptModel getReceiptUsingID(string id) {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Recipt WHERE ID = @ID");
			command.Parameters.Add("@ID", System.Data.SqlDbType.VarChar, 20).Value = id;
			List<ReciptModel> models = excuteObject<ReciptModel>(command).ToList();
			return models.Count == 0 ? null : models[0];
		}
		public void addRecipt(ReciptModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Recipt (ID, Sale_ID) VALUES (@ID, @Sale_ID)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@ID", System.Data.SqlDbType.VarChar, 20).Value	= model.ID.value;
					command.Parameters.Add("@Sale_ID", System.Data.SqlDbType.Int).Value		= model.SaleID.value;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully recipt added to database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public void addReturnProduct(ProductReturnModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.ProductReturn (Recipt_ID, Product_ID, Quantity, RefundAmount, TransactionTime) VALUES (@Recipt_ID, @Product_ID, @Quantity, @RefundAmount, @TransactionTime)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Recipt_ID", System.Data.SqlDbType.VarChar, 20).Value = model.ReciptID.value;
					command.Parameters.Add("@Product_ID", System.Data.SqlDbType.Int).Value = model.ProductID.value;
					command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = model.Qunatity.value;
					command.Parameters.Add("@RefundAmount", System.Data.SqlDbType.Float).Value = model.RefuntAmount.value;
					command.Parameters.Add("@TransactionTime", System.Data.SqlDbType.DateTime).Value = model.TransactionTime.value;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully returnproduct added to database");
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
