using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    internal class DownloaderTvSeriesInfo
    {
        private Uri address;
        private bool isEnableTorProxy;
        private bool isHeadless;

        public DownloaderTvSeriesInfo(Uri address, bool isEnableTorProxy, bool isHeadless)
        {
            this.address = address;
            this.isEnableTorProxy = isEnableTorProxy;
            this.isHeadless = isHeadless;
        }

        internal List<Uri> DownloadInfo()
        {
            string seriesName = null;
            List<Uri> seasonUriList = new List<Uri>();
            IWebDriver webDriver = null;

            try
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments(new List<string>()
                {
                    "--window-size=1200,1000",
                    "--blink-settings=imagesEnabled=false",
                });

                if (isHeadless)
                    chromeOptions.AddArgument("--headless");
                if (isEnableTorProxy)
                    chromeOptions.AddArgument("--proxy-server=socks5://localhost:9050");

                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                webDriver = new ChromeDriver(chromeDriverService, chromeOptions);

                webDriver.Navigate().GoToUrl(address);

                // TODO: Реализовать получение имени сереала через свойство
                string serealName = webDriver.FindElement(By
                    .CssSelector("body > div.wrapper > div > div.container > div > div:nth-child(1) > div > div > div > div.pgs-sinfo-info > div:nth-child(3) > span"))
                    .Text;

                var seasonLinkHtml = webDriver.FindElements(By.CssSelector("div.pgs-seaslist ul.tabs-result a"));
                foreach (var item in seasonLinkHtml)
                {
                    string seasonUriStr = item.GetAttribute("href");
                    seasonUriList.Add(new Uri(seasonUriStr));
                }
            }
            catch (Exception e)
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

        internal async Task<List<Uri>> DownloadInfoAsync()
        {
            var task = Task.Factory.StartNew(DownloadInfo);
            await task;
            return task.Result;
        }
    }
}