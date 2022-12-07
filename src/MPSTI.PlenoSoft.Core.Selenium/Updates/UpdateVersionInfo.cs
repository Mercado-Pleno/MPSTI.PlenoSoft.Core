namespace MPSTI.PlenoSoft.Core.Selenium.Updates
{
    public class UpdateVersionInfo
	{
		public bool Updated { get; }
		public string BrowserPath { get; }
		public string BrowserName { get; }
		public string BrowserVersion { get; }
		public string DriverVersion { get; }

		public string Message => Updated ? UpdatedMessage : DownloadMessage;
		private string DownloadMessage => $@"Browser {BrowserName} does not exists in '{BrowserPath}'.
Please, Download and Install {BrowserName}!";

		private string UpdatedMessage => $@"Updated!
{BrowserName} Version:
Browser: {BrowserVersion}
Driver : {DriverVersion}.";

		public UpdateVersionInfo(bool updated, string browserPath, string browserName, string browserVersion = null, string driverVersion = null)
		{
			Updated = updated;
			BrowserPath = browserPath;
			BrowserName = browserName;
			BrowserVersion = browserVersion ?? IoExtension.EmptyVersion;
			DriverVersion = driverVersion ?? IoExtension.EmptyVersion;
		}
	}
}