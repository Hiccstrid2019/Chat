using ChatServer.Data;
using ChatServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserService _service;
        public HomeController(UserService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User { UserName = model.Nickname, Password = model.Password };
                _service.CreateUser(newUser);
                return View("Thanks", model);
            }
            return View(model);
        }
    }
}