using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.ObjectModel.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Order> Orders
        {
            get { return context.Orders; }
        }

        public void SaveOrder(Order order)
        {
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            else
            {
                var ord = context.Orders.FirstOrDefault(p => p.OrderID == order.OrderID);
                ord.SubmittedBy = order.SubmittedBy;
                ord.ProcessedBy = order.ProcessedBy;
                ord.Paid = order.Paid;
                ord.Delivered = order.Delivered;
                ord.TheOrderIsExecuted = order.TheOrderIsExecuted;
            }
            context.SaveChanges();
        }


        public void DeleteOrder(Order order)
        {
            context.Orders.Remove(order);
            context.SaveChanges();
        }
    }
}
