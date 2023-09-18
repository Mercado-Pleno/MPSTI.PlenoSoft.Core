using MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Controller;
using System;
using System.IO;

namespace MPSTI.PlenoSoft.Core.Office.OpenXml.Planilhas.Util
{
    public static class PlenoExcelExtension
	{
		public static Boolean Is(this Modo self, Modo modo)
		{
			return (self & modo) == modo;
		}

		public static bool Exists(this FileSystemInfo fileSystemInfo, bool refresh = true)
		{
			if (refresh) fileSystemInfo.Refresh();
			return fileSystemInfo.Exists;
		}

        public static void Backup(this FileInfo arquivo)
		{
			if (File.Exists(arquivo.FullName))
			{
				var dir = new DirectoryInfo(Path.Combine(arquivo.Directory.FullName, "Bak"));
				if (!dir.Exists) dir.Create();

				var novoArquivo = ObterProximoNomeDeArquivo(dir.FullName, Path.GetFileNameWithoutExtension(arquivo.Name), 1, arquivo.Extension);
				arquivo.CopyTo(novoArquivo);
			}
		}

		private static String ObterProximoNomeDeArquivo(String fullDirectoryName, String fileName, Int32 index, String ext)
		{
			var nomeDoArquivo = String.Format("{0}_{1}", fileName, index.ToString().PadLeft(4, '0'));
			var arquivo = Path.Combine(fullDirectoryName, Path.ChangeExtension(nomeDoArquivo, ext));
			return File.Exists(arquivo) ? ObterProximoNomeDeArquivo(fullDirectoryName, fileName, index + 1, ext) : arquivo;
		}
	}
}