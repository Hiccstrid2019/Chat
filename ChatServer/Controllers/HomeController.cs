using ChatServer.Data;
using ChatServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}