using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.ObjectModel.Interfaces
{
    public interface IOrderProductRepository
    {
        IQueryable<OrderProduct> OrderProducts { get; }
        void SaveOrderProduct(OrderProduct orderProduct);
        void DeleteOrderProduct(OrderProduct orderProduct);
    }
}
