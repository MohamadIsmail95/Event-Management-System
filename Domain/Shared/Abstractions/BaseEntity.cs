using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared.Abstractions
{
    public abstract class BaseEntity : IEntityKey<Guid>
    {
        public Guid Id { get;  init; } = Guid.NewGuid();


    }
}
