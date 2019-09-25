using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class TvSeriesInfoDownloader : IInfoDownloader<Uri>
    {
        private Uri lastInfoRequestAddress;
        private string originalName;

        public TvSeriesInfoDownloader(bool enableTorProxy = false, bool headless = true)
        {
            IsEnableTorProxy = enableTorProxy;
            IsHeadless = headless;
            lastInfoRequestAddress = new Uri("http://seasonvar.ru");
        }

        public bool IsEnableTorProxy { get; }

        public bool IsHeadless { get; }

        public List<Uri> GetInfoList(Uri address)
        {
            lastInfoRequestAddress = address;
            List<Uri> seasonUriList = new List<Uri>();
            IWebDriver webDriver = null;

            try
            {
                webDriver = InitializeWebDriver();
                webDriver.Navigate().GoToUrl(address);
                originalName = GetNameFromPage(webDriver);

                var seasonLinkHtml = webDriver.FindElements(By
                    .CssSelector("div.pgs-seaslist ul.tabs-result a"));
                foreach (var item in seasonLinkHtml)
                {
                    string seasonUriStr = item.GetAttribute("href");
                    seasonUriList.Add(new Uri(seasonUriStr));
                }
            }
            catch (Exception)
            {
                throw new Exception("Ошибка загрузки данных");
            }
            finally
            {
                webDriver.Close();
                webDriver.Quit();
            }

            return seasonUriList;
        }

        public async Task<List<Uri>> GetInfoListAsync(Uri address)
        {
            var task = Task.Factory.StartNew(() => GetInfoList(address));
            await task;
            return task.Result;
        }

        public string GetOriginalName(Uri address)
        {
            if (!string.IsNullOrEmpty(originalName) &
                address.Equals(lastInfoRequestAddress))
                return originalName;

            IWebDriver webDriver = null;
            try
            {
                webDriver = InitializeWebDriver();
                webDriver.Navigate().GoToUrl(address);
                originalName = GetNameFromPage(webDriver);
            }
            catch (Exception)
            {
                throw new Exception("Ошибка загрузки данных");
            }
            finally
            {
                webDriver.Close();
                webDriver.Quit();
            }

            return originalName;
        }

        public async Task<string> GetOriginalNameAsync(Uri address)
        {
            var task = Task.Factory.StartNew(() => GetOriginalName(address));
            await task;
            return task.Result;
        }

        private string GetNameFromPage(IWebDriver webDriver)
        {
            string originalName = webDriver.FindElement(By
                    .CssSelector("div.pgs-sinfo-info > div:nth-child(2) > span"))
                    .Text;
            return originalName;
        }

        private IWebDriver InitializeWebDriver()
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