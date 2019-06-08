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
            var selectedEvent = _context.Events.Where(e => e.EventId == id)
                .Include(c => c.Participants).FirstOrDefault();
            foreach (var user in selectedEvent.Participants)
            {
                user.User = _userService.GetById(user.UserId);
            }
            var organizer = _userService.GetById(selectedEvent.OrganizerId);
            TempData["organizerEmail"] = organizer.Email;
            TempData["organizerName"] = organizer.Name + " " + organizer.Surname;
            return View(selectedEvent);
        }

        [Authorize]
        public IActionResult RedirectToEditEvent(int id)
        {
            var selectedEvent = _eventService.GetById(id);
            var currentUserId = _userService.GetByFilter(u => u.Email == User.Identity.Name).UserId;
            if (selectedEvent == null || currentUserId != selectedEvent.OrganizerId)
                return RedirectToAction("Index", "Home");
            return View("EditEvent", selectedEvent);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditEvent(Event model)
        {
            var currentEvent = _eventService.GetById(model.EventId);
            currentEvent.EventName = model.EventName;
            currentEvent.Description = model.Description;
            if (model.Date != default(DateTime))
                currentEvent.Date = model.Date;
            currentEvent.Contacts = model.Contacts;
            currentEvent.MaxParticipants = model.MaxParticipants;
            currentEvent.Place = model.Place;
            if (model.Time != default(DateTime))
                currentEvent.Time = model.Time;
            currentEvent.EventTheme = model.EventTheme;
            _eventService.UpdateEvent(currentEvent);
            TempDataMessage("message", "primary", $"Информация о мероприятии изменена");
            return RedirectToAction("Index", new { id = model.EventId });
        }

        [Authorize]
        public IActionResult SubscribeToEvent(int EventId)
        {
            var user = _userService.GetByFilter(u => u.Email == User.Identity.Name);
            //Постараться убрать в другое место все вспомогательные вещи
            var newModel = _context.Users.Where(e => e.UserId == user.UserId)
                .Include(c => c.SubscribedEvents).FirstOrDefault();
            if (user.SubscribedEvents.Select(e => e.EventId).Contains(EventId))
            {
                TempDataMessage("message", "primary", $"Вы уже участвуете в этом мероприятии");
                return RedirectToAction("Index", new { id = EventId });
            }
            var model = _eventService.GetById(EventId);
            _eventService.AddNewParticipant(model, user); //или можно использовать _userService
            TempDataMessage("message", "success", $"Вы добавлены к списку участников мероприятия");
            return RedirectToAction("Index", new { id = EventId });
        }

        [Authorize]
        public IActionResult ExitEvent(int EventId)
        {
            var user = _userService.GetByFilter(u => u.Email == User.Identity.Name);
            //Постараться убрать всё в другое место, например в:
            //_eventService.DeleteParticipant(model, user);
            var selectedEvent = _context.Events.Where(e => e.EventId == EventId)
                .Include(c => c.Participants).FirstOrDefault();
            foreach (var participant in selectedEvent.Participants)
            {
                if (participant.User == user)
                {
                    selectedEvent.Participants.Remove(participant);
                    break;
                }
            }
            _context.SaveChanges();
            TempDataMessage("message", "primary", $"Вы отказались от участия в этом мероприятии");
            return RedirectToAction("Index", new { id = EventId });
        }

        [Authorize]
        public IActionResult DeleteEvent(int EventId)
        {
            var model = _eventService.GetById(EventId);
            _eventService.DeleteEvent(model);
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