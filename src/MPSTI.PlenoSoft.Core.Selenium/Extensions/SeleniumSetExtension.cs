﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace MPSTI.PlenoSoft.Core.Selenium.Extensions
{
	public static class SeleniumSetExtension
	{
		public static IWebElement Set(this ISearchContext searchContext, string idOrName, bool click)
			=> searchContext?.GetElementByIdOrName(idOrName)?.Set(click);

		private static IWebElement Set(this IWebElement webElement, bool click)
		{
			if (click)
				webElement?.Click();
			else
				webElement?.SetFocus();

			return webElement;
		}

		public static IWebElement Set(this ISearchContext searchContext, string idOrName, string text)
			=> searchContext?.GetElementByIdOrName(idOrName)?.Set(text);

		private static IWebElement Set(this IWebElement webElement, string text)
			=> (webElement?.TagName == "select") ? webElement?.SetSelect(text) : webElement?.SetInput(text);

		public static IWebElement SetInput(this ISearchContext searchContext, string idOrName, string text)
			=> searchContext?.GetElementByIdOrName(idOrName)?.SetInput(text);

		public static IWebElement SetInput(this IWebElement webElement, string text)
		{
			var formatedText = text?.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Keys.Shift + Keys.Enter + Keys.Shift);
			webElement.Clear();
			webElement.TypeKeys(formatedText);
			return webElement;
		}

		public static IWebElement SetSelect(this ISearchContext searchContext, string idOrName, string text)
			=> searchContext?.GetElementByIdOrName(idOrName)?.SetSelect(text);

		public static IWebElement SetSelect(this IWebElement webElement, string text)
			=> webElement.GetSelect().SetSelect(text).WrappedElement;

		public static SelectElement SetSelect(this SelectElement selectElement, string text)
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

			return selectElement;
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