using MPSTI.PlenoSoft.Core.Selenium.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MPSTI.PlenoSoft.Core.Selenium
{
	public class SeleniumDriver
	{
		public IWebDriver WebDriver { get; }

		public string Source => WebDriver.PageSource;

		public bool IsEmptyPageSource => string.IsNullOrWhiteSpace(Source) || (Source == "<html><head></head><body></body></html>");

		public SeleniumDriver(IWebDriver webDriver) => WebDriver = webDriver;

		public SeleniumDriver Alert(Action<IAlert> action)
		{
			action?.Invoke(WebDriver.SwitchTo().Alert());
			return this;
		}

		public SeleniumDriver Wait(TimeSpan waitTime)
		{
			Thread.Sleep(waitTime);
			return this;
		}

		public IWebElement Set(string idOrName, bool click) => WebDriver.Set(idOrName, click);

		public IWebElement Set(string idOrName, string text) => WebDriver.Set(idOrName, text);

		public IWebElement GetBody(int skip = 0) => WebDriver.GetBody(skip);

		public IWebElement GetButtonByText(string text, int skip = 0) => WebDriver.GetButtonByText(text, skip);

		public SelectElement GetSelectByIdOrName(string idOrName, int skip = 0) => WebDriver.GetSelectByIdOrName(idOrName, skip);

		public IWebElement GetElementByIdOrName(string idOrName, int skip = 0, Func<IWebElement, bool> filter = null)
			=> WebDriver.GetElementByIdOrName(idOrName, skip, filter);

		public IEnumerable<IWebElement> GetElementsByTagName(string idOrName, int skip = 0, Func<IWebElement, bool> filter = null)
			=> WebDriver.GetElementsByTagName(idOrName, skip, filter);

		public bool WaitUntilContainsAllText(CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
			=> WebDriver.WaitUntilContainsAllText(cancellationToken, caseSensitive, texts);

		public bool WaitWhileContainsAllText(CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
			=> WebDriver.WaitWhileContainsAllText(cancellationToken, caseSensitive, texts);

		public bool ContainsAllText(bool caseSensitive, params string[] texts) => WebDriver.ContainsAllText(caseSensitive, texts);

		public bool ContainsAnyText(bool caseSensitive, params string[] texts) => WebDriver.ContainsAnyText(caseSensitive, texts);

		public bool IsEquals(string idOrName, string value) => WebDriver.IsEquals(idOrName, value);

		public SeleniumDriver GoTo(string address)
		{
			WebDriver.GoTo(address);
			return this;
		}

		public object Scroll(int x, int y) => WebDriver.Scroll(x, y);

		public object RunScript(string script) => WebDriver.RunScript(script);

		public SeleniumDriver SetPosition(int windowIndex, int windowCount, int offSet = 0)
		{
			WebDriver.SetPosition(windowIndex, windowCount, offSet);
			return this;
		}

		public SeleniumDriver PrintScreen(FileInfo fileInfo)
		{
			WebDriver.PrintScreen(fileInfo);
			return this;
		}

		public Screenshot PrintScreen() => WebDriver.PrintScreen();

		public void CloseAndDispose() => WebDriver.CloseAndDispose();
	}
}