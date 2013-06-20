using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.UnitTests.SushiVesla.ObjectModel.Tests
{
    [TestFixture]
    public class CartTests
    {
        Product Product1, Product2, Product3;
        Cart TargetCart;

        private void PrepareData()
        {
            Product1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            Product3 = new Product { ProductID = 3, Name = "P3", Price = 25M };
            TargetCart = new Cart();
        }

        [Test]
        public void AddItem_AddingItems_AllItemsAreSavedInCart()
        {
            PrepareData();

            TargetCart.AddItem(Product1, 1);
            TargetCart.AddItem(Product2, 1);
            CartLine[] results = TargetCart.Lines.ToArray();

            Assert.AreEqual(2, results.Length, "Cart.AddItem saved incorrect count of the items!");
            Assert.AreEqual(Product1, results[0].Product, "Cart.AddItem saved incorrect item!");
            Assert.AreEqual(Product2, results[1].Product, "Cart.AddItem saved incorrect item!");
        }

        [Test]
        public void AddItem_AddingItems_IfItemAlreadyInCartQuantityShouldBeIncreased()
        {
            PrepareData();

            TargetCart.AddItem(Product1, 1);
            TargetCart.AddItem(Product2, 1);
            TargetCart.AddItem(Product1, 10);
            CartLine[] results = TargetCart.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            Assert.AreEqual(2, results.Length, "Cart.AddItem saved incorrect count of the items!");
            Assert.AreEqual(11, results[0].Quantity, "Cart.AddItem saved incorrect quantity of the item!");
            Assert.AreEqual(1, results[1].Quantity, "Cart.AddItem saved incorrect quantity of the item!");
        }

        [Test]
        public void RemoveLine_RemoveItem_ItemShouldBeRemovedFromTheList()
        {
            PrepareData();
            TargetCart.AddItem(Product1, 1);
            TargetCart.AddItem(Product2, 3);
            TargetCart.AddItem(Product3, 5);
            TargetCart.AddItem(Product2, 1);

            TargetCart.RemoveLine(Product2);

            Assert.AreEqual(0, TargetCart.Lines.Where(c => c.Product == Product2).Count(), "Cart.RemoveItem doesn't remove needed items!");
            Assert.AreEqual(2, TargetCart.Lines.Count(), "Cart.RemoveItem removed items incorrectly!");
        }

        [Test]
        public void ComputeTotalValue_ListWithItems_CorrectlyCalculatedTotalPrice()
        {
            PrepareData();
            TargetCart.AddItem(Product1, 1);
            TargetCart.AddItem(Product2, 1);
            TargetCart.AddItem(Product1, 3);

            decimal result = TargetCart.ComputeTotalValue();

            Assert.AreEqual(450M, result, "Cart.ComputeTotalValue calculates total value incorrectly!");
        }

        [Test]
        public void Clear_ClearItems_CartIsEmpty()
        {
            PrepareData();
            TargetCart.AddItem(Product1, 1);
            TargetCart.AddItem(Product2, 1);

            TargetCart.Clear();

            Assert.AreEqual(0, TargetCart.Lines.Count(), "Cart.Clear didn't clear items from the list!");
        }
    }
}
