using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SushiVesla.ObjectModel.Entities;
using SushiVesla.ObjectModel.Interfaces;

namespace SushiVesla.ObjectModel.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<User> Users
        {
            get { return context.Users; }
        }

        public void SaveUser(User user)
        {
            if (user.UserID == 0)
            {
                context.Users.Add(user);
            }
            else
            {
                var usr = context.Users.FirstOrDefault(u => u.UserID == user.UserID);
                usr.Name = user.Name;
                usr.Login = user.Login;
                usr.Name = user.Name;
                usr.Password = user.Password;
                usr.PhoneNumber = user.PhoneNumber;
                usr.Role = user.Role;
                usr.Surnaame = user.Surnaame;
                usr.EmailAddress = user.EmailAddress;
            }
            context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }
    }
}
