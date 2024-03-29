﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DownloaderSeriesWithSeasonvar.Core.Tests
{
    [TestClass]
    public class PlaylistParserTests
    {
        private Season correctConvertSeason;
        private string correctJsonPlaylist;

        [TestMethod]
        public void JsonPlaylistConvertToSeasonObject_EmptyNoisePattern_Exception()
        {
            // Arrage
            var noisePattern = "";
            // Act
            var exception = Assert.ThrowsException<Exception>(
                () => PlaylistParser.JsonPlaylistConvertToSeasonObject(correctJsonPlaylist, noisePattern));
            // Assert
            StringAssert.Contains(exception.Message, "Ошибка парсинга плейл");
        }

        [TestMethod]
        public void JsonPlaylistConvertToSeasonObject_JsonString_CorrectSeasonObject()
        {
            // Arrage
            //var pParser = new PlaylistParser("Dr. Horrible's Sing-Along Blog");
            // Act
            var seriesList = PlaylistParser.JsonPlaylistConvertToSeasonObject(correctJsonPlaylist);
            var result = new Season(null, correctJsonPlaylist);
            result.EpisodeList = seriesList;
            // Assert
            Assert.AreEqual(correctConvertSeason, result);
        }

        [TestMethod]
        public void JsonPlaylistConvertToSeasonObject_NotCorrectJsonString_Exception()
        {
            // Arrage
            var notCorrectJson = "[{\"title\":\"1 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2R";
            // Act
            var ex = Assert.ThrowsException<Exception>(
                () => { PlaylistParser.JsonPlaylistConvertToSeasonObject(notCorrectJson); });
            // Assert
            StringAssert.Contains(ex.Message, "Ошибка парсинга плейлиста");
        }

        [TestMethod]
        public void JsonPlaylistConvertToSeasonObject_NotCorrectNoisePattern_Exception()
        {
            // Arrage
            var noisePattern = "Empty";
            // Act
            var ex = Assert.ThrowsException<Exception>(
                () => { PlaylistParser.JsonPlaylistConvertToSeasonObject(correctJsonPlaylist, noisePattern); });
            // Assert
            StringAssert.Contains(ex.Message, "Ошибка парсинга плейлиста");
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            correctJsonPlaylist = "[{\"title\":\"1 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2Rhd\\/\\/b2xvbG8=GEwMC1jZG4uZGF0YWxvY2sucnUvZmkybG0vMjJmMzUxOTAwOGIwOGNmMjg5N2I5ODlhNjE3N2ZkZWUvN2ZfTXV6eWthbG55ai5ibG9nLkRva3RvcmEuVXpoYXNub2dvLkFrdC5QZXJ2eWouMjAwOC5YdmlELlRWUmlwLmExLjA5LjExLjEyLm1wNA==\",\"subtitle\":\"\",\"galabel\":\"5487_193210\",\"id\":\"1\",\"vars\":\"193210\"},{\"title\":\"2 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2RhdGEwMC1jZG4uZGF0YWxvY2sucnUvZmkybG0vMj\\/\\/b2xvbG8=JmMzUxOTAwOGIwOGNmMjg5N2I5ODlhNjE3N2ZkZWUvN2ZfTXV6eWthbG55ai5ibG9nLkRva3RvcmEuVXpoYXNub2dvLkFrdC5WdG9yb2ouMjAwOC5YdmlELlRWUmlwLmExLjA5LjExLjEyLm1wNA==\",\"subtitle\":\"\",\"galabel\":\"5487_193212\",\"id\":\"2\",\"vars\":\"193212\"},{\"title\":\"3 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2RhdGEwMC1jZG4uZGF0\\/\\/b2xvbG8=YWxvY2sucnUvZmkybG0vMjJmMzUxOTAwOGIwOGNmMjg5N2I5ODlhNjE3N2ZkZWUvN2ZfTXV6eWthbG55ai5ibG9nLkRva3RvcmEuVXpoYXNub2dvLkFrdC5UcmV0aWouMjAwOC5YdmlELlRWUmlwLmExLjA5LjExLjEyLm1wNA==\",\"subtitle\":\"\",\"galabel\":\"5487_193211\",\"id\":\"3\",\"vars\":\"193211\"}]";
            correctConvertSeason = new Season(null, correctJsonPlaylist);
            correctConvertSeason.EpisodeList = new List<Episode>()
            {
                new Episode(
                    "Episode 1",
                    new Uri("http://data00-cdn.datalock.ru/fi2lm/22f3519008b08cf2897b989a6177fdee/7f_Muzykalnyj.blog.Doktora.Uzhasnogo.Akt.Pervyj.2008.XviD.TVRip.a1.09.11.12.mp4"),
                    0,
                    1),
                new Episode(
                    "Episode 2",
                    new Uri("http://data00-cdn.datalock.ru/fi2lm/22f3519008b08cf2897b989a6177fdee/7f_Muzykalnyj.blog.Doktora.Uzhasnogo.Akt.Vtoroj.2008.XviD.TVRip.a1.09.11.12.mp4"),
                    0,
                    2),
                new Episode(
                    "Episode 3",
                    new Uri("http://data00-cdn.datalock.ru/fi2lm/22f3519008b08cf2897b989a6177fdee/7f_Muzykalnyj.blog.Doktora.Uzhasnogo.Akt.Tretij.2008.XviD.TVRip.a1.09.11.12.mp4"),
                    0,
                    3),
            };
        }
    }
}