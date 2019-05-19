using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApp.Helpers;
using WebApp.Models.DataModels;
using WebApp.Models.DataModels.Entities;
using WebApp.Models.ViewModels;
using WebApp.Services;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOptions<EmailSettings> _emailConfig;
        private readonly IActivationService _activationService;
        private readonly IEventService _eventService;
        private readonly EFDBContext _context;

        public UserProfileController(IUserService userService, 
            IOptions<EmailSettings> options, IActivationService activationService,
            IEventService eventService, EFDBContext context)
        {
            _userService = userService;
            _emailConfig = options;
            _activationService = activationService;
            _context = context;
            _eventService = eventService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var user = _userService.GetByFilter(i => i.Email == User.Identity.Name);
            return View(new UserProfileViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
            });
        }

        [Authorize]
        public IActionResult Settings()
        {
            var user = _userService.GetByFilter(i => i.Email == User.Identity.Name);
            return View(new UserProfileViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
            });
        }

        [Authorize]
        public IActionResult MyEvents()
        {
            var user = _userService.GetByFilter(i => i.Email == User.Identity.Name);
            _context.Users.Include(c => c.SubscribedEvents).ToList();
            foreach (var participant in user.SubscribedEvents)
            {
                participant.Event = _eventService.GetById(participant.EventId);
            }
            return View(new UserProfileViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                SubscribedEvents = user.SubscribedEvents
            });
        }

        [Authorize]
        public IActionResult DeleteUser(UserProfileViewModel model)
        {
            User user = _userService.GetByFilter(i => i.Email == User.Identity.Name);
            _eventService.DeleteAllUserEvents(user);
            _userService.DeleteUser(user);
            ForceLogout();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public JsonResult PasswordCheckBeforeDelete(string DeleteConfirm)
        {
            User user= _userService.GetByFilter(i => i.Email == User.Identity.Name);
            return Json(user != null && user.Password == new PasswordEncode().Encoder(DeleteConfirm));
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangeUserData(User model)
        {
            User user = _userService.GetByFilter(i => i.Email == User.Identity.Name);
            UpdateData(user, model);
            return View("Index", new UserProfileViewModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email
            });
        }

        private void UpdateData(User oldData, User newData)
        {
            oldData.Name = char.ToUpper(newData.Name[0]) + newData.Name.Substring(1).ToLower();
            oldData.Surname = char.ToUpper(newData.Surname[0]) + newData.Surname.Substring(1).ToLower();
            oldData.Password = new PasswordEncode().Encoder(newData.Password);
            if (oldData.Email != newData.Email)
            {
                oldData.Email = newData.Email;
                oldData.EmailValid = false;
                ChangeEmail(oldData.Email);
                TempDataMessage("message", "primary", $"Почта изменена! Подтвердите новую почту!");
            }
            else
            {
                Relogin(oldData.Email);
                TempDataMessage("message", "success", $"Ваши данные успешно изменены");
            }
            _userService.UpdateUserData();
        }

        private async void ForceLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async void Relogin(string userName)
        {
            ForceLogout();
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private void ChangeEmail(string userName)
        {
            var emailSender = new EmailSender(_emailConfig);
            emailSender.SendEmail
                (userName,
                "Изменение почты",
                $"</br><a href='https://{HttpContext.Request.Host}/UserProfile/Activation?key=" + HttpUtility.UrlEncode(new EmailActivaitonKey(_activationService).ActivationKey(userName)) + "'><h1>Нажмите для активации<h1><a>"
                );
            ForceLogout();
        }

        [HttpGet]
        public IActionResult Activation(string key)
        {
            string output = new AESEncryption().DecryptText(key);
            string[] tokens = output.Split(":OSK:");
            EmailValid emailValid = _activationService.GetByFilter(i => i.EmailToValid == tokens[0] && i.ActivationKey == tokens[2] && DateTime.Parse(i.Time.ToString()) == DateTime.Parse(tokens[1]));
            if (emailValid != null)
            {
                _activationService.Delete(emailValid);
                User user = _userService.GetByFilter(i => emailValid.EmailToValid == i.Email);
                user.EmailValid = true;
                _userService.UpdateUser(user);
                return View("SuccessfulEmailChange");
            }
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