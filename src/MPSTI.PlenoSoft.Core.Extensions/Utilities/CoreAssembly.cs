using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MPSTI.PlenoSoft.Core.Extensions.Utilities
{
	[SuppressMessage("Major Code Smell", "S2743:Static fields should not be used in generic types", Justification = "This is my intent")]
	public static class CoreAssembly<TClass>
	{
		private static readonly Type Type = typeof(TClass);
		private static readonly Assembly Reference = Type.Assembly;
		private static readonly AssemblyName AssemblyName = Reference.GetName();
		private static readonly Version Version = AssemblyName.Version;
		private static readonly FileVersionInfo FileVersionInfo = GetFileVersionInfo(Reference.Location);

		public static string AssemblyVersion => Version.ToString();
		public static string ProductVersion => FileVersionInfo?.ProductVersion ?? AssemblyVersion;
		public static string FileVersion => FileVersionInfo?.FileVersion ?? AssemblyVersion;
		public static string FullVersion => $"{AssemblyVersion} ({ProductVersion} - {FileVersion})";

		private static FileVersionInfo GetFileVersionInfo(string location)
		{
			try
			{
				return FileVersionInfo.GetVersionInfo(location);
			}
			catch { return null; }
		}
	}
}