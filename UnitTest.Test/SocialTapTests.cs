using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms;
using System.Reflection;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;

namespace social_tapX.Test
{

    [TestClass]
    public class SocialTapTests
    {
        [TestMethod]
        public void IsBarAndComentsAndRatingCorrectlyInitialised()
        {
            // arrange
            String comment = "Test Comment";
            String barName = "Test Bar";
            int rating = 123;
            social_tapX.BarAndCommentsAndRating test = new social_tapX.BarAndCommentsAndRating("Test Bar", "Test Comment", 123);
            Assert.AreEqual(comment, test.Comment);
            Assert.AreEqual(barName, test.BarName);
            Assert.AreEqual(rating, test.Rating);
        }
        [TestMethod]
        public void AppCreatesMainPage()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            App testApp = new App();
            Assert.IsNotNull(testApp.MainPage);
        }
        [TestMethod]
        public void MainPageInitialisesXamarinForms()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            MainPage testMainPage = new MainPage();
            Assert.IsNotNull (MainPage.BackroundImage);
        }
        [TestMethod]
        public void BarCommentsInitialisesCorrectly()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            new App();
            String BarName = "Test Bar";
            BarComments testComments = new BarComments(BarName, 10, 0);
            Assert.IsNotNull(testComments);
            Assert.AreNotEqual(testComments, new BarComments(BarName, 10, 1));
        }
        [TestMethod]
        public void CommentsInitialisation()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            new App();
            Comments testComments = new Comments();
            Assert.IsNotNull(testComments);
        }
        [TestMethod]
        public void ChooseCommentPageSyncWithComments()
        {

        }
        [TestMethod]
        public void test()
        {
  
            

        }
        public static void Init()
        {
            Device.Info = new MockDeviceInfo();
            Device.PlatformServices = new MockPlatformServices();
            DependencyService.Register<MockResourcesProvider>();
            DependencyService.Register<MockDeserializer>();
        }
    }
}

