using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DownloaderSeriesWithSeasonvar.Core.Tests
{
    [TestClass]
    public class ModelObjectsBuilderTests
    {
        [TestMethod]
        public void BuildSeasonFromJson_CorrectJson_ReturnCorrectObject()
        {
            // Arrange
            var modelObjectsBuilder = new ModelObjectsBuilder(true, true);

            var correctJsonPlaylist = "[{\"title\":\"1 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2Rhd\\/\\/b2xvbG8=GEwMC1jZG4uZGF0YWxvY2sucnUvZmkybG0vMjJmMzUxOTAwOGIwOGNmMjg5N2I5ODlhNjE3N2ZkZWUvN2ZfTXV6eWthbG55ai5ibG9nLkRva3RvcmEuVXpoYXNub2dvLkFrdC5QZXJ2eWouMjAwOC5YdmlELlRWUmlwLmExLjA5LjExLjEyLm1wNA==\",\"subtitle\":\"\",\"galabel\":\"5487_193210\",\"id\":\"1\",\"vars\":\"193210\"},{\"title\":\"2 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2RhdGEwMC1jZG4uZGF0YWxvY2sucnUvZmkybG0vMj\\/\\/b2xvbG8=JmMzUxOTAwOGIwOGNmMjg5N2I5ODlhNjE3N2ZkZWUvN2ZfTXV6eWthbG55ai5ibG9nLkRva3RvcmEuVXpoYXNub2dvLkFrdC5WdG9yb2ouMjAwOC5YdmlELlRWUmlwLmExLjA5LjExLjEyLm1wNA==\",\"subtitle\":\"\",\"galabel\":\"5487_193212\",\"id\":\"2\",\"vars\":\"193212\"},{\"title\":\"3 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2RhdGEwMC1jZG4uZGF0\\/\\/b2xvbG8=YWxvY2sucnUvZmkybG0vMjJmMzUxOTAwOGIwOGNmMjg5N2I5ODlhNjE3N2ZkZWUvN2ZfTXV6eWthbG55ai5ibG9nLkRva3RvcmEuVXpoYXNub2dvLkFrdC5UcmV0aWouMjAwOC5YdmlELlRWUmlwLmExLjA5LjExLjEyLm1wNA==\",\"subtitle\":\"\",\"galabel\":\"5487_193211\",\"id\":\"3\",\"vars\":\"193211\"}]";
            var correctConvertSeason = new Season(null, correctJsonPlaylist);
            correctConvertSeason.EpisodeList = new List<Episode>()
            {
                new Episode(
                    "����� 1",
                    new Uri("http://data00-cdn.datalock.ru/fi2lm/22f3519008b08cf2897b989a6177fdee/7f_Muzykalnyj.blog.Doktora.Uzhasnogo.Akt.Pervyj.2008.XviD.TVRip.a1.09.11.12.mp4"),
                    0,
                    1),
                new Episode(
                    "����� 2",
                    new Uri("http://data00-cdn.datalock.ru/fi2lm/22f3519008b08cf2897b989a6177fdee/7f_Muzykalnyj.blog.Doktora.Uzhasnogo.Akt.Vtoroj.2008.XviD.TVRip.a1.09.11.12.mp4"),
                    0,
                    2),
                new Episode(
                    "����� 3",
                    new Uri("http://data00-cdn.datalock.ru/fi2lm/22f3519008b08cf2897b989a6177fdee/7f_Muzykalnyj.blog.Doktora.Uzhasnogo.Akt.Tretij.2008.XviD.TVRip.a1.09.11.12.mp4"),
                    0,
                    3),
            };

            // Act
            var result = modelObjectsBuilder.BuildSeasonFromJson(correctJsonPlaylist);

            // Assert
            Assert.AreEqual(correctConvertSeason, result);
        }
    }
}