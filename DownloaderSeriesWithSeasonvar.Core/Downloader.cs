using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class Downloader
    {
        private string plistReceivedManually;
        private string patternStr;

        public Downloader(Uri serealUri)
        {
            SerealUri = serealUri;
            patternStr = "//b2xvbG8=";
        }

        public Downloader(string plistString) : this(plistString, "//b2xvbG8=")
        {

        }

        public Downloader(string plistString, string patternString)
        {
            patternStr = patternString;
            plistReceivedManually = plistString;
        }

        public event EventHandler<string> NewStageOfWork;

        public string RandomCode { get; set; } = "b2xvbG8";
        public Season Season { get; private set; }
        public Uri SerealUri { get; }

        public bool CheckLoadSeries(Series series)
        {
            throw new NotImplementedException();
        }

        public Season FillOutSeasonInformation()
        {
            IWebDriver webDriver = null;

            try
            {

                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments(new List<string>()
                {
                    "--headless",
                    "--disable-gpu",
                    "--window-size=1200,1000",
                    "--blink-settings=imagesEnabled=false",
                    //"--proxy-server=socks5://localhost:9150"
                });

                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                webDriver = new ChromeDriver(chromeDriverService, chromeOptions);

                NewStageOfWork?.Invoke(this, "Запущен браузер.");

                if (SerealUri != null)
                {
                    webDriver.Navigate().GoToUrl(SerealUri);

                    string serealName = webDriver.FindElement(By
                        .CssSelector("body > div.wrapper > div > div.container > div > div:nth-child(1) > div > div > div > div.pgs-sinfo-info > div:nth-child(3) > span"))
                        .Text;

                    Season = new Season(serealName);
                }
                else
                {
                    Season = new Season("Empty");
                }
                string plistJson = GetPlist(webDriver);

                NewStageOfWork?.Invoke(this, "Получен Json с плейлистом.");

                var allSeriesJson = JArray.Parse(plistJson);

                foreach (var item in allSeriesJson)
                {
                    byte seriesNumber = (byte)item.SelectToken("id");
                    Uri seriesUri = ValidateUriSeries((string)item.SelectToken("file"));
                    //int fileSize = GetFileSize(seriesUri);
                    int fileSize = 0;

                    Season.AddSeries(seriesUri, fileSize, seriesNumber);
                }
                NewStageOfWork?.Invoke(this, "Ссылки на все серии получены.");
            }
            catch (Exception e)
            {
                NewStageOfWork?.Invoke(this, e.Message);
            }
            finally
            {
                webDriver.Close();
                webDriver.Quit();

                NewStageOfWork?.Invoke(this, "Браузер закрыт.");
            }

            return Season;
        }

        public async Task<Season> FillOutSeasonInformationAsync()
        {
            var task = Task<Season>.Factory.StartNew(FillOutSeasonInformation);
            await task;
            return task.Result;
        }

        private int GetFileSize(Uri uriPath)
        {
            var webRequest = HttpWebRequest.Create(uriPath);
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {
                var fileSize = webResponse.Headers.Get("Content-Length");
                var fileSizeInByte = Convert.ToInt32(fileSize);
                return fileSizeInByte;
            }
        }

        private string GetPlist(IWebDriver webDriver)
        {
            if (plistReceivedManually != null)
                return plistReceivedManually;

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

        private Uri ValidateUriSeries(string strUri)
        {
            strUri = strUri.Remove(0, 2);

            if (patternStr != "")
            {
                int indexM3 = strUri.IndexOf(patternStr);
                if (indexM3 > 0)
                {
                    var patternRemoveStr = strUri.Remove(indexM3, patternStr.Length);

                    string decodedStringBase = Encoding.UTF8.GetString(
                        Convert.FromBase64String(patternRemoveStr));

                    return new Uri(decodedStringBase);
                }
            }

            string surchPattern = "//";

            int startPatternPos = 0;
            int lastPatternPos = 0;

            if (strUri.Contains(surchPattern))
                startPatternPos = strUri.IndexOf('/');
            else
                throw new Exception("Не найден паттерн начала строки с шумом");

            for (int j = startPatternPos; j <= strUri.Length - 1; j++)
            {
                var sdf = strUri.Substring(startPatternPos, j - startPatternPos);

                if (strUri[j] == '=')
                {
                    lastPatternPos = j;
                    break;
                }
            }

            if (startPatternPos != lastPatternPos)
                strUri = strUri.Remove(startPatternPos, lastPatternPos - startPatternPos);

            for (int i = strUri.Length - 3; i >= 0; i--)
            {
                if (strUri[i] == '=')
                    strUri = strUri.Remove(i, 1);
            }


            byte[] data = Convert.FromBase64String(strUri);
            string decodedString = Encoding.UTF8.GetString(data);

            //Вырезание индекс файлов (index.m3u8 or )
            var subString = "index.m3u8 or ";
            var index = decodedString.IndexOf(subString);
            if (index > 0)
                decodedString = decodedString
                    .Remove(0, index + subString.Length);

            return new Uri(decodedString);
        }
    }
}