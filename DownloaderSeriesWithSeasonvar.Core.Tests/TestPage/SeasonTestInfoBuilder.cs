using AngleSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DownloaderSeriesWithSeasonvar.Core.Tests.TestPage
{
    internal static class SeasonTestInfoBuilder
    {
        private static string pathToPlaylists = @"TestPage\Playlists\";
        private static string pathToWebPages = @"TestPage\WebPages\";

        public enum ExistingSeasons
        {
            TouchOfClothS1,
            TouchOfClothS2,
            TouchOfClothS3,
            FireflyS1
        }

        public static SeasonTestInfo GetSeasonTestView(ExistingSeasons seasonName)
        {
            SeasonTestInfo testView = null;
            switch (seasonName)
            {
                case ExistingSeasons.TouchOfClothS1:
                    testView = BuildTouchOfClothS1();
                    break;

                case ExistingSeasons.TouchOfClothS2:
                    testView = BuildTouchOfClothS2();
                    break;

                case ExistingSeasons.TouchOfClothS3:
                    testView = BuildTouchOfClothS3();
                    break;

                case ExistingSeasons.FireflyS1:
                    testView = BuildFireflyS1();
                    break;
            }
            return testView;
        }

        private static SeasonTestInfo BuildFireflyS1()
        {
            string fileName = "FireflyS1.txt";
            string tvSeriesName = "Firefly";
            string uri = "http://seasonvar.ru/serial-41-Svetlyachok.html";
            string[] episodeFileNames =
            {
                "7f_Svetljachek.01.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.02.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.03.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.04.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.05.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.06.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.07.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.08.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.09.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.10.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.11.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.12.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.13.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Svetljachek.14.serija.iz.14.2002.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Missija.sereniti.2005.x264.BDRip.720p.kinozal.tv.HD.a1.04.08.16.mp4",
                "7f_Firefly.10th.Anniversary.Special.Browncoats.Unite.HDTVRip.Rus.Eng.AlexFilm.a1.20.12.12.mp4"
            };

            return GenericBuildSeason(fileName, tvSeriesName, uri, episodeFileNames);
        }

        private static SeasonTestInfo BuildTouchOfClothS1()
        {
            string fileName = "TouchOfClothS1.txt";
            string tvSeriesName = "A Touch of Cloth";
            string uri = "http://seasonvar.ru/serial-5583-Inspektor_Klot.html";
            string[] episodeFileNames =
            {
                "7f_A.Touch.Of.Cloth.S01E01.720p.HDTV.ViruseProject.a1.17.03.16.mp4",
                "7f_A.Touch.Of.Cloth.S01E02.720p.HDTV.ViruseProject.a1.17.03.16.mp4"
            };

            return GenericBuildSeason(fileName, tvSeriesName, uri, episodeFileNames);
        }

        private static SeasonTestInfo BuildTouchOfClothS2()
        {
            string fileName = "TouchOfClothS2.txt";
            string tvSeriesName = "A Touch of Cloth";
            string uri = "http://seasonvar.ru/serial-7755-Inspektor_Klot-2-season.html";
            string[] episodeFileNames =
            {
                "7f_A.Touch.Of.Cloth.S02E01.720p.HDTV.Rus.Eng.Subs-ViruseProject.a1.17.03.16.mp4",
                "7f_A.Touch.Of.Cloth.S02E02.720p.HDTV.Rus.Eng.Subs-ViruseProject.a1.17.03.16.mp4"
            };

            return GenericBuildSeason(fileName, tvSeriesName, uri, episodeFileNames);
        }

        private static SeasonTestInfo BuildTouchOfClothS3()
        {
            string fileName = "TouchOfClothS3.txt";
            string tvSeriesName = "A Touch of Cloth";
            string uri = "http://seasonvar.ru/serial-10287-Inspektor_Klot-3-season.html";
            string[] episodeFileNames =
            {
                "7f_A.Touch.of.Cloth.S03E01.ViruseProject.a1.02.09.14.mp4",
                "7f_A.Touch.of.Cloth.S03E02.ViruseProject.a1.09.09.14.mp4"
            };

            return GenericBuildSeason(fileName, tvSeriesName, uri, episodeFileNames);
        }

        private static SeasonTestInfo GenericBuildSeason(string fileName,
                                                         string tvSeriesName,
                                                         string uri,
                                                         string[] episodeFileNames)
        {
            string webSource = GetTextFromFile(pathToWebPages + fileName);
            string jsonWebSourcen = GetTextFromFile(pathToPlaylists + fileName);
            string jsonString = GetJsonFromPage(jsonWebSourcen);

            return new SeasonTestInfo(webSource,
                                      jsonWebSourcen,
                                      jsonString,
                                      tvSeriesName,
                                      uri,
                                      episodeFileNames);
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