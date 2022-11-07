using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Web.Web
{
	public class WebAppConfig
	{
		public static readonly string _rootPath = Path.GetTempPath();
		public static readonly string _appPath = Combine(_rootPath, "{ProjectId}");
		public static readonly string _staticFiles = Combine(_appPath, "static-files");

		public static readonly string _userPath = Combine(_staticFiles, "{UserId}");
		public static readonly string _downloadPath = Combine(_staticFiles, "[Download]");


		public static string Combine(params string[] paths)
		{
			var path = Path.Combine(paths);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			return path;
		}

		public static DirectoryInfo GetUserPath(string userId, params string[] paths)
		{
			var rootPath = _userPath.Replace("{UserId}", userId);
			return new DirectoryInfo(GetUserFileName(rootPath, paths));
		}

		public static FileInfo GetDownloadFile(params string[] paths)
		{
			return new FileInfo(GetUserFileName(_downloadPath, paths));
		}

		private static string GetUserFileName(string rootPath, params string[] paths)
		{
			var userPath = rootPath;

			foreach (var path in paths)
				userPath = Path.Combine(userPath, path);

			return userPath;
		}

		public static Uri FileToUri(Uri baseUri, FileInfo fileInfo)
		{
			var url = fileInfo.FullName.Replace(_appPath + Path.DirectorySeparatorChar, baseUri.AbsoluteUri);
			return new Uri(url);
		}

		public static IEnumerable<Uri> FileToUri(Uri baseUri, IEnumerable<FileInfo> arquivos)
		{
			return arquivos.Select(a => FileToUri(baseUri, a));
		}
	}
}