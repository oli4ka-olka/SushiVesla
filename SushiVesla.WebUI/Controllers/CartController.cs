using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;
using SushiVesla.WebUI.Models;

namespace SushiVesla.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        private IUserRepository userRepository;
        private IOrderRepository orderRepository;
        private IOrderProductRepository orderProductRepository;

        public CartController(IProductRepository productRepository, IOrderProcessor processor, IUserRepository usrRepository, IOrderRepository ordRepository, IOrderProductRepository ordProductRepository)
        {
            repository = productRepository;
            orderProcessor = processor;
            userRepository = usrRepository;
            orderRepository = ordRepository;
            orderProductRepository = ordProductRepository;
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveSingleItemFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, -1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public ViewResult Summary(Cart cart)
        {
            return View(cart);
        }

        public ViewResult CartTextSummary(Cart cart)
        {
            return View(cart);
        }

        public ViewResult Checkout(Cart cart)
        {
            ViewBag.TotalSum = cart.ComputeTotalValue();
            return View(GetShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(orderRepository, orderProductRepository, cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(GetShippingDetails());
            }
        }

        private ShippingDetails GetShippingDetails()
        {
            var shippingDetails = new ShippingDetails();
            if (User.Identity.IsAuthenticated)
            {
                //var user = userRepository.GetUserByLogin(User.Identity.Name);
                var user = userRepository.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                shippingDetails.UserId = user.UserID;
                shippingDetails.Name = string.Format("{0} {1}", user.Name, user.Surnaame);
                shippingDetails.PhoneNumber = user.PhoneNumber;
            }
            return shippingDetails;
        }
    }
}
