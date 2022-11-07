using MPSTI.PlenoSoft.Core.CrossProject.Connections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MPSTI.PlenoSoft.Core.CrossProject.Configurations
{
	public class Setting
	{
		public List<Database> Databases { get; set; }

		public Database this[string key] => Databases.Single(d => d.Key == key);

		public static Setting Load(FileInfo settingsFileInfo)
		{
			if (settingsFileInfo.Exists)
			{
				using var textFile = settingsFileInfo.OpenText();
				var xmlContent = textFile.ReadToEnd();
				return Load(xmlContent);
			}
			return new Setting();
		}

		public static Setting Load(string xmlContent)
		{
			var xmlSerializer = new XmlSerializer(typeof(Setting));
			using var xmlStream = new StringReader(xmlContent);
			var settings = xmlSerializer.Deserialize(xmlStream);
			return settings as Setting;
		}

	}
	public class Database
	{
		public string Key { get; set; }
		public string Name { get; set; }
		public string RDBMS { get; set; }
		public string Type { get; set; }
		public string ConnectionString { get; set; }

		public Provider Provider => Enum.Parse<Provider>(RDBMS, ignoreCase: true);
	}
}