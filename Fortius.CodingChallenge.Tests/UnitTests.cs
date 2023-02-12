using ConstructionLine.CodingChallenge;
using ConstructionLine.CodingChallenge.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Fortius.CodingChallenge.Tests
{
    [TestFixture]
    public class UnitTests: SearchEngineTestsBase
    {
        [Test]
        public void Test4TshirtsAndTwoLarge()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red,Color.White },
                Sizes = new List<Size> { Size.Small, Size.Large }
            };

            var results = searchEngine.Search(searchOptions);

            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        }

        [Test]
        public void BringAllColorsWhenTshirtHaveEmptyColors()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Small, Size.Large }
            };

            var results = searchEngine.Search(searchOptions);

            Assert.IsTrue(results.ColorCounts.FirstOrDefault(x=>x.Color== Color.Red)!=null);
            Assert.IsTrue(results.ColorCounts.FirstOrDefault(x=>x.Color== Color.White)!=null);
            Assert.IsTrue(results.ColorCounts.FirstOrDefault(x=>x.Color== Color.Black)!=null);
        }

        [Test]
        public void EmptyTshirts()
        {
            var shirts = new List<Shirt>
            {
            };

            var searchEngine = new SearchEngineBasic(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { },
                Sizes = new List<Size> { Size.Small, Size.Large }
            };

            var results = searchEngine.Search(searchOptions);

            Assert.IsTrue(results.Shirts.Count== 0);
        }

        [Test]
        public void EmptyColorsAndSizesInTshirts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
            };

            var searchEngine = new SearchEngineBasic(shirts);

            var searchOptions = new SearchOptions
            {
            };

            var results = searchEngine.Search(searchOptions);

            Assert.IsTrue(results.SizeCounts.Count> 0);
            Assert.IsTrue(results.ColorCounts.Count> 0);
        }
    }
}
