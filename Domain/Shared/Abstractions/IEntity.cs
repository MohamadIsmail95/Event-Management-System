using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared.Abstractions
{
    public interface IEntity
    {
    }
    public interface IEntityKey<out TKey> : IEntity where TKey : notnull
    {
        TKey Id { get; }
    }
}
