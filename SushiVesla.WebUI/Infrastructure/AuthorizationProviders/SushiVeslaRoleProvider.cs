using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Ninject;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.WebUI.Infrastructure.AuthorizationProviders
{
    public class SushiVeslaRoleProvider : RoleProvider
    {
        [Inject]
        public IUserRepository UserRepository { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            //if (username == "Admin")
            //{
            //    return new string[] { "Administrator" };
            //}
            //else
            //    if (username == "Adam")
            //    {
            //        return new string[] { "User" };
            //    }
            //    else
            //    {
            //        return new string[] { };
            //    }

            var result = new string[] { UserRepository.Users.FirstOrDefault(u => u.Login == username).Role.ToString() };
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}