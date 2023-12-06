using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Event
{
    public class UserTicket
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<EventDto> Events { get; set; }
    }
}
