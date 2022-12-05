using MPSTI.PlenoSoft.Core.Selenium.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MPSTI.PlenoSoft.Core.Selenium
{
	public class SeleniumRwd
	{
		public RemoteWebDriver RemoteWebDriver { get; }

		public string Source => RemoteWebDriver.PageSource;

		public bool IsEmptyPageSource => string.IsNullOrWhiteSpace(Source) || (Source == "<html><head></head><body></body></html>");

		public SeleniumRwd(RemoteWebDriver remoteWebDriver) => RemoteWebDriver = remoteWebDriver;

		public SeleniumRwd Alert(Action<IAlert> action)
		{
			action?.Invoke(RemoteWebDriver.SwitchTo().Alert());
			return this;
		}

		public SeleniumRwd Wait(TimeSpan waitTime)
		{
			Thread.Sleep(waitTime);
			return this;
		}

		public IWebElement Set(string idOrName, bool click) => RemoteWebDriver.Set(idOrName, click);

		public IWebElement Set(string idOrName, string text) => RemoteWebDriver.Set(idOrName, text);

		public IWebElement GetBody(int skip = 0) => RemoteWebDriver.GetBody(skip);

		public IWebElement GetButton(string text, int skip = 0) => RemoteWebDriver.GetButton(text, skip);

		public SelectElement GetSelect(string text, int skip = 0) => RemoteWebDriver.GetSelect(text, skip);

		public IWebElement GetByIdOrName(string idOrName, int skip = 0, Func<IWebElement, bool> filter = null)
			=> RemoteWebDriver.GetElementByIdOrName(idOrName, skip, filter);

		public IEnumerable<IWebElement> GetElementsByTagName(string idOrName, int skip = 0)
			=> RemoteWebDriver.GetElementsByTagName(idOrName, skip);

		public bool WaitUntilContainsAllText(CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
			=> RemoteWebDriver.WaitUntilContainsAllText(cancellationToken, caseSensitive, texts);

		public bool WaitWhileContainsAllText(CancellationToken cancellationToken, bool caseSensitive, params string[] texts)
			=> RemoteWebDriver.WaitWhileContainsAllText(cancellationToken, caseSensitive, texts);

		public bool ContainsAllText(bool caseSensitive, params string[] texts) => RemoteWebDriver.ContainsAllText(caseSensitive, texts);

		public bool ContainsAnyText(bool caseSensitive, params string[] texts) => RemoteWebDriver.ContainsAnyText(caseSensitive, texts);

		public bool TextIsEquals(string idOrName, string value) => RemoteWebDriver.TextIsEquals(idOrName, value);

		public void GoTo(string address) => RemoteWebDriver.GoTo(address);

		public object Scroll(int x, int y) => RemoteWebDriver.Scroll(x, y);

		public object RunScript(string script) => RemoteWebDriver.RunScript(script);
		
		public void SetPosition(int windowIndex, int windowCount, int offSet = 0) => RemoteWebDriver.SetPosition(windowIndex, windowCount, offSet);

		public void PrintScreen(FileInfo fileInfo) => RemoteWebDriver.PrintScreen(fileInfo);

		public void CloseAndDispose() => RemoteWebDriver.CloseAndDispose();
	}
}