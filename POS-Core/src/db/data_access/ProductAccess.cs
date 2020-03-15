using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Core.DB.Models;
using CoreApp = Core.Application;

namespace Core.DB.Access
{
	public class ProductAccess : DataAccess
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
		public List<ProductModel> getProducts(string search_string) {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Product WHERE Name LIKE '%' + @SearchString + '%' ORDER BY Code");
			command.Parameters.Add("@SearchString", System.Data.SqlDbType.VarChar, 100).Value = search_string;
			return excuteObject<ProductModel>(command).ToList();
		}
		public List<ProductModel> searchProducts(string search_string) {
			SqlCommand command = new SqlCommand("SELECT TOP 20 * FROM dbo.Product WHERE Name LIKE '%' + @SearchString + '%'");
			command.Parameters.Add("@SearchString", System.Data.SqlDbType.VarChar, 100).Value = search_string;
			return excuteObject<ProductModel>(command).ToList();
		}
		public List<ProductModel> getProductsWithBarcodes(string search_string) {
			List<ProductModel> products = getProducts(search_string);
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
		public ProductModel getProductUsingProductID(int id) {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Product WHERE ID = @ID");
			command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;
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
						CoreApp.logger.log("Successfully last product code fetched from database");
						return Code;
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public void addProduct(ProductModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Product (Name, ProductGroup_ID, Code, Description, Price, Cost, IsService, DateCreated, DateUpdated) VALUES (@Name, @ProductGroup_ID, @Code, @Description, @Price, @Cost, @IsService, @DateCreated, @DateUpdated);";
				if (!string.IsNullOrEmpty(model.Barcode.value)) { command_text += "INSERT INTO dbo.Barcode(Product_ID, Value) VALUES(SCOPE_IDENTITY(), @Value);"; }
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 100).Value		= model.Name.value;
					command.Parameters.Add("@ProductGroup_ID", System.Data.SqlDbType.Int).Value		= model.ProductGroupID.value;
					command.Parameters.Add("@Code", System.Data.SqlDbType.Int).Value				= model.Code.value;
					command.Parameters.Add("@Description", System.Data.SqlDbType.Text).Value		= !string.IsNullOrEmpty(model.Description.value) ? model.Description.value : (object)DBNull.Value;
					command.Parameters.Add("@Price", System.Data.SqlDbType.Float).Value				= model.Price.value;
					command.Parameters.Add("@Cost", System.Data.SqlDbType.Float).Value				= model.Cost.value;
					command.Parameters.Add("@IsService", System.Data.SqlDbType.Bit).Value			= model.IsService.value;
					command.Parameters.Add("@DateCreated", System.Data.SqlDbType.DateTime).Value	= model.DateCreated.value;
					command.Parameters.Add("@DateUpdated", System.Data.SqlDbType.DateTime).Value	= model.DateUpdated.value;
					if (!string.IsNullOrEmpty(model.Barcode.value)) { command.Parameters.Add("@Value", System.Data.SqlDbType.VarChar, 128).Value = model.Barcode.value; }
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully product added to database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public void addProductGroup(ProductGroupModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.ProductGroup (Name, ParentGroup_ID, Color) VALUES (@Name, @ParentGroup_ID, @Color)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 20).Value	= model.Name.value;
					command.Parameters.Add("@ParentGroup_ID", System.Data.SqlDbType.Int).Value	= !model.ParentGroup_ID.isNull() ? model.ParentGroup_ID.value : (object)DBNull.Value;
					command.Parameters.Add("@Color", System.Data.SqlDbType.VarChar, 20).Value	= !string.IsNullOrEmpty(model.Color.value) ? model.Color.value : (object)DBNull.Value;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully product group added to database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally
					{
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
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
						CoreApp.logger.log("Successfully product deleted from database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public void updateProduct(ProductModel model, int id) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "UPDATE dbo.Product SET Name = @Name, ProductGroup_ID = @ProductGroup_ID, Code = @Code, Description = @Description, Price = @Price, Cost = @Cost, IsService = @IsService, DateCreated = @DateCreated, DateUpdated = @DateUpdated WHERE ID = @ID;";
				if (!string.IsNullOrEmpty(model.Barcode.value)) { command_text += @"UPDATE dbo.Barcode SET Value = @Value WHERE Product_ID = @ID
																					IF @@ROWCOUNT = 0
																					INSERT INTO dbo.Barcode(Product_ID, Value) VALUES(@ID, @Value);"; }
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 100).Value		= model.Name.value;
					command.Parameters.Add("@ProductGroup_ID", System.Data.SqlDbType.Int).Value		= model.ProductGroupID.value;
					command.Parameters.Add("@Code", System.Data.SqlDbType.Int).Value				= model.Code.value;
					command.Parameters.Add("@Description", System.Data.SqlDbType.Text).Value		= !string.IsNullOrEmpty(model.Description.value) ? model.Description.value : (object)DBNull.Value;
					command.Parameters.Add("@Price", System.Data.SqlDbType.Float).Value				= model.Price.value;
					command.Parameters.Add("@Cost", System.Data.SqlDbType.Float).Value				= model.Cost.value;
					command.Parameters.Add("@IsService", System.Data.SqlDbType.Bit).Value			= model.IsService.value;
					command.Parameters.Add("@DateCreated", System.Data.SqlDbType.DateTime).Value	= model.DateCreated.value;
					command.Parameters.Add("@DateUpdated", System.Data.SqlDbType.DateTime).Value	= model.DateUpdated.value;
					if (!string.IsNullOrEmpty(model.Barcode.value)) { command.Parameters.Add("@Value", System.Data.SqlDbType.VarChar, 128).Value = model.Barcode.value; }
					command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully product updated in database");
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
