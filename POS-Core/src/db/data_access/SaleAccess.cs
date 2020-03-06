using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB.Access
{
	public class SaleAccess
	{

		private static readonly SaleAccess instance = new SaleAccess();
		private SaleAccess() { }
		public static SaleAccess singleton {
			get { return instance; }
		}

		//public List<SupplierModel> getSuppliers() {
		//	SqlCommand command = new SqlCommand("SELECT * FROM dbo.Supplier");
		//	return excuteObject<SupplierModel>(command).ToList();
		//}
		public int addPayment(PaymentModel model) {
			int ID;
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Payment (PaymentMethod_ID, Amount, TransactionTime) VALUES (@PaymentMethod_ID, @Amount, @TransactionTime); SELECT SCOPE_IDENTITY()";
				using (SqlCommand command = new SqlCommand(command_text))
				{
					command.Connection = connection;
					command.Parameters.Add("@PaymentMethod_ID", System.Data.SqlDbType.Int).Value = !model.PaymentMethodID.isNull() ? model.PaymentMethodID.value : (object)DBNull.Value;
					command.Parameters.Add("@Amount", System.Data.SqlDbType.Float).Value = model.Amount.value;
					command.Parameters.Add("@TransactionTime", System.Data.SqlDbType.DateTime).Value = !model.TransactionTime.isNull() ? model.TransactionTime.value : (object)DBNull.Value;
					try {
						connection.Open();
						ID = Convert.ToInt32(command.ExecuteScalar());
						return ID;
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally { connection.Close(); }
				}
			}
		}
		public int addSale(SaleModel model) {
			int ID;
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Sale (User_ID, Customer_ID, Payment_ID) VALUES (@User_ID, @Customer_ID, @Payment_ID); SELECT SCOPE_IDENTITY();";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@User_ID", System.Data.SqlDbType.Int).Value = !model.UserID.isNull() ? model.UserID.value : (object)DBNull.Value;
					command.Parameters.Add("@Customer_ID", System.Data.SqlDbType.Int).Value = !model.CustomerID.isNull() ? model.CustomerID.value : (object)DBNull.Value;
					command.Parameters.Add("@Payment_ID", System.Data.SqlDbType.Int).Value = model.PaymentID.value;
					try {
						connection.Open();
						ID = Convert.ToInt32(command.ExecuteScalar());
						return ID;
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally { connection.Close(); }
				}
			}
		}
		public void addSaleProduct(SaleProductModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.SaleProduct (Sale_ID, Product_ID, Quantity, Discount, Price) VALUES (@Sale_ID, @Product_ID, @Quantity, @Discount, @Price)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Sale_ID", System.Data.SqlDbType.Int).Value = model.SaleID.value;
					command.Parameters.Add("@Product_ID", System.Data.SqlDbType.Int).Value = model.ProductID.value;
					command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = model.Qunatity.value;
					command.Parameters.Add("@Discount", System.Data.SqlDbType.Float).Value = !model.Discount.isNull() ? model.Discount.value : (object)DBNull.Value;
					command.Parameters.Add("@Price", System.Data.SqlDbType.Float).Value = model.Price.value;
					try {
						connection.Open();
						command.ExecuteNonQuery();
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally { connection.Close(); }
				}
			}
		}

		private DataTable select(SqlCommand sql_command)
		{
			DataTable data_table = new DataTable();
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING))
			{
				using (SqlCommand command = sql_command)
				{
					command.Connection = connection;
					try
					{
						connection.Open();
						SqlDataAdapter data_adapter = new SqlDataAdapter(command);
						data_adapter.Fill(data_table);
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally
					{
						connection.Close();
					}
					return data_table;
				}
			}
		}
		private IEnumerable<T> excuteObject<T>(SqlCommand sql_command)
		{
			List<T> items = new List<T>();
			var dataTable = select(sql_command);
			foreach (var row in dataTable.Rows)
			{
				T item = (T)Activator.CreateInstance(typeof(T), row);
				items.Add(item);
			}
			return items;
		}
	}
}
