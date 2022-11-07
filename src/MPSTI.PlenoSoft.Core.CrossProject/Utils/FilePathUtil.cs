using System.IO;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Utils
{
	public static class FilePathUtil
	{
		public static DirectoryInfo NewDirectoryInfo(params string[] paths)
		{
			var fullPath = Path.Combine(paths);
			return new DirectoryInfo(fullPath);
		}

		public static FileInfo NewFileInfo(params string[] paths)
		{
			var fullPath = Path.Combine(paths);
			return new FileInfo(fullPath);
		}

		public static FileInfo CombineToFile(this DirectoryInfo directoryInfo, params string[] paths)
		{
			var fullFileName = CombineToPath(directoryInfo, paths);
			return new FileInfo(fullFileName);
		}

		public static FileInfo CombineToFile(this DirectoryInfo directoryInfo, FileInfo fileInfo)
		{
			return directoryInfo.CombineToFile(fileInfo.Name);
		}

		public static DirectoryInfo Combine(this DirectoryInfo directoryInfo, params string[] paths)
		{
			var fullPath = CombineToPath(directoryInfo, paths);
			return new DirectoryInfo(fullPath);
		}

		public static string CombineToPath(DirectoryInfo directoryInfo, string[] paths)
		{
			var fullPath = Path.Combine(paths);
			return Path.Combine(directoryInfo.FullName, fullPath);
		}

		public static bool TryDeleteIfExists(this FileInfo fileInfo, bool all = false)
		{
			if (all)
				return fileInfo.Directory.GetFiles($"*{fileInfo.Extension}").All(f => f.TryDeleteIfExistsImpl());

			return fileInfo.TryDeleteIfExistsImpl();
		}

		private static bool TryDeleteIfExistsImpl(this FileSystemInfo fileInfo)
		{
			if (fileInfo == null)
				return false;

			try
			{
				if (fileInfo.Exists)
					fileInfo.Delete();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool TryCreateDirectoryIfNotExists(this FileInfo fileInfo) => fileInfo?.Directory?.TryCreateDirectoryIfNotExists() ?? false;

		public static bool TryCreateDirectoryIfNotExists(this DirectoryInfo directoryInfo)
		{
			if (directoryInfo == null)
				return false;

			try
			{
				if (!directoryInfo.Exists)
					directoryInfo.Create();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}