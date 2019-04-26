using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.DataModels;
using WebApp.Models.DataModels.Entities;
using WebApp.Services;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class EventPageController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;
        private readonly IDataBaseService<EventParticipant> _databaseEvent;
        private readonly EFDBContext _context;

        public EventPageController(IEventService eventService, IUserService userService, IDataBaseService<EventParticipant> databaseEvent, EFDBContext context)
        {
            _eventService = eventService;
            _userService = userService;
            _databaseEvent = databaseEvent;
            _context = context;
        }

        public IActionResult Index(int id)
        {
            if (_eventService.GetById(id) == null)
                return RedirectToAction("Index", "Home");
            //Постараться убрать в другое место
            var selectedEvent = _context.Events.Where(e => e.EventId == id).Include(c => c.Participants).FirstOrDefault();
            foreach (var user in selectedEvent.Participants)
                user.User = _userService.GetById(user.UserId);
            return View(selectedEvent);
        }

        [Authorize]
        public IActionResult SubscribeToEvent(Event model)
        {
            var user = _userService.GetByFilter(u => u.Email == User.Identity.Name);
            //Постараться убрать в другое место все вспомогательные вещи
            var newParticipant = new EventParticipant
            {
                Event = model,
                User = user
            };
            var newModel = _context.Users.Where(e => e.UserId == user.UserId).Include(c => c.SubscribedEvents).FirstOrDefault();
            if (user.SubscribedEvents.Select(e => e.EventId).Contains(model.EventId))
            {
                TempDataMessage("message", "primary", $"Вы уже участвуете в этом мероприятии");
                return RedirectToAction("Index", new { id = model.EventId });
            }
            _eventService.AddNewParticipant(model, newParticipant); //или можно использовать _userService.AddNewParticipant(user, newParticipant);
            TempDataMessage("message", "success", $"Вы добавлены к списку участников мероприятия");
            return RedirectToAction("Index", new { id = model.EventId });
        }

        public void TempDataMessage(string key, string alert, string value)
        {
            TempData.Remove(key);
            TempData.Add(key, value);
            TempData.Add("alertType", alert);
        }
    }
}