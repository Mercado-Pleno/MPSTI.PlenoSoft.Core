using System;

namespace MPSTI.PlenoSoft.Core.Selenium.Extensions
{
	public static class SeleniumConfig
	{
		public static TimeSpan WaitLoopContainsText { get; set; } = TimeSpan.FromMilliseconds(100);
		public static TimeSpan WaitBeforeGetTextOnSelectedOption { get; set; } = TimeSpan.FromMilliseconds(50);
	}
}