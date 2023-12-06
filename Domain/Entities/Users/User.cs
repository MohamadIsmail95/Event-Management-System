using Domain.Dtos.User;
using Domain.Entities.Events;
using Domain.Entities.Roles;
using Domain.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class User: BaseEntity
    {
        [Required]
        public string Fname { get; set; }
        [Required]
        public string Lname { get; set; }
        [Required]
        public string FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<EventBook> EventBooks { get;set; }
        public User() { }
        
        public User(CreateUser user)
        {
            
            Fname= user.Fname;
            Lname= user.Lname;
            FullName= user.FullName;
            Email= user.Email;
            Password= user.Password;
            IsActive = true;
            IsDeleted = false;
            CreatedAt=DateTime.Now;
        }



       
    }
}
