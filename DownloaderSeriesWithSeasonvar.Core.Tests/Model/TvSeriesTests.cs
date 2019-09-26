using DownloaderSeriesWithSeasonvar.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DownloaderSeriesWithSeasonvar.Core.Tests
{
    [TestClass]
    public class TvSeriesTests
    {
        [TestMethod]
        public void TvSeriesDefaultCtor_ObjectCreation_AllFildsNotNull()
        {
            // Arrange
            List<Uri> uriList = new List<Uri>()
            {
                new Uri("https://test.com"),
                new Uri("https://test.com")
            };
            var tvSeries = new TvSeries(new Uri("https://test.com"), uriList);

            // Act
            var seasonList = tvSeries.SeasonList;
            var seasonUriList = tvSeries.SeasonUriList;
            var tvSeriesAddress = tvSeries.Uri;

            // Assert
            Assert.IsNotNull(seasonList);
            Assert.IsNotNull(seasonUriList);
            Assert.IsNotNull(tvSeriesAddress);
        }
    }
}