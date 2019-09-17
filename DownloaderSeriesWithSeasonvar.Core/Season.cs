using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class Season
    {
        internal Season(string serealName)
        {
            SerealName = serealName;
            SeriesList = new List<Series>();
        }

        public string SerealName { get; }


        public List<Series> SeriesList { get; private set; }

        internal void AddSeries(Uri fileUri, int fileSize, byte number)
        {
            SeriesList.Add(new Series($"{SerealName} Серия {number}", fileUri, fileSize, number));
        }
    }
}
