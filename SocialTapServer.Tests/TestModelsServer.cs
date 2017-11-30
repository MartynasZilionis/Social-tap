using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SocialTapServer.Tests
{
    [TestClass]
    public class TestModelsServer
    {

        [TestMethod]
        public void TestCorrectBarInitialisation()
        {
            Models.Bar test = new Models.Bar
            {
                Id = Guid.NewGuid(),
                Name = "Test Name",
                Location = new System.Device.Location.GeoCoordinate(0, 0)
            };
            test.AddComment(new Models.Comment());
            test.AddComment(new Models.Comment());
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            Models.Bar testNotEqual = new Models.Bar();
            Assert.AreNotEqual(test, testNotEqual);

        }
        [TestMethod]
        public void NotSameObjects()
        {
            Assert.AreNotSame(new Models.Bar(), new Models.Bar());
            Assert.AreNotSame(new Models.Comment(), new Models.Comment());
            Assert.AreNotSame(new Models.Rating(), new Models.Rating());
        }
        [TestMethod]
        public void TestCommentCount()
        {
            Models.Bar test = new Models.Bar();
            test.AddComment(new Models.Comment());
            test.AddComment(new Models.Comment());
            int commentCount = 2;
            Assert.AreEqual(commentCount, test.Comments);

        }
        [TestMethod]
        public void TestCommentObject()
        {
            String Author = "Test Author";
            String Content = "Test Content";
            DateTime Date = DateTime.UtcNow;
            Models.Comment test = new Models.Comment();
        }
        [TestMethod]
        public void TestRatingCount()
        {
            Models.Bar test = new Models.Bar();
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            test.AddRating(new Models.Rating());
            int ratingCount = 10;
            Assert.AreEqual(ratingCount, test.Ratings);

        }
        [TestMethod]
        public void AveragePriceCheck()
        {
            DateTime date = DateTime.UtcNow;
            Models.Bar testBar = new Models.Bar();
            testBar.AddRating(new Models.Rating(97f, 0.5f, 3.5f, date));
            testBar.AddRating(new Models.Rating(97f, 0.4f, 4f, date));
            testBar.AddRating(new Models.Rating(97f, 0.6f, 3f, date));
            testBar.AddRating(new Models.Rating(97f, 0.7f, 4.2f, date));
            testBar.AddRating(new Models.Rating(97f, 0.5f, 4.5f, date));
            float averagePrice = 7400;
            Assert.AreEqual(averagePrice, testBar.AveragePrice);

        }
        [TestMethod]
        public void ScoreCheck()
        {
            DateTime date = DateTime.UtcNow;
            Models.Bar testBar = new Models.Bar();
            testBar.AddRating(new Models.Rating(97f, 0.5f, 3.5f, date));
            testBar.AddRating(new Models.Rating(91f, 0.4f, 4f, date));
            testBar.AddRating(new Models.Rating(69f, 0.6f, 3f, date));
            testBar.AddRating(new Models.Rating(100f, 0.7f, 4.2f, date));
            testBar.AddRating(new Models.Rating(89f, 0.5f, 4.5f, date));
            float score = 89.2f;
            Assert.AreEqual(score, testBar.Score);
        }
        [TestMethod]
        public void AverageAndScoreIsNullCheck()
        {
            Models.Bar testBar = new Models.Bar();
            Assert.AreEqual(0, testBar.AveragePrice);
            Assert.AreEqual(0, testBar.Score);
        }
        [TestMethod]
        public void NegativeMugSizeTest()
        {
            try
            {
                Models.Rating testRating = new Models.Rating(0f, -1f, 1f, DateTime.UtcNow);
            }
            catch (ArgumentException e) when (e.ParamName == "MugSize")
            {
                Assert.AreNotEqual("", e);
            }
        }
        [TestMethod]
        public void TooLargeMugSizeTest()
        {
            try
            {
                Models.Rating testRating = new Models.Rating(0f, 10000f, 1f, DateTime.UtcNow);
            }
            catch (ArgumentException e) when (e.ParamName == "MugSize")
            {
                Assert.AreNotEqual("", e);
            }
        }


    }
}
