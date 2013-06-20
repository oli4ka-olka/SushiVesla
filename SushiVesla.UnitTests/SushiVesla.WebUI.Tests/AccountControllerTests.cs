using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SushiVesla.WebUI.Infrastructure.Interfaces;
using SushiVesla.WebUI.Models;
using SushiVesla.WebUI.Controllers;

namespace SushiVesla.UnitTests.SushiVesla.WebUI.Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void LogOn_TryToAuthicateWithCorrectCredentials_AuthenticationIsSuccessfull()
        {
            Mock<IAuthProvider> mockAuthProvider = new Mock<IAuthProvider>();
            mockAuthProvider.Setup(m => m.Authenticate("Admin", "AdminPassword")).Returns(true);
            LogOnViewModel model = new LogOnViewModel
            {
                UserName = "Admin",
                Password = "AdminPassword"
            };
            AccountController targetAccountController = new AccountController(mockAuthProvider.Object, null);

            ActionResult result = targetAccountController.LogOn(model, "/MyURL");

            Assert.IsInstanceOfType(typeof(RedirectResult), result, "User with correct credentials is not authenticated!");
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url, "AccountController.LogOn method redirect to incorrect page!");
        }

        [Test]
        public void LogOn_TryToAuthicateWithIncorrectCredentials_AuthenticationIsFailed()
        {
            Mock<IAuthProvider> mockAuthProvider = new Mock<IAuthProvider>();
            mockAuthProvider.Setup(m => m.Authenticate("Admin", "AdminPassword")).Returns(true);
            LogOnViewModel model = new LogOnViewModel
            {
                UserName = "badUser",
                Password = "badPass"
            };
            AccountController targetAccountController = new AccountController(mockAuthProvider.Object, null);

            ActionResult result = targetAccountController.LogOn(model, "/MyURL");

            Assert.IsInstanceOfType(typeof(ViewResult), result, "User with incorrect credentials is authenticated!");
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid, "AccountController.LogOn with incorrect credentials validation is correct!");
        }
    }
}
