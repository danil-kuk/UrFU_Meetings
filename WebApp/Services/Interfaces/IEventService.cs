using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;

namespace WebApp.Services.Interfaces
{
    public interface IEventService
    {
        Event GetById(int id);
        Event GetByFilter(Expression<Func<Event, bool>> filter);
        void InsertEvent(Event selectedEvent);
        void UpdateEvent(Event selectedEvent);
        void DeleteEvent(Event selectedEvent);
        void AddNewParticipant(Event selectedEvent, User userToAdd);
    }
}
