﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Entities
{
    public class AppRole:IdentityRole<int>
    {
        public ICollection<AppUserRole> AppUserRoles { get; set; }

    }
}
