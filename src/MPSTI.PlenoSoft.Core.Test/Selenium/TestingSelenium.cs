using MPSTI.PlenoSoft.Core.Selenium.Extensions;
using MPSTI.PlenoSoft.Core.Selenium.Factories;
using MPSTI.PlenoSoft.Core.Selenium.Updates;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Test.Selenium
{
	public class TestingSelenium
	{
		[FactDebuggerOnly]
		public void QuandoFazUpdate_ChromeUpdateDriverVersion()
		{
			var updateInfo = ChromeDriverUpdateVersion.Update();

			Assert.NotNull(updateInfo);
			Assert.True(updateInfo.Updated);
			Assert.NotNull(updateInfo.BrowserName);
			Assert.NotNull(updateInfo.BrowserPath);
			Assert.NotNull(updateInfo.BrowserVersion);
			Assert.NotNull(updateInfo.DriverVersion);
			Assert.NotNull(updateInfo.Message);
		}

		[FactDebuggerOnly]
		public void QuandoFazUpdate_EdgeUpdateDriverVersion()
		{
			var updateInfo = EdgeDriverUpdateVersion.Update();

			Assert.NotNull(updateInfo);
			Assert.True(updateInfo.Updated);
			Assert.NotNull(updateInfo.BrowserName);
			Assert.NotNull(updateInfo.BrowserPath);
			Assert.NotNull(updateInfo.BrowserVersion);
			Assert.NotNull(updateInfo.DriverVersion);
			Assert.NotNull(updateInfo.Message);
		}

		[FactDebuggerOnly]
		public void QuandoFazUpdateFirefoxUpdateDriverVersion()
		{
			var updateInfo = FirefoxDriverUpdateVersion.Update();

			Assert.NotNull(updateInfo);
			Assert.True(updateInfo.Updated);
			Assert.NotNull(updateInfo.BrowserName);
			Assert.NotNull(updateInfo.BrowserPath);
			Assert.NotNull(updateInfo.BrowserVersion);
			Assert.NotNull(updateInfo.DriverVersion);
			Assert.NotNull(updateInfo.Message);
		}

		[FactDebuggerOnly]
		public void QuandoChamaChromeWebDriver_SeleniumFactory()
		{
			var webDriver = SeleniumFactory.GetChromeDriver(null, null);

			Assert.NotNull(webDriver);
			webDriver.Create().CloseAndDispose();
		}


		[FactDebuggerOnly]
		public void QuandoChamaChromeWebDriver_Google()
		{
			_ = ChromeDriverUpdateVersion.Update();
			var webDriver = SeleniumFactory.GetDriver(null, null);
			var selenium = webDriver.Create();
			selenium.GoTo("https://www.google.com.br/");
			var element = selenium.GetElementsByTagName("div").FirstOrDefault();
			var txt = element.GetValueOrTextOrContent();
			Assert.NotNull(txt);
		}
	}
}