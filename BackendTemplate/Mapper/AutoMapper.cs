using AutoMapper;
using Domain.Dtos.Event;
using Domain.Dtos.User;
using Domain.Entities.Events;
using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Shared.Abstractions;
using static Domain.Shared.Enums;

namespace ClinicSystem.Mapper
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {

            //---------------------Event---------------------------
            CreateMap<Event, CreateEvent>().ReverseMap();
            CreateMap<Event, UpdateEvent>().ReverseMap();
            CreateMap<Event, EventDto>().ReverseMap();
            //---------------------User-----------------------
            CreateMap<User, CreateUser>().ReverseMap();
            CreateMap<User, UserTicket>()
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FullName))
               .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.EventBooks.Select(y=>new Event()
               {
                   Id=y.Event.Id,
                   Name=y.Event.Name,
                   Description=y.Event.Description,
                   Location=y.Event.Location,
                   PresentDate=y.Event.PresentDate,
                   AvailableTikect=y.Event.AvailableTikect,
                   CreatedAt=y.Event.CreatedAt,
                   CreatedBy=y.Event.CreatedBy,
                   IsActive=y.Event.IsActive
               }).ToList())).ReverseMap();

            
        }

    }
}
