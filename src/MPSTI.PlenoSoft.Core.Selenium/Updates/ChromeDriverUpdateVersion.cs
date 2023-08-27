using MPSTI.PlenoSoft.Core.Selenium.Factories;
using Newtonsoft.Json;
using System;
using System.IO;
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
        protected virtual string AlternateBaseUrlDownload => "https://googlechromelabs.github.io/chrome-for-testing/known-good-versions-with-downloads.json";
        protected override string XmlKeyDriverVersion => "chromedriver_win32";
        protected override string XmlPath => "/a:ListBucketResult/a:Contents";
        protected override string XmlKey => "Key";
        protected override string GetBaseUrl(string versao) => BaseUrlDownload;

        public ChromeDriverUpdateVersion() : base(BrowserType.Chrome) { }

        public static UpdateVersionInfo Update(params string[] browserFileLocations)
        {
            var driverUpdateVersion = new ChromeDriverUpdateVersion();
            return driverUpdateVersion.Start(browserFileLocations);
        }

        protected override void DownloadWebDriverInAlternateServer(string[] versionArray, FileInfo driverFile)
        {
            var take = versionArray.Length;
            var versao = string.Join(".", versionArray.Take(take));
            var jsonString = XmlUtil.GetFromUrl(AlternateBaseUrlDownload);
            var version = JsonConvert.DeserializeObject<Version>(jsonString);
            var keys = version.versions.Where(x => x?.downloads?.chromedriver != null).SelectMany(v => v.downloads.chromedriver);
            var files = keys.Where(k => XmlKeyDriverVersion.Contains(k.platform)).ToArray();

            var versoes = new string[0];
            while (versoes.Length == 0 && take > 0)
            {
                versoes = files.Where(x => x.url.Contains(versao)).Select(n => n.url).ToArray();
                take--;
                versao = string.Join(".", versionArray.Take(take));
            }

            if (!string.IsNullOrWhiteSpace(versao))
            {
                DownloadWebDriver(Path.Combine(DriverDefaultPath, versao), versoes.LastOrDefault(), DriverFileName, driverFile);
            }
        }

        public class Version
        {
            public DateTime timestamp { get; set; }
            public Versions[] versions { get; set; }
        }

        public class Versions
        {
            public Downloads downloads { get; set; }
        }

        public class Downloads
        {
            public Chromedriver[] chromedriver { get; set; }
        }

        public class Chromedriver
        {
            public string platform { get; set; }
            public string url { get; set; }
        }
    }
}