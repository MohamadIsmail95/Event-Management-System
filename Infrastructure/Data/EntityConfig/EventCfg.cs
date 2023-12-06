using Domain.Entities.Events;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityConfig
{
    public class EventCfg : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ConfigureBaseEntity();
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
