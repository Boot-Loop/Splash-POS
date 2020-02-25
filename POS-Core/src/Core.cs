using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
	public class Core
	{
	}

	public class ValidationError : Exception
	{
		public ValidationError() : base() { }
		public ValidationError(string message) : base(message) { }
	}

}
