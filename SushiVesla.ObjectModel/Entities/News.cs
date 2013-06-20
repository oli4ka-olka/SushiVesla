using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SushiVesla.ObjectModel.Entities
{
    public class News
    {
        [HiddenInput(DisplayValue = false)]
        public int NewsID { get; set; }

        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a body")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public bool ShowOnMainPage { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime PublishTime { get; set; }

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
    }
}
