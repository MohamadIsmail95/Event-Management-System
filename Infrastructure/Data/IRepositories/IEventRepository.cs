using Domain.Entities.Events;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.IRepositories
{
    public interface IEventRepository : IAsyncRepository<Event, Guid>
    {
    }
}
