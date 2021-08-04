using ChatServer.Models;
using System.Collections.Generic;

namespace ChatServer.DAL
{
    public interface IRepository
    {
        void Create(ApplicationUser user);
        ApplicationUser Get(string username);
        List<string> GetAllUserNames();
    }
}
