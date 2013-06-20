using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SushiVesla.ObjectModel.Entities
{
    public class User
    {
        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter a login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a surname")]
        public string Surnaame { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter a correct email address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Choose a role")]
        public UserRoles Role { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool Deleted { get; set; }
    }

    public enum UserRoles
    {
        Admin = 3,
        ServiceStaff = 2,
        User = 1
    }
}
