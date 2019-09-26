using DownloaderSeriesWithSeasonvar.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core.Tests
{
    [TestClass]
    public class ModelObjectsBuilderTests
    {
        private IInfoDownloader<Episode> subInfoDownloaderEpisode;
        private IInfoDownloader<Uri> subInfoDownloaderUri;

        [TestMethod]
        [ExpectedException(typeof(Exception), "TestMessage")]
        public async Task BuildSeasonAsync_GenerateException_ExpectException()
        {
            // Arrage
            var subIDEpisode = Substitute.For<IInfoDownloader<Episode>>();
            subIDEpisode.GetInfoList(Arg.Any<Uri>()).Returns(x => { throw new Exception("TestMessage"); });
            subIDEpisode.GetInfoListAsync(Arg.Any<Uri>()).Returns(x => subIDEpisode.GetInfoList(null));

            var modelObjectsBuilder = new ModelObjectsBuilder(subIDEpisode, subInfoDownloaderUri);
            // Act
            var result = await modelObjectsBuilder.BuildSeasonAsync(new Uri("https://test.com"));
            // Assert - Expected Exception
        }

        [TestMethod]
        public async Task BuildSeasonAsync_GettingCorrectobject_Correctobject()
        {
            // Arrange
            var subIDEpisode = Substitute.For<IInfoDownloader<Episode>>();
            var subEpisodeList = new List<Episode>()
            {
                new Episode("test", new Uri("https://test.com"), 42, 42)
            };
            subIDEpisode.GetInfoListAsync(Arg.Any<Uri>()).Returns(subEpisodeList);
            var modelObjectsBuilder = new ModelObjectsBuilder(subIDEpisode, subInfoDownloaderUri);
            // Act
            var result = await modelObjectsBuilder.BuildSeasonAsync(new Uri("https://test.com"));
            // Assert
            Assert.AreEqual(1, result.EpisodeList.Count);
            Assert.AreEqual(subEpisodeList.First(), result.EpisodeList.First());
        }

        [TestMethod]
        public void BuildSeasonFromJson_CorrectJson_ReturnCorrectObject()
        {
            // Arrange
            var modelObjectsBuilder = this.CreateModelObjectsBuilder();

            var correctJsonPlaylist = "[{\"title\":\"1 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2Rhd\\/\\/b2xvbG8=GEwMC1jZG4uZGF0YWxvY2sucnUvZmkybG0vMjJmMzUxOTAwOGIwOGNmMjg5N2I5ODlhNjE3N2ZkZWUvN2ZfTXV6eWthbG55ai5ibG9nLkRva3RvcmEuVXpoYXNub2dvLkFrdC5QZXJ2eWouMjAwOC5YdmlELlRWUmlwLmExLjA5LjExLjEyLm1wNA==\",\"subtitle\":\"\",\"galabel\":\"5487_193210\",\"id\":\"1\",\"vars\":\"193210\"},{\"title\":\"2 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2RhdGEwMC1jZG4uZGF0YWxvY2sucnUvZmkybG0vMj\\/\\/b2xvbG8=JmMzUxOTAwOGIwOGNmMjg5N2I5ODlhNjE3N2ZkZWUvN2ZfTXV6eWthbG55ai5ibG9nLkRva3RvcmEuVXpoYXNub2dvLkFrdC5WdG9yb2ouMjAwOC5YdmlELlRWUmlwLmExLjA5LjExLjEyLm1wNA==\",\"subtitle\":\"\",\"galabel\":\"5487_193212\",\"id\":\"2\",\"vars\":\"193212\"},{\"title\":\"3 \\u0441\\u0435\\u0440\\u0438\\u044f SD\r\n\",\"file\":\"#2aHR0cDovL2RhdGEwMC1jZG4uZGF0\\/\\/b2xvbG8=YWxvY2sucnUvZmkybG0vMjJmMzUxOTAwOGIwOGNmMjg5N2I5ODlhNjE3N2ZkZWUvN2ZfTXV6eWthbG55ai5ibG9nLkRva3RvcmEuVXpoYXNub2dvLkFrdC5UcmV0aWouMjAwOC5YdmlELlRWUmlwLmExLjA5LjExLjEyLm1wNA==\",\"subtitle\":\"\",\"galabel\":\"5487_193211\",\"id\":\"3\",\"vars\":\"193211\"}]";
            var correctConvertSeason = new Season(null, correctJsonPlaylist);
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

            // Act
            var result = modelObjectsBuilder.BuildSeasonFromJson(correctJsonPlaylist);

            // Assert
            Assert.AreEqual(correctConvertSeason, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "TestMessage")]
        public async Task BuildTvSeriesAsync_GenerateException_ExpectException()
        {
            // Arrage
            var subIDUri = Substitute.For<IInfoDownloader<Uri>>();
            subIDUri.GetInfoList(Arg.Any<Uri>()).Returns(x => { throw new Exception("TestMessage"); });
            subIDUri.GetInfoListAsync(Arg.Any<Uri>()).Returns(x => subIDUri.GetInfoList(null));

            var modelObjectsBuilder = new ModelObjectsBuilder(subInfoDownloaderEpisode, subIDUri);
            // Act
            var result = await modelObjectsBuilder.BuildTvSeriesAsync(new Uri("https://test.com"));
            // Assert - Expected Exception
        }

        [TestMethod]
        public async Task BuildTvSeriesAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var subIDUri = Substitute.For<IInfoDownloader<Uri>>();
            var subUriList = new List<Uri>()
            {
                new Uri("https://test1.com"),
                new Uri("https://test2.com"),
                new Uri("https://test3.com")
            };
            subIDUri.GetInfoListAsync(Arg.Any<Uri>()).Returns(subUriList);
            var modelObjectsBuilder = new ModelObjectsBuilder(subInfoDownloaderEpisode, subIDUri);
            // Act
            var result = await modelObjectsBuilder.BuildTvSeriesAsync(new Uri("https://test.com"));
            // Assert
            Assert.AreEqual(subUriList.Count, result.SeasonUriList.Count);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.subInfoDownloaderEpisode = Substitute.For<IInfoDownloader<Episode>>();
            this.subInfoDownloaderUri = Substitute.For<IInfoDownloader<Uri>>();
        }

        private ModelObjectsBuilder CreateModelObjectsBuilder()
        {
            return new ModelObjectsBuilder(
                this.subInfoDownloaderEpisode,
                this.subInfoDownloaderUri);
        }
    }
}