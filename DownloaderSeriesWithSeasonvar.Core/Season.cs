using System;
using System.Collections.Generic;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class Season
    {
        public Season(string serealName) : this(serealName, null)
        {
        }

        internal Season(string serealName, string playlistJson)
        {
            SerealName = serealName;
            PlaylistJson = playlistJson;
            if (PlaylistJson != null)
            {
                SeriesList = PlaylistParser.JsonPlaylistConvertToSeasonObject(playlistJson);
            }
        }

        public string PlaylistJson { get; }

        public string SerealName { get; }

        private List<Series> seriesList;

        public List<Series> SeriesList
        {
            get
            {
                if (seriesList == null)
                    seriesList = new List<Series>();
                return seriesList;
            }
            internal set
            {
                seriesList = new List<Series>();
                foreach (var item in value)
                {
                    AddSeries(item.FileUri, item.FileSize, item.Number);
                }
            }
        }

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
            for (int i = 0; i < SeriesList.Count; i++)
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