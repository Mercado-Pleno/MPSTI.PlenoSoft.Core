using MPSTI.PlenoSoft.Core.Selenium.Factories;

namespace MPSTI.PlenoSoft.Core.Selenium.Updates
{
	public class ChromeUpdateDriverVersion : BrowserUpdateDriverVersion
    {
        protected override string BrowserName => "Google Chrome";
        protected override string BrowserDefaultPath => @"Google\Chrome\Application\chrome.exe";
        protected override string BrowserFileName => "Chrome*.exe";
        protected override string DriverFileName => "ChromeDriver*.exe";
        protected override string BaseUrlDownload => "https://chromedriver.storage.googleapis.com";
        protected override string XmlKeyDriverVersion => "chromedriver_win32";
        protected override string XmlPath => "/a:ListBucketResult/a:Contents";
        protected override string XmlKey => "Key";
        protected override string GetBaseURL(string versao) => BaseUrlDownload;

        public static UpdateInfo Update(params string[] browserFileLocations)
        {
            SeleniumFactory.BrowserType = BrowserType.Chrome;
            return new ChromeUpdateDriverVersion().Start(browserFileLocations);
        }
    }
}