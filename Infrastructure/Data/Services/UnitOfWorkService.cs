using AutoMapper;
using ClinicSystem.Services.IServices;
using Domain.Interfaces;
using Domain.Shared.Abstractions;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Services
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        IEventService _eventService;
        IPasswordCryption _passwordCryption;
        IMapper _mapper;
        IUnitOfWork _unitOfWork ;
        IHttpContextAccessor _httpContextAccessor;
        IConfiguration _configuration;
        AppDbContext _dbContext;
        IjwtService _ijwtService;

        public UnitOfWorkService(IMapper mapper, IUnitOfWork unitOfWork, IPasswordCryption passwordCryption, IjwtService _jwtService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordCryption = passwordCryption;
            _ijwtService= _jwtService;
        }

       
        public IEventService EventService
        {

            get
            {
                if (_eventService == null)
                    _eventService = new EventService(_unitOfWork.EventRepository, _mapper, _ijwtService);

                return _eventService;
            }
        }

    }
}
