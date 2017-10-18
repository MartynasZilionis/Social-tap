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
            Regex valid = new Regex("^[A-ZĄČĘĖĮŠŲŪŽ][a-ząčęėįšųūž]{1,10}$");
            string test = "Ąsila5";

            //Act
            obj.RegexValidate(valid, test);

            //Assert
            Assert.AreNotEqual(true, obj.tf);
        }
    }
}