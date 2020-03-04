using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.DB.Models;
using Core.DB.Access;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using Core;

namespace Tester
{
	public class Program
	{
		static void Main(string[] args)
		{
			//List<StockModel> stocks = new List<StockModel>();
			//stocks = StockAccess.singleton.getStocks();
			//foreach (StockModel stock in stocks)
			//{
			//	Console.WriteLine(stock.Date.value);
			//}
			//StaffModel mod = new StaffModel();
			//mod.FirstName.value = "updatefn";
			//mod.LastName.value = "updateln";
			//mod.UserName.value = "updateun";
			//mod.Password.value = "updatepw";
			//mod.AccessLevel.value = 7;
			//StaffAccess.singleton.updateStaff(mod, 2);
			//Console.WriteLine("updated");
			//try
			//{

			//	StaffModel logged_in_user = StaffAccess.singleton.login("Passwoddrd");
			//	Console.WriteLine(logged_in_user.UserName.value);
			//}catch (Exception ex)
			//{
			//	Console.WriteLine(ex);
			//}

			//BarcodeModel bm = new BarcodeModel();
			//bm.Value.value = "NEWTEST";
			//List<BarcodeModel> bl = new List<BarcodeModel>();
			//bl.Add(bm);

			StockModel stock = new StockModel();
			stock.ProductID.value = 5;
			stock.SupplierID.value = 2;
			stock.Quantity.value = 120;
			stock.Date.value = DateTime.Now;
			StockAccess.singleton.updateStock(stock, 1);

			////var prod = DBAccess.singleton.getProducts();
			////Console.WriteLine(supplier.LastName.value);
			//Console.ReadKey();

			//updatedatabase();
			//Application.singleton.initialize();

			//updatedatabase();
			//ProductModel md = new ProductModel();
			//md = ProductAccess.singleton.getProductsWithBarcodes()[0];
			//Console.WriteLine(md.ID.getName() + " " + md.ID.value);
			//Console.WriteLine(md.Name.getName() + " " + md.Name.value);
			//Console.WriteLine(md.ProductGroupID.getName() + " " + md.ProductGroupID.value);
			//Console.WriteLine(md.BrandID.getName() + " " + md.BrandID.value);
			//Console.WriteLine(md.MeasurementUnitID.getName() + " " + md.MeasurementUnitID.value);
			//Console.WriteLine(md.Code.getName() + " " + md.Code.value);
			//Console.WriteLine(md.Description.getName() + " " + md.Description.value);
			//Console.WriteLine(md.PLU.getName() + " " + md.PLU.value);
			//Console.WriteLine(md.Image.getName() + " " + md.Image.value);
			//Console.WriteLine(md.Color.getName() + " " + md.Color.value);
			//Console.WriteLine(md.Price.getName() + " " + md.Price.value);
			//Console.WriteLine(md.IsService.getName() + " " + md.IsService.value);
			//Console.WriteLine(md.DateCreated.getName() + " " + md.DateCreated.value);
			//Console.WriteLine(md.DateUpdated.getName() + " " + md.DateUpdated.value);
			//Console.WriteLine("barcode: " + md.Barcode.ToString());
			Console.ReadKey();

		}













		//public static void addStaff(StaffModel model)
		//{
		//	using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=POS-DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
		//	{
		//		string sqlQuery = "INSERT INTO dbo.Staff (FirstName, LastName, UserName, Password, EMail, AccessLevel) VALUES (@FirstName, @LastName, @UserName, @Password, @EMail, @AccessLevel)";
		//		using (SqlCommand query = new SqlCommand(sqlQuery))
		//		{
		//			query.Connection = connection;
		//			query.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 100).Value = model.FirstName.value;
		//			query.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 100).Value = model.LastName.value;
		//			query.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 100).Value = model.UserName.value;
		//			query.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 128).Value = model.Password.value;
		//			query.Parameters.Add("@EMail", System.Data.SqlDbType.VarChar, 100).Value = model.EMail.value;
		//			query.Parameters.Add("@AccessLevel", System.Data.SqlDbType.TinyInt).Value = model.AccessLevel.value;
		//			try
		//			{
		//				connection.Open();
		//				Console.WriteLine("Opened");
		//			}
		//			catch (Exception ex) { return; }
		//			query.ExecuteNonQuery();
		//			Console.WriteLine("Inserted");
		//		}
		//	}
		//}

		//public static List<StaffModel> getStafff(string first_name)
		//{
		//	List<StaffModel> staffs = new List<StaffModel>();

		//	using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=POS-DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
		//	{
		//		string sqlQuery = "SELECT * FROM dbo.Staff WHERE FirstName = @FirstName";
		//		using (SqlCommand query = new SqlCommand(sqlQuery))
		//		{
		//			query.Connection = connection;
		//			query.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100).Value = first_name;
		//			try
		//			{
		//				connection.Open();
		//				Console.WriteLine("Opened");
		//			}
		//			catch (Exception ex)
		//			{

		//			}
		//			using (SqlDataReader reader = query.ExecuteReader())
		//			{
		//				while (reader.Read())
		//				{
		//					StaffModel staff = new StaffModel();
		//					staff.FirstName.value = reader["FirstName"].ToString();
		//					staff.LastName.value = reader["LastName"].ToString();
		//					staff.Password.value = reader["Password"].ToString();
		//					staffs.Add(staff);
		//				}
		//			}
		//			Console.WriteLine("REad");
		//		}
		//	}
		//	return staffs;
		//}
	}
}
