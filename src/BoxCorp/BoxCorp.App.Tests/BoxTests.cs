using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoxCorp.App.Tests
{
    [TestClass]
    public class BoxTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<CorpBox> testData = new List<CorpBox>
            {
                new CorpBox {X = 2, Y = 2, Width = 6, Height = 6, Rank = 0.8},
                new CorpBox {X = 3, Y = 3, Width = 6, Height = 4, Rank = 0.6},
                new CorpBox {X = 2, Y = 8, Width = 4, Height = 3, Rank = 0.9},
                new CorpBox {X = 8, Y = 9, Width = 2, Height = 2, Rank = 0.3}
            };

            var results = testData.FilterBoxes();

            Assert.AreEqual(results.Contains(testData[0]), true);
            Assert.AreEqual(results.Contains(testData[1]), false);
            Assert.AreEqual(results.Contains(testData[2]), true);
            Assert.AreEqual(results.Contains(testData[3]), false);
        }
    }
}