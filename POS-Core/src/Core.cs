using System;
using System.IO;

namespace Core
{
    public class Paths
    {
        public static readonly string APP_DATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string PROGRAMME_DATA = Path.Combine(APP_DATA, "Splash-POS/");
        public static readonly string LOGS = Path.Combine(PROGRAMME_DATA, "Logs/");
        public static readonly string PROGRAME_DATA_FILE = Path.Combine(PROGRAMME_DATA, "programe-data.xml");
    }

    public class Constants
    {
        public static readonly string INITIAL_CONNECTION_STRING = @"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True"
        public static readonly string CONNECTION_STRING         = @"Server=.\SQLEXPRESS;Database=POS-DB;Trusted_Connection=True";
    }

    public class ValidationError : Exception {
		public ValidationError() : base("ValidationError") { }
		public ValidationError(string message) : base("ValidationError" + message) { }
	}

    public class WrongPasswordError : Exception {
        public WrongPasswordError() : base("WrongPasswordError") { }
        public WrongPasswordError(string message) : base("ValidationError" + message) { }
    }

    public class InvalidPathError : Exception {
        public InvalidPathError() : base("InvalidPathError") { }
        public InvalidPathError(string name) : base("InvalidPathError: " + name) { }
    }

}
