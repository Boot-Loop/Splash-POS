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
    public class DBAccess
    {
		private const string CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=POS-DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


		private static readonly DBAccess instance = new DBAccess();
        private DBAccess() { }
        public static DBAccess singleton {
            get { return instance; } 
        }

		private DataTable select(string command_text) {
			DataTable data_table = new DataTable();
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING)) {
				using (SqlCommand command = new SqlCommand()) {
					command.Connection = connection;
					command.CommandType = CommandType.Text;
					command.CommandText = command_text;
					try {
						connection.Open();
						SqlDataAdapter data_adapter = new SqlDataAdapter(command);
						data_adapter.Fill(data_table);
					}
					catch (Exception ex) { }
					finally {
						connection.Close();
					}
					return data_table;
				}
			}
		}

		private IEnumerable<T> excuteObject<T>(string command_text) {
			List<T> items = new List<T>();
			var dataTable = select(command_text);
			foreach (var row in dataTable.Rows) {
				T item = (T)Activator.CreateInstance(typeof(T), row);
				items.Add(item);
			}
			return items;
		}

		public void addStaff(StaffModel model) {
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Staff (FirstName, LastName, UserName, Password, EMail, AccessLevel) VALUES (@FirstName, @LastName, @UserName, @Password, @EMail, @AccessLevel)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@FirstName",	System.Data.SqlDbType.NVarChar, 100).Value	= model.FirstName.value;
					command.Parameters.Add("@LastName",		System.Data.SqlDbType.NVarChar, 100).Value	= !string.IsNullOrEmpty(model.LastName.value) ? model.LastName.value : (object)DBNull.Value;
					command.Parameters.Add("@UserName",		System.Data.SqlDbType.VarChar, 100).Value	= model.UserName.value;
					command.Parameters.Add("@Password",		System.Data.SqlDbType.VarChar, 128).Value	= model.Password.value;
					command.Parameters.Add("@EMail",		System.Data.SqlDbType.VarChar, 100).Value	= !string.IsNullOrEmpty(model.EMail.value) ? model.EMail.value : (object)DBNull.Value;
					command.Parameters.Add("@AccessLevel",	System.Data.SqlDbType.TinyInt).Value		= model.AccessLevel.value;
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
		public void addSupplier(SupplierModel model) {
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Supplier (FirstName, LastName, Company_ID, Address, EMail, Telephone, Comments) VALUES (@FirstName, @LastName, @Company_ID, @Address, @EMail, @Telephone, @Comments)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@FirstName",	System.Data.SqlDbType.NVarChar, 100).Value	= model.FirstName.value;
					command.Parameters.Add("@LastName",		System.Data.SqlDbType.NVarChar, 100).Value	= !string.IsNullOrEmpty(model.LastName.value)	? model.LastName.value	: (object)DBNull.Value;
					command.Parameters.Add("@Company_ID",	System.Data.SqlDbType.Int).Value			= !(model.CompanyID.isNull())					? model.CompanyID.value : (object)DBNull.Value;
					command.Parameters.Add("@Address",		System.Data.SqlDbType.NVarChar, 200).Value	= !string.IsNullOrEmpty(model.Address.value)	? model.Address.value : (object)DBNull.Value;
					command.Parameters.Add("@EMail",		System.Data.SqlDbType.VarChar, 100).Value	= !string.IsNullOrEmpty(model.EMail.value)		? model.EMail.value : (object)DBNull.Value;
					command.Parameters.Add("@Telephone",	System.Data.SqlDbType.VarChar, 100).Value	= !string.IsNullOrEmpty(model.Telephone.value)	? model.Telephone.value : (object)DBNull.Value;
					command.Parameters.Add("@Comments",		System.Data.SqlDbType.Text).Value			= !string.IsNullOrEmpty(model.Comments.value)	? model.Comments.value : (object)DBNull.Value;

					try
					{
						connection.Open();
						command.ExecuteNonQuery();
						Console.WriteLine("successfully inserted");
					}
					catch (Exception ex) { Console.WriteLine("error caught" + ex); }
					finally { connection.Close(); }
				}
			}
		}
		public void deleteSupplier(int id) {
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING)) {
				string command_text = "DELETE FROM dbo.Supplier	WHERE ID = @ID";
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
		public void updateSupplier(SupplierModel model , int id) {
			using (SqlConnection connection = new SqlConnection(CONNECTION_STRING)) {
				string command_text = "UPDATE dbo.Supplier SET FirstName = @FirstName, LastName = @LastName, Company_ID = @Company_ID, Address = @Address, EMail = @EMail, Telephone = @Telephone, Comments = @Comments WHERE ID = @ID";
				using (SqlCommand command = new SqlCommand(command_text))
				{
					command.Connection = connection;
					command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 100).Value = model.FirstName.value;
					command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 100).Value = !string.IsNullOrEmpty(model.LastName.value) ? model.LastName.value : (object)DBNull.Value;
					command.Parameters.Add("@Company_ID", System.Data.SqlDbType.Int).Value = !(model.CompanyID.isNull()) ? model.CompanyID.value : (object)DBNull.Value;
					command.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar, 200).Value = !string.IsNullOrEmpty(model.Address.value) ? model.Address.value : (object)DBNull.Value;
					command.Parameters.Add("@EMail", System.Data.SqlDbType.VarChar, 100).Value = !string.IsNullOrEmpty(model.EMail.value) ? model.EMail.value : (object)DBNull.Value;
					command.Parameters.Add("@Telephone", System.Data.SqlDbType.VarChar, 100).Value = !string.IsNullOrEmpty(model.Telephone.value) ? model.Telephone.value : (object)DBNull.Value;
					command.Parameters.Add("@Comments", System.Data.SqlDbType.Text).Value = !string.IsNullOrEmpty(model.Comments.value) ? model.Comments.value : (object)DBNull.Value;
					command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;

					try
					{
						connection.Open();
						command.ExecuteNonQuery();
						Console.WriteLine("successfully updated");
					}
					catch (Exception ex) { Console.WriteLine("error caught" + ex); }
					finally { connection.Close(); }
				}
			}
		}
		public List<SupplierModel> getSuppliers() {
			return excuteObject<SupplierModel>("SELECT * FROM dbo.Supplier").ToList();
		}
		public List<StaffModel> getStaffs() {
			return excuteObject<StaffModel>("SELECT * FROM dbo.Staff").ToList();
		}
		public List<ProductModel> getProducts() {
			return excuteObject<ProductModel>("SELECT * FROM dbo.Product").ToList();
		}
	}
}
