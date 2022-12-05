using OpenQA.Selenium;
using System.Threading;

namespace MPSTI.PlenoSoft.Core.Selenium.Extensions
{
	public static class SeleniumWaitExtension
	{
		public static bool WaitUntilContainsAllText(this IWebDriver webDriver, CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
			=> webDriver.GetBody().WaitUntilContainsAllText(cancellationToken, caseSensitive, texts);

		public static bool WaitUntilContainsAllText(this IWebElement webElement, CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
		{
			while (!cancellationToken.IsCancellationRequested && !webElement.ContainsAllText(caseSensitive, texts))
				Thread.Sleep(SeleniumConfig.WaitLoopContainsText);

			return webElement.ContainsAllText(caseSensitive, texts);
		}

		public static bool WaitWhileContainsAllText(this IWebDriver webDriver, CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
			=> webDriver.GetBody().WaitWhileContainsAllText(cancellationToken, caseSensitive, texts);

		public static bool WaitWhileContainsAllText(this IWebElement webElement, CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
		{
			while (!cancellationToken.IsCancellationRequested && webElement.ContainsAllText(caseSensitive, texts))
				Thread.Sleep(SeleniumConfig.WaitLoopContainsText);

			return !webElement.ContainsAllText(caseSensitive, texts);
		}
	}
}