using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SushiVesla.WebUI.Infrastructure.Interfaces
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}