using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;


namespace MPSTI.PlenoSoft.Core.Selenium.Updates
{
	public static class IoExtension
	{
		public const string EmptyVersion = "0000.0000.0000.0000";

		public static string GetBrowserVersion(this FileInfo browserFile) => FileVersionInfo.GetVersionInfo(browserFile.FullName).FileVersion;

		public static string GetDriverVersion(this FileInfo driverFile)
		{
			if (!driverFile.Exists()) return EmptyVersion;

			var startInfo = new ProcessStartInfo(driverFile.FullName, "--Version") { RedirectStandardOutput = true, CreateNoWindow = true, UseShellExecute = false };
			var process = new Process() { StartInfo = startInfo };
			process.Start();
			var driverVersion = process.StandardOutput.ReadToEnd() + " ";
			process.Close();
			var versions = driverVersion.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var version = versions.FirstOrDefault(v => v.All(x => char.IsDigit(x) || char.IsPunctuation(x)));
			return version;
		}

		public static FileInfo DownloadDriver(string downloadPath, string baseUrl, string version)
		{
			var arquivoZip = new FileInfo(Path.Combine(downloadPath, version));
			return XmlUtil.DownloadFromUrl($"{baseUrl}/{version}", arquivoZip);
		}

		public static DirectoryInfo Unzip(FileInfo arquivoZip)
		{
			var files = arquivoZip.Directory.GetFiles().Where(f => f.FullName != arquivoZip.FullName);
			foreach (var file in files) file.Delete();

			var dirs = arquivoZip.Directory.GetDirectories();
			foreach (var dir in dirs) dir.Delete(true);

			ZipFile.ExtractToDirectory(arquivoZip.FullName, arquivoZip.Directory.FullName);

			return arquivoZip.Directory;
		}

		public static FileInfo FindFile(this FileInfo fileInfo, string fileNamePattern, SearchOption searchOption)
			=> fileInfo.Exists() ? fileInfo : (fileInfo?.Directory).FindFile(fileNamePattern, searchOption);

		public static FileInfo FindFile(this DirectoryInfo directoryInfo, string fileNamePattern, SearchOption searchOption)
			=> (directoryInfo?.FullName).FindFile(fileNamePattern, searchOption);

		public static FileInfo FindFile(this string relativePath, string fileNamePattern, SearchOption searchOption)
		{
			try
			{
				var directory = new DirectoryInfo(relativePath ?? ".");
				while (!directory.Exists() && directory.Parent != null)
					directory = directory.Parent;

				if (directory.Exists())
				{
					var fileLocation = directory.EnumerateFiles(fileNamePattern, SearchOption.AllDirectories).FirstOrDefault();
					while (!fileLocation.Exists() && directory.Parent.Exists())
					{
						directory = directory.Parent;
						fileLocation = directory.EnumerateFiles(fileNamePattern, searchOption).FirstOrDefault();
					}
					return fileLocation ?? CreateFileInfo(relativePath, fileNamePattern);
				}
			}
			catch { return CreateFileInfo(relativePath, fileNamePattern); }

			return CreateFileInfo(relativePath, fileNamePattern);
		}

		public static FileInfo CreateFileInfo(string relativePath, string fileNamePattern) => new FileInfo(Path.Combine(relativePath, fileNamePattern.Replace("*", "")));

		public static bool Exists(this FileSystemInfo fileSystemInfo)
		{
			fileSystemInfo?.Refresh();
			return fileSystemInfo != null && fileSystemInfo.Exists;
		}
	}
}