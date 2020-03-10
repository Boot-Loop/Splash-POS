using Core.DB.Access;
using Core.DB.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

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
			if (!Directory.Exists(Paths.PROGRAMME_DATA)) Directory.CreateDirectory(Paths.PROGRAMME_DATA);
			if (!Directory.Exists(Paths.LOGS)) Directory.CreateDirectory(Paths.LOGS);

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
						StaffModel model = new StaffModel();
						model.FirstName.value = "admin";
						model.LastName.value = "admin";
						model.UserName.value = "admin";
						model.Password.value = "admin";
						model.EMail.value = "admin@mail.com";
						model.AccessLevel.value = 10;
						StaffAccess.singleton.addStaff(model);
					}
					catch (Exception ex) {
						Console.WriteLine(ex);
					}
					finally { connection.Close(); }
				}
			}
		}

        public void updateReciptPrinterName(string name) {
            ReciptPrinter printer = new ReciptPrinter() { Name = name };
            XmlHelper.ToXmlFile(printer, Paths.PROGRAME_DATA_FILE);
        }
        public string readReciptPrinterName() {
            ReciptPrinter printer = XmlHelper.FromXmlFile<ReciptPrinter>(Paths.PROGRAME_DATA_FILE);
            return printer.Name;
        }

	}

	public class ReciptPrinter {
		public string Name { get; set; }
	}


    public static class XmlHelper
    {
        public static bool NewLineOnAttributes { get; set; }

        /// <summary>
        /// Deserializes an object from an XML string.
        /// </summary>
        public static T FromXml<T>(string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(xml))
            {
                return (T)xs.Deserialize(sr);
            }
        }

        /// <summary>
        /// Serializes an object to an XML file.
        /// </summary>
        public static void ToXmlFile(Object obj, string filePath)
        {
            var xs = new XmlSerializer(obj.GetType());
            var ns = new XmlSerializerNamespaces();
            var ws = new XmlWriterSettings { Indent = true, NewLineOnAttributes = NewLineOnAttributes, OmitXmlDeclaration = true };
            ns.Add("", "");

            using (XmlWriter writer = XmlWriter.Create(filePath, ws))
            {
                xs.Serialize(writer, obj);
            }
        }

        /// <summary>
        /// Deserializes an object from an XML file.
        /// </summary>
        public static T FromXmlFile<T>(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            try
            {
                var result = FromXml<T>(sr.ReadToEnd());
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("There was an error attempting to read the file " + filePath + "\n\n" + e.InnerException.Message);
            }
            finally
            {
                sr.Close();
            }
        }
    }

}
