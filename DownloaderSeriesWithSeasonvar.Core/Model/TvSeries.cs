using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    internal class TvSeries
    {
        private List<Season> seasonList;

        public TvSeries(string uri)
        {
            Uri = new Uri(uri);
        }

        public List<Season> SeasonList
        {
            get
            {
                if (seasonList != null)
                    return seasonList;
                seasonList = GetSeasonList();
                return seasonList;
            }
        }

        public List<Uri> SeasonUriList { get; }
        public Uri Uri { get; }

        private List<Season> GetSeasonList()
        {
            throw new NotImplementedException();
        }
    }
}