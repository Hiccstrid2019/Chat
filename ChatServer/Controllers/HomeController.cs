using System.Web.Mvc;
using ChatServer.Models;
using ChatServer.DAL;

namespace ChatServer.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;
        public HomeController(IRepository repositoryParam)
        {
            repository = repositoryParam;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User { UserName = model.Nickname, Password = model.Password };
                repository.Create(newUser);
                return View("Thanks", model);
            }
            else
            {
                return View();
            }
        }
    }
}