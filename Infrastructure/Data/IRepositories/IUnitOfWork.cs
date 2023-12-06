using ClinicSystem.Services.IServices;
using Infrastructure.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IEventRepository EventRepository { get; }
        Task<int> SaveChangesAsync();
        IAsyncRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : class;
    }
}
