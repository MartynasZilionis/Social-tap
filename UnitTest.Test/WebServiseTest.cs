using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTest.Test
{
    [TestClass]
    public class WebServiseTest
    {
        [TestMethod]
        public async Task WebServiceRatingTest()
        {
            social_tapX.RestModels.Rating testRating = new social_tapX.RestModels.Rating();

            SocialTapServer.Models.Bar testBar = new SocialTapServer.Models.Bar();
            Guid id = testBar.Id;
            testBar.AddRating(new SocialTapServer.Models.Rating());

        }
    }
}
