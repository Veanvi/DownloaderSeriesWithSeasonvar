using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class Season
    {
        internal Season(string serealName):this(serealName, new List<Series>()){}

        internal Season(string serealName, List<Series> seriesList)
        {
            SerealName = serealName;
            SeriesList = seriesList;
        }

        public string SerealName { get; }


        public List<Series> SeriesList { get; private set; }

        internal void AddSeries(Uri fileUri, int fileSize, byte number)
        {
            SeriesList.Add(new Series($"{SerealName} Серия {number}", fileUri, fileSize, number));
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as Season;
            if (SeriesList.Count != other.SeriesList.Count)
                return false;

            bool isSeriesListEquals = true;
            for(int i = 0; i < SeriesList.Count; i++)
            {
                if (isSeriesListEquals == false)
                    break;
                isSeriesListEquals = SeriesList[i].Equals(other.SeriesList[i]);
            }

            if (isSeriesListEquals == true &&
                SerealName == other.SerealName)
                return true;

            return false;
        }
    }
}
