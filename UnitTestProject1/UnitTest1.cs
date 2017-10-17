using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialTap;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        Image<Hsv, byte> test = new Image<Hsv, byte>(400, 400);
        recognition.Recognition RC = new recognition.Recognition();

        [TestMethod]
        public void BeerTopTest()
        {
            // arrange
            test.Bitmap = (Bitmap)Image.FromFile(@"...\...\pvz5.jpg");
            RC.ADD_TO_LIST();

            // Act
            RC.BeerTop(test);

            // assert
            Assert.AreEqual(29, RC.xv);
            Assert.AreEqual(200, RC.yv);
        }

        [TestMethod]
        public void BeerBottomTest()
        {
            // arrange
            test.Bitmap = (Bitmap)Image.FromFile(@"...\...\pvz5.jpg");
            RC.ADD_TO_LIST();

            // Act
            RC.BeerBottom(test);

            // assert
            Assert.AreEqual(372, RC.xa);
            Assert.AreEqual(200, RC.ya);
        }

        [TestMethod]
        public void BeerUpperLeftPoint()
        {
            // arrange
            test.Bitmap = (Bitmap)Image.FromFile(@"...\...\pvz5.jpg");
            RC.ADD_TO_LIST();

            // Act
            RC.BeerUpperLeftPoint(test);

            // assert
            Assert.AreEqual(0, RC.xvk);
            Assert.AreEqual(200, RC.yvk);
        }

        [TestMethod]
        public void BeerLowerLeftPoint()
        {
            // arrange
            test.Bitmap = (Bitmap)Image.FromFile(@"...\...\pvz5.jpg");
            RC.ADD_TO_LIST();

            // Act
            RC.BeerLowerLeftPoint(test);

            // assert
            Assert.AreEqual(0, RC.xak);
            Assert.AreEqual(200, RC.yak);
        }

        [TestMethod]
        public void BeerUpperRightPoint()
        {
            // arrange
            test.Bitmap = (Bitmap)Image.FromFile(@"...\...\pvz5.jpg");
            RC.ADD_TO_LIST();

            // Act
            RC.BeerUpperRightPoint(test);

            // assert
            Assert.AreEqual(0, RC.xvd);
            Assert.AreEqual(200, RC.yvd);
        }

        [TestMethod]
        public void BeerLowerRightPoint()
        {
            // arrange
            test.Bitmap = (Bitmap)Image.FromFile(@"...\...\pvz5.jpg");
            RC.ADD_TO_LIST();

            // Act
            RC.BeerLowerRightPoint(test);

            // assert
            Assert.AreEqual(0, RC.xad);
            Assert.AreEqual(200, RC.yad);
        }
    }
}
