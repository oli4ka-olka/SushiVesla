﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.WebUI.Models
{
    public class ShippingDetailsViewModel
    {
        public ShippingDetails ShippingDetails { get; set; }
        public Cart Cart { get; set; }
    }
}