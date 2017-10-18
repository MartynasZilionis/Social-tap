using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialTap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SocialTap.Tests
{
    [TestClass()]
    public class MainMenuTests
    {
        [TestMethod()]
        public void RegexValidateTest()
        {
            //Arrange
            MainMenu obj = new MainMenu();

            //Assert
            Assert.IsTrue(obj.RegexValidate("Ąsilas"));
            Assert.IsFalse(obj.RegexValidate("asilas"));
            Assert.IsFalse(obj.RegexValidate("Ąsi1as"));
            Assert.IsTrue(obj.RegexValidate("Asilas"));
            Assert.IsTrue(obj.RegexValidate("Aąčęėįšųūž"));
            Assert.IsFalse(obj.RegexValidate("Asilasasilas"));
            Assert.IsFalse(obj.RegexValidate("ASILAS"));
        }
    }
}