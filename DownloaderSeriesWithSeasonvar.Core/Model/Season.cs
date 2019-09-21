using System;
using System.Collections.Generic;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class Season
    {
        private List<Episode> seriesList;

        internal Season(Uri address) : this(address, null)
        {
        }

        internal Season(Uri address, string playlistJson)
        {
            Address = address;
            PlaylistJson = playlistJson;
        }

        public Uri Address { get; private set; }
        public string PlaylistJson { get; }

        public List<Episode> SeriesList
        {
            get
            {
                if (seriesList == null)
                    seriesList = new List<Episode>();
                return seriesList;
            }
            set
            {
                seriesList = new List<Episode>();
                foreach (var item in value)
                {
                    AddSeries(item.FileUri, item.FileSize, item.Number);
                }
            }
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
                PlaylistJson == other.PlaylistJson)
                return true;

            return false;
        }

        internal void AddSeries(Uri fileUri, int fileSize, byte number)
        {
            SeriesList.Add(new Episode($"Серия {number}", fileUri, fileSize, number));
        }
    }
}