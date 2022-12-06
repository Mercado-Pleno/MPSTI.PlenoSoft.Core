using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Selenium.Extensions
{
	public static class SeleniumGetExtension
	{
		public static IWebElement GetBody(this ISearchContext searchContext, int skip = 0)
			=> searchContext.GetElementsByTagName("body", skip).FirstOrDefault();

		public static IWebElement GetButtonByText(this ISearchContext searchContext, string text, int skip = 0)
		{
			var lowerText = text?.Trim()?.ToLower();
			return searchContext.GetElementsByTagName("button", skip, b => b.Text.ToLower().Contains(lowerText)).FirstOrDefault();
		}

		public static SelectElement GetSelectByIdOrName(this ISearchContext searchContext, string idOrName, int skip = 0) 
			=> searchContext.GetElementByIdOrName(idOrName, skip).GetSelect();

		public static SelectElement GetSelect(this IWebElement webElement) => new SelectElement(webElement);

		public static IWebElement GetElementByIdOrName(this ISearchContext searchContext, string idOrName, int skip = 0, Func<IWebElement, bool> filter = null)
		{
			var predicate = filter ?? (x => true);

			return searchContext.FindElements(By.Id(idOrName)).Skip(skip).FirstOrDefault(predicate)
				?? searchContext.FindElements(By.Name(idOrName)).Skip(skip).FirstOrDefault(predicate)
				?? searchContext.FindElements(By.CssSelector($"[id$='{idOrName}' i]")).Skip(skip).FirstOrDefault(predicate)
				?? searchContext.FindElements(By.CssSelector($"[id^='{idOrName}' i]")).Skip(skip).FirstOrDefault(predicate)
				?? searchContext.FindElements(By.CssSelector($"[id*='{idOrName}' i]")).Skip(skip).FirstOrDefault(predicate)
				?? searchContext.FindElements(By.CssSelector($"[name$='{idOrName}' i]")).Skip(skip).FirstOrDefault(predicate)
				?? searchContext.FindElements(By.CssSelector($"[name^='{idOrName}' i]")).Skip(skip).FirstOrDefault(predicate)
				?? searchContext.FindElements(By.CssSelector($"[name*='{idOrName}' i]")).Skip(skip).FirstOrDefault(predicate)
			;
		}

		public static IEnumerable<IWebElement> GetElementsByTagName(this ISearchContext searchContext, string tagName, int skip = 0, Func<IWebElement, bool> filter = null)
			=> searchContext.FindElements(By.TagName(tagName)).Skip(skip).Where(filter ?? (x => true));


		public static string GetValueOrTextOrContent(this IWebElement webElement)
		{
			var result = webElement.GetValue();

			if (string.IsNullOrWhiteSpace(result))
				result = webElement.Text;
		
			if (string.IsNullOrWhiteSpace(result))
				result = webElement.GetContent();

			return result;
		}

		public static string GetValue(this IWebElement webElement) => webElement.GetAttribute("value");

		public static string GetContent(this IWebElement webElement) => webElement.GetAttribute("innerText");

		public static string GetInnerHtml(this IWebElement webElement) => webElement.GetAttribute("innerHTML");

		public static bool IsAlive(this IWebElement webElement)
		{
			try { return !string.IsNullOrWhiteSpace(webElement?.TagName); } catch { return false; }
		}

		public static bool FindByIdOrName(this IWebElement webElement, string idOrName)
			=> webElement.FindById(idOrName) || webElement.FindByName(idOrName);

		public static bool FindById(this IWebElement webElement, string id)
			=> webElement.GetAttribute("id") == id;

		public static bool FindByName(this IWebElement webElement, string name)
			=> webElement.GetAttribute("name") == name;
	}
}