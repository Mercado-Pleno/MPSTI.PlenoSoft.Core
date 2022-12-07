using MPSTI.PlenoSoft.Core.Selenium.Factories;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Selenium.Updates
{
	public class EdgeDriverUpdateVersion : DriverUpdateVersion
    {
        protected override string BrowserName => "Microsoft Edge";
        protected override string BrowserDefaultPath => @"Microsoft\Edge\Application\msedge.exe";
        protected override string BrowserFileName => "*edge*.exe";
        protected override string DriverFileName => "*edgedriver*.exe";
        protected override string BaseUrlDownload => "https://msedgewebdriverstorage.blob.core.windows.net/edgewebdriver";
        protected override string XmlKeyDriverVersion => "edgedriver_win32";
        protected override string XmlPath => "EnumerationResults/Blobs/Blob";
        protected override string XmlKey => "Name";
        protected override string GetBaseUrl(string versao) => BaseUrlDownload + $"?comp=list&maxresults=500&prefix={versao}";

		public EdgeDriverUpdateVersion() : base(BrowserType.Edge) { }
		
        public static UpdateVersionInfo Update(params string[] browserFileLocations)
        {
			var driverUpdateVersion = new EdgeDriverUpdateVersion();
			return driverUpdateVersion.Start(browserFileLocations.Union(DefaultLocations));
		}
	}
}