using Domain.Entities.Events;
using Domain.Entities.Roles;
using Domain.Entities.Users;
using Infrastructure.Data.EntityConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }

        // DbSet area
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventBook> EventBooks { get; set; }

        //-----Fininsh

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Add Entities Configuration
            
            builder.ApplyConfiguration(new UserCfg());
            builder.ApplyConfiguration(new EventCfg());

        }
    }
}
