using System;

namespace MPSTI.PlenoSoft.Core.Extensions.Utilities
{
	public static class DateTimes
    {
        public const string DateTimeFormatWithoutTimeZone = "yyyy-MM-dd'T'HH:mm:ss";

        public static string ToIsoString(this DateTime dateTime) => dateTime.ToIsoStringCore(BrazilTimeZone.Default);

        public static string ToIsoString(this DateTime? dateTime) => dateTime?.ToIsoStringCore(BrazilTimeZone.Default);

        public static string ToIsoString(this DateTime? dateTime, TimeZoneInfo timeZoneInfo) => dateTime?.ToIsoStringCore(timeZoneInfo);

		public static string ToIsoString(this DateTime dateTime, TimeZoneInfo timeZoneInfo) => dateTime.ToIsoStringCore(timeZoneInfo);
		
        private static string ToIsoStringCore(this DateTime localOrUtcDateTime, TimeZoneInfo timeZoneInfo)
        {
            var localDateTime = localOrUtcDateTime.ToLocalTime();
			var dateTime = localDateTime.ToString(DateTimeFormatWithoutTimeZone);
            var signZone = timeZoneInfo.BaseUtcOffset < TimeSpan.Zero ? "-" : "+";
            var timeZone = timeZoneInfo.BaseUtcOffset.ToString("hh':'mm");

            return dateTime + signZone + timeZone;
        }

        public static DateTime? SetDay(this DateTime? date, int day = 1) => date?.SetDay(day);

        public static DateTime SetDay(this DateTime date, int day = 1) => new(date.Year, date.Month, day);
    }
}