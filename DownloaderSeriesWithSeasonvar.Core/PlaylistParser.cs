using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public static class PlaylistParser
    {
        public static List<Episode> JsonPlaylistConvertToSeasonObject(string playlistJson, string noisePattern = "//b2xvbG8=")
        {
            var allSeriesJson = JArray.Parse(playlistJson);
            var seriesList = new List<Episode>();

            foreach (var series in allSeriesJson)
            {
                byte seriesNumber = (byte)series.SelectToken("id");
                Uri seriesUri = RemoveNoiseSubstring((string)series.SelectToken("file"), noisePattern);
                //int fileSize = GetFileSize(seriesUri);
                int fileSize = 0;

                seriesList.Add(new Episode($"Серия seriesNumber", seriesUri, fileSize, seriesNumber));
            }

            return seriesList;
        }

        private static Uri RemoveNoiseSubstring(string strUriInBase64, string noisePattern)
        {
            if (noisePattern == "")
                throw new Exception("Передана пустая строка для поиска шума");

            strUriInBase64 = strUriInBase64.Remove(0, 2);
            int startNoiseSubstr = strUriInBase64.IndexOf(noisePattern);

            if (startNoiseSubstr == -1)
                throw new Exception("Не найдена подстрока с шумом");

            var strWithoutNoise = strUriInBase64.Remove(startNoiseSubstr, noisePattern.Length);
            string decodedStringBase = Encoding.UTF8
                .GetString(Convert.FromBase64String(strWithoutNoise));

            return new Uri(decodedStringBase);
        }
    }
}