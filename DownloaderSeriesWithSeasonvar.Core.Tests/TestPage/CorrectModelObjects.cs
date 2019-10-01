using AngleSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DownloaderSeriesWithSeasonvar.Core.Tests.TestPage
{
    public static class CorrectModelObjects
    {
        #region Season1

        public static Season GetSeason1Object()
        {
            var season = new Season(new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html"));

            var playlistJsonSource = GetSeason1Playlist();
            season.EpisodeList = GetEpisodes(playlistJsonSource);

            return season;
        }

        public static string GetSeason1Playlist()
        {
            var path = @"TestPage\Playlists\TouchOfClothS1.txt";
            return GetTextFromFile(path);
        }

        public static string GetSeason1PlaylistJson()
        {
            var path = @"TestPage\WebPages\TouchOfClothS1.txt";
            var pageSouce = GetTextFromFile(path);
            return GetJsonFromPage(pageSouce);
        }

        public static string GetSeason1Source()
        {
            var path = @"TestPage\WebPages\TouchOfClothS1.txt";
            return GetTextFromFile(path);
        }

        #endregion Season1

        #region Season2

        public static Season GetSeason2Object()
        {
            var season = new Season(new Uri("http://seasonvar.ru/serial-7755-Inspektor_Klot-2-season.html"));

            var playlistJsonSource = GetSeason1Playlist();
            season.EpisodeList = GetEpisodes(playlistJsonSource);

            return season;
        }

        public static string GetSeason2Playlist()
        {
            var path = @"TestPage\Playlists\TouchOfClothS2.txt";
            return GetTextFromFile(path);
        }

        public static string GetSeason2PlaylistJson()
        {
            var path = @"TestPage\WebPages\TouchOfClothS2.txt";
            var pageSouce = GetTextFromFile(path);
            return GetJsonFromPage(pageSouce);
        }

        public static string GetSeason2Source()
        {
            var path = @"TestPage\WebPages\TouchOfClothS2.txt";
            return GetTextFromFile(path);
        }

        #endregion Season2

        #region Season3

        public static Season GetSeason3Object()
        {
            var season = new Season(new Uri("http://seasonvar.ru/serial-10287-Inspektor_Klot-3-season.html"));

            var playlistJsonSource = GetSeason1Playlist();
            season.EpisodeList = GetEpisodes(playlistJsonSource);

            return season;
        }

        public static string GetSeason3Playlist()
        {
            var path = @"TestPage\Playlists\TouchOfCloth3.txt";
            return GetTextFromFile(path);
        }

        public static string GetSeason3PlaylistJson()
        {
            var path = @"TestPage\WebPages\TouchOfCloth3.txt";
            var pageSouce = GetTextFromFile(path);
            return GetJsonFromPage(pageSouce);
        }

        public static string GetSeason3Source()
        {
            var path = @"TestPage\WebPages\TouchOfCloth3.txt";
            return GetTextFromFile(path);
        }

        #endregion Season3

        #region Firefly

        public static string GetFireflyPlaylistJson()
        {
            var path = @"TestPage\Playlist\FireflyS1.txt";
            var pageSouce = GetTextFromFile(path);
            return GetJsonFromPage(pageSouce);
        }

        #endregion Firefly

        private static List<Episode> GetEpisodes(string Playlistsource)
        {
            string playlistJson = GetJsonFromPage(Playlistsource);

            var result = PlaylistParser
                .JsonPlaylistConvertToSeasonObject(playlistJson);
            return result;
        }

        private static string GetJsonFromPage(string Playlistsource)
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = context.OpenAsync(req => req.Content(Playlistsource)).Result;
            var playlistJson = document.QuerySelector("body").TextContent;
            return playlistJson;
        }

        private static string GetTextFromFile(string path)
        {
            if (!File.Exists(path))
                return "";
            string source;
            using (var reader = new StreamReader(path))
            {
                source = reader.ReadToEnd();
            }
            return source;
        }
    }
}