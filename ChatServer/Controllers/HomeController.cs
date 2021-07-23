using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatServer.Models;
using ChatServer.DAL;
using System.Data.Entity;

namespace ChatServer.Controllers
{
    public class HomeController : Controller
    {
        private UsersContext userContext = new UsersContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register( RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser { UserName = model.Nickname, Password = model.Password };
                userContext.ApplicationUsers.Add(newUser);
                userContext.SaveChanges();
                return View("Thanks", model);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Chat()
        {
            return View();
        }

    }
}