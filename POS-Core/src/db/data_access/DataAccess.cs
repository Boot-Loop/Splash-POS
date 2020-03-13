using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using CoreApp = Core.Application;

namespace Core.DB.Access
{
    public abstract class DataAccess
    {

		public IEnumerable<T> excuteObject<T>(SqlCommand sql_command) {
			List<T> items = new List<T>();
			var dataTable = select(sql_command);
			foreach (var row in dataTable.Rows) {
				T item = (T)Activator.CreateInstance(typeof(T), row);
				items.Add(item);
			}
			return items;
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
						CoreApp.logger.log("Successfully data fetched from database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
					return data_table;
				}
			}
		}
	}
}
