using System;

namespace MPSTI.PlenoSoft.Core.Extensions.Utilities
{
	public static class BrazilTimeZone
	{
		public static readonly TimeZoneInfo Default = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

		public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Default);

		public static DateTime Today => Now.Date;
	}
}