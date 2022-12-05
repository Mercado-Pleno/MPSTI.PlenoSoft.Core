namespace MPSTI.PlenoSoft.Core.Selenium.Updates
{
    public class EdgeUpdateDriverVersion : BrowserUpdateDriverVersion
    {
        protected override string BrowserName => "Microsoft Edge";
        protected override string BrowserDefaultPath => @"Microsoft\Edge\Application\msedge.exe";
        protected override string BrowserFileName => "*edge*.exe";
        protected override string DriverFileName => "*edgedriver*.exe";
        protected override string XmlKeyDriverVersion => "edgedriver_win32";
        protected override string BaseUrlDownload => "https://msedgewebdriverstorage.blob.core.windows.net/edgewebdriver";
        protected override string XmlPath => "EnumerationResults/Blobs/Blob";
        protected override string XmlKey => "Name";
        protected override string GetBaseURL(string versao) => BaseUrlDownload + $"?comp=list&maxresults=500&prefix={versao}";

        public static UpdateInfo Update(params string[] browserFileLocations)
        {
            SeleniumFactory.BrowserType = BrowserType.Edge;
            return new EdgeUpdateDriverVersion().Start(browserFileLocations);
        }
    }
}