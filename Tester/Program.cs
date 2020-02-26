using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.DB.Models;

namespace Tester
{
	public class Program
	{
		static void Main(string[] args)
		{
			StaffModel user_model = new StaffModel();
			user_model.FirstName.value = "Azeem";
			user_model.LastName.value = "Muzammil";
			user_model.UserName.value = "AzeemMuza";
			user_model.Password.value = "Password";
			user_model.EMail.value = "azeem@123.com";
			user_model.AccessLevel.value = 10;

			addUser(user_model);
		}

		public static void addUser(StaffModel model) {
			using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=POS-DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")) {
				string sqlQuery = "INSERT INTO dbo.Staff (FirstName, LastName, UserName, Password, EMail, AccessLevel) VALUES (@FirstName, @LastName, @UserName, @Password, @EMail, @AccessLevel)";
				using (SqlCommand query = new SqlCommand(sqlQuery))
				{
					query.Connection = connection;
					query.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 100).Value = model.FirstName.value;
					query.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 100).Value = model.LastName.value;
					query.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 100).Value = model.UserName.value;
					query.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 128).Value = model.Password.value;
					query.Parameters.Add("@EMail", System.Data.SqlDbType.VarChar, 100).Value = model.EMail.value;
					query.Parameters.Add("@AccessLevel", System.Data.SqlDbType.TinyInt).Value = model.AccessLevel.value;
					try
					{
						connection.Open();
						Console.WriteLine("Opened");
					}
					catch (Exception ex) { return; }
					query.ExecuteNonQuery();
					Console.WriteLine("Inserted");
				}
			}
		}
	}
}
