using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.User
{
    public class CurrentUserViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
