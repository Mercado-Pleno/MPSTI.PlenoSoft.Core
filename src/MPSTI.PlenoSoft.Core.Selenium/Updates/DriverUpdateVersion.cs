using MPSTI.PlenoSoft.Core.Selenium.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Selenium.Updates
{
	public abstract class DriverUpdateVersion
	{
		public static BrowserType BrowserType { get; private set; }
		public static string[] DefaultLocations { get; } = new[] { @"C:\Program Files", @"C:\Program Files (x86)" };
		public static string DriverDefaultPath { get; } = @".\WebDriver\";

		protected abstract string BrowserName { get; }
		protected abstract string BrowserDefaultPath { get; }
		protected abstract string BrowserFileName { get; }
		protected abstract string DriverFileName { get; }
		protected abstract string BaseUrlDownload { get; }
		protected abstract string XmlKeyDriverVersion { get; }
		protected abstract string XmlPath { get; }
		protected abstract string XmlKey { get; }
		protected abstract string GetBaseUrl(string versao);

		protected DriverUpdateVersion(BrowserType browserType) => BrowserType = browserType;

		protected UpdateVersionInfo Start(IEnumerable<string> browserFileLocations)
		{
			return browserFileLocations.Select(x => StartUpdate(x)).FirstOrDefault(x => x.Updated)
				?? new UpdateVersionInfo(false, "", BrowserName);
		}

		private UpdateVersionInfo StartUpdate(string browserFileLocation, int level = 0)
		{
			var browserFile = GetBrowserFile(browserFileLocation);
			if (browserFile.Exists())
			{
				var driverFile = IoExtension.FindFile(DriverDefaultPath, DriverFileName, SearchOption.TopDirectoryOnly);
				var driverVersion = driverFile.GetDriverVersion();
				var browserVersion = browserFile.GetBrowserVersion();
				if ((level == 0) && NeedUpdate(browserVersion, driverVersion))
				{
					var versions = SearchDriverVersions(browserVersion.Split('.'));
					var version = ChooseBetterDriverVersion(versions);
					DownloadWebDriver(DriverDefaultPath, BaseUrlDownload, version, DriverFileName, driverFile);
					
					return StartUpdate(browserFileLocation, level + 1);
				}
				else
					return new UpdateVersionInfo(true, browserFileLocation, BrowserName, browserVersion, driverVersion);
			}
			return new UpdateVersionInfo(false, browserFileLocation, BrowserName);
		}

		public static FileInfo DownloadWebDriver(string downloadPath, string baseUrl, string version, string driverFileName, FileInfo driverFile = null)
		{
			var zipFile = IoExtension.DownloadDriver(downloadPath, baseUrl, version);
			var zipPath = IoExtension.Unzip(zipFile);
			var newFile = IoExtension.FindFile(zipPath, driverFileName, SearchOption.TopDirectoryOnly);

			if ((driverFile != null) && (driverFile.Directory.FullName != newFile.Directory.FullName))
				newFile.CopyTo(driverFile.FullName, true);

			return newFile;
		}


		private bool NeedUpdate(string browserVersion, string driverVersion) => !VersionCompatible(browserVersion.Split('.'), driverVersion.Split('.'), 0, 1, 2);

		private bool VersionCompatible(string[] bv, string[] dv, params int[] indexes) => indexes.All(i => bv[i] == dv[i]);

		private string ChooseBetterDriverVersion(IEnumerable<string> versions)
		{
			int[] Converter(string path)
			{
				var paths = path.Split('/');
				var versao = paths[0];
				var versoes = versao.Split('.');
				var intVers = versoes.Select(i => Convert.ToInt32(i));
				return intVers.ToArray();
			}

			var version = versions.Select(v => Converter(v)).Where(v => v.Length >= 4)
				.OrderBy(v => v[0]).ThenBy(v => v[1]).ThenBy(v => v[2]).ThenBy(v => v[3])
				.Select(v => string.Join(".", v)).LastOrDefault();

			return versions.FirstOrDefault(v => v.StartsWith(version));
		}

		protected virtual string[] SearchDriverVersions(IEnumerable<string> versionArray)
		{
			try
			{
				var take = versionArray.Count();
				var versao = string.Join(".", versionArray.Take(take));
				var url = GetBaseUrl(versao);
				var xmlUtil = XmlUtil.CreateFromUrl(url);
				var keys = xmlUtil.Nodes(XmlPath, XmlKey);
				var files = keys.Where(k => k.InnerXml.Contains(XmlKeyDriverVersion)).ToArray();

				var versoes = new string[0];
				while (versoes.Length == 0 && take > 0)
				{
					versoes = files.Where(x => x.InnerXml.StartsWith(versao)).Select(n => n.InnerXml).ToArray();
					take--;
					versao = string.Join(".", versionArray.Take(take));
				}

				if (string.IsNullOrWhiteSpace(versao))
					versoes = files.Select(n => n.InnerXml).ToArray();

				return versoes;
			}
			catch (Exception)
			{
				return new[] { IoExtension.EmptyVersion };
			}
		}

		private FileInfo GetBrowserFile(string browserFileLocation)
		{
			var fileInfo = new FileInfo(Path.Combine(browserFileLocation, BrowserDefaultPath));
			return IoExtension.FindFile(fileInfo, BrowserFileName, SearchOption.AllDirectories);
		}
	}
}