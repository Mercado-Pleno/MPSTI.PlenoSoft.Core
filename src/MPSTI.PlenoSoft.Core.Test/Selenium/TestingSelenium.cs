using MPSTI.PlenoSoft.Core.Selenium.Factories;
using MPSTI.PlenoSoft.Core.Selenium.Updates;

namespace MPSTI.PlenoSoft.Core.Test.Selenium
{
	public class TestingSelenium
    {
        private readonly string[] locations = new[] { @"C:\Program Files", @"C:\Program Files (x86)" };

        [FactDebuggerOnly]
        public void QuandoFazUpdate_ChromeUpdateDriverVersion()
        {
            var updateInfo = ChromeUpdateDriverVersion.Update(locations);

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
            var updateInfo = EdgeUpdateDriverVersion.Update(locations);

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
            var webDriver = SeleniumFactory.ChromeWebDriver(null, null);

            Assert.NotNull(webDriver);
            webDriver.Create().CloseAndDispose();
		}
    }
}
