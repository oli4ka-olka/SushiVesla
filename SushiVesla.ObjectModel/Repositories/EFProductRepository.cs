using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.ObjectModel.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Product> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                var prod = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                prod.Name = product.Name;
                prod.Description = product.Description;
                prod.Category = product.Category;
                prod.Price = product.Price;
                prod.ImageMimeType = product.ImageMimeType;
                prod.ImageData = product.ImageData;
            }
            context.SaveChanges();
        }


        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }
    }
}
