using OpenQA.Selenium;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Selenium.Extensions
{
	public static class SeleniumContainsExtension
	{
		public static bool ContainsAllText(this IWebDriver webDriver, bool caseSensitive, params string[] texts)
			=> webDriver.GetBody(0).ContainsAllText(caseSensitive, texts);

		public static bool ContainsAllText(this IWebElement webElement, bool caseSensitive, params string[] texts)
		{
			var webElementText = caseSensitive ? webElement.TryGetText() : webElement.TryGetText()?.ToUpper();
			return texts.Length > 0 && texts.Select(t => caseSensitive ? t : t.ToUpper()).All(t => webElementText?.Contains(t) == true);
		}

		public static bool ContainsAnyText(this IWebDriver webDriver, bool caseSensitive, params string[] texts)
			=> webDriver.GetBody(0).ContainsAnyText(caseSensitive, texts);

		public static bool ContainsAnyText(this IWebElement webElement, bool caseSensitive, params string[] texts)
		{
			var webElementText = caseSensitive ? webElement.TryGetText() : webElement.TryGetText()?.ToUpper();
			return texts.Length == 0 || texts.Select(t => caseSensitive ? t : t.ToUpper()).Any(t => webElementText?.Contains(t) == true);
		}

		public static bool TextIsEquals(this IWebDriver webElement, string idOrName, string value)
			=> webElement.GetElementByIdOrName(idOrName)?.TryGetText() == value;

		public static string TryGetText(this IWebElement webElement)
		{
			try { return webElement?.Text; }
			catch { return null; }
		}
	}
}