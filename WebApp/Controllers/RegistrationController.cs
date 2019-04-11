using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Helpers;
using WebApp.Models.DataModels;
using WebApp.Models.DataModels.Entities;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOptions<EmailSettings> _emailConfig;

        public RegistrationController(IUserService userService, IOptions<EmailSettings> options)
        {
            _userService = userService;
            _emailConfig = options;
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
                    Name = char.ToUpper(model.Name[0]) + model.Name.Substring(1).ToLower(),
                    Surname = char.ToUpper(model.Surname[0]) + model.Surname.Substring(1).ToLower(),
                    Email = model.Email,
                    Password = new PasswordEncode().Encoder(model.Password) // SHA256
                };
                SendHelloEmail(user);
                //_userService.InsertUser(user);
                //return RedirectToAction("", "Login");
            }
            return RedirectToAction("Index", "Home");
        }

        public void SendHelloEmail(User user)
        {
            var emailSender = new EmailSender(_emailConfig);
            emailSender.SendEmail
                (user.Email,
                "Регистрация в сервисе УрФУ Встречи",
                $"Здравствуйте, {user.Name} {user.Surname}! Спасибо, что зарегистрировались в нашем сервисе!"
                );
        }

        [HttpPost]
        public JsonResult EmailCheck(string Email)
        {
            return Json(_userService.GetByFilter(i => i.Email == Email) == null || Email == User.Identity.Name);
        }
    }
}