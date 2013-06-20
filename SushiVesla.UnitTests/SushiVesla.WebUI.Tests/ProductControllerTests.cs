using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SushiVesla.ObjectModel.Interfaces;
using SushiVesla.WebUI.Controllers;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.WebUI.Models;

namespace SushiVesla.UnitTests.SushiVesla.WebUI.Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductRepository> ProductRepositoryMock;
        private ProductController PrController;
        private Mock<ICategoryRepository> CategoryRepositoryMock;
        private NavController navController;

        [TestFixtureSetUp]
        public void PrepareData()
        {
            ProductRepositoryMock = new Mock<IProductRepository>();
            ProductRepositoryMock.Setup(m => m.Products).Returns(new Product[] 
            {
                new Product { ProductID = 1, Name = "P1", Category = 1 },
                new Product { ProductID = 2, Name = "P2", Category = 2 },
                new Product { ProductID = 3, Name = "P3", Category = 1 },
                new Product { ProductID = 4, Name = "P4", Category = 2 },
                new Product { ProductID = 5, Name = "P5", Category = 3 }
            }.AsQueryable());

            CategoryRepositoryMock = new Mock<ICategoryRepository>();
            CategoryRepositoryMock.Setup(m => m.Categories).Returns(new Category[] 
            {
                new Category { CategoryID = 1, Name = "Cat1" },
                new Category { CategoryID = 2, Name = "Cat2" },
                new Category { CategoryID = 3, Name = "Cat3" }
            }.AsQueryable());

            PrController = new ProductController(ProductRepositoryMock.Object, CategoryRepositoryMock.Object, null, null);
        }

        [Test]
        public void List_ListWithProducts_ReturnsNeededCountOfTheRecordsOnPage()
        {
            PrController.PageSize = 3;

            ProductListViewModel result = (ProductListViewModel)PrController.List(null, 2).Model;
            Product[] productArray = result.Products.ToArray();

            Assert.IsTrue(productArray.Length == 2, "ProductController.List returns incorrect count of the products on one page!");
            Assert.AreEqual("P4", productArray[0].Name, "ProductController.List select incorrect products!");
            Assert.AreEqual("P5", productArray[1].Name, "ProductController.List select incorrect products!");
        }

        [Test]
        public void LIst_ListWithProducts_PaginationViewModel()
        {
            PrController.PageSize = 3;

            ProductListViewModel result = (ProductListViewModel)PrController.List(null, 2).Model;
            PagingInfo pageInfo = result.PagingInfo;

            Assert.AreEqual(2, pageInfo.CurrentPage, "ProductController.List returns Paging Info with incorrect current page!");
            Assert.AreEqual(3, pageInfo.ItemsPerPage, "ProductController.List returns Paging Info with incorrect items per page!");
            Assert.AreEqual(5, pageInfo.TotalItems, "ProductController.List returns Paging Info with incorrect total items!");
            Assert.AreEqual(2, pageInfo.TotalPages, "ProductController.List returns Paging Info with incorrect total pages!");
        }

        [Test]
        public void List_ListWithProducts_FiltrListByCategory()
        {
            PrController.PageSize = 3;

            ProductListViewModel result = (ProductListViewModel)PrController.List("Cat1", 1).Model;
            Product[] productArray = result.Products.ToArray();

            Assert.AreEqual(2, productArray.Length, "ProductController.List returns incorrect count of the filturing products by category!");
            Assert.IsTrue(productArray[0].Name == "P1" && productArray[0].Category == 1, "ProductController.List returns incorrect filturing products by category!");
            Assert.IsTrue(productArray[1].Name == "P3" && productArray[1].Category == 1, "ProductController.List returns incorrect filturing products by category!");
        }

        [Test]
        public void List_ListWithProducts_ReturnsCorrectCountForEachCategory()
        {
            PrController.PageSize = 3;

            int res1 = ((ProductListViewModel)PrController.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductListViewModel)PrController.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductListViewModel)PrController.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductListViewModel)PrController.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(2, res1, "ProductController.List returns incorrect count of the products for category /'Cat1/'!");
            Assert.AreEqual(2, res2, "ProductController.List returns incorrect count of the products for category /'Cat1/'!");
            Assert.AreEqual(1, res3, "ProductController.List returns incorrect count of the products for category /'Cat1/'!");
            Assert.AreEqual(5, resAll, "ProductController.List returns incorrect count of the products when category is null!");
        }

        [Test]
        public void GetImage_SetProductWithImage_CorrectImageAndTypeReturned()
        {
            Product prod = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1,  Name = "P2"},
                prod,
                new Product {ProductID = 3,  Name = "P3"}
            }.AsQueryable());
            ProductController target = new ProductController(mock.Object, null, null, null);

            ActionResult result = target.GetImage(2);

            Assert.IsNotNull(result, "Image is not executed!");
            Assert.IsInstanceOfType(typeof(FileResult), result, "ProductController.GetImage method returnes incorrect result type!");
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType, "ProductController.GetImage method returnes incorrect content type!");
        }

        [Test]
        public void GetImage_TryToGetIncorrectImage_ImageIsNotExecuted()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1,  Name = "P1"},
                new Product {ProductID = 2,  Name = "P2"}
            }.AsQueryable());
            ProductController target = new ProductController(mock.Object, null, null, null);

            ActionResult result = target.GetImage(100);

            Assert.IsNull(result, "ProductController.GetImage method executes nonexistent image!");
        }
    }
}
