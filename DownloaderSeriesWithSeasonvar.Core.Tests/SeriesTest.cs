using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DownloaderSeriesWithSeasonvar.Core.Tests
{
    [TestClass]
    public class SeriesTest
    {
        [TestMethod]
        public void Equals_CopyObject_True()
        {
            // Arrage
            var firstObj = new Series("FirstObj", new Uri("http://FirstObj.ru"), 1, 3);
            var secondObj = new Series("FirstObj", new Uri("http://FirstObj.ru"), 1, 3);
            // Act
            var result = firstObj.Equals(secondObj);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_NotCopyObject_False()
        {
            // Arrage
            var firstObj = new Series("FirstObj", new Uri("http://FirstObj.ru"), 1, 3);
            var secondObj = new Series("SecondObj", new Uri("http://SecondObj.ru"), 0, 2);
            // Act
            var result = firstObj.Equals(secondObj);
            // Assert
            Assert.IsFalse(result);
        }
    }
}