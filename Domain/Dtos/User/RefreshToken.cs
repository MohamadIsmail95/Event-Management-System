﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.User
{
    public class RefreshToken
    {
        public string Token { get;set; }
        public DateTime Expired { get; set; }
        public DateTime Created { get; set; }

    }
}
