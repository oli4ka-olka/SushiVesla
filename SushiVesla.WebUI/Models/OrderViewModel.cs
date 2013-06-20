using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.WebUI.Models
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public User SubmittedBy { get; set; }
        public List<Product> Products { get; set; }
        public IEnumerable<OrderProduct> OrderProducts { get; set; }
    }
}