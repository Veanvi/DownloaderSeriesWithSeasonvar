using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public static class PlaylistParser
    {
        public static List<Series> JsonPlaylistConvertToSeasonObject(string playlistJson, string noisePattern = "//b2xvbG8=")
        {
            var allSeriesJson = JArray.Parse(playlistJson);
            var seriesList = new List<Series>();

            foreach (var item in allSeriesJson)
            {
                byte seriesNumber = (byte)item.SelectToken("id");
                Uri seriesUri = ValidateUriSeries((string)item.SelectToken("file"), noisePattern);
                //int fileSize = GetFileSize(seriesUri);
                int fileSize = 0;

                seriesList.Add(new Series($"Серия seriesNumber", seriesUri, fileSize, seriesNumber));
            }

            return seriesList;
        }

        private static Uri ValidateUriSeries(string strUri, string noisePattern)
        {
            strUri = strUri.Remove(0, 2);

            if (noisePattern != "")
            {
                int indexM3 = strUri.IndexOf(noisePattern);
                if (indexM3 > 0)
                {
                    var patternRemoveStr = strUri.Remove(indexM3, noisePattern.Length);

                    string decodedStringBase = Encoding.UTF8.GetString(
                        Convert.FromBase64String(patternRemoveStr));

                    return new Uri(decodedStringBase);
                }
            }

            string surchPattern = "//";

            int startPatternPos = 0;
            int lastPatternPos = 0;

            if (strUri.Contains(surchPattern))
                startPatternPos = strUri.IndexOf('/');
            else
                throw new Exception("Не найден паттерн начала строки с шумом");

            for (int j = startPatternPos; j <= strUri.Length - 1; j++)
            {
                var sdf = strUri.Substring(startPatternPos, j - startPatternPos);

                if (strUri[j] == '=')
                {
                    lastPatternPos = j;
                    break;
                }
            }

            if (startPatternPos != lastPatternPos)
                strUri = strUri.Remove(startPatternPos, lastPatternPos - startPatternPos);

            for (int i = strUri.Length - 3; i >= 0; i--)
            {
                if (strUri[i] == '=')
                    strUri = strUri.Remove(i, 1);
            }

            byte[] data = Convert.FromBase64String(strUri);
            string decodedString = Encoding.UTF8.GetString(data);

            //Вырезание индекс файлов (index.m3u8 or )
            var subString = "index.m3u8 or ";
            var index = decodedString.IndexOf(subString);
            if (index > 0)
                decodedString = decodedString
                    .Remove(0, index + subString.Length);

            return new Uri(decodedString);
        }
    }
}