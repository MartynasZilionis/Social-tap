using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Threading.Tasks;
using social_tapX;
using System.Net.Http;

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

            X = await ws.GetTopBars(from, to);            
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetTopBars3()
        {
            social_tapX.WebService ws = new social_tapX.WebService();
            int from = 0;
            int to = -1;

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => ws.GetTopBars(from, to));
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

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => ws.GetNearestBars(c, -1));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetNearestBars3()
        {
            social_tapX.WebService ws = new social_tapX.WebService();

            await Assert.ThrowsExceptionAsync<NullReferenceException>(() => ws.GetNearestBars(null, 1));
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
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => ws.GetComments(new Guid(), -1, 5));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetComments3()
        {
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => ws.GetComments(new Guid(), 0, -5));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetComments4()
        {
            social_tapX.WebService ws = new social_tapX.WebService();

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => ws.GetComments(new Guid(), 0, 5));
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
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => ws.GetRatings(id, -1, 5));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetRatings3()
        {
            social_tapX.WebService ws = new social_tapX.WebService();
            IEnumerable<social_tapX.RestModels.Bar> B = await ws.GetAllBars();
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar();
            bar = B.First();
            Guid id = bar.Id;

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => ws.GetRatings(id, 0, -5));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetRatings4()
        {
            social_tapX.WebService ws = new social_tapX.WebService();

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => ws.GetRatings(new Guid(), 0, -5));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestUploadBar()
        {
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar("Test", new social_tapX.RestModels.Coordinate(0, 0));

            var mock = new Mock<IHttpClientHandler>();
            mock.Setup(framework => framework.IPostAsync("http://socialtapx.azurewebsites.net/api" + "/Bar", It.IsAny<System.Net.Http.StringContent>())).Returns(Task.FromResult(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            social_tapX.WebService.client = mock.Object;
            social_tapX.WebService ws = new social_tapX.WebService();

            await ws.UploadBar(bar);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestUploadBar2()
        {
            var mock = new Mock<IHttpClientHandler>();
            mock.Setup(framework => framework.IPostAsync("http://socialtapx.azurewebsites.net/api" + "/Bar", It.IsAny<System.Net.Http.StringContent>())).Throws(new ArgumentNullException());

            social_tapX.WebService.client = mock.Object;
            social_tapX.WebService ws = new social_tapX.WebService();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ws.UploadBar(null));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestUploadComment()
        {
            social_tapX.RestModels.Comment c = new social_tapX.RestModels.Comment("Test", "Test", 10);
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar("Test", new social_tapX.RestModels.Coordinate(0, 0));

            var mock = new Mock<IHttpClientHandler>();
            mock.Setup(framework => framework.IPostAsync("http://socialtapx.azurewebsites.net/api" + "/Comment/" + bar.Id, It.IsAny<System.Net.Http.StringContent>())).Returns(Task.FromResult(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            social_tapX.WebService.client = mock.Object;
            social_tapX.WebService ws = new social_tapX.WebService();

            await ws.UploadComment(bar.Id, c);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestUploadComment2()
        {
            social_tapX.RestModels.Comment c = null;
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar("Test", new social_tapX.RestModels.Coordinate(0, 0));

            var mock = new Mock<IHttpClientHandler>();

            mock.Setup(framework => framework.IPostAsync("http://socialtapx.azurewebsites.net/api" + "/Comment/" + bar.Id, It.IsAny<System.Net.Http.StringContent>())).Throws(new ArgumentNullException());
            social_tapX.WebService.client = mock.Object;
            social_tapX.WebService ws = new social_tapX.WebService();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ws.UploadComment(bar.Id, c));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestUploadRating()
        {
            social_tapX.RestModels.Rating r = new social_tapX.RestModels.Rating(89, 1, 1);
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar("Test", new social_tapX.RestModels.Coordinate(0, 0));

            var mock = new Mock<IHttpClientHandler>();
            mock.Setup(framework => framework.IPostAsync("http://socialtapx.azurewebsites.net/api" + "/Rating/" + bar.Id, It.IsAny<System.Net.Http.StringContent>())).Returns(Task.FromResult(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            social_tapX.WebService.client = mock.Object;
            social_tapX.WebService ws = new social_tapX.WebService();

            await ws.UploadRating(bar.Id, r);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestUploadRating2()
        {
            social_tapX.RestModels.Rating r = null;
            social_tapX.RestModels.Bar bar = new social_tapX.RestModels.Bar("Test", new social_tapX.RestModels.Coordinate(0, 0));

            var mock = new Mock<IHttpClientHandler>();
            mock.Setup(framework => framework.IPostAsync("http://socialtapx.azurewebsites.net/api" + "/Rating/" + bar.Id, It.IsAny<System.Net.Http.StringContent>())).Throws(new ArgumentNullException());

            social_tapX.WebService.client = mock.Object;
            social_tapX.WebService ws = new social_tapX.WebService();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ws.UploadRating(bar.Id, r));
        }
    }
}
