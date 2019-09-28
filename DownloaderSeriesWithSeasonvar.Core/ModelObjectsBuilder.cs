using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class ModelObjectsBuilder
    {
        private readonly IInfoDownloader seasonInfoDownloader;
        private readonly IInfoDownloader tvSeriesInfoDownloader;

        public ModelObjectsBuilder(
            IInfoDownloader seasonInfoDownloader,
            IInfoDownloader tvSeriesInfoDownloader)
        {
            this.seasonInfoDownloader = seasonInfoDownloader;
            this.tvSeriesInfoDownloader = tvSeriesInfoDownloader;
        }

        public async Task<Season> BuildSeasonAsync(Uri address)
        {
            List<Uri> episodeAddressList;
            string seasonName;

            try
            {
                episodeAddressList = await seasonInfoDownloader.GetInfoListAsync(address);
                seasonName = await seasonInfoDownloader.GetOriginalNameAsync(address);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var episodeList = new List<Episode>();
            for (int i = 0; i < episodeAddressList.Count; i++)
            {
                var episode = new Episode(seasonName, episodeAddressList[i], 0, i);
                episodeList.Add(episode);
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
                {
                    var season = await BuildSeasonAsync(seasonUri);
                    tvSeries.SeasonList.Add(season);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return tvSeries;
        }
    }
}