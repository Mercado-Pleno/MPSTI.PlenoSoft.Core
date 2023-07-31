using MPSTI.PlenoSoft.Core.Selenium.Factories;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Selenium.Updates
{
	/// <summary>
	/// https://firefox-source-docs.mozilla.org/testing/geckodriver/Support.html
	/// </summary>
	public class FirefoxDriverUpdateVersion : DriverUpdateVersion
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

		public FirefoxDriverUpdateVersion() : base(BrowserType.FireFox) { }

		public static UpdateVersionInfo Update(params string[] browserFileLocations)
        {
			var driverUpdateVersion = new FirefoxDriverUpdateVersion();
			return driverUpdateVersion.Start(browserFileLocations);
		}
	}
}