using Domain.Entities.Users;
using Domain.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Events
{
    public class EventBook:BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [ForeignKey("Event")]
        public Guid EventId { get; set;}
        [Required,Range(1,3,ErrorMessage ="You can book ticket number between 1 and 3")]
        public int NumberOfTicket { get; set; }

        public virtual User User { get; set; }
        public virtual Event Event { get; set; }

        public EventBook() { }
        public EventBook(Guid userId, Guid eventId, int numberOfTicket)
        {
            UserId=userId;
            EventId=eventId;
            NumberOfTicket=numberOfTicket;
        }
    }
}
