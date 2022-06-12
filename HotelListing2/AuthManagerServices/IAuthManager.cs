using HotelListing2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing2.AuthManagerServices
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(UserLoginDTO userDTO);

        Task<string> CreateToken();
    }
}
