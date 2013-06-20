using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.WebUI.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}