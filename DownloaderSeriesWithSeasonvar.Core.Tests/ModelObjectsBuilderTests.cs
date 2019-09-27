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
        private IInfoDownloader subInfoDownloaderEpisode;
        private IInfoDownloader subInfoDownloaderUri;

        [TestMethod]
        public async Task BuildSeasonAsync_GenerateException_ExpectException()
        {
            // Arrage
            var subIDEpisode = Substitute.For<IInfoDownloader>();
            subIDEpisode.GetInfoList(Arg.Any<Uri>()).Returns(x => { throw new Exception("TestMessage"); });
            subIDEpisode.GetInfoListAsync(Arg.Any<Uri>()).Returns(x => subIDEpisode.GetInfoList(null));

            var modelObjectsBuilder = new ModelObjectsBuilder(subIDEpisode, subInfoDownloaderUri);
            // Act
            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                async () => { await modelObjectsBuilder.BuildSeasonAsync(new Uri("https://test.com")); });
            // Assert
            StringAssert.Contains(ex.Message, "TestMessage");
        }

        [TestMethod]
        public async Task BuildSeasonAsync_GettingCorrectobject_Correctobject()
        {
            // Arrange
            var subIDEpisode = Substitute.For<IInfoDownloader>();
            var subEpisodeList = new List<Uri>()
            {
                new Uri("https://test1.com")
            };
            subIDEpisode.GetInfoListAsync(Arg.Any<Uri>()).Returns(subEpisodeList);
            var modelObjectsBuilder = new ModelObjectsBuilder(subIDEpisode, subInfoDownloaderUri);
            // Act
            var result = await modelObjectsBuilder.BuildSeasonAsync(new Uri("https://test.com"));
            // Assert
            Assert.AreEqual(1, result.EpisodeList.Count);
            Assert.AreEqual(subEpisodeList.First(), result.EpisodeList.First().FileUri);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("//b2xvbG8=")]
        public void BuildSeasonFromJson_CastomNoisePattern_ReturnCorrectObject(string noisePattern)
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
            var result = modelObjectsBuilder.BuildSeasonFromJson(correctJsonPlaylist, noisePattern);

            // Assert
            Assert.AreEqual(correctConvertSeason, result);
        }

        [TestMethod]
        public async Task BuildTvSeriesAsync_GenerateException_ExpectException()
        {
            // Arrage
            var subIDUri = Substitute.For<IInfoDownloader>();
            subIDUri.GetInfoList(Arg.Any<Uri>()).Returns(x => { throw new Exception("TestMessage"); });
            subIDUri.GetInfoListAsync(Arg.Any<Uri>()).Returns(x => subIDUri.GetInfoList(null));

            var modelObjectsBuilder = new ModelObjectsBuilder(subInfoDownloaderEpisode, subIDUri);
            // Act
            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                async () => { await modelObjectsBuilder.BuildTvSeriesAsync(new Uri("https://test.com")); });
            // Assert - Expected Exception
            StringAssert.Contains(ex.Message, "TestMessage");
        }

        [TestMethod]
        public async Task BuildTvSeriesAsync_GettingCorrectobject_Correctobject()
        {
            // Arrange
            var subIDTvSeries = Substitute.For<IInfoDownloader>();
            var subUriList = new List<Uri>()
            {
                new Uri("https://test1.com"),
                new Uri("https://test2.com"),
                new Uri("https://test3.com")
            };
            subIDTvSeries.GetInfoListAsync(Arg.Any<Uri>()).Returns(subUriList);
            subIDTvSeries.GetOriginalNameAsync(Arg.Any<Uri>()).Returns("TestName");
            var modelObjectsBuilder = new ModelObjectsBuilder(subInfoDownloaderEpisode, subIDTvSeries);
            // Act
            var result = await modelObjectsBuilder.BuildTvSeriesAsync(new Uri("https://test.com"));
            // Assert
            Assert.AreEqual(subUriList.Count, result.SeasonUriList.Count);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.subInfoDownloaderEpisode = Substitute.For<IInfoDownloader>();
            this.subInfoDownloaderUri = Substitute.For<IInfoDownloader>();

            subInfoDownloaderEpisode.GetInfoListAsync(Arg.Any<Uri>())
                .Returns(Task.FromResult(new List<Uri>()));
        }

        private ModelObjectsBuilder CreateModelObjectsBuilder()
        {
            return new ModelObjectsBuilder(
                this.subInfoDownloaderEpisode,
                this.subInfoDownloaderUri);
        }
    }
}