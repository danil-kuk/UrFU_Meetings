using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Helpers;
using WebApp.Models.DataModels;
using WebApp.Models.DataModels.Entities;
using WebApp.Models.ViewModels;
using WebApp.Services;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOptions<EmailSettings> _emailConfig;
        private readonly IActivationService _activationService;
        private readonly IPasswordResetService _passwordResetService;

        public LoginController(IUserService userService, IOptions<EmailSettings> options, 
            IActivationService activationService, IPasswordResetService passwordResetService)
        {
            _userService = userService;
            _emailConfig = options;
            _activationService = activationService;
            _passwordResetService = passwordResetService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult SendResetEmail(LoginViewModel model)
        {
            if (_userService.GetByFilter(i => i.Email == model.Email) == null)
            {
                ModelState.AddModelError("Email", "Проверьте правильность введеной электронной почты");
                return View("ResetPassword");
            }
            var emailSender = new EmailSender(_emailConfig);
            emailSender.SendEmail
                (model.Email,
                "Восстановление пароля",
                $"</br><a href='https://{HttpContext.Request.Host}/Login/EnterNewPassword?key=" + HttpUtility.UrlEncode(new PasswordResetKey(_passwordResetService).ActivationKey(model.Email)) + "'><h1>Нажмите для восстановления<h1><a>"
                );
            TempDataMessage("message", "primary", $"Инструкции по восстановлению пароля отправлены на указанную почту");
            return View("ResetPassword");
        }

        [HttpGet]
        public IActionResult EnterNewPassword(string key)
        {
            string output = new AESEncryption().DecryptText(key);
            string[] tokens = output.Split(":OSK:");
            PasswordReset passwordReset = _passwordResetService.GetByFilter(i => i.Email == tokens[0] && i.ActivationKey == tokens[2] && DateTime.Parse(i.Time.ToString()) == DateTime.Parse(tokens[1]));
            if (passwordReset != null)
            {
                _passwordResetService.Delete(passwordReset);
                return View("EnterNewPassword", new User
                {
                    Email = passwordReset.Email
                });
            }
            return View("EmailValidFailed");//TODO
        }

        public IActionResult ChangeToNewPassword(LoginViewModel model)
        {
            User user = _userService.GetByFilter(i => model.Email == i.Email);
            user.Password = new PasswordEncode().Encoder(model.Password); //TODO
            _userService.UpdateUser(user);
            return RedirectToAction("Index", "UserProfile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _userService.GetByFilter(i => i.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Проверьте правильность введеной электронной почты");
                    return View("Index");
                }
                else
                {
                    if (!user.EmailValid)
                    {
                        ModelState.AddModelError("Email", "Активируйте аккаунт");
                        return View("Index");
                    }
                    else
                        await Authenticate(user.Email);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult PasswordCheck(string password,string Email)
        {
            var user = _userService.GetByFilter(i => i.Email == Email);
            return Json(user != null && user.Password == new PasswordEncode().Encoder(password));
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public void TempDataMessage(string key, string alert, string value)
        {
            TempData.Remove(key);
            TempData.Add(key, value);
            TempData.Add("alertType", alert);
        }
    }
}