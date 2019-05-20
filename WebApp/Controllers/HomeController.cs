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

        public IActionResult Index(string sortOrder)
        {
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["PartisipantsSortParm"] = sortOrder == "Partisipants" ? "part_desc" : "Partisipants";
            var events = _context.Events.Include(c => c.Participants);
            var sortedList = events.OrderBy(s => s.Date);
            switch (sortOrder)
            {
                case "part_desc":
                    sortedList = events.OrderByDescending(s => s.Participants.Count);
                    break;
                case "Partisipants":
                    sortedList = events.OrderBy(s => s.Participants.Count);
                    break;
                case "date_desc":
                    sortedList = events.OrderByDescending(s => s.Date);
                    break;
            }
            return View(sortedList.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
