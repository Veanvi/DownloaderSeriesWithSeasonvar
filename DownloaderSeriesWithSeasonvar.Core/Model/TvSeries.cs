using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    internal class TvSeries
    {
        public TvSeries(string uri, List<Uri> seasonUriList)
        {
            Uri = new Uri(uri);
            SeasonUriList = seasonUriList;
        }

        public List<Season> SeasonList { get; set; }
        public Uri Uri { get; }
        internal List<Uri> SeasonUriList { get; }
    }
}