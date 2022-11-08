using MPSTI.PlenoSoft.Core.Selenium;

namespace MPSTI.PlenoSoft.Core.Test.Selenium
{
    public class TestingSelenium
    {
        [FactDebuggerOnly]
        public void QuandoFazUpdate_ChromeUpdateDriverVersion()
        {
            var update = ChromeUpdateDriverVersion.Update(@"Program Files");

            Assert.NotNull(update);
        }

        [FactDebuggerOnly]
        public void QuandoChamaChromeWebDriver_SeleniumFactory()
        {
            var driverPath = @"D:\Prj\Git\MPSC.PlenoSoft.WhatsApp.API\src\Libs\";

            var webDriver = SeleniumFactory.ChromeWebDriver(webDriverLocation: new FileInfo(driverPath));

            Assert.NotNull(webDriver);
        }
    }
}
