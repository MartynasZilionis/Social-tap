using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class TestBar
    {
        [TestMethod]
        public void IsNewPicture()
        {

            // arrange
            social_tapX.BarComments test = new social_tapX.BarComments("Test Baras Name", 10, 1);
            social_tapX.BarComments test2 = new social_tapX.BarComments("Test Bar Name", 10, 0);

            Assert.AreSame(test, test2, "Back was not enabled in BarComments");


        }
    }
}

