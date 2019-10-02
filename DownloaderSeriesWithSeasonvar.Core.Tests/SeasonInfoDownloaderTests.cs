using DownloaderSeriesWithSeasonvar.Core.Tests.TestPage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core.Tests
{
    [TestClass]
    public class SeasonInfoDownloaderTests
    {
        [DataTestMethod]
        [DataRow(SeasonTestInfoBuilder.ExistingSeasons.FireflyS1)]
        [DataRow(SeasonTestInfoBuilder.ExistingSeasons.TouchOfClothS1)]
        [DataRow(SeasonTestInfoBuilder.ExistingSeasons.TouchOfClothS2)]
        [DataRow(SeasonTestInfoBuilder.ExistingSeasons.TouchOfClothS3)]
        public void GetInfoList_GettingEpisodesUri_CorrectUri(
            SeasonTestInfoBuilder.ExistingSeasons existSeasonTestInfo)
        {
            // Arrange
            var testInfo = SeasonTestInfoBuilder.GetSeasonTestInfo(existSeasonTestInfo);
            var seasonInfoDownloader = this.CreateSeasonInfoDownloader(testInfo);

            // Act
            var resultUriList = seasonInfoDownloader.GetInfoList(new Uri(testInfo.Uri));

            // Assert
            for (int i = 0; i < testInfo.EpisodeCounts; i++)
            {
                StringAssert.Contains(resultUriList[i].ToString(), testInfo.EpisodeFileNames[i]);
            }
        }

        [TestMethod]
        public async Task GetInfoListAsync_CheckThrowException_Exception()
        {
            // Arrage
            var subWebRequester = Substitute.For<IWebRequester>();
            subWebRequester.GetWebPageSourceAsync(Arg.Any<string>())
                .Returns(Task.FromResult(""));
            var seasonInfoDownloader = new SeasonInfoDownloader(subWebRequester);

            // Act

            // Assert
            var result = await Assert.ThrowsExceptionAsync<Exception>(
                 () => seasonInfoDownloader.GetInfoListAsync(
                    new Uri("http://seasonvar.ru")));
        }

        [DataTestMethod]
        [DataRow(SeasonTestInfoBuilder.ExistingSeasons.FireflyS1)]
        [DataRow(SeasonTestInfoBuilder.ExistingSeasons.TouchOfClothS1)]
        [DataRow(SeasonTestInfoBuilder.ExistingSeasons.TouchOfClothS2)]
        [DataRow(SeasonTestInfoBuilder.ExistingSeasons.TouchOfClothS3)]
        public async Task GetInfoListAsync_GettingEpisodesUri_CorrectUriTest(
            SeasonTestInfoBuilder.ExistingSeasons existSeasonTestInfo)
        {
            // Arrange
            var testInfo = SeasonTestInfoBuilder.GetSeasonTestInfo(existSeasonTestInfo);
            var seasonInfoDownloader = this.CreateSeasonInfoDownloader(testInfo);

            // Act
            var resultUriList = await seasonInfoDownloader.GetInfoListAsync(new Uri(testInfo.Uri));

            // Assert
            for (int i = 0; i < testInfo.EpisodeCounts; i++)
            {
                StringAssert.Contains(resultUriList[i].ToString(), testInfo.EpisodeFileNames[i]);
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
        public async Task GetOriginalNameAsync_CheckThrowException_Exception()
        {
            // Arrage
            var subWebRequester = Substitute.For<IWebRequester>();
            subWebRequester.GetWebPageSourceAsync(Arg.Any<string>())
                .Returns(Task.FromResult(""));
            var seasonInfoDownloader = new SeasonInfoDownloader(subWebRequester);

            // Act

            // Assert
            var result = await Assert.ThrowsExceptionAsync<Exception>(
                 () => seasonInfoDownloader.GetOriginalNameAsync(
                    new Uri("http://seasonvar.ru")));
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

        [TestMethod]
        public async Task GetOriginalNameAsync_ReternSavedName_CorrectNameSeason1()
        {
            // Arrange
            var seasonInfoDownloader = this.CreateSeasonInfoDownloader();
            Uri address = new Uri("http://seasonvar.ru/serial-5583-Inspektor_Klot.html");
            var firstName = seasonInfoDownloader.GetOriginalName(address);

            // Act
            var result = await seasonInfoDownloader.GetOriginalNameAsync(address);

            // Assert
            StringAssert.Equals(firstName, result);
        }

        private SeasonInfoDownloader CreateSeasonInfoDownloader(
            SeasonTestInfo seasonTestInfo)
        {
            var subWebRequester = Substitute.For<IWebRequester>();
            subWebRequester.GetWebPageSource(Arg.Is<string>(x => x.Contains(".txt")))
                .Returns(seasonTestInfo.JsonWebSource);
            subWebRequester.GetWebPageSource(Arg.Is<string>(x => !x.Contains(".txt")))
                .Returns(seasonTestInfo.WebSource);

            subWebRequester.GetWebPageSourceAsync(Arg.Is<string>(x => x.Contains(".txt")))
                .Returns(seasonTestInfo.JsonWebSource);
            subWebRequester.GetWebPageSourceAsync(Arg.Is<string>(x => !x.Contains(".txt")))
                .Returns(seasonTestInfo.WebSource);

            return new SeasonInfoDownloader(subWebRequester);
        }

        private SeasonInfoDownloader CreateSeasonInfoDownloader()
        {
            var existSeasonTestInfo = SeasonTestInfoBuilder.ExistingSeasons.TouchOfClothS1;
            var seasonTestInfo = SeasonTestInfoBuilder.GetSeasonTestInfo(existSeasonTestInfo);
            return CreateSeasonInfoDownloader(seasonTestInfo);
        }
    }
}