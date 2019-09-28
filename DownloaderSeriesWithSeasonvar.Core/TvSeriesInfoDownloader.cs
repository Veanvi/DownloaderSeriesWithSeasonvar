using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class TvSeriesInfoDownloader : IInfoDownloader
    {
        private Uri lastInfoRequestAddress;
        private string originalName;

        public TvSeriesInfoDownloader(IWebRequester webRequester)
        {
            lastInfoRequestAddress = new Uri("http://seasonvar.ru");
            WebRequester = webRequester;
        }

        public IWebRequester WebRequester { get; }

        public List<Uri> GetInfoList(Uri address)
        {
            return GetInfoListAsync(address).Result;
        }

        public async Task<List<Uri>> GetInfoListAsync(Uri address)
        {
            lastInfoRequestAddress = address;
            List<Uri> seasonUriList = new List<Uri>();

            try
            {
                var pageSource = await WebRequester.GetWebPageSourceAsync(address.ToString());
                originalName = await GetNameFromPageAsync(pageSource);

                var document = await GetDocumentAsync(pageSource);
                var seasonLinkHtml = document.QuerySelectorAll("div.pgs-seaslist ul.tabs-result a");
                foreach (var item in seasonLinkHtml)
                {
                    string seasonUriStr = "http://seasonvar.ru" + item.GetAttribute("href");
                    seasonUriList.Add(new Uri(seasonUriStr));
                }
            }
            catch (Exception)
            {
                throw new Exception("Ошибка загрузки данных");
            }

            return seasonUriList;
        }

        public string GetOriginalName(Uri address)
        {
            return GetOriginalNameAsync(address).Result;
        }

        public async Task<string> GetOriginalNameAsync(Uri address)
        {
            if (!string.IsNullOrEmpty(originalName) &
                address.Equals(lastInfoRequestAddress))
                return originalName;

            try
            {
                var pageSource = await WebRequester.GetWebPageSourceAsync(address.ToString());
                originalName = await GetNameFromPageAsync(pageSource);
            }
            catch (Exception)
            {
                throw new Exception("Ошибка загрузки данных");
            }

            return originalName;
        }

        private async Task<IDocument> GetDocumentAsync(string pageSource)
        {
            var context = BrowsingContext.New(Configuration.Default);
            return await context.OpenAsync(req => req.Content(pageSource));
        }

        private async Task<string> GetNameFromPageAsync(string pageSource)
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(pageSource));
            string originalName = document
                .QuerySelector("div.pgs-sinfo-info > div:nth-child(3) > span")
                .TextContent;
            return originalName;
        }
    }
}