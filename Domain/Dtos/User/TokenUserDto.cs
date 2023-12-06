using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.User
{
    public class TokenUserDto
    {
        public string userName { get; set; }
        public string refreshToken { get; set; }
        public DateTime tokenCreated { get; set; }
        public DateTime tokenExpired { get; set; }
    }
}
