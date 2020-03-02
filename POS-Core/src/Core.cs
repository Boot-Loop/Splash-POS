﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Paths
    {
        /* if these dirs not exists -> create them on initialize */
        public static readonly string APP_DATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string PROGRAMME_DATA = Path.Combine(APP_DATA, "Splash-POS/");
        public static readonly string LOGS = Path.Combine(PROGRAMME_DATA, "Logs/");

    }

    public class ValidationError : Exception
	{
		public ValidationError() : base() { }
		public ValidationError(string message) : base(message) { }
	}

}