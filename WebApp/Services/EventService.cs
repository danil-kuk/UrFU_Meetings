using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class EventService : IEventService
    {
        private readonly IDataBaseService<Event> _databaseEvent;

        public EventService(IDataBaseService<Event> databaseEvent)
        {
            _databaseEvent = databaseEvent;
        }

        public void DeleteEvent(Event selectedEvent)
        {
            _databaseEvent.Remove(selectedEvent);
        }

        public Event GetByFilter(Expression<Func<Event, bool>> filter)
        {
            return _databaseEvent.GetByFilter(filter);
        }

        public Event GetById(int id)
        {
            return _databaseEvent.GetById(id);
        }

        public void InsertEvent(Event selectedEvent)
        {
            _databaseEvent.Insert(selectedEvent);
        }

        public void AddNewParticipant(Event selectedEvent, EventParticipant eventParticipant)
        {
            selectedEvent.Participants.Add(eventParticipant);
            UpdateEvent(selectedEvent);
        }

        public void UpdateEvent(Event selectedEvent)
        {
            _databaseEvent.Update(selectedEvent);
        }
    }
}
