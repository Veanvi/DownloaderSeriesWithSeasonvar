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
            var firstObj = new Season("FirstObj", new List<Series>()
            {
                new Series("FirstObj", new Uri("http://FirstObj.ru"), 1, 3)
            });
            var secondObj = new Season("FirstObj", new List<Series>()
            {
                new Series("FirstObj", new Uri("http://FirstObj.ru"), 1, 3)
            });
            // Act
            var result = firstObj.Equals(secondObj);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_NotCopyObject_False()
        {
            // Arrage
            var firstObj = new Season("FirstObj", new List<Series>()
            {
                new Series("FirstObj", new Uri("http://FirstObj.ru"), 1, 3)
            });
            var secondObj = new Season("FirstObj", new List<Series>()
            {
                new Series("FirstObj", new Uri("http://FirstObj.ru"), 1, 3),
                new Series("SecondObj", new Uri("http://SecondObj.ru"), 2, 4)
            });
            // Act
            var result = firstObj.Equals(secondObj);
            // Assert
            Assert.IsFalse(result);
        }
    }
}