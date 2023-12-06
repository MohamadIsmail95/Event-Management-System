using AutoMapper;
using ClinicSystem.Services;
using ClinicSystem.Services.IServices;
using Domain.Interfaces;
using Infrastructure.Data.IRepositories;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        IEventRepository _EventRepository { get; set; }
        IMapper _mapper;
        IjwtService _jwtService;
        public UnitOfWork(AppDbContext dbContext, IMapper mapper, IjwtService jwtService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _jwtService = jwtService;
        }
        
   
        public IEventRepository EventRepository
        {
            get
            {
                if (_EventRepository == null)
                    _EventRepository = new EventRepository(_dbContext, _mapper,_jwtService);

                return _EventRepository;
            }
        }
        public IAsyncRepository<TEntity,TKey> Repository<TEntity, TKey>() where TEntity : class
        {
            return new RepositoryBase<TEntity, TKey>(_dbContext);
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
