using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialTapServer.Controllers;
using SocialTapServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace SocialTapServer.Controllers.Tests
{
    [TestClass()]
    public class RatingControllerTests
    {
        [TestMethod()]
        public async void RatingGetTest()
        {
            // Arrange
            Bar bar = new Bar();
            Rating rating = new Rating();
            BarController barController = new BarController();
            RatingController controller = new RatingController();
            barController.Post(bar);
            Guid id = bar.Id;
            controller.Post(rating, id);

            // Act
            IHttpActionResult actionResult = await controller.Get(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Rating>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(value: contentResult.Content);

        }

        [TestMethod()]
        [Route("api/Rating/{id}/{index}/{count}")]
        public async void RatingGet3Test()
        {
            // Arrange
            Bar bar = new Bar();
            Rating rating = new Rating();
            BarController barController = new BarController();
            RatingController controller = new RatingController();
            barController.Post(bar);
            Guid id = bar.Id;
            controller.Post(rating, id);
            int index = 0;
            int count = 1;
            Console.WriteLine(id);

            // Act
            IHttpActionResult actionResult = await controller.Get(id, index, count);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(value: actionResult, expectedType: typeof(IHttpActionResult));
            
        }
    }
}