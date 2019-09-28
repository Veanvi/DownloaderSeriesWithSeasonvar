using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class SeasonInfoDownloader : IInfoDownloader
    {
        private Uri lastInfoRequestAddress;
        private string originalName;

        public SeasonInfoDownloader(IWebRequester webRequester)
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
            string playlistJson = null;

            try
            {
                string pageSource = WebRequester.GetWebPageSource(address.ToString());
                originalName = await GetNameFromPageAsync(pageSource);
                playlistJson = await GetPlistAsync(pageSource);
            }
            catch (Exception)
            {
                throw new Exception("Ошибка загрузки данных");
            }

            var uriList = new List<Uri>();

            foreach (var item in PlaylistParser.JsonPlaylistConvertToSeasonObject(playlistJson))
            {
                uriList.Add(item.FileUri);
            }

            return uriList;
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
                var pageSource = WebRequester.GetWebPageSource(address.ToString());
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

        private async Task<string> GetPlistAsync(string pageSource)
        {
            var document = await GetDocumentAsync(pageSource);

            string plistUri = document
                .QuerySelector("#player_wrap > div.pgs-player-inside > script:nth-child(5)")
                .InnerHtml
                .Remove(0, 9);

            plistUri = plistUri.Split('"')[1];
            var timeSubstr = plistUri.IndexOf("?time=");
            plistUri = "http://seasonvar.ru" + plistUri.Remove(timeSubstr, plistUri.Length - timeSubstr);

            var plistJsonPage = WebRequester.GetWebPageSource(plistUri);
            document = await GetDocumentAsync(plistJsonPage);
            return document.QuerySelector("body").TextContent;
        }
    }
}