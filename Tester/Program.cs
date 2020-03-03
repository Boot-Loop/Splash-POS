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

namespace Tester
{
	public class Program
	{
		static void Main(string[] args)
		{
			////List<ProductModel> products = new List<ProductModel>();
			////products = DBAccess.singleton.getProducts();
			////foreach (ProductModel product in  products) {
			////	foreach (PropertyInfo propty in typeof(ProductModel).GetProperties()) {
			////		Console.WriteLine(propty.GetValue(product, null).ToString());
			////	}
			////}
			//StaffModel mod = new StaffModel();
			//mod.FirstName.value = "updatefn";
			//mod.LastName.value = "updateln";
			//mod.UserName.value = "updateun";
			//mod.Password.value = "updatepw";
			//mod.AccessLevel.value = 6;
			//StaffAccess.singleton.updateStaff(mod, 5);
			//Console.WriteLine("updated");
			//try
			//{

			//	StaffModel logged_in_user = StaffAccess.singleton.login("Passwoddrd");
			//	Console.WriteLine(logged_in_user.UserName.value);
			//}catch (Exception ex)
			//{
			//	Console.WriteLine(ex);
			//}

			////SupplierModel supplier = new SupplierModel();
			////supplier.FirstName.value = "upHello My Name";
			////supplier.LastName.value = "upLast Name";
			//////supplier.CompanyID.value = 3;
			////supplier.Address.value = "uppasddd";
			////supplier.EMail.value = "asf@e1mail.com";
			////supplier.Telephone.value = "hello";
			////supplier.Comments.value = "comments";

			////var prod = DBAccess.singleton.getProducts();
			////Console.WriteLine(supplier.LastName.value);
			//Console.ReadKey();

			updatedatabase();

		}


		public static void updatedatabase()
		{

			SqlConnection conn = new SqlConnection(@"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True");
			try
			{

				conn.Open();

				string script = File.ReadAllText(@"C:\Users\Azeem Muzammil\Documents\script.sql");

				// split script on GO command
				IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
				foreach (string commandString in commandStrings)
				{
					if (commandString.Trim() != "")
					{
						new SqlCommand(commandString, conn).ExecuteNonQuery();
					}
				}
				Console.WriteLine("Database updated successfully.");

			}
			catch (SqlException er)
			{
				Console.WriteLine(er.Message);
			}
			finally
			{
				conn.Close();
			}
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
