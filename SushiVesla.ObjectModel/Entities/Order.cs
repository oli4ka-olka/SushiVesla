using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushiVesla.ObjectModel.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public int SubmittedBy { get; set; }
        public int ProcessedBy { get; set; }
        public bool Paid { get; set; }
        public bool Delivered { get; set; }
        public bool TheOrderIsExecuted { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
        public bool RushOrder { get; set; }
        public string PaymentMethod { get; set; }
    }
}
