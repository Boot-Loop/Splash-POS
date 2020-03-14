using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

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
			if (!Directory.Exists(Paths.PROGRAMME_DATA)) Directory.CreateDirectory(Paths.PROGRAMME_DATA);
			if (!Directory.Exists(Paths.LOGS)) Directory.CreateDirectory(Paths.LOGS);
            if (!Directory.Exists(Paths.DOCUMENT_SAVE_PATH)) Directory.CreateDirectory(Paths.DOCUMENT_SAVE_PATH);

		}
        public void updateReciptPrinterName(string name) {
            ReciptPrinter printer = new ReciptPrinter() { Name = name };
            try { XMLFile.ToXmlFile(printer, Paths.PROGRAME_DATA_FILE); logger.log("Recipt printer name successfully updated!"); }
            catch (Exception ex) { logger.log($"Failed to update printer name: {ex}", Logger.LogLevel.LEVEL_ERROR); } 
        }
        public string readReciptPrinterName() {
            try {
                ReciptPrinter printer = XMLFile.FromXmlFile<ReciptPrinter>(Paths.PROGRAME_DATA_FILE);
                logger.log("Recipt printer name successfully read!");
                return printer.Name;
            }
            catch (Exception ex) {
                logger.log($"Failed to read printer name: {ex}", Logger.LogLevel.LEVEL_ERROR);
                return "";
            }
        }
        public void updateDocumentSavePath(string path) {
            DocumentSavePath document = new DocumentSavePath() { Path = path };
            try { XMLFile.ToXmlFile(document, Paths.PROGRAME_DATA_FILE); logger.log("Document save path successfully updated!"); }
            catch (Exception ex) { logger.log($"Failed to update document save path: {ex}", Logger.LogLevel.LEVEL_ERROR); }
        }
        public string readDocumentSavePath() {
            try {
                DocumentSavePath document = XMLFile.FromXmlFile<DocumentSavePath>(Paths.PROGRAME_DATA_FILE);
                logger.log("Document save path successfully read!");
                return document.Path;
            }
            catch (Exception ex) {
                logger.log($"Failed to read document save path: {ex}", Logger.LogLevel.LEVEL_ERROR);
                return Paths.DOCUMENT_SAVE_PATH;
            }
        }

        private bool checkDatabaseExists(string database_name) {
            bool isExists;
			using (SqlConnection connection = new SqlConnection(Constants.INITIAL_CONNECTION_STRING)) {
                string command_text = $"SELECT db_id('{database_name}')";
                using (SqlCommand command = new SqlCommand(command_text)) {
                    command.Connection = connection;
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
	}

	public class ReciptPrinter {
		public string Name { get; set; }
	}
	public class DocumentSavePath {
		public string Path { get; set; }
	}
}
