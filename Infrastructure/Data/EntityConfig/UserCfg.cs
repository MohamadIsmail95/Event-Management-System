using Domain.Entities.Users;
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
    public class UserCfg: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ConfigureBaseEntity();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.FullName).IsUnique();
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
