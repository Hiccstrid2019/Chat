using ChatServer.Models;
using System.Data.Entity;


namespace ChatServer.DAL
{
    public class UsersContext : DbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}