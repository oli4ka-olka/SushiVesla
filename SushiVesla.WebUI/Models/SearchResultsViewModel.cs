﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.WebUI.Models
{
    public class SearchResultsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string TextForSearch { get; set; }
    }
}