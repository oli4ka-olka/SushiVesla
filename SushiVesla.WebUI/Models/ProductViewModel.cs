using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SushiVesla.ObjectModel.Entities;

namespace SushiVesla.WebUI.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public string Category { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}