﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Models
{
    public class UserContext : IUserContext
    {
        public string Email { get;  set; }
        public string UserId { get;  set; }

    }
}
