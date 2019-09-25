using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DownloaderSeriesWithSeasonvar.Core.Tests
{
    [TestClass]
    public class SeasonTest
    {
        [TestMethod]
        public void Equals_CopyObject_True()
        {
            // Arrage
            var firstObj = new Season(
                new Uri("http://seasonvar.ru/serial-17482-Doktor_Kto-11-season.html"), null);
            firstObj.EpisodeList = new List<Episode>()
            {
                new Episode("FirstObj", new Uri("http://FirstObj.ru"), 1, 3)
            };
            var secondObj = new Season(
                new Uri("http://seasonvar.ru/serial-17482-Doktor_Kto-11-season.html"), null);
            secondObj.EpisodeList = new List<Episode>()
            {
                new Episode("FirstObj", new Uri("http://FirstObj.ru"), 1, 3)
            };

            // Act

            // Assert
            Assert.AreEqual(firstObj, secondObj);
        }

        [TestMethod]
        public void Equals_NotCopyObject_False()
        {
            // Arrage
            var firstObj = new Season(new Uri("http://1.html"), null);
            firstObj.EpisodeList = new List<Episode>()
            {
                new Episode("FirstObj", new Uri("http://FirstObj.ru"), 1, 3)
            };
            var secondObj = new Season(new Uri("http://2.html"), null);
            secondObj.EpisodeList = new List<Episode>()
            {
                new Episode("FirstObj", new Uri("http://FirstObj.ru"), 1, 3),
                new Episode("SecondObj", new Uri("http://SecondObj.ru"), 2, 4)
            };
            // Act
            var result = firstObj.Equals(secondObj);
            // Assert
            Assert.IsFalse(result);
        }
    }
}