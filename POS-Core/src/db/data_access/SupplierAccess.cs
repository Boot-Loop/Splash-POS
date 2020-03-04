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
	public class SupplierAccess
	{

		private static readonly SupplierAccess instance = new SupplierAccess();
		private SupplierAccess() { }
		public static SupplierAccess singleton {
			get { return instance; }
		}

		public List<SupplierModel> getSuppliers() {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Supplier");
			return excuteObject<SupplierModel>(command).ToList();
		}
		public void addSupplier(SupplierModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Supplier (FirstName, LastName, Company_ID, Address, EMail, Telephone, Comments) VALUES (@FirstName, @LastName, @Company_ID, @Address, @EMail, @Telephone, @Comments)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 100).Value = model.FirstName.value;
					command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 100).Value = !string.IsNullOrEmpty(model.LastName.value) ? model.LastName.value : (object)DBNull.Value;
					command.Parameters.Add("@Company_ID", System.Data.SqlDbType.Int).Value = !(model.CompanyID.isNull()) ? model.CompanyID.value : (object)DBNull.Value;
					command.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar, 200).Value = !string.IsNullOrEmpty(model.Address.value) ? model.Address.value : (object)DBNull.Value;
					command.Parameters.Add("@EMail", System.Data.SqlDbType.VarChar, 100).Value = !string.IsNullOrEmpty(model.EMail.value) ? model.EMail.value : (object)DBNull.Value;
					command.Parameters.Add("@Telephone", System.Data.SqlDbType.VarChar, 100).Value = !string.IsNullOrEmpty(model.Telephone.value) ? model.Telephone.value : (object)DBNull.Value;
					command.Parameters.Add("@Comments", System.Data.SqlDbType.Text).Value = !string.IsNullOrEmpty(model.Comments.value) ? model.Comments.value : (object)DBNull.Value;

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
		public void deleteSupplier(int id) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
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
		public void updateSupplier(SupplierModel model, int id) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "UPDATE dbo.Supplier SET FirstName = @FirstName, LastName = @LastName, Company_ID = @Company_ID, Address = @Address, EMail = @EMail, Telephone = @Telephone, Comments = @Comments WHERE ID = @ID";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 100).Value = model.FirstName.value;
					command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 100).Value = !string.IsNullOrEmpty(model.LastName.value) ? model.LastName.value : (object)DBNull.Value;
					command.Parameters.Add("@Company_ID", System.Data.SqlDbType.Int).Value = !(model.CompanyID.isNull()) ? model.CompanyID.value : (object)DBNull.Value;
					command.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar, 200).Value = !string.IsNullOrEmpty(model.Address.value) ? model.Address.value : (object)DBNull.Value;
					command.Parameters.Add("@EMail", System.Data.SqlDbType.VarChar, 100).Value = !string.IsNullOrEmpty(model.EMail.value) ? model.EMail.value : (object)DBNull.Value;
					command.Parameters.Add("@Telephone", System.Data.SqlDbType.VarChar, 100).Value = !string.IsNullOrEmpty(model.Telephone.value) ? model.Telephone.value : (object)DBNull.Value;
					command.Parameters.Add("@Comments", System.Data.SqlDbType.Text).Value = !string.IsNullOrEmpty(model.Comments.value) ? model.Comments.value : (object)DBNull.Value;
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
					catch (Exception ex) { }
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
