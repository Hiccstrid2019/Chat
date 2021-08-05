using ChatServer.Models;
using System.Collections.Generic;

namespace ChatServer.DAL
{
    public interface IRepository
    {
        void Create(User user);
        User Get(string username);
        List<string> GetAllUserNames();
    }
}
