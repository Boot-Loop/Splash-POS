using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Utils
{
	public class Validator
	{
		private static Validator _singleton;
		public static Validator singleton {
			get {
				if (_singleton is null) _singleton = new Validator();
				return _singleton;
			}
		}

		private Validator() { }

		public bool validateEmail(string email){
			Regex regex = new Regex(@"^[a-zA-Z_\-0-9]+@[a-zA-Z_\-0-9]+\.[a-zA-Z]+$");
			Match match = regex.Match(email);
			return match.Success;
		}
	}
}
