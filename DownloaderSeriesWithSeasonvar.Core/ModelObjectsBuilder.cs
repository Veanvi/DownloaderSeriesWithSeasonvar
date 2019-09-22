using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class ModelObjectsBuilder
    {
        private readonly bool isEnableTorProxy;
        private readonly bool isHeadless;

        public ModelObjectsBuilder(bool enableTorProxy, bool headless)
        {
            isEnableTorProxy = enableTorProxy;
            isHeadless = headless;
        }

        public async Task<Season> BuildSeasonAsync(Uri address)
        {
            var downloaderPlist = new DownloaderSeasonInfo(
                address, isEnableTorProxy, isHeadless);
            string seasonJson = "";

            try
            {
                seasonJson = await downloaderPlist.DownloadInfoAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return BuildSeasonFromJson(seasonJson);
        }

        public Season BuildSeasonFromJson(string seasonJson, string noisePattern = "")
        {
            Season season = null;
            season = new Season(null, seasonJson);

            if (noisePattern == "")
                season.SeriesList = PlaylistParser
                    .JsonPlaylistConvertToSeasonObject(seasonJson);
            else
                season.SeriesList = PlaylistParser
                    .JsonPlaylistConvertToSeasonObject(seasonJson, noisePattern);

            return season;
        }

        public async Task<TvSeries> BuildTvSeriesAsync(Uri address)
        {
            var infoDownloader = new DownloaderTvSeriesInfo(
                address, isEnableTorProxy, isHeadless);
            List<Uri> seasonAddressList;
            TvSeries tvSeries;

            try
            {
                seasonAddressList = await infoDownloader.DownloadInfoAsync();
                tvSeries = new TvSeries(address, seasonAddressList);
                foreach (var seasonUri in tvSeries.SeasonUriList)
                    tvSeries.SeasonList.Add(await BuildSeasonAsync(seasonUri));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return tvSeries;
        }
    }
}