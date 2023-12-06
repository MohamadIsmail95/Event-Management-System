using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Event
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EventName { get; set; }
        public int NumOfBookingTicket { get; set; }
    }
}
