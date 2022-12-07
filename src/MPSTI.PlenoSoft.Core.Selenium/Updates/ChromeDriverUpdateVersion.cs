using MPSTI.PlenoSoft.Core.Selenium.Factories;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Selenium.Updates
{
	public class ChromeDriverUpdateVersion : DriverUpdateVersion
	{
		protected override string BrowserName => "Google Chrome";
		protected override string BrowserDefaultPath => @"Google\Chrome\Application\chrome.exe";
		protected override string BrowserFileName => "Chrome*.exe";
		protected override string DriverFileName => "ChromeDriver*.exe";
		protected override string BaseUrlDownload => "https://chromedriver.storage.googleapis.com";
		protected override string XmlKeyDriverVersion => "chromedriver_win32";
		protected override string XmlPath => "/a:ListBucketResult/a:Contents";
		protected override string XmlKey => "Key";
		protected override string GetBaseUrl(string versao) => BaseUrlDownload;

		public ChromeDriverUpdateVersion() : base(BrowserType.Chrome) { }

		public static UpdateVersionInfo Update(params string[] browserFileLocations)
		{
			var driverUpdateVersion = new ChromeDriverUpdateVersion();
			return driverUpdateVersion.Start(browserFileLocations.Union(DefaultLocations));
		}
	}
}