using Domain.Dtos.Event;
using Domain.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Events
{
    public class Event:BaseEntity
    {
        public string Name { get;set; }
        public string Description { get;set; }
        public string Location { get; set; }
        public DateTime PresentDate { get; set; }
        public int AvailableTikect { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }=false;

        public virtual ICollection<EventBook> EventBooks { get; set; }

        
    }
}

