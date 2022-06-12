using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing2.Data
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { set; get; }

        public string LastName { set; get; }
    }
}
