using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Core.DB.Access
{
    public class DBAccess
    {

		private static readonly DBAccess instance = new DBAccess();
        private DBAccess() { }
        public static DBAccess singleton {
            get { return instance; } 
        }

		private DataTable select(string command_text) {
			DataTable data_table = new DataTable();
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
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
		
		
		
		public List<SupplierModel> getSuppliers() {
			return excuteObject<SupplierModel>("SELECT * FROM dbo.Supplier").ToList();
		}
		public List<ProductModel> getProducts() {
			return excuteObject<ProductModel>("SELECT * FROM dbo.Product").ToList();
		}
	}
}
