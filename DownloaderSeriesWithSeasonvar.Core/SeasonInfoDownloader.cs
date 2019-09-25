using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class SeasonInfoDownloader : IInfoDownloader<Episode>
    {
        private Uri lastInfoRequestAddress;
        private string originalName;

        public SeasonInfoDownloader(bool enableTorProxy = false, bool headless = true)
        {
            IsEnableTorProxy = enableTorProxy;
            IsHeadless = headless;
            lastInfoRequestAddress = new Uri("http://seasonvar.ru");
        }

        public bool IsEnableTorProxy { get; }
        public bool IsHeadless { get; }

        public List<Episode> GetInfoList(Uri address)
        {
            lastInfoRequestAddress = address;
            string playlistJson = null;
            IWebDriver webDriver = null;

            try
            {
                webDriver = InitializeWebDriver();
                webDriver.Navigate().GoToUrl(address);
                originalName = GetNameFromPage(webDriver);
                playlistJson = GetPlist(webDriver);
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

            return PlaylistParser.JsonPlaylistConvertToSeasonObject(playlistJson);
        }

        public async Task<List<Episode>> GetInfoListAsync(Uri address)
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