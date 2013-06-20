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
    [Authorize(Roles = "ServiceStaff")]
    public class ServiceStaffController : Controller
    {
        private IOrderRepository orderRepository;
        private IUserRepository userRepository;
        private IOrderProductRepository orderProductRepository;
        private IProductRepository productRepository;

        public ServiceStaffController(IOrderRepository ordRepository, IUserRepository usrRepository, IOrderProductRepository ordProductRepository, IProductRepository prodRepository)
        {
            orderRepository = ordRepository;
            userRepository = usrRepository;
            orderProductRepository = ordProductRepository;
            productRepository = prodRepository;
        }

        public ActionResult Index()
        {
            return View(orderRepository.Orders);
        }

        [HttpPost]
        public ActionResult AddToQueue(int OrderId)
        {
            SushiVesla.ObjectModel.Entities.User user = userRepository.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            Order order = orderRepository.Orders.FirstOrDefault(o => o.OrderID == OrderId);
            if (order.ProcessedBy == 0)
            {
                order.ProcessedBy = user.UserID;
            }
            orderRepository.SaveOrder(order);
            return RedirectToAction("Index");
        }

        public ActionResult MyQueue()
        {
            User processedBy = userRepository.Users.FirstOrDefault(u => u.Login == User.Identity.Name);

            List<OrderViewModel> viewModel = new List<OrderViewModel>();
            foreach (var order in orderRepository.Orders.Where(o => o.ProcessedBy == processedBy.UserID && !o.TheOrderIsExecuted))
            {
                IEnumerable<OrderProduct> ordProds = orderProductRepository.OrderProducts.Where(op => op.OrderID == order.OrderID);
                List<Product> prodList = new List<Product>();
                foreach (var ordProd in ordProds)
                {
                    foreach (var product in productRepository.Products)
                    {
                        if (ordProd.ProductID == product.ProductID)
                        {
                            prodList.Add(product);
                            break;
                        }
                    }
                }

                viewModel.Add(new OrderViewModel
                {
                    Order = order,
                    SubmittedBy = userRepository.Users.FirstOrDefault(u => u.UserID == order.SubmittedBy),
                    OrderProducts = ordProds,
                    Products = prodList
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeliverOrder(int OrderId)
        {
            Order order = orderRepository.Orders.FirstOrDefault(o => o.OrderID == OrderId);
            order.Delivered = true;
            orderRepository.SaveOrder(order);
            return RedirectToAction("MyQueue");
        }

        [HttpPost]
        public ActionResult PayOrder(int OrderId)
        {
            Order order = orderRepository.Orders.FirstOrDefault(o => o.OrderID == OrderId);
            order.Paid = true;
            orderRepository.SaveOrder(order);
            return RedirectToAction("MyQueue");
        }

        [HttpPost]
        public ActionResult ExecuteOrder(int OrderId)
        {
            Order order = orderRepository.Orders.FirstOrDefault(o => o.OrderID == OrderId);
            order.TheOrderIsExecuted = true;
            orderRepository.SaveOrder(order);
            return RedirectToAction("MyQueue");
        }
    }
}
