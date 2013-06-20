using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using System.Web.Mvc;
using SushiVesla.ObjectModel.Interfaces;
using SushiVesla.WebUI.Controllers;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.UnitTests.SushiVesla.WebUI.Tests
{
    [TestFixture]
    public class AdminControllerTests
    {
        Mock<IProductRepository> mockProductRepository;
        Mock<ICategoryRepository> mockCategoryRepository;
        Mock<IOrderProductRepository> mockOrderProductRepository;
        AdminController targetAdminController;

        public void PrepereData()
        {
            mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = 1},
                new Product {ProductID = 2, Name = "P2", Category = 2},
                new Product {ProductID = 3, Name = "P3", Category = 3}
            }.AsQueryable());

            mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(m => m.Categories).Returns(new Category[]
                {
                    new Category {CategoryID = 1, Name = "Category1"},
                    new Category {CategoryID = 2, Name = "Category2"},
                    new Category {CategoryID = 3, Name = "Category3"}
                }.AsQueryable());

            targetAdminController = new AdminController(mockProductRepository.Object, mockCategoryRepository.Object, null, null, null);
        }

        public void EraseData()
        {
            mockProductRepository = null;
            targetAdminController = null;
        }

        [Test]
        public void Index_SetProducts_ViewPageWithAllProducts()
        {
            PrepereData();

            Product[] result = ((IEnumerable<Product>)targetAdminController.Products().ViewData.Model).ToArray();

            Assert.AreEqual(3, result.Length, "AdminController.Index returned incorrect count of the products!");
            Assert.AreEqual("P1", result[0].Name, "AdminController.Index returned incorrect products!");
            Assert.AreEqual("P2", result[1].Name, "AdminController.Index returned incorrect products!");
            Assert.AreEqual("P3", result[2].Name, "AdminController.Index returned incorrect products!");
            EraseData();
        }

        [Test]
        public void Edit_TryToEditExistingProduct_ProductIsEditable()
        {
            PrepereData();

            Product p1 = targetAdminController.EditProduct(1).ViewData.Model as Product;
            Product p2 = targetAdminController.EditProduct(2).ViewData.Model as Product;
            Product p3 = targetAdminController.EditProduct(3).ViewData.Model as Product;

            Assert.AreEqual(1, p1.ProductID, "AdminController.Edit edits incorrect product!");
            Assert.AreEqual(2, p2.ProductID, "AdminController.Edit edits incorrect product!");
            Assert.AreEqual(3, p3.ProductID, "AdminController.Edit edits incorrect product!");
            EraseData();
        }

        [Test]
        public void Edit_TryToEditNotExistingProduct_ProductIsNotEditable()
        {
            PrepereData();

            Product result = (Product)targetAdminController.EditProduct(4).ViewData.Model;

            Assert.IsNull(result, "AdminController.Edit can edits not existing product!");
            EraseData();
        }

        [Test]
        public void Edit_SaveValidChanges_AllChangesAreSaved()
        {
            mockProductRepository = new Mock<IProductRepository>();
            targetAdminController = new AdminController(mockProductRepository.Object, null, null, null, null);
            Product product = new Product { Name = "Test" };

            ActionResult result = targetAdminController.EditProduct(product, null);

            mockProductRepository.Verify(m => m.SaveProduct(product));
            Assert.IsInstanceOfType(typeof(RedirectToRouteResult), result, "AdminController.Edit returns incorrect instance!");
            EraseData();
        }

        [Test]
        public void Edit_SaveInvalidChanges_ChangesAreNotSaved()
        {
            mockProductRepository = new Mock<IProductRepository>();
            targetAdminController = new AdminController(mockProductRepository.Object, null, null, null, null);
            Product product = new Product { Name = "Test" };
            targetAdminController.ModelState.AddModelError("error", "error");

            ActionResult result = targetAdminController.EditProduct(product, null);

            mockProductRepository.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            EraseData();
        }

        [Test]
        public void Delete_SendValidProductId_ProductShouldBeDeleted()
        {
            Product prod = new Product { ProductID = 2, Name = "Test" };
            mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1,  Name = "P1"},
                prod,
                new Product {ProductID = 3,  Name = "P3"},
            }.AsQueryable());
            mockOrderProductRepository = new Mock<IOrderProductRepository>();
            mockOrderProductRepository.Setup(m => m.OrderProducts).Returns(new OrderProduct[] 
            {
                new OrderProduct {ID = 1, OrderID = 1, ProductID = 1},
                new OrderProduct {ID = 2, OrderID = 2, ProductID = 3}
            }.AsQueryable());
            targetAdminController = new AdminController(mockProductRepository.Object, null, null, null, mockOrderProductRepository.Object);

            targetAdminController.DeleteProduct(prod.ProductID);

            mockProductRepository.Verify(m => m.DeleteProduct(prod), "Delete methot has been invoked for incorrect product!");
            EraseData();
        }

        [Test]
        public void DeleteSendInvalidProsuctId_DeleteMethodShouldNotBeInvoked()
        {
            mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1,  Name = "P1"},
                new Product {ProductID = 2,  Name = "P2"},
                new Product {ProductID = 3,  Name = "P3"},
            }.AsQueryable());
            targetAdminController = new AdminController(mockProductRepository.Object, null, null, null, null);

            targetAdminController.DeleteProduct(100);

            mockProductRepository.Verify(m => m.DeleteProduct(It.IsAny<Product>()), Times.Never(), "Delete method has been invoked for incorrect product id");
            EraseData();
        }

    }
}
