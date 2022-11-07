using Microsoft.AspNetCore.Http;
using MPSTI.PlenoSoft.Core.CrossProject.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.CrossProject.Web.Web
{
	public class FileService
	{
		private readonly DirectoryInfo _root = FilePathUtil.NewDirectoryInfo(Path.GetTempPath(), "Planilhas", DateTime.Now.ToString("yyyyMMdd/HH"));

		public async Task<List<FileInfo>> SaveFiles(IEnumerable<IFormFile> formFiles, DirectoryInfo directoryInfo = null)
		{
			var fileInfos = new List<FileInfo>();

			foreach (var formFile in formFiles)
			{
				var fileInfo = await SaveFile(formFile, directoryInfo);
				fileInfos.Add(fileInfo);
			}

			return fileInfos;
		}

		public async Task<FileInfo> SaveFile(IFormFile formFile, DirectoryInfo directoryInfo = null)
		{
			var root = directoryInfo ?? _root;
			var fileInfo = root.CombineToFile(formFile.FileName);

			if (formFile.Length > 0)
			{
				if (fileInfo.Exists)
					fileInfo.Delete();
				else if (!fileInfo.Directory.Exists)
					fileInfo.Directory.Create();

				using var stream = new FileStream(fileInfo.FullName, FileMode.Create);
				await formFile.CopyToAsync(stream);
				await stream.FlushAsync();
				stream.Close();
			}

			return fileInfo;
		}
	}
}