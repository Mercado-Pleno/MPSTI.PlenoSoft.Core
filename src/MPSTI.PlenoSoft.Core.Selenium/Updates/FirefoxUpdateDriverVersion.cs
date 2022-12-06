using MPSTI.PlenoSoft.Core.Selenium.Factories;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Selenium.Updates
{
	public class FirefoxUpdateDriverVersion : BrowserUpdateDriverVersion
    {
        protected override string BrowserName => "Mozilla Firefox";
        protected override string BrowserDefaultPath => @"Mozilla Firefox\firefox.exe";
        protected override string BrowserFileName => "*firefox*.exe";
        protected override string DriverFileName => "*geckodriver*.exe";
        protected override string BaseUrlDownload => "https://github.com/mozilla/geckodriver/releases";
        protected override string XmlKeyDriverVersion => "";
        protected override string XmlPath => "";
        protected override string XmlKey => "";
        protected override string GetBaseUrl(string versao) => BaseUrlDownload + $"?q={versao}&expanded=false";

        public static UpdateInfo Update(params string[] browserFileLocations)
        {
            SeleniumFactory.BrowserType = BrowserType.FireFox;
            return new FirefoxUpdateDriverVersion().Start(browserFileLocations.Union(DefaultLocations));
        }
    }
}