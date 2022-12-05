using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace MPSTI.PlenoSoft.Core.Selenium.Extensions
{
	public static class SeleniumSetExtension
	{
		public static IWebElement Set(this ISearchContext searchContext, string idOrName, bool click)
		{
			var webElement = searchContext.GetElementByIdOrName(idOrName);

			if (click)
				webElement?.Click();
			else
				webElement?.SetFocus();

			return webElement;
		}

		public static IWebElement Set(this ISearchContext searchContext, string idOrName, string text)
		{
			var webElement = searchContext.GetElementByIdOrName(idOrName);

			if (webElement.TagName == "select")
				webElement.SetSelect(text);
			else
				webElement.SetInput(text);

			return webElement;
		}

		public static void SetInput(this ISearchContext searchContext, string idOrName, string text) => searchContext.GetElementByIdOrName(idOrName).SetInput(text);

		public static void SetInput(this IWebElement webElement, string text)
		{
			var formatedText = text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Keys.Shift + Keys.Enter + Keys.Shift);
			webElement.Clear();
			webElement.TypeKeys(formatedText);
		}

		public static void SetSelect(this IWebElement webElement, string text) => webElement.GetSelect().SetSelect(text);

		public static void SetSelect(this SelectElement selectElement, string text)
		{
			if (text.Any())
			{
				selectElement?.SelectByValue(text);
				Thread.Sleep(SeleniumConfig.WaitBeforeGetTextOnSelectedOption);

				if (string.IsNullOrWhiteSpace(selectElement?.SelectedOption?.Text))
				{
					selectElement?.SelectByText(text);
					Thread.Sleep(SeleniumConfig.WaitBeforeGetTextOnSelectedOption);
				}

				if (string.IsNullOrWhiteSpace(selectElement?.SelectedOption?.Text))
				{
					selectElement?.SelectByText(text, true);
					Thread.Sleep(SeleniumConfig.WaitBeforeGetTextOnSelectedOption);
				}

				if (text.All(char.IsDigit) && string.IsNullOrWhiteSpace(selectElement?.SelectedOption?.Text))
				{
					selectElement?.SelectByIndex(Convert.ToInt32(text));
					Thread.Sleep(SeleniumConfig.WaitBeforeGetTextOnSelectedOption);
				}
			}
		}

		public static IWebElement Enter(IWebElement webElement) => webElement.TypeKeys(Keys.Enter);

		public static IWebElement Escape(IWebElement webElement) => webElement.TypeKeys(Keys.Escape);

		public static IWebElement TypeKeys(this IWebElement webElement, string text)
		{
			webElement.SendKeys(text);
			return webElement;
		}
	}
}