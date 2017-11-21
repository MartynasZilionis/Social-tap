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
    public class CommentControllerTests
    {
        [TestMethod()]
        public void CommentGetTest()
        {
            // Arrange
            Bar bar = new Bar();
            Comment comment = new Comment();
            BarController barController = new BarController();
            CommentController controller = new CommentController();
            barController.Post(bar);
            Guid id = bar.Id;
            controller.Post(comment, id);
            int index = 0;
            int count = 1;

            // Act
            IHttpActionResult actionResult = controller.Get(id, index, count);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(value: actionResult, expectedType: typeof(IHttpActionResult));

        }
    }
}