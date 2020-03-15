using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using Core.DB.Models;
using CoreApp = Core.Application;

namespace Core.DB.Access
{
	public class StockAccess : DataAccess
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
		public StockModel getStockOfProduct(int product_id) {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.StockView WHERE Product_ID = @Product_ID");
			command.Parameters.Add("@Product_ID", System.Data.SqlDbType.Int).Value = product_id;
			List<StockModel> stocks = excuteObject<StockModel>(command).ToList();
			return stocks.Count == 0 ? null : stocks[0];
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
						CoreApp.logger.log("Successfully stock added to database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
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
						CoreApp.logger.log("Successfully stock deleted from database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
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
						CoreApp.logger.log("Successfully stock updated in database");
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
