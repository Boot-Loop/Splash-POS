using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB.Access
{
	public class ProductAccess
	{
		private const string CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=POS-DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


		private static readonly ProductAccess instance = new ProductAccess();
		private ProductAccess() { }
		public static ProductAccess singleton {
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
	}
}
