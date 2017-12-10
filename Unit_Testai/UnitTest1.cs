using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Unit_Testai
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestGetAllBars()
        {
            IEnumerable<social_tapX.RestModels.Bar> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();

            X = await ws.GetAllBars();

            Assert.IsNotNull(X);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetTopBars()
        {
            IEnumerable<social_tapX.RestModels.Bar> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            int from = 0;
            int to = 1;

            X = await ws.GetTopBars(from, to);

            Assert.IsNotNull(X);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetTopBars2()
        {
            IEnumerable<social_tapX.RestModels.Bar> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            int from = -1;
            int to = 1;

            try
            {
                X = await ws.GetTopBars(from, to);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }            
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetTopBars3()
        {
            IEnumerable<social_tapX.RestModels.Bar> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            int from = 0;
            int to = -1;

            try
            {
                X = await ws.GetTopBars(from, to);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetNearestBars()
        {
            IEnumerable<social_tapX.RestModels.Bar> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            social_tapX.RestModels.Coordinate c = new social_tapX.RestModels.Coordinate(0, 0);

            X = await ws.GetNearestBars(c, 1);

            Assert.IsNotNull(X);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetNearestBars2()
        {
            IEnumerable<social_tapX.RestModels.Bar> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            social_tapX.RestModels.Coordinate c = new social_tapX.RestModels.Coordinate(0, 0);

            try
            {
                X = await ws.GetNearestBars(c, -1);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetNearestBars3()
        {
            IEnumerable<social_tapX.RestModels.Bar> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();

            try
            {
                X = await ws.GetNearestBars(null, 1);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetComments()
        {
            IEnumerable<social_tapX.RestModels.Comment> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            X = await ws.GetComments(id, 0, 5);

            Assert.IsNotNull(X);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetComments2()
        {
            IEnumerable<social_tapX.RestModels.Comment> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            try
            {
                X = await ws.GetComments(id, -1, 5);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }            
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetComments3()
        {
            IEnumerable<social_tapX.RestModels.Comment> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            try
            {
                X = await ws.GetComments(id, 0, -5);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetComments4()
        {
            IEnumerable<social_tapX.RestModels.Comment> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();

            try
            {
                X = await ws.GetComments(new Guid(), 0, 5);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetRatings()
        {
            IEnumerable<social_tapX.RestModels.Rating> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            X = await ws.GetRatings(id, 0, 5);

            Assert.IsNotNull(X);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetRatings2()
        {
            IEnumerable<social_tapX.RestModels.Rating> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            try
            {
                X = await ws.GetRatings(id, -1, 5);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetRatings3()
        {
            IEnumerable<social_tapX.RestModels.Rating> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            try
            {
                X = await ws.GetRatings(id, 0, -5);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetRatings4()
        {
            IEnumerable<social_tapX.RestModels.Rating> X = null;
            social_tapX.WebService ws = new social_tapX.WebService();

            try
            {
                X = await ws.GetRatings(new Guid(), 0, 5);
            }
            catch (Exception e)
            {
                Assert.IsNull(X);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestBitmapHandler()
        {



        }
    }
}
