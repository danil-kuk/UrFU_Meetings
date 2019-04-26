using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.DataModels.Entities
{
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Введите название мероприятия")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Введите описание мероприятия")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Введите дату проведения мероприятия")]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Введите время начала мероприятия")]
        public DateTime Time { get; set; }

        [Required(ErrorMessage = "Укажите, где будет проходить мероприятие")]
        public string Place { get; set; }
        
        public virtual ICollection<EventParticipant> Participants { get; set; } = new HashSet<EventParticipant>();

        [Required(ErrorMessage = "Укажите Id организатора мероприятия")]
        public int OrganizerId { get; set; }

        [Required(ErrorMessage = "Укажите свои контакты")]
        public string Contacts { get; set; }
        
        public int? MaxParticipants { get; set; }
    }
}
