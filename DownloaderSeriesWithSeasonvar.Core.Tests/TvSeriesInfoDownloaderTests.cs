using DownloaderSeriesWithSeasonvar.Core;
using DownloaderSeriesWithSeasonvar.Core.Tests.TestPage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core.Tests
{
    [TestClass]
    public class TvSeriesInfoDownloaderTests
    {
        private IWebRequester subWebRequester;

        [TestMethod]
        public void GetInfoList_GettingSeasonsUri_CorrectUriTestPageSeason1()
        {
            // Arrange
            var tvSeriesInfoDownloader = this.CreateTvSeriesInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");
            var expectedLinks = new List<Uri>()
            {
                new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html"),
                new Uri("http://seasonvar.ru/serial-7755-Inspektor_Klot-2-season.html"),
                new Uri("http://seasonvar.ru/serial-10287-Inspektor_Klot-3-season.html")
            };

            // Act
            var result = tvSeriesInfoDownloader.GetInfoList(
                address);

            // Assert
            for (int i = 0; i < expectedLinks.Count; i++)
            {
                Assert.AreEqual(expectedLinks[i], result[i]);
            }
        }

        [TestMethod]
        public async Task GetInfoListAsync_CheckThrowException_Exception()
        {
            // Arrage
            var subWebRequeter = Substitute.For<IWebRequester>();
            var tvSeriesInfoDownloader = new TvSeriesInfoDownloader(subWebRequeter);

            // Act

            // Assert
            var result = await Assert.ThrowsExceptionAsync<Exception>(
                 () => tvSeriesInfoDownloader.GetInfoListAsync(
                    new Uri("http://seasonvar.ru")));
        }

        [TestMethod]
        public async Task GetInfoListAsync_GettingSeasonsUri_CorrectUriTestPageSeason1()
        {
            // Arrange
            var tvSeriesInfoDownloader = this.CreateTvSeriesInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");
            var expectedLinks = new List<Uri>()
            {
                new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html"),
                new Uri("http://seasonvar.ru/serial-7755-Inspektor_Klot-2-season.html"),
                new Uri("http://seasonvar.ru/serial-10287-Inspektor_Klot-3-season.html")
            };

            // Act
            var result = await tvSeriesInfoDownloader.GetInfoListAsync(
                address);

            // Assert
            for (int i = 0; i < expectedLinks.Count; i++)
            {
                Assert.AreEqual(expectedLinks[i], result[i]);
            }
        }

        [TestMethod]
        public void GetOriginalName_GettingTvSeriesName_CorrectNameTestPageSeason1()
        {
            // Arrange
            var tvSeriesInfoDownloader = this.CreateTvSeriesInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");

            // Act
            var result = tvSeriesInfoDownloader.GetOriginalName(address);

            // Assert
            StringAssert.Equals("A Touch of Cloth", result);
        }

        [TestMethod]
        public async Task GetOriginalNameAsync_CheckThrowException_Exception()
        {
            // Arrage
            var subWebRequeter = Substitute.For<IWebRequester>();
            var tvSeriesInfoDownloader = new TvSeriesInfoDownloader(subWebRequeter);

            // Act

            // Assert
            var result = await Assert.ThrowsExceptionAsync<Exception>(
                 () => tvSeriesInfoDownloader.GetOriginalNameAsync(
                    new Uri("http://seasonvar.ru")));
        }

        [TestMethod]
        public async Task GetOriginalNameAsync_GettingTvSeriesName_CorrectNameTestPageSeason1()
        {
            // Arrange
            var tvSeriesInfoDownloader = this.CreateTvSeriesInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");

            // Act
            var result = await tvSeriesInfoDownloader.GetOriginalNameAsync(address);

            // Assert
            StringAssert.Equals("A Touch of Cloth", result);
        }

        [TestMethod]
        public async Task GetOriginalNameAsync_ReternSavedName_CorrectNameSeason1()
        {
            // Arrange
            var tvSeriesInfoDownloader = this.CreateTvSeriesInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");
            var firstName = tvSeriesInfoDownloader.GetOriginalName(address);

            // Act
            var result = await tvSeriesInfoDownloader.GetOriginalNameAsync(address);

            // Assert
            StringAssert.Equals(firstName, result);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var existSeasonTestInfo = SeasonTestInfoBuilder.ExistingSeasons.TouchOfClothS1;
            var seasonTestInfo = SeasonTestInfoBuilder.GetSeasonTestView(existSeasonTestInfo);

            subWebRequester = Substitute.For<IWebRequester>();
            subWebRequester.GetWebPageSource(Arg.Is<string>(x => x.Contains(".txt")))
                .Returns(seasonTestInfo.JsonWebSource);
            subWebRequester.GetWebPageSource(Arg.Is<string>(x => !x.Contains(".txt")))
                .Returns(seasonTestInfo.WebSource);

            subWebRequester.GetWebPageSourceAsync(Arg.Is<string>(x => x.Contains(".txt")))
                .Returns(seasonTestInfo.JsonWebSource);
            subWebRequester.GetWebPageSourceAsync(Arg.Is<string>(x => !x.Contains(".txt")))
                .Returns(seasonTestInfo.WebSource);
        }

        private TvSeriesInfoDownloader CreateTvSeriesInfoDownloader()
        {
            return new TvSeriesInfoDownloader(
                this.subWebRequester);
        }
    }
}