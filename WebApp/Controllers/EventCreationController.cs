﻿using System;
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
                Event newEvent = new Event
                {
                    EventName = char.ToUpper(model.EventName[0]) + model.EventName.Substring(1).ToLower(),
                    Description = model.Description,
                    Date = model.Date,
                    Time = model.Time,
                    Place = model.Place,
                    EventTheme = model.EventTheme,
                    MaxParticipants = model.MaxParticipants,
                    OrganizerId = _userService.GetByFilter(i => i.Email == User.Identity.Name).UserId,
                    Contacts = model.Contacts
                };
                _eventService.InsertEvent(newEvent);
                return RedirectToAction("Index", "EventPage", new { id = newEvent.EventId });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}