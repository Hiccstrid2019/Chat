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

        public void Create(User user)
        {
            context.Users.Add(user);
            SaveChanges();
        }
        public User Get(string username)
        {
            return context.Users.FirstOrDefault(user => user.UserName == username);
        }

        public List<string> GetAllUserNames()
        {
            return (from u in context.Users
                    orderby u.UserName
                    select u.UserName).ToList();
        }

        private void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}