using ClinicSystem.Dtos;
using Domain.Dtos.Event;
using Domain.Entities.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEventService: ICrudService<Event, EventDto, Guid, CreateEvent, UpdateEvent>
    {
        Task<ApiResponse<BookDto>> Booking(Guid userId, Guid eventId, int numOfTicket);
        ApiResponse<UserTicket> GetUserTickets(Guid userId);
        ApiResponse<UserTicket> CancelTickets(Guid userId, Guid eventId);

    }
}
