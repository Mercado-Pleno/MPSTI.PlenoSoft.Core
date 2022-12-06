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
			var webElementText = caseSensitive ? webElement.GetValueOrTextOrContent() : webElement.GetValueOrTextOrContent()?.ToUpper();
			return texts.Length > 0 && texts.Select(t => caseSensitive ? t : t.ToUpper()).All(t => webElementText?.Contains(t) == true);
		}

		public static bool ContainsAnyText(this IWebDriver webDriver, bool caseSensitive, params string[] texts)
			=> webDriver.GetBody(0).ContainsAnyText(caseSensitive, texts);

		public static bool ContainsAnyText(this IWebElement webElement, bool caseSensitive, params string[] texts)
		{
			var webElementText = caseSensitive ? webElement.GetValueOrTextOrContent() : webElement.GetValueOrTextOrContent()?.ToUpper();
			return texts.Length == 0 || texts.Select(t => caseSensitive ? t : t.ToUpper()).Any(t => webElementText?.Contains(t) == true);
		}

		public static bool IsEquals(this IWebDriver webDriver, string idOrName, string value)
			=> webDriver.GetElementByIdOrName(idOrName)?.IsEquals(value) ?? false;

		public static bool IsEquals(this IWebElement webElement, string value)
			=> webElement?.GetValueOrTextOrContent() == value;
	}
}