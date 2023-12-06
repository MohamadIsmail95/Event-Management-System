using AutoMapper;
using Domain.Entities.Events;
using Domain.Interfaces;
using Infrastructure.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class EventRepository : RepositoryBase<Event, Guid>, IEventRepository
    {
        IMapper _mapper;
        AppDbContext _dbContext;
        IjwtService _jwtService;

        public EventRepository(AppDbContext dbContext, IMapper mapper, IjwtService jwtService) : base(dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _jwtService = jwtService;
        }
       
    }
}
