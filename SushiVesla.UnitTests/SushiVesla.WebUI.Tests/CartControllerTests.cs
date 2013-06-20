using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SushiVesla.ObjectModel.Interfaces;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.WebUI.Controllers;
using SushiVesla.WebUI.Models;

namespace SushiVesla.UnitTests.SushiVesla.WebUI.Tests
{
    [TestFixture]
    public class CartControllerTests
    {
        Mock<IProductRepository> mockProductRepository;
        Mock<IOrderProcessor> mockOrderProcessor;
        Cart cart;
        CartController targetCartController;

        private void PrepareData()
        {
            mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(m => m.Products).Returns(new Product[] 
            { 
                new Product 
                { 
                    ProductID = 1, 
                    Name = "PI", 
                    Category = 1 
                } 
            }.AsQueryable());
            cart = new Cart();
            targetCartController = new CartController(mockProductRepository.Object, null, null, null, null);
        }

        private void PrepareDataForTestingCheckout()
        {
            mockOrderProcessor = new Mock<IOrderProcessor>();
            cart = new Cart();
            targetCartController = new CartController(null, mockOrderProcessor.Object, null, null, null);
        }

        private void ClearTestData()
        {
            mockProductRepository = null;
            mockOrderProcessor = null;
            cart = null;
            targetCartController = null;
        }

        [Test]
        public void AddToCart_AddingItems_AllItemsAreSavedInCart()
        {
            PrepareData();

            targetCartController.AddToCart(cart, 1, null);

            Assert.AreEqual(1, cart.Lines.Count(), "CartController.AddToCart saved incorrect count of the items!");
            Assert.AreEqual(1, cart.Lines.ToArray()[0].Product.ProductID, "CartController.AddToCart saved incorrect item!");
            ClearTestData();
        }

        [Test]
        public void AddToCart_AddingItems_AfterAddingItemToCartTheReroutingToIndexPageShouldBeHappend()
        {
            PrepareData();

            RedirectToRouteResult result = targetCartController.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual("Index", result.RouteValues["action"], "CartController.AddToCart rerouts to incorrect action method!");
            Assert.AreEqual("myUrl", result.RouteValues["returnUrl"], "CartController.AddToCart rerouts to incorrect Url!");
            ClearTestData();
        }

        [Test]
        public void Index_CallIndexMethod_ReverseLinkShouldBeCorrecly()
        {
            PrepareData();

            CartIndexViewModel result = (CartIndexViewModel)targetCartController.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(cart, result.Cart, "CartController.Index operates with incorrect cart!");
            Assert.AreEqual("myUrl", result.ReturnUrl, "CartController.Index returns incorrect reverse Url!");
            ClearTestData();
        }

        //[Test]
        //public void Checkout_TryToCheckoutEmptyCart_OrderDoesNotCheckouted()
        //{
        //    PrepareDataForTestingCheckout();
        //    ShippingDetails shippingDetails = new ShippingDetails();

        //    ViewResult result = targetCartController.Checkout(cart, shippingDetails);

        //    mockOrderProcessor.Verify(m => m.ProcessOrder(null, null, It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never(), "Empty order was send for processed!");
        //    Assert.AreEqual("", result.ViewName, "Default page isn't returned!");
        //    Assert.AreEqual(false, result.ViewData.ModelState.IsValid, "Incorrect model is valid!");
        //    ClearTestData();
        //}

        //[Test]
        //public void Checkout_InvalidShippingDetailsSendForCheckout_OrderDoesNotCheckouted()
        //{
        //    PrepareDataForTestingCheckout();
        //    cart.AddItem(new Product(), 1);
        //    targetCartController.ModelState.AddModelError("error", "error");

        //    ViewResult result = targetCartController.Checkout(cart, new ShippingDetails());

        //    mockOrderProcessor.Verify(m => m.ProcessOrder(null, null, It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never(), "Order with incorrect ShippingDetails send to processed!");
        //    Assert.AreEqual("", result.ViewName, "Default page isn't returned!");
        //    Assert.AreEqual(false, result.ViewData.ModelState.IsValid, "Incorrect model is valid!");
        //    ClearTestData();
        //}

        [Test]
        public void Checkout_ValidInfomationSendForCheckout_OrderCheckouted()
        {
            PrepareDataForTestingCheckout();
            cart.AddItem(new Product(), 1);

            ViewResult result = targetCartController.Checkout(cart, new ShippingDetails());

            mockOrderProcessor.Verify(m => m.ProcessOrder(null, null, It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once(), "Order with correct information isn't send to processed!");
            Assert.AreEqual("Completed", result.ViewName, "Completed hasn't returned!");
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid, "Correct model is invalid!");
            ClearTestData();
        }

    }
}
