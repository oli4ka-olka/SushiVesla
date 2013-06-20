using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Interfaces;
using SushiVesla.WebUI.Controllers;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.UnitTests.SushiVesla.WebUI.Tests
{
    [TestFixture]
    public class NavControllerTests
    {
        private Mock<ICategoryRepository> CategoryRepositoryMock;
        private NavController navController;

        [TestFixtureSetUp]
        public void PrepareData()
        {
            CategoryRepositoryMock = new Mock<ICategoryRepository>();
            CategoryRepositoryMock.Setup(m => m.Categories).Returns(new Category[] 
            {
                new Category { CategoryID = 1, Name = "Apples" },
                new Category { CategoryID = 2, Name = "Big Apples" },
                new Category { CategoryID = 3, Name = "Plums" },
                new Category { CategoryID = 4, Name = "Oranges" },
                new Category { CategoryID = 5, Name = "Good Apples" }
            }.AsQueryable());

            navController = new NavController(CategoryRepositoryMock.Object);
        }

        [Test]
        public void Menu_ListWithManyRecords_ReturnsAllCategoriesWithoutDuplication()
        {
            var results = ((IEnumerable<string>)navController.Menu().Model).ToArray();

            Assert.AreEqual(5, results.Length, "NavController.Menu returns incorrect count of the categories!");
            Assert.AreEqual("Apples", results[0], "NavController.Menu returns incorrect category!");
            Assert.AreEqual("Good Apples", results[2], "NavController.Menu returns incorrect category!");
            Assert.AreEqual("Oranges", results[3], "NavController.Menu returns incorrect category!");
        }

        //[Test]
        //public void Menu_ChooseCategory_ReturnsCorreccategoryToView()
        //{
        //    string categoryToSelect = "Apples";

        //    string result = navController.Menu(categoryToSelect).ViewBag.SelectedCategory;

        //    Assert.AreEqual(categoryToSelect, result, "NavController.Menu returns incorrect selected category!");
        //}
    }
}
