using MPSTI.PlenoSoft.Core.Selenium.Drivers;
using MPSTI.PlenoSoft.Core.Selenium.Updates;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;

namespace MPSTI.PlenoSoft.Core.Selenium.Factories
{
    public enum BrowserType { Chrome, Edge, FireFox, }

	public static class SeleniumFactory
	{
		public static IWebDriver GetDriver(BrowserType? browserType = null, string webDriverLocation = null, int? portaTCP = null)
		{
			switch (browserType ?? DriverUpdateVersion.BrowserType)
			{
				case BrowserType.Chrome:
					return GetChromeDriver(webDriverLocation, portaTCP);
				case BrowserType.Edge:
					return GetEdgeDriver(webDriverLocation, portaTCP);
				case BrowserType.FireFox:
					return GetFirefoxDriver(webDriverLocation, portaTCP);
				default:
					return GetChromeDriver(webDriverLocation, portaTCP);
			}
		}

		public static IWebDriver GetChromeDriver(string webDriverLocation = null, int? portaTCP = null)
		{
			var fileLocation = (webDriverLocation ?? DriverUpdateVersion.DriverDefaultPath).FindFile("ChromeDriver*.exe", SearchOption.TopDirectoryOnly);
			var driverService = ChromeDriverService.CreateDefaultService(fileLocation.Directory.FullName, fileLocation.Name);
			if (portaTCP.HasValue)
			{
				driverService.Port = portaTCP.Value;
				driverService.PortServerAddress = portaTCP.Value.ToString();
			}
			var driverOptions = new ChromeOptions { AcceptInsecureCertificates = true, PageLoadStrategy = PageLoadStrategy.Normal };
			return new ChromeDriver(driverService, driverOptions, TimeSpan.FromSeconds(30));
		}

		public static IWebDriver GetFirefoxDriver(string webDriverLocation = null, int? portaTCP = null)
		{
			var fileLocation = (webDriverLocation ?? DriverUpdateVersion.DriverDefaultPath).FindFile("geckodriver*.exe", SearchOption.TopDirectoryOnly);
			var driverService = FirefoxDriverService.CreateDefaultService(fileLocation.Directory.FullName, fileLocation.Name);
			driverService.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
			if (portaTCP.HasValue)
				driverService.Port = portaTCP.Value;
			var driverOptions = new FirefoxOptions { AcceptInsecureCertificates = true, PageLoadStrategy = PageLoadStrategy.Normal };
			return new FirefoxDriver(driverService, driverOptions, TimeSpan.FromSeconds(30));
		}

		public static IWebDriver GetEdgeDriver(string webDriverLocation = null, int? portaTCP = null)
		{
			var fileLocation = (webDriverLocation ?? DriverUpdateVersion.DriverDefaultPath).FindFile("*EdgeDriver*.exe", SearchOption.TopDirectoryOnly);
			var driverService = EdgeDriverService.CreateDefaultService(fileLocation.Directory.FullName, fileLocation.Name);
			if (portaTCP.HasValue)
				driverService.Port = portaTCP.Value;
			var driverOptions = new EdgeOptions { AcceptInsecureCertificates = true, PageLoadStrategy = PageLoadStrategy.Normal };
			return new EdgeDriver(driverService, driverOptions, TimeSpan.FromSeconds(30));
		}

		public static SeleniumDriver Create(this IWebDriver webDriver) => new SeleniumDriver(webDriver);
	}
}