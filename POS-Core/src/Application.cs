using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using Core.DB.Access;
using Core.DB.Models;
using Core.Utils;

namespace Core
{
    public class Application
    {
		private static readonly Application instance = new Application();

        public static Logger logger = new Logger("logs");

		private Application() { }

		public static Application singleton {
			get { return instance; }
		}

		public void initialize() {
			createDatabase();
            insertInitialDatas();
			if (!Directory.Exists(Paths.PROGRAMME_DATA)) Directory.CreateDirectory(Paths.PROGRAMME_DATA);
			if (!Directory.Exists(Paths.LOGS)) Directory.CreateDirectory(Paths.LOGS);

		}

		private bool checkDatabaseExists(string databaseName) {
            bool isExists;
			using (SqlConnection connection = new SqlConnection(Constants.INITIAL_CONNECTION_STRING)) {
                string command_text = "SELECT db_id('@DB_Name')";
                using (SqlCommand command = new SqlCommand(command_text)) {
                    command.Connection = connection;
                    command.Parameters.Add("@DB_Name", System.Data.SqlDbType.Text).Value = databaseName;
                    try {
						connection.Open();
                        isExists = command.ExecuteScalar() != DBNull.Value;
                        logger.log("Successfully existance of database checked");
                        return isExists;
					}
                    catch (Exception ex) { logger.log($"Failed to check existance of database: {ex}", Logger.LogLevel.LEVEL_ERROR); }
                    finally {
                        try { connection.Close(); logger.log("Successfully connection closed"); }
                        catch (Exception ex) { logger.log($"Failed to close connection while checking existance of database: {ex}", Logger.LogLevel.LEVEL_ERROR); }
                    }
				}
			}
			return false;
		}
		private void createDatabase() {
			if (!checkDatabaseExists("POS-DB")) {
				using (SqlConnection connection = new SqlConnection(Constants.INITIAL_CONNECTION_STRING)) {
					string strAppPath       = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
					string strFilePath      = Path.Combine(strAppPath, "Resources");
					string strFullFilename  = Path.Combine(strFilePath, "script.sql");
					string script           = File.ReadAllText(strFullFilename);
					try {
						connection.Open();
						IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
						foreach (string commandString in commandStrings) {
							if (commandString.Trim() != "") {
								new SqlCommand(commandString, connection).ExecuteNonQuery();
							}
						}
                        logger.log("Successfully database created!");
                    }
					catch (Exception ex) { logger.log($"Failed to create database: {ex}", Logger.LogLevel.LEVEL_ERROR); }
					finally {
                        try { connection.Close(); logger.log("Successfully connection closed"); }
                        catch (Exception ex) { logger.log($"Failed to close connection while creatig database: {ex}", Logger.LogLevel.LEVEL_ERROR); }
                    }
				}
			}
		}
        private void insertInitialDatas() {
            StaffModel staff_model                  = new StaffModel();
            PaymentMethodModel payment_method_model = new PaymentMethodModel();
            ProductGroupModel product_group_model   = new ProductGroupModel();
            staff_model.FirstName.value     = "admin";
            staff_model.LastName.value      = "admin";
            staff_model.UserName.value      = "admin";
            staff_model.Password.value      = "admin";
            staff_model.EMail.value         = "admin@mail.com";
            staff_model.AccessLevel.value   = 10;
            payment_method_model.Type.value = "Cash";
            product_group_model.Name.value  = "Products";
            try {
                StaffAccess.singleton.addStaff(staff_model);
                SaleAccess.singleton.addPaymentMethod(payment_method_model);
                ProductAccess.singleton.addProductGroup(product_group_model);
                logger.log("Successfully initial datas inserted");
            }
            catch (Exception ex) { logger.log($"Failed to insert initial datas to the database: {ex}", Logger.LogLevel.LEVEL_ERROR); }
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
