using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatServer.Models;

namespace ChatServer.DAL
{
    public class EFUserRepository : IRepository
    {
        private UsersContext context = new UsersContext();

        public void Create(ApplicationUser user)
        {
            context.ApplicationUsers.Add(user);
            SaveChanges();
        }
        public ApplicationUser Get(string username)
        {
            return context.ApplicationUsers.FirstOrDefault(user => user.UserName == username);
        }

        public List<string> GetAllUserNames()
        {
            return (from u in context.ApplicationUsers
                    orderby u.UserName
                    select u.UserName).ToList();
        }

        private void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}