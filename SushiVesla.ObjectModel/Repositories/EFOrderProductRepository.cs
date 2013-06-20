using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.ObjectModel.Repositories
{
    public class EFOrderProductRepository : IOrderProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<OrderProduct> OrderProducts
        {
            get { return context.OrderProducts; }
        }

        public void SaveOrderProduct(OrderProduct orderProduct)
        {
            if (orderProduct.ID == 0)
            {
                context.OrderProducts.Add(orderProduct);
            }
            else
            {
            }
            context.SaveChanges();
        }


        public void DeleteOrderProduct(OrderProduct orderProduct)
        {
            context.OrderProducts.Remove(orderProduct);
            context.SaveChanges();
        }
    }
}
