using MPSTI.PlenoSoft.Core.Selenium;
using System.IO;

namespace MPSTI.PlenoSoft.Core.Test.Core
{
	public class TestingSelenium
	{
		[Fact]
		public void Test()
		{
			Assert.NotNull("");
		}

		[Fact(Skip = "ignore")]
		public void Test1()
		{
			var update = ChromeUpdateDriverVersion.Update(@"Program Files");

			Assert.NotNull(update);
		}

		[Fact(Skip = "ignore")]
		public void Test2()
		{
			var driverPath = @"D:\Prj\Git\MPSC.PlenoSoft.WhatsApp.API\src\Libs\";

			var webDriver = SeleniumFactory.ChromeWebDriver(webDriverLocation: new FileInfo(driverPath));

			Assert.NotNull(webDriver);
		}
	}
}
