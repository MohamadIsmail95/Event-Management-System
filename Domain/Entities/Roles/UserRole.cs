using Domain.Entities.Users;
using Domain.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Roles
{
    public class UserRole:BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }


        public UserRole() { }
        public UserRole(Guid userid,Guid roleid)
        {
            UserId = userid;
            RoleId=roleid;
        }
    }
}
