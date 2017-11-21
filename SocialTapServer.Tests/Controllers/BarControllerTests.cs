using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;
using SocialTapServer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialTapServer.Models;
using System.Web.Http.Results;
using System.Threading;
using System.Net;

namespace SocialTapServer.Controllers.Tests
{
    [TestClass()]
    public class BarControllerTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BarGetTestOutOfRange()
        {
            // Arrange
            BarController obj = new BarController();
            var loc = "9;1111";
            int num = 3;

            // Act
            obj.Get(loc, num);

        }

        [TestMethod()]
        public void BarGetTest1()
        {
            // Arrange
            Bar bar = new Bar();
            BarController controller = new BarController();
            controller.Post(bar);
            Guid id = bar.Id;

            // Act
            IHttpActionResult actionResult = controller.Get(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Bar>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsInstanceOfType(value: actionResult, expectedType: typeof(IHttpActionResult));
            Assert.AreEqual(expected: typeof(OkNegotiatedContentResult<Bar>), actual: contentResult.GetType());
            Assert.IsNotNull(value: contentResult.Content);
            Assert.AreEqual(id, contentResult.Content.Id);
        }
    }
}