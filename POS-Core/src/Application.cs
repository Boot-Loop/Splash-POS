using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
    public class Application
    {
		private static readonly Application instance = new Application();
		private Application() { }
		public static Application singleton {
			get { return instance; }
		}

		public void initialize() {
			createDatabase();
		}

		private bool checkDatabase(string database_name) {
			string query = @"select count(*) from master.dbo.sysdatabases d where d.name like @db";
			using (SqlConnection connection = new SqlConnection(query)) {
				try {
					connection.Open();
					using (SqlCommand command = new SqlCommand(query, connection)) {
						command.Parameters.Add("@db", SqlDbType.NVarChar).Value = database_name;
						int nRet = Convert.ToInt32(command.ExecuteScalar());
						return (nRet > 0);
					}
				}
				catch (Exception) { return false; }
				finally { connection.Close(); }
			}
		}

		private void createDatabase() {
			using (SqlConnection connection = new SqlConnection(@"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True")) {
				string strAppPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
				string strFilePath = Path.Combine(strAppPath, "Resources");
				string strFullFilename = Path.Combine(strFilePath, "script.sql");
				string script = File.ReadAllText(strFullFilename);
				try {
					connection.Open();
					Console.WriteLine("Connection Created");
					IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
					foreach (string commandString in commandStrings) {
						if (commandString.Trim() != "")
						{
							new SqlCommand(commandString, connection).ExecuteNonQuery();
						}
					}
				}
				catch (Exception ex) {
					Console.WriteLine(ex);
				}
				finally { connection.Close(); }
			}
		}
	}
}
