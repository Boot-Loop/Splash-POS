using System.IO;
using System.Xml.Serialization;

namespace Core.Utils
{
	public class XMLFile<DataClass>
	{
		static Logger logger = new Logger();

		public string path { get; set; } = null;
		public DataClass data { get; set; } = default(DataClass);

		public XMLFile() { }
		public XMLFile(string file_path = null, DataClass data = default(DataClass)) { this.path = file_path; this.data = data; }

		public void save() {
			if (this.path == null) logger.logError("save xml file path was null");
			if (this.data.Equals(default(DataClass))) logger.logError("data was default(DataClass) : null data");
			XmlSerializer serializer = new XmlSerializer(typeof(DataClass));
			using (TextWriter writer = new StreamWriter(this.path)) {
				serializer.Serialize(writer, this.data);
			}
		}

		/// <summary>
		/// throws :
		///		InvalidOperationException - if the xml file is currupted
		///		FileNotFoundException     - if the file not found
		///		ArgumentNullException     - if the file_path is null
		/// </summary>
		/// <returns></returns>
		public DataClass load() {
			if (this.path == null) logger.logError("save xml file path was null");
			XmlSerializer deserializer = new XmlSerializer(typeof(DataClass));
			using (TextReader reader = new StreamReader(this.path))
			{
				data = (DataClass)deserializer.Deserialize(reader);
				return data;
			}
		}
	}
}