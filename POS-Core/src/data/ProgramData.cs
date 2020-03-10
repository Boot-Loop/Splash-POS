using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

using Core.Utils;
using System.Collections.ObjectModel;

namespace Core.Data.Files
{
	[Serializable]
	public class ProgrameData
	{
		[Serializable]
		public class PrinterData
		{
			[XmlAttribute] public string name;

			public PrinterData() { }
			public PrinterData(string name) {
				this.name = name;
			}
		}

		[XmlElement("PrinterName")]
		public PrinterData ReciptPrinter { get; set; }

		public void addPrinter(string name)
		{
			ReciptPrinter = new PrinterData(name);
		}
	}
}
