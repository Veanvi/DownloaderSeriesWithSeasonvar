using DownloaderSeriesWithSeasonvar.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core.Tests
{
    [TestClass]
    public class SeasonInfoDownloaderTests
    {
        private IWebRequester subWebRequester;

        [TestMethod]
        public void GetInfoList_GettingEpisodesUri_CorrectUriTestPageSeason1()
        {
            // Arrange
            var seasonInfoDownloader = this.CreateSeasonInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");
            var correctSeason = TestPage.CorrectModelObjects.GetSeason1Object();

            // Act
            var resultUriList = seasonInfoDownloader.GetInfoList(address);

            // Assert
            for (int i = 0; i < correctSeason.EpisodeList.Count; i++)
            {
                Assert.AreEqual(correctSeason.EpisodeList[i].FileUri, resultUriList[i]);
            }
        }

        [TestMethod]
        public async Task GetInfoListAsync_GettingEpisodesUri_CorrectUriTestPageSeason1()
        {
            // Arrange
            var seasonInfoDownloader = this.CreateSeasonInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");
            var correctSeason = TestPage.CorrectModelObjects.GetSeason1Object();

            // Act
            var resultUriList = await seasonInfoDownloader.GetInfoListAsync(address);

            // Assert
            for (int i = 0; i < correctSeason.EpisodeList.Count; i++)
            {
                Assert.AreEqual(correctSeason.EpisodeList[i].FileUri, resultUriList[i]);
            }
        }

        [TestMethod]
        public void GetOriginalName_GettingTvSeriesName_CorrectNameTestPageSeason1()
        {
            // Arrange
            var seasonInfoDownloader = this.CreateSeasonInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");

            // Act
            var result = seasonInfoDownloader.GetOriginalName(
                address);

            // Assert
            StringAssert.Equals("A Touch of Cloth", result);
        }

        [TestMethod]
        public async Task GetOriginalNameAsync_GettingTvSeriesName_CorrectNameTestPageSeason1()
        {
            // Arrange
            var seasonInfoDownloader = this.CreateSeasonInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");

            // Act
            var result = await seasonInfoDownloader.GetOriginalNameAsync(
                address);

            // Assert
            StringAssert.Equals("A Touch of Cloth", result);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.subWebRequester = Substitute.For<IWebRequester>();
            subWebRequester.GetWebPageSource(Arg.Is<string>(x => x.Contains(".txt")))
                .Returns(TestPage.CorrectModelObjects.GetSeason1Playlist());
            subWebRequester.GetWebPageSource(Arg.Is<string>(x => !x.Contains(".txt")))
                .Returns(TestPage.CorrectModelObjects.GetSeason1Source());

            subWebRequester.GetWebPageSourceAsync(Arg.Is<string>(x => x.Contains(".txt")))
                .Returns(TestPage.CorrectModelObjects.GetSeason1Playlist());
            subWebRequester.GetWebPageSourceAsync(Arg.Is<string>(x => !x.Contains(".txt")))
                .Returns(TestPage.CorrectModelObjects.GetSeason1Source());
        }

        private SeasonInfoDownloader CreateSeasonInfoDownloader()
        {
            return new SeasonInfoDownloader(
                this.subWebRequester);
        }
    }
}