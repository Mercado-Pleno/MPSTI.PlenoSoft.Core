using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace MPSTI.PlenoSoft.Core.Selenium
{
	public static class SeleniumExtension
	{
		public static int WaitLoopSleep = 250;

		public static bool WaitUntilContainsAllText(this IWebDriver webDriver, CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
		{
			return webDriver.GetBody().WaitUntilContainsAllText(cancellationToken, caseSensitive, texts);
		}

		public static bool WaitWhileContainsAllText(this IWebDriver webDriver, CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
		{
			return webDriver.GetBody().WaitWhileContainsAllText(cancellationToken, caseSensitive, texts);
		}

		public static bool ContainsAllText(this IWebDriver webDriver, bool caseSensitive, params string[] texts)
		{
			return webDriver.GetBody().ContainsAllText(caseSensitive, texts);
		}

		public static bool ContainsAnyText(this IWebDriver webDriver, bool caseSensitive, params string[] texts)
		{
			return webDriver.GetBody().ContainsAnyText(caseSensitive, texts);
		}

		public static bool WaitUntilContainsAllText(this IWebElement webElement, CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
		{
			while (!cancellationToken.IsCancellationRequested && !webElement.ContainsAllText(caseSensitive, texts))
				Thread.Sleep(WaitLoopSleep);

			return webElement.ContainsAllText(caseSensitive, texts);
		}

		public static bool WaitWhileContainsAllText(this IWebElement webElement, CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
		{
			while (!cancellationToken.IsCancellationRequested && webElement.ContainsAllText(caseSensitive, texts))
				Thread.Sleep(WaitLoopSleep);

			return !webElement.ContainsAllText(caseSensitive, texts);
		}


		public static bool ContainsAllText(this IWebElement webElement, bool caseSensitive, params string[] texts)
		{
			var webElementText = caseSensitive ? webElement.TryGetText() : webElement.TryGetText().ToUpper();
			return texts.Length > 0 && texts.Select(t => caseSensitive ? t : t.ToUpper()).All(t => webElementText.Contains(t));
		}

		public static bool ContainsAnyText(this IWebElement webElement, bool caseSensitive, params string[] texts)
		{
			var webElementText = caseSensitive ? webElement.TryGetText() : webElement.TryGetText().ToUpper();
			return texts.Length == 0 || texts.Select(t => caseSensitive ? t : t.ToUpper()).Any(t => webElementText.Contains(t));
		}

		public static bool EstaPreenchido(this IWebDriver webElement, string idOrName, string value)
		{
			return webElement.GetElementByIdOrName(idOrName)?.TryGetText() == value;
		}

		public static string TryGetText(this IWebElement webElement)
		{
			try { return webElement.Text; }
			catch { return ""; }
		}



		public static IWebElement Set(this IWebElement webElement, bool click)
		{
			if (click)
				webElement.Click();
			else
				webElement.Focus();

			return webElement;
		}

		public static IWebElement Set(this IWebElement webElement, string text, bool force = false)
		{
			if (webElement.TagName == "select")
			{
				var dropdown = new SelectElement(webElement);
				dropdown.SelectByValue(text);

				if (string.IsNullOrWhiteSpace(dropdown?.SelectedOption?.Text))
					dropdown.SelectByText(text);

				if (string.IsNullOrWhiteSpace(dropdown?.SelectedOption?.Text))
					dropdown.SelectByText(text, true);

				if (text.Any() && text.All(char.IsDigit) && string.IsNullOrWhiteSpace(dropdown?.SelectedOption?.Text))
					dropdown.SelectByIndex(Convert.ToInt32(text));
			}
			else
			{
				var formatedText = text.Replace("\r\n", "\n").Replace("\r", "\n")
					.Replace("\n", Keys.Shift + Keys.Enter + Keys.Shift);
				webElement.Clear();
				webElement.TypeKeys(formatedText);
			}

			return webElement;
		}

		public static IWebElement Enter(this IWebElement webElement, int dalayOfEnter)
		{
			Thread.Sleep(dalayOfEnter);
			webElement.TypeKeys(Keys.Enter);
			return webElement;
		}

		public static IWebElement Escape(this IWebElement webElement)
		{
			webElement.TypeKeys(Keys.Escape);
			Thread.Sleep(WaitLoopSleep);
			webElement.TypeKeys(Keys.Escape);
			Thread.Sleep(WaitLoopSleep);
			return webElement;
		}

		public static IWebElement TypeKeys(this IWebElement webElement, string text)
		{
			webElement.SendKeys(text);
			return webElement;
		}

		public static IWebElement GetBody(this IWebDriver webDriver)
		{
			return webDriver.GetElement("body", 0);
		}

		public static IWebElement GetElement(this ISearchContext searchContext, string tagName, int skip = 0)
		{
			return searchContext.FindElements(By.TagName(tagName)).Skip(skip).FirstOrDefault();
		}

		public static IWebElement GetElementByIdOrName(this ISearchContext searchContext, string idOrName, int skip = 0)
		{
			return searchContext.FindElements(By.Id(idOrName)).Skip(skip).FirstOrDefault()
				?? searchContext.FindElements(By.Name(idOrName)).Skip(skip).FirstOrDefault()
				?? searchContext.FindElements(By.CssSelector($"[id$='{idOrName}']")).Skip(skip).FirstOrDefault()
				?? searchContext.FindElements(By.CssSelector($"[name$='{idOrName}']")).Skip(skip).FirstOrDefault()
				?? searchContext.FindElements(By.CssSelector($"[id^='{idOrName}']")).Skip(skip).FirstOrDefault()
				?? searchContext.FindElements(By.CssSelector($"[name^='{idOrName}']")).Skip(skip).FirstOrDefault()
				?? searchContext.FindElements(By.CssSelector($"[id*='{idOrName}']")).Skip(skip).FirstOrDefault()
				?? searchContext.FindElements(By.CssSelector($"[name*='{idOrName}']")).Skip(skip).FirstOrDefault()
			;
		}

		public static IWebElement GetButton(this IFindsByCssSelector searchContext, string text)
		{
			var lowerText = text?.Trim()?.ToLower();
			return searchContext.FindElementsByCssSelector("button")?.FirstOrDefault(b => b.Text.ToLower().Contains(lowerText));
		}

		public static void PrintScreen(this IWebDriver webDriver, FileInfo fileInfo)
		{
			var camera = webDriver as ITakesScreenshot;
			var foto = camera.GetScreenshot();
			foto.SaveAsFile(fileInfo.FullName, ScreenshotImageFormat.Png);
		}

		public static void Encerrar(this IWebDriver webDriver)
		{
			try
			{
				webDriver.Close();
				webDriver.Quit();
				webDriver.Dispose();
			}
			catch (Exception) { }
		}

		public static object Scroll(this IWebDriver webDriver, int x, int y)
		{
			return webDriver.ExecuteScript($"window.scrollBy({x}, {y});");
		}

		public static void IrParaEndereco(this IWebDriver webDriver, string address, int timeout = 0)
		{
			webDriver.Navigate().GoToUrl(address);
		}

		public static void Focus(this IWebElement webElement)
		{
			var element = webElement as RemoteWebElement;
			var id = element?.GetAttribute("id");
			element?.WrappedDriver?.ExecuteScript($"document.getElementById('{id}').focus();");
		}

		public static bool Exists(this IWebElement webElement)
		{
			return webElement != null;
		}

		/// <summary>
		/// js.ExecuteScript("alert('Hello World!');")
		/// js.ExecuteScript("arguments[0].onmouseover()", webDriver.FindElement(By.LinkText("NomeDoMenu")));
		/// js.ExecuteScript("window.scrollBy(0, 300)", "");
		/// </summary>
		/// <param name="webDriver"></param>
		/// <param name="script"></param>
		/// <returns></returns>
		public static object ExecuteScript(this IWebDriver webDriver, string script)
		{
			var js = webDriver as IJavaScriptExecutor;
			return js?.ExecuteScript(script);
		}

		public static void Posicionar(this IWebDriver webDriver, int windowIndex, int windowCount, int offSet = 0)
		{
			webDriver.Manage().Window.Posicionar(windowIndex, windowCount, offSet);
		}

		public static void Posicionar(this IWindow window, int windowIndex, int windowCount, int offSet = 0)
		{
			window.Position = new Point(offSet, 0);
			window.Maximize();
			if (windowCount > 1)
			{
				var position = new Point(new Size(window.Position));
				var size = new Size(new Point(window.Size));

				window.Size = new Size(size.Width / windowCount + 8, size.Height);
				var maxWidth = (size.Width - window.Size.Width) / (windowCount - 1);

				window.Position = new Point(windowIndex * maxWidth + position.X, position.Y);
			}
		}
	}
}