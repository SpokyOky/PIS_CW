using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Logics;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        private UserLogic _userLogic;

        public LoginController(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var _user = _userLogic.Read(user).FirstOrDefault();

            if (_user == null)
            {
                ModelState.AddModelError("Ошибка", "Пользователь не найден");

                return View("index", user);
            }
            else
            {
                Program.User = _user;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            Program.User = null;
            return RedirectToAction("Login");
        }
    }
}
