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

		public static bool checkDatabaseExists(string databaseName) {
			using (SqlConnection connection = new SqlConnection(@"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True")) {
				using (SqlCommand command = new SqlCommand($"SELECT db_id('{databaseName}')", connection)) {
					try {
						connection.Open();
						return (command.ExecuteScalar() != DBNull.Value);
					}catch (Exception ex) { Console.WriteLine(ex); }
				}
			}
			return false;
		}

		private void createDatabase() {
			if (!checkDatabaseExists("POS-DB")) {
				using (SqlConnection connection = new SqlConnection(@"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True")) {
					string strAppPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
					string strFilePath = Path.Combine(strAppPath, "Resources");
					string strFullFilename = Path.Combine(strFilePath, "script.sql");
					string script = File.ReadAllText(strFullFilename);
					try {
						connection.Open();
						IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
						foreach (string commandString in commandStrings) {
							if (commandString.Trim() != "") {
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
}
