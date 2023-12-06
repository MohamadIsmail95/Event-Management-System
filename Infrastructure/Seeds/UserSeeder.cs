using Domain.Entities.Roles;
using Domain.Entities.Users;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeds
{
    public class UserSeeder
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordCryption _passwordCryption;

        public UserSeeder(AppDbContext db, IPasswordCryption passwordCryption)
        {
            _dbContext = db;
            _passwordCryption = passwordCryption;
        }

        public void Seed()
        {
            if (!_dbContext.Roles.Any())
            {
                var roles = new List<Role>()
                {
                    new Role()
                        {
                            Name="Admin",
                        },
                       new Role()
                        {
                            Name="Normal",
                        }
                };

                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }

            if (!_dbContext.Users.Any())
            {
                var adminRole=_dbContext.Roles.FirstOrDefault(x=>x.Name=="Admin");
                var user = new User()
                {
                    Fname = "Admin",
                    Lname = "Admin",
                    FullName = "Admin",
                    Email = "Admin@gmail.com",
                    Password = _passwordCryption.EncodePasswordToBase64("Admin123#"),
                    IsActive = true,
                    IsDeleted=false,
                    CreatedAt=DateTime.Now
                };
                user.UserRoles = new List<UserRole>() { new UserRole(user.Id, adminRole.Id)};
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }

        }

    }
}
