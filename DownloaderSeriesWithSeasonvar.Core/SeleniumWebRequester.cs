using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class SeleniumWebRequester : IWebRequester
    {
        public SeleniumWebRequester(bool enableTorProxy, bool headless)
        {
            IsEnableTorProxy = enableTorProxy;
            IsHeadless = headless;
            WebDriver = SetupWebDriver();
        }

        ~SeleniumWebRequester()
        {
            Dispose();
        }

        public bool IsEnableTorProxy { get; }
        public bool IsHeadless { get; }
        public IWebDriver WebDriver { get; }

        public void Dispose()
        {
            WebDriver.Close();
            WebDriver.Quit();
            GC.SuppressFinalize(this);
        }

        public string GetWebPageSource(string address)
        {
            if (WebDriver.Url != address)
                WebDriver.Navigate().GoToUrl(address);

            if (!WebDriver.Url.Contains("plist.txt"))
                new WebDriverWait(WebDriver, TimeSpan.FromSeconds(10))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By
                    .CssSelector("#player_wrap > div.pgs-player-inside > script:nth-child(5)")));

            return WebDriver.PageSource;
        }

        public async Task<string> GetWebPageSourceAsync(string address)
        {
            var task = Task.Factory.StartNew(() => GetWebPageSource(address));
            await task;
            return task.Result;
        }

        private IWebDriver SetupWebDriver()
        {
            IWebDriver webDriver;
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new List<string>()
                {
                    "--window-size=1200,1000",
                    "--blink-settings=imagesEnabled=false",
                });

            if (IsHeadless)
                chromeOptions.AddArgument("--headless");
            if (IsEnableTorProxy)
                chromeOptions.AddArgument("--proxy-server=socks5://localhost:9050");

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            webDriver = new ChromeDriver(chromeDriverService, chromeOptions);
            return webDriver;
        }
    }
}