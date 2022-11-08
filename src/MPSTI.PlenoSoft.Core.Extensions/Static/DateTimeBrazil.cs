using System;

namespace MPSTI.PlenoSoft.Core.Extensions.Static
{
	public static class DateTimeBrazil
	{
		public const string DateTimeFormatWithoutTimeZone = "yyyy-MM-dd'T'HH:mm:ss";
		private static readonly TimeZoneInfo tzBrazil = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

		public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzBrazil);
		public static DateTime Today => Now.Date;

		public static string ToCamundaString(this DateTime dateTime) => dateTime.ToCamundaString(tzBrazil);

		public static string ToCamundaString(this DateTime localDateTime, TimeZoneInfo timeZoneInfo)
		{
			var dateTime = localDateTime.ToString(DateTimeFormatWithoutTimeZone);
			var signZone = timeZoneInfo.BaseUtcOffset < TimeSpan.Zero ? "-" : "+";
			var timeZone = timeZoneInfo.BaseUtcOffset.ToString("hh':'mm");

			return dateTime + signZone + timeZone;
		}

		public static DateTime? Competencia(this DateTime? date) => date?.Competencia();

		public static DateTime Competencia(this DateTime date) => new(date.Year, date.Month, 1);
	}
}