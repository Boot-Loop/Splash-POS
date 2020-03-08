using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Core.DB.Access
{
	public class ProductAccess
	{
		private static readonly ProductAccess instance = new ProductAccess();
		private ProductAccess() { }
		public static ProductAccess singleton {
			get { return instance; }
		}

		public List<BarcodeModel> getBarcodes() {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Barcode");
			return excuteObject<BarcodeModel>(command).ToList();
		}
		public List<BrandModel> getBrands() {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Brand");
			return excuteObject<BrandModel>(command).ToList();
		}
		public List<MeasurementUnitModel> getMeasurementUnits() {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Brand");
			return excuteObject<MeasurementUnitModel>(command).ToList();
		}
		public List<ProductModel> getProducts() {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Product ORDER BY Code");
			return excuteObject<ProductModel>(command).ToList();
		}
		public List<ProductModel> searchProducts(string search_string) {
			SqlCommand command = new SqlCommand("SELECT TOP 20 * FROM dbo.Product WHERE Name LIKE '%' + @Name + '%'");
			command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 100).Value = search_string;
			return excuteObject<ProductModel>(command).ToList();
		}
		public List<ProductModel> getProductsWithBarcodes() {
			List<ProductModel> products = getProducts();
			List<ProductModel> return_products = new List<ProductModel>();
			foreach (ProductModel product in products) {
				SqlCommand command = new SqlCommand("SELECT * FROM dbo.Barcode WHERE Product_ID = @ID");
				command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = product.ID.value;
				List<BarcodeModel> barcodes = excuteObject<BarcodeModel>(command).ToList();
				product.Barcode.value = barcodes.Count == 0 ? null : barcodes[0].Value.value;
				return_products.Add(product);
			}
			return return_products;
		}

		//public List<ProductModel> getProductsWithBarcodes() {
		//	SqlCommand command = new SqlCommand("SELECT * FROM dbo.Product AS pr FULL OUTER JOIN dbo.Barcode AS bc ON pr.ID = bc.Product_ID  ORDER BY pr.Code");
		//	return excuteObject<ProductModel>(command).ToList();
		//}

		public ProductModel getProductUsingBarcode(string barcode) {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Product AS pr LEFT JOIN dbo.Barcode AS bc ON pr.ID = bc.Product_ID  WHERE Value = @Value");
			command.Parameters.Add("@Value", System.Data.SqlDbType.VarChar, 128).Value = barcode;
			List<ProductModel> models = excuteObject<ProductModel>(command).ToList();
			return models.Count == 0 ? null : models[0];
		}
		public ProductModel getProductUsingCode(int code) {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Product WHERE Code = @Code");
			command.Parameters.Add("@Code", System.Data.SqlDbType.Int).Value = code;
			List<ProductModel> models = excuteObject<ProductModel>(command).ToList();
			return models.Count == 0 ? null : models[0];
		}
		public int getLastProductCode() {
			int Code;
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "SELECT TOP 1 Code FROM dbo.Product ORDER BY Code DESC";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					try {
						connection.Open();
						Code = Convert.ToInt32(command.ExecuteScalar());
						return Code;
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally { connection.Close(); }
				}
			}
		}
		public void addProduct(ProductModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Product (Name, Code, Description, Price, IsService, DateCreated, DateUpdated) VALUES (@Name, @Code, @Description, @Price, @IsService, @DateCreated, @DateUpdated);";
				if (!string.IsNullOrEmpty(model.Barcode.value)) { command_text += "INSERT INTO dbo.Barcode(Product_ID, Value) VALUES(SCOPE_IDENTITY(), @Value);"; }
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 100).Value = model.Name.value;
					command.Parameters.Add("@Code", System.Data.SqlDbType.Int).Value = model.Code.value;
					command.Parameters.Add("@Description", System.Data.SqlDbType.Text).Value = !string.IsNullOrEmpty(model.Description.value) ? model.Description.value : (object)DBNull.Value;
					command.Parameters.Add("@Price", System.Data.SqlDbType.Float).Value = model.Price.value;
					command.Parameters.Add("@IsService", System.Data.SqlDbType.Bit).Value = model.IsService.value;
					command.Parameters.Add("@DateCreated", System.Data.SqlDbType.DateTime).Value = model.DateCreated.value;
					command.Parameters.Add("@DateUpdated", System.Data.SqlDbType.DateTime).Value = model.DateUpdated.value;
					if (!string.IsNullOrEmpty(model.Barcode.value)) { command.Parameters.Add("@Value", System.Data.SqlDbType.VarChar, 128).Value = model.Barcode.value; }
					try {
						connection.Open();
						command.ExecuteNonQuery();
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally { connection.Close(); }
				}
			}
		}
		public void deleteProduct(int id) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "DELETE FROM dbo.Product	WHERE ID = @ID";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;
					try {
						connection.Open();
						command.ExecuteNonQuery();
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally { connection.Close(); }
				}
			}
		}
		public void updateProduct(ProductModel model, int id) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "UPDATE dbo.Product SET Name = @Name, Code = @Code, Description = @Description, Price = @Price, IsService = @IsService, DateCreated = @DateCreated, DateUpdated = @DateUpdated WHERE ID = @ID;";
				if (!string.IsNullOrEmpty(model.Barcode.value)) { command_text += @"UPDATE dbo.Barcode SET Value = @Value WHERE Product_ID = @ID
																					IF @@ROWCOUNT = 0
																					INSERT INTO dbo.Barcode(Product_ID, Value) VALUES(@ID, @Value);"; }
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 100).Value = model.Name.value;
					command.Parameters.Add("@Code", System.Data.SqlDbType.Int).Value = model.Code.value;
					command.Parameters.Add("@Description", System.Data.SqlDbType.Text).Value = !string.IsNullOrEmpty(model.Description.value) ? model.Description.value : (object)DBNull.Value;
					command.Parameters.Add("@Price", System.Data.SqlDbType.Float).Value = model.Price.value;
					command.Parameters.Add("@IsService", System.Data.SqlDbType.Bit).Value = model.IsService.value;
					command.Parameters.Add("@DateCreated", System.Data.SqlDbType.DateTime).Value = model.DateCreated.value;
					command.Parameters.Add("@DateUpdated", System.Data.SqlDbType.DateTime).Value = model.DateUpdated.value;
					if (!string.IsNullOrEmpty(model.Barcode.value)) { command.Parameters.Add("@Value", System.Data.SqlDbType.VarChar, 128).Value = model.Barcode.value; }
					command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;

					try {
						connection.Open();
						command.ExecuteNonQuery();
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
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
					catch (Exception ex) { throw new Exception(ex.Message); }
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
