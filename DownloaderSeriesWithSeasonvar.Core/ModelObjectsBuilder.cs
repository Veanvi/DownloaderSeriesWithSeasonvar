using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class ModelObjectsBuilder
    {
        private readonly IInfoDownloader<Episode> seasonInfoDownloader;
        private readonly IInfoDownloader<Uri> tvSeriesInfoDownloader;

        public ModelObjectsBuilder(
            IInfoDownloader<Episode> seasonInfoDownloader,
            IInfoDownloader<Uri> tvSeriesInfoDownloader)
        {
            this.seasonInfoDownloader = seasonInfoDownloader;
            this.tvSeriesInfoDownloader = tvSeriesInfoDownloader;
        }

        public async Task<Season> BuildSeasonAsync(Uri address)
        {
            List<Episode> episodeList;

            try
            {
                episodeList = await seasonInfoDownloader.GetInfoListAsync(address);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var season = new Season(address)
            {
                EpisodeList = episodeList
            };

            return season;
        }

        public Season BuildSeasonFromJson(string seasonJson, string noisePattern = "")
        {
            Season season = null;
            season = new Season(null, seasonJson);

            if (noisePattern == "")
                season.EpisodeList = PlaylistParser
                    .JsonPlaylistConvertToSeasonObject(seasonJson);
            else
                season.EpisodeList = PlaylistParser
                    .JsonPlaylistConvertToSeasonObject(seasonJson, noisePattern);

            return season;
        }

        public async Task<TvSeries> BuildTvSeriesAsync(Uri address)
        {
            List<Uri> seasonAddressList;
            TvSeries tvSeries;

            try
            {
                seasonAddressList = await tvSeriesInfoDownloader.GetInfoListAsync(address);
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