using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.ObjectModel.Repositories
{
    public class DBOrderProcessor : IOrderProcessor
    {
        public void ProcessOrder(IOrderRepository orderRepository, IOrderProductRepository orderProductRepository, Cart cart, ShippingDetails shippingInfo)
        {
            Order order = new Order();
            List<OrderProduct> orderProducts = new List<OrderProduct>();

            order.SubmittedBy = shippingInfo.UserId;
            order.ProcessedBy = 0;
            order.Paid = false;
            order.Delivered = false;
            order.TheOrderIsExecuted = false;
            order.UserName = shippingInfo.Name;
            order.PhoneNumber = shippingInfo.PhoneNumber;
            order.Address = shippingInfo.Address;
            order.City = shippingInfo.City;
            order.State = shippingInfo.State;
            order.Country = shippingInfo.Country;
            order.GiftWrap = shippingInfo.GiftWrap;
            order.RushOrder = shippingInfo.RushOrder;
            order.PaymentMethod = shippingInfo.PaymentMethod.ToString();
            orderRepository.SaveOrder(order);

            foreach (var item in cart.Lines)
            {
                orderProductRepository.SaveOrderProduct(new OrderProduct { OrderID = order.OrderID, ProductID = item.Product.ProductID, Price = item.Product.Price, Quantity = item.Quantity });
            }
        }
    }
}
