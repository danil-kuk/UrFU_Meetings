using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Helpers;
using WebApp.Models.DataModels;
using WebApp.Models.DataModels.Entities;
using WebApp.Services;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOptions<EmailSettings> _emailConfig;
        private readonly IActivationService _activationService;

        public RegistrationController(IUserService userService, IOptions<EmailSettings> options, IActivationService activationService)
        {
            _userService = userService;
            _emailConfig = options;
            _activationService = activationService;
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
                    Password = new PasswordEncode().Encoder(model.Password)
                };
                if (_userService.GetByFilter(i => i.Email == user.Email) != null)
                    return RedirectToAction("Index", "Home");
                SendActivationEmail(user);
                _userService.InsertUser(user);
                return View("SuccessfulRegistration", user);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Activation(string key)
        {
            string output = new AESEncryption().DecryptText(key);
            string[] tokens = output.Split(":OSK:");
            EmailValid emailValid = _activationService.GetByFilter(i => i.EmailToValid == tokens[0] && i.ActivationKey == tokens[2] && DateTime.Parse(i.Time.ToString()) == DateTime.Parse(tokens[1]));
            if (emailValid != null)
            {
                if (DateTime.Now > DateTime.Parse(tokens[1]).AddDays(1)) //email о подтверждении истекает через 1 день
                {
                    _activationService.Delete(emailValid);
                    return View("EmailValidExpired", new User { Email = emailValid.EmailToValid });
                }
                _activationService.Delete(emailValid);
                User user = _userService.GetByFilter(i => emailValid.EmailToValid == i.Email);
                user.EmailValid = true;
                _userService.UpdateUser(user);
                return View("EmailValidSuccess");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult RepeatEmailActivation(User model)
        {
            User user = _userService.GetByFilter(i => model.Email == i.Email);
            SendActivationEmail(user);
            return RedirectToAction("RepeatActivation", model);
        }

        [HttpGet]
        public IActionResult RepeatActivation(User model)
        {
            return View("RepeatActivation", model);
        }

        public void SendActivationEmail(User user)
        {
            var emailSender = new EmailSender(_emailConfig);
            emailSender.SendEmail
                (user.Email,
                "Активация аккаунта",
                $"</br><a href='https://{HttpContext.Request.Host}/Registration/Activation?key=" + HttpUtility.UrlEncode(new EmailActivaitonKey(_activationService).ActivationKey(user.Email)) + "'><h1>Нажмите для активации<h1><a>"
                );
        }

        [HttpPost]
        public JsonResult EmailCheck(string Email)
        {
            return Json(_userService.GetByFilter(i => i.Email == Email) == null || Email == User.Identity.Name);
        }
    }
}