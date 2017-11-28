using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SocialTapServer.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void BarAddedToDBCacheTest()
        {
            Models.Bar testBar = new Models.Bar();
            Guid id = testBar.Id; ;
            DatabaseCache newDB = new DatabaseCache();
            newDB.AddBar(testBar);
            Assert.AreEqual(testBar, newDB.GetBar(id));
        }
        [TestMethod]
        public void ComplexBarAddedTODBCacheTest()
        {
            Models.Bar testBar = new Models.Bar();
            testBar.AddComment(new Models.Comment());
            testBar.AddComment(new Models.Comment());
            testBar.AddComment(new Models.Comment());
            testBar.AddRating(new Models.Rating());
            testBar.AddRating(new Models.Rating());
            testBar.AddRating(new Models.Rating());
            Guid id = testBar.Id;
            DatabaseCache newDB = new DatabaseCache();
            newDB.AddBar(testBar);
            Assert.AreEqual(testBar, newDB.GetBar(id));
        }
        [TestMethod]
        public void DBCacheIntegrationWithModels()
        {
            Models.Bar testBar = new Models.Bar();
            Guid id = testBar.Id;
            testBar.AddRating(new Models.Rating());
            testBar.AddComment(new Models.Comment());
            DatabaseCache newDB = new DatabaseCache();
            newDB.AddBar(testBar);
            Assert.IsNotNull(newDB.GetRatings(id, 0, 1));
            Assert.IsNotNull(newDB.GetComments(id, 0, 1));
        }
    }
}
