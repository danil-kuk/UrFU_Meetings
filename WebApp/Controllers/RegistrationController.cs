using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.DataModels;
using WebApp.Models.DataModels.Entities;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUserService _userService;

        public RegistrationController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Password = model.Password
                };
                _userService.InsertUser(user);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult EmailCheck(string Email)
        {
            return Json(_userService.GetByFilter(i => i.Email == Email) == null);
        }
    }
}