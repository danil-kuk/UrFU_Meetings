using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Models.DataModels;
using WebApp.Models.DataModels.Entities;
using WebApp.Services;
using Xunit;

namespace WebAppTests
{
    public class EventServiceTests
    {
        private readonly DataBaseService<Event> _eventDatabaseService;
        private readonly EventService _eventService;
        private readonly EFDBContext _context;
        private readonly Event testEvent;
        private readonly Random random;

        public EventServiceTests()
        {
            var options = new DbContextOptionsBuilder<EFDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new EFDBContext(options);
            random = new Random();
            _eventDatabaseService = new DataBaseService<Event>(_context);
            _eventService = new EventService(_eventDatabaseService);
            testEvent = new Event
            {
                EventName = "TestEvent",
                Description = "TestDescription",
                EventTheme = EventTheme.Кино,
                Date = DateTime.Today.Date,
                Time = DateTime.Now.AddHours(10),
                Place = "TestPlace",
                Contacts = "TestContacts"
            };
            CreateEventsDatabase();
        }

        private void CreateEventsDatabase()
        {
            if (_context.Events.Count() >= 9)
                return;
            for (int i = 1; i < 10; i++)
            {
                var newTestEvent = new Event
                {
                    EventName = "TestEvent" + i,
                    Description = "TestDescription" + i,
                    EventTheme = (EventTheme)i,
                    Date = DateTime.Today.Date,
                    Time = DateTime.Now.AddHours(10),
                    Place = "TestPlace" + i,
                    Contacts = "TestContacts" + i
                };
                _eventDatabaseService.Insert(newTestEvent);
            }
        }

        private Event GetRandomTestEvent()
        {
            var id = random.Next(1, 10);
            return new Event
            {
                EventName = "TestEvent" + id,
                Description = "TestDescription" + id,
                EventTheme = (EventTheme)id,
                Date = DateTime.Today.Date,
                Time = DateTime.Now.AddHours(10),
                Place = "TestPlace" + id,
                Contacts = "TestContacts" + id
            };
        }

        private void CheckEventData(Event expectedEvent, Event actualEvent)
        {
            Assert.Equal(expectedEvent.EventName, actualEvent.EventName);
            Assert.Equal(expectedEvent.Description, actualEvent.Description);
            Assert.Equal(expectedEvent.EventTheme, actualEvent.EventTheme);
            Assert.Equal(expectedEvent.Date.ToShortDateString(), actualEvent.Date.ToShortDateString());
            Assert.Equal(expectedEvent.Time.ToShortTimeString(), actualEvent.Time.ToShortTimeString());
            Assert.Equal(expectedEvent.Place, actualEvent.Place);
            Assert.Equal(expectedEvent.MaxParticipants, actualEvent.MaxParticipants);
            Assert.Equal(expectedEvent.OrganizerId, actualEvent.OrganizerId);
        }

        [Fact]
        public void InsertNewTestEvent()
        {
            //Arrange
            var eventToInsert = testEvent;
            var startDbRecordsCount = _context.Events.Count();

            //Act
            _eventService.InsertEvent(testEvent);

            //Assert
            var actualRes = _context.Events.Count();
            var expectedRes = startDbRecordsCount + 1;
            Assert.Equal(expectedRes, actualRes);
            Assert.Contains(eventToInsert, _eventDatabaseService.GetAll());
        }

        [Fact]
        public void DeleteLastEvent()
        {
            var startDbRecordsCount = _context.Events.Count();
            var eventToDelete = _eventDatabaseService.GetAll().LastOrDefault();
            
            _eventService.DeleteEvent(eventToDelete);
            
            var actualRes = _context.Events.Count();
            var expectedRes = startDbRecordsCount - 1;
            Assert.Equal(expectedRes, actualRes);
            Assert.DoesNotContain(eventToDelete, _eventDatabaseService.GetAll());
        }

        [Fact]
        public void DeleteTestEvent()
        {
            _eventDatabaseService.Insert(testEvent);
            var startDbRecordsCount = _context.Events.Count();
            var eventToDelete = testEvent;
            
            _eventService.DeleteEvent(eventToDelete);
            
            var actualRes = _context.Events.Count();
            var expectedRes = startDbRecordsCount - 1;
            Assert.Equal(expectedRes, actualRes);
            Assert.DoesNotContain(eventToDelete, _eventDatabaseService.GetAll());
        }

        [Fact]
        public void FindByIdTestEvent()
        {
            var id = random.Next(1, 10);
            var actualEvent = _eventService.GetById(id);
            var expectedEvent = new Event
            {
                EventName = "TestEvent" + id,
                Description = "TestDescription" + id,
                EventTheme = (EventTheme)id,
                Date = DateTime.Today.Date,
                Time = DateTime.Now.AddHours(10),
                Place = "TestPlace" + id,
                Contacts = "TestContacts" + id
            };

            CheckEventData(expectedEvent, actualEvent);
        }

        [Fact]
        public void FindByEventNameTestEvent()
        {
            var expectedEvent = GetRandomTestEvent();
            var actualEvent = _eventService.GetByFilter(e => e.EventName == expectedEvent.EventName);

            CheckEventData(expectedEvent, actualEvent);
        }

        [Fact]
        public void UpdateTestEvent()
        {
            var rnd = random.Next(1, 10);
            var newEvent = new Event
            {
                EventName = rnd + "TestEvent",
                Description = rnd + "TestDescription",
                EventTheme = (EventTheme)rnd,
                Date = DateTime.Today.Date,
                Time = DateTime.Now.AddHours(10),
                Place = rnd + "TestPlace",
                Contacts = rnd + "TestContacts"
            };
            _eventDatabaseService.Insert(newEvent);

            newEvent.EventName += rnd;
            newEvent.Description += rnd;
            newEvent.Place += rnd;
            newEvent.Contacts += rnd;
            _eventService.UpdateEvent(newEvent);

            var actualRes = _context.Events.Where(e => e.EventName == (rnd + testEvent.EventName + rnd)).FirstOrDefault();
            var expectedRes = newEvent;
            CheckEventData(expectedRes, actualRes);
        }
    }
}
