using Domain.Shared.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}
