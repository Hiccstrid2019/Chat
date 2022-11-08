using System.Collections.Generic;
using System.Linq;
using ChatServer.Data;

namespace ChatServer;

public class UserService
{
    private readonly AppDbContext _context;
    
    public UserService(AppDbContext context)
    {
        _context = context;
    }
    
    public int CreateUser(User user)
    {
        _context.Add(user);
        _context.SaveChanges();
        return user.UserId;
    }

    public User GetUser(string name)
    {
        return _context.Users.FirstOrDefault(u => u.UserName == name);
    }

    public ICollection<string> GetUserNames()
    {
        return _context.Users.Select(user => user.UserName).ToList();
    }
}