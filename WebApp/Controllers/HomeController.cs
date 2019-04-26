using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Models.DataModels;
using WebApp.Models.DataModels.Entities;
using WebApp.Models.ViewModels;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly EFDBContext _context;

        public HomeController(IUserService userService, EFDBContext context)
        {
            _userService = userService;
            _context = context;
        }
        
        public IActionResult Index()
        {
            var events = _context.Events.Include(c => c.Participants);
            return View(events.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
