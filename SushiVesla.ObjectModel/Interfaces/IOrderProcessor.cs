using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.ObjectModel.Interfaces
{
    public interface IOrderProcessor
    {
        void ProcessOrder(IOrderRepository orderRepository, IOrderProductRepository orderProductRepository, Cart cart, ShippingDetails shippingInfo);
    }
}
