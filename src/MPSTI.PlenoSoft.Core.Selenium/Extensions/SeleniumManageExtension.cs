using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Drawing;
using System.IO;

namespace MPSTI.PlenoSoft.Core.Selenium.Extensions
{
	public static class SeleniumManageExtension
	{
		public const String ScriptForDeleteAllCookies = @"
function deleteAllCookies() {
    var cookies = document.cookie.split("";"");

    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
		var eqPos = cookie.indexOf(""="");
		var name = (eqPos > -1) ? cookie.substr(0, eqPos) : cookie;
		document.cookie = name + ""=;expires=Thu, 01 Jan 1970 00:00:00 GMT"";
    }
}
deleteAllCookies();";



		public static void GoTo(this IWebDriver webDriver, string address) => webDriver.Navigate().GoToUrl(address);

		public static void SetFocus(this IWebElement webElement)
		{
			var element = webElement as RemoteWebElement;
			var id = element?.GetAttribute("id");
			element?.WrappedDriver?.RunScript($"document.getElementById('{id}').focus();");
		}

		public static object Scroll(this IWebDriver webDriver, int x, int y)
		{
			return webDriver.RunScript($"window.scrollBy({x}, {y});");
		}

		/// <summary>
		/// js.ExecuteScript("alert('Hello World!');")
		/// js.ExecuteScript("arguments[0].onmouseover()", webDriver.FindElement(By.LinkText("NomeDoMenu")));
		/// js.ExecuteScript("window.scrollBy(0, 300)", "");
		/// </summary>
		/// <param name="webDriver"></param>
		/// <param name="script"></param>
		/// <returns></returns>
		public static object RunScript(this IWebDriver webDriver, string script)
		{
			var js = webDriver as IJavaScriptExecutor;
			return js?.ExecuteScript(script);
		}

		public static void SetPosition(this IWebDriver webDriver, int windowIndex, int windowCount, int offSet = 0)
		{
			webDriver.Manage().Window.SetPosition(windowIndex, windowCount, offSet);
		}

		public static void SetPosition(this IWindow window, int windowIndex, int windowCount, int offSet = 0)
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

		public static void PrintScreen(this IWebDriver webDriver, FileInfo fileInfo)
		{
			var camera = webDriver as ITakesScreenshot;
			var foto = camera.GetScreenshot();
			foto.SaveAsFile(fileInfo.FullName, ScreenshotImageFormat.Png);
		}

		public static void CloseAndDispose(this IWebDriver webDriver)
		{
			try
			{
				webDriver.Close();
				webDriver.Quit();
				webDriver.Dispose();
			}
			catch (Exception)
			{
				GC.SuppressFinalize(webDriver);
			}
		}
	}
}