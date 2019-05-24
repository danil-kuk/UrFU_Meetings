using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.DataModels.Entities;
using WebApp.Services;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class EventCreationController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public EventCreationController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateNewEvent(Event model)
        {
            if (ModelState.IsValid)
            {
                var organiser = _userService.GetByFilter(i => i.Email == User.Identity.Name);
                Event newEvent = new Event
                {
                    EventName = char.ToUpper(model.EventName[0]) + model.EventName.Substring(1).ToLower(),
                    Description = model.Description,
                    Date = model.Date,
                    Time = model.Time,
                    Place = model.Place,
                    EventTheme = model.EventTheme,
                    MaxParticipants = model.MaxParticipants,
                    OrganizerId = organiser.UserId,
                    Contacts = model.Contacts
                };
                _eventService.InsertEvent(newEvent);
                _eventService.AddNewParticipant(newEvent, organiser);
                return RedirectToAction("Index", "EventPage", new { id = newEvent.EventId });
            }
            return RedirectToAction("Index", "Home");
        }
        
        public JsonResult EventDateCheck(DateTime Date)
        {
            return Json(Date >= DateTime.Today && Date < DateTime.Today.AddMonths(6));
        }

        public JsonResult EventTimeCheck(DateTime Time, DateTime Date)
        {
            return (Date == DateTime.Today) ? Json(DateTime.Now < Time): Json(true);
        }
    }
}