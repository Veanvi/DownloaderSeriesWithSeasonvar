using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class TvSeries
    {
        internal TvSeries(Uri uri, List<Uri> seasonUriList)
        {
            Uri = uri;
            SeasonUriList = seasonUriList;
            SeasonList = new List<Season>();
        }

        public List<Season> SeasonList { get; set; }
        public Uri Uri { get; }
        internal List<Uri> SeasonUriList { get; }
    }
}