using System;
using System.Collections.Generic;
using System.Linq;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class Season
    {
        internal Season(Uri address, string playlistJson)
        {
            Address = address;
            PlaylistJson = playlistJson ?? string.Empty;
            EpisodeList = new List<Episode>();
        }

        public Uri Address { get; private set; }
        public List<Episode> EpisodeList { get; set; }
        public string PlaylistJson { get; }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"Address:{Address}, PlaylistJsonHashCode:{PlaylistJson.GetHashCode()}, EpisodeListHashCode:{GetEpisodeListHashCode()}";
        }

        internal void AddSeries(Uri fileUri, int fileSize, byte number)
        {
            EpisodeList.Add(new Episode($"Серия {number}", fileUri, fileSize, number));
        }

        private string GetEpisodeListHashCode()
        {
            var result = string.Empty;
            foreach (var item in EpisodeList)
                result += item.GetHashCode().ToString();
            return result;
        }
    }
}