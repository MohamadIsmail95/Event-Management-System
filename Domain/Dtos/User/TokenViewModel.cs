using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.User
{
    public class TokenViewModel
    {
        public string token { get; set; }
        public DateTime expiredDate { get; set; }
        public string refreshToken { get;set; }
    }
}
