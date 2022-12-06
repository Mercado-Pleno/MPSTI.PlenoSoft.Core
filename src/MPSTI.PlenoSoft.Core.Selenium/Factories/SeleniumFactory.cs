﻿using MPSTI.PlenoSoft.Core.Selenium.Extensions;
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
		public static BrowserType BrowserType { get; set; } = BrowserType.Chrome;

		public static IWebDriver BrowserWebDriver(BrowserType? browserType = null, string webDriverLocation = null, int? portaTCP = null)
		{
			switch (browserType ?? BrowserType)
			{
				case BrowserType.Chrome:
					return ChromeWebDriver(webDriverLocation, portaTCP);
				case BrowserType.Edge:
					return EdgeWebDriver(webDriverLocation, portaTCP);
				case BrowserType.FireFox:
					return FirefoxWebDriver(webDriverLocation, portaTCP);
				default:
					return ChromeWebDriver(webDriverLocation, portaTCP);
			}
		}

		public static IWebDriver ChromeWebDriver(string webDriverLocation = null, int? portaTCP = null)
		{
			var fileLocation = (webDriverLocation ?? BrowserUpdateDriverVersion.DriverDefaultPath).FindFile("ChromeDriver*.exe", SearchOption.TopDirectoryOnly);
			var driverService = ChromeDriverService.CreateDefaultService(fileLocation.Directory.FullName, fileLocation.Name);
			if (portaTCP.HasValue)
			{
				driverService.Port = portaTCP.Value;
				driverService.PortServerAddress = portaTCP.Value.ToString();
			}
			var driverOptions = new ChromeOptions { AcceptInsecureCertificates = true, PageLoadStrategy = PageLoadStrategy.Normal };
			return new ChromeDriver(driverService, driverOptions, TimeSpan.FromSeconds(30));
		}

		public static IWebDriver FirefoxWebDriver(string webDriverLocation, int? portaTCP)
		{
			var fileLocation = (webDriverLocation ?? BrowserUpdateDriverVersion.DriverDefaultPath).FindFile("geckodriver*.exe", SearchOption.TopDirectoryOnly);
			var driverService = FirefoxDriverService.CreateDefaultService(fileLocation.Directory.FullName, fileLocation.Name);
			driverService.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
			if (portaTCP.HasValue)
				driverService.Port = portaTCP.Value;
			var driverOptions = new FirefoxOptions { AcceptInsecureCertificates = true, PageLoadStrategy = PageLoadStrategy.Normal };
			return new FirefoxDriver(driverService, driverOptions, TimeSpan.FromSeconds(30));
		}

		public static IWebDriver EdgeWebDriver(string webDriverLocation, int? portaTCP)
		{
			var fileLocation = (webDriverLocation ?? BrowserUpdateDriverVersion.DriverDefaultPath).FindFile("*EdgeDriver*.exe", SearchOption.TopDirectoryOnly);
			var driverService = EdgeDriverService.CreateDefaultService(fileLocation.Directory.FullName, fileLocation.Name);
			if (portaTCP.HasValue)
				driverService.Port = portaTCP.Value;
			var driverOptions = new EdgeOptions { AcceptInsecureCertificates = true, PageLoadStrategy = PageLoadStrategy.Normal };
			return new EdgeDriver(driverService, driverOptions, TimeSpan.FromSeconds(30));
		}

		public static SeleniumDriver Create(this IWebDriver webDriver) => new SeleniumDriver(webDriver);
	}
}