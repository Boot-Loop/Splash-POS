using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Core.DB.Access
{
	public class StockAccess
	{
		private static readonly StockAccess instance = new StockAccess();
		private StockAccess() { }
		public static StockAccess singleton {
			get { return instance; }
		}

		public List<StockModel> getStocks() {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.StockView");
			return excuteObject<StockModel>(command).ToList();
		}
		public void addStock(StockModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Stock (Product_ID, Warehouse_ID, Supplier_ID, Quantity, Date) VALUES (@Product_ID, @Warehouse_ID, @Supplier_ID, @Quantity, @Date)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Product_ID", System.Data.SqlDbType.Int).Value = model.ProductID.value;
					command.Parameters.Add("@Warehouse_ID", System.Data.SqlDbType.Int).Value = model.WarehouseID.isNull() ? (object)DBNull.Value : model.WarehouseID.value;
					command.Parameters.Add("@Supplier_ID", System.Data.SqlDbType.Int).Value = model.SupplierID.isNull() ? (object)DBNull.Value : model.SupplierID.value;
					command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = model.Quantity.value;
					command.Parameters.Add("@Date", System.Data.SqlDbType.DateTime).Value = model.Date.isNull() ? (object)DBNull.Value : model.Date.value;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						Console.WriteLine("successfully inserted");
					}
					catch (Exception ex) { Console.WriteLine("error caught" + ex); }
					finally { connection.Close(); }
				}
			}
		}
		public void deleteStock(int id) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "DELETE FROM dbo.Stock WHERE ID = @ID";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						Console.WriteLine("successfully deleted");
					}
					catch (Exception ex) { Console.WriteLine("error caught" + ex); }
					finally { connection.Close(); }
				}
			}
		}
		public void updateStock(StockModel model, int id) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "UPDATE dbo.Stock SET Product_ID = @Product_ID, Warehouse_ID = @Warehouse_ID, Supplier_ID = @Supplier_ID, Quantity = @Quantity, Date = @Date WHERE ID = @ID";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Product_ID", System.Data.SqlDbType.Int).Value = model.ProductID.value;
					command.Parameters.Add("@Warehouse_ID", System.Data.SqlDbType.Int).Value = model.WarehouseID.isNull() ? (object)DBNull.Value : model.WarehouseID.value;
					command.Parameters.Add("@Supplier_ID", System.Data.SqlDbType.Int).Value = model.SupplierID.isNull() ? (object)DBNull.Value : model.SupplierID.value;
					command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = model.Quantity.value;
					command.Parameters.Add("@Date", System.Data.SqlDbType.DateTime).Value = model.Date.isNull() ? (object)DBNull.Value : model.Date.value;
					command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;

					try {
						connection.Open();
						command.ExecuteNonQuery();
						Console.WriteLine("successfully updated");
					}
					catch (Exception ex) { Console.WriteLine("error caught" + ex); }
					finally { connection.Close(); }
				}
			}
		}


		private DataTable select(SqlCommand sql_command) {
			DataTable data_table = new DataTable();
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				using (SqlCommand command = sql_command) {
					command.Connection = connection;
					try {
						connection.Open();
						SqlDataAdapter data_adapter = new SqlDataAdapter(command);
						data_adapter.Fill(data_table);
					}
					catch (Exception ex) { Console.WriteLine(ex); }
					finally {
						connection.Close();
					}
					return data_table;
				}
			}
		}
		private IEnumerable<T> excuteObject<T>(SqlCommand sql_command) {
			List<T> items = new List<T>();
			var dataTable = select(sql_command);
			foreach (var row in dataTable.Rows) {
				T item = (T)Activator.CreateInstance(typeof(T), row);
				items.Add(item);
			}
			return items;
		}
	}
}
