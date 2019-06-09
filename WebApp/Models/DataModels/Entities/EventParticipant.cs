using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.DataModels.Entities
{
    public class EventParticipant
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        
        public User User { get; set; }
        public Event Event { get; set; }
    }
}
