using System.IO;

namespace MPSTI.PlenoSoft.Core.CrossProject.Configurations
{
	public class SettingFactory
	{
		private static Setting _databases = null;

		public static string GetConnectionString(string key, FileInfo privateSettingsXmlFileInfo)
		{
			return GetDatabase(key, privateSettingsXmlFileInfo)?.ConnectionString;
		}

		public static Database GetDatabase(string key, FileInfo privateSettingsXmlFileInfo)
		{
			_databases ??= GetPrivateSettings(privateSettingsXmlFileInfo);
			return _databases[key];
		}

		public static Setting GetPrivateSettings(FileInfo privateSettingsXmlFileInfo)
		{
			return Setting.Load(privateSettingsXmlFileInfo);
		}
	}
}