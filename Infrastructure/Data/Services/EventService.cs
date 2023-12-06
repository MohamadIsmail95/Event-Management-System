using AutoMapper;
using Azure;
using ClinicSystem.Dtos;
using ClinicSystem.Services.IServices;
using Domain.Dtos.Event;
using Domain.Entities.Events;
using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Shared.Abstractions;
using Infrastructure.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Services
{
    public class EventService: CurdService<Event, EventDto, Guid, CreateEvent, UpdateEvent>,
        IEventService
    {
        IMapper _mapper;
        IEventRepository _repository;
        IjwtService _jwtService;
        public EventService( IEventRepository repository, IMapper mapper, IjwtService jwtService) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<BookDto>> Booking(Guid userId,Guid eventId,int numOfTicket)
        {
            if(await IsAvailableTicket(eventId, numOfTicket))
            {
                var response= await _jwtService.EventBook(userId, eventId, numOfTicket);
                return new ApiResponse<BookDto>(response);
            }
            return new ApiResponse<BookDto>("There are not enough Tickets");
        }

        public ApiResponse<UserTicket> CancelTickets(Guid userId, Guid eventId)
        {
            _jwtService.CancelUserTicket(userId,eventId);
            var user = _jwtService.GetUserById(userId);
            var userTicket = _mapper.Map<UserTicket>(user);
            return  new  ApiResponse<UserTicket>(userTicket);
        }

        public ApiResponse<UserTicket> GetUserTickets(Guid userId)
        {
            var user = _jwtService.GetUserById(userId);
            var userTicket = _mapper.Map<UserTicket>(user);
            return  new ApiResponse<UserTicket>(userTicket);
        }

        private async Task<bool> IsAvailableTicket(Guid eventid,int ticketNumber)
        {
            var eventBalance =  await _repository.GetByIdAsync(eventid);
            if(eventBalance.AvailableTikect >= ticketNumber)
            {
                eventBalance.AvailableTikect = eventBalance.AvailableTikect - ticketNumber;
                _repository.UpdateAsync(eventBalance);
                return true;
            }
            return false;
        }


    }
}
