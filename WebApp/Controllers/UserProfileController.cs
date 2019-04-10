using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.Models.DataModels.Entities;
using WebApp.Models.ViewModels;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;

        public UserProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        public IActionResult Index()
        {
            User user = _userService.GetByFilter(i => i.Email == User.Identity.Name);
            return View(new UserProfileViewModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email
            });
        }

        [Authorize]
        public IActionResult DeleteUser()
        {
            User user = _userService.GetByFilter(i => i.Email == User.Identity.Name);
            _userService.DeleteUser(user);
            ForceLogout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangeUserData(User model)
        {
            User user = _userService.GetByFilter(i => i.Email == User.Identity.Name);
            UpdateData(user, model);

            return RedirectToAction("Index", "Home");
        }

        private void UpdateData(User oldData, User newData)
        {
            oldData.Name = char.ToUpper(newData.Name[0]) + newData.Name.Substring(1).ToLower();
            oldData.Surname = char.ToUpper(newData.Surname[0]) + newData.Surname.Substring(1).ToLower();
            oldData.Email = newData.Email;
            oldData.Password = new PasswordEncode().Encoder(newData.Password);
            ChangeEmail(oldData.Email);
            _userService.UpdateUser();
        }

        private async void ForceLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async void ChangeEmail(string userName)
        {
            ForceLogout();
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}