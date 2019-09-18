using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class DownloaderSeasonInfo
    {
        public DownloaderSeasonInfo(Uri uri, bool enableTorProxy = false, bool headless = true)
        {
            SeasonUri = uri;
            IsEnableTorProxy = enableTorProxy;
            IsHeadless = headless;
        }

        public bool IsEnableTorProxy { get; }
        public bool IsHeadless { get; }
        public Uri SeasonUri { get; }

        public Season DownloadSeasonInfo()
        {
            string playlistJson = null;
            string serialName = null;
            IWebDriver webDriver = null;

            try
            {
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

                webDriver.Navigate().GoToUrl(SeasonUri);

                string serealName = webDriver.FindElement(By
                    .CssSelector("body > div.wrapper > div > div.container > div > div:nth-child(1) > div > div > div > div.pgs-sinfo-info > div:nth-child(3) > span"))
                    .Text;

                playlistJson = GetPlist(webDriver);
            }
            catch (Exception e)
            {
            }
            finally
            {
                webDriver.Close();
                webDriver.Quit();
            }

            return new Season(serialName, playlistJson);
        }

        public async Task<Season> DownloadSeasonInfoAsync()
        {
            var task = Task<Season>.Factory.StartNew(DownloadSeasonInfo);
            await task;
            return task.Result;
        }

        private string GetPlist(IWebDriver webDriver)
        {
            var plistUri = webDriver.FindElement(By
                .CssSelector("#player_wrap > div.pgs-player-inside > script:nth-child(5)"))
                .GetAttribute("innerHTML")
                .Remove(0, 9);

            plistUri = plistUri.Split('"')[1];
            var timeSubstr = plistUri.IndexOf("?time=");
            plistUri = "http://seasonvar.ru" + plistUri.Remove(timeSubstr, plistUri.Length - timeSubstr);

            webDriver.Navigate().GoToUrl(plistUri);
            string plistJson = webDriver.FindElement(By.TagName("body")).Text;
            return plistJson;
        }
    }
}