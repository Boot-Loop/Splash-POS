﻿using System;
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
using Core.Documents;

namespace Tester
{
	public class Program
	{
		static void Main(string[] args)
		{
			List<ProductModel> models = ProductAccess.singleton.getProducts("");
			//Console.WriteLine(models.Count);
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
