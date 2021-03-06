using HotelListing2.Data;
using HotelListing2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing2.AuthManagerServices
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;
        private readonly ILogger<AuthManager> _logger;

        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration, ILogger<AuthManager> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var expiration = DateTime.Now.AddHours(
                Convert.ToDouble(jwtSettings.GetSection("lifetime").Value));
            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                audience: jwtSettings.GetSection("Audience").Value, 
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );
            
            return token;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //_logger.LogInformation("................... key2: " + secret);
            //_logger.LogInformation("................... 333333333333333333333333: + " + key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName )
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        
        }

        public async Task<bool> ValidateUser(UserLoginDTO userDTO)
        {
            _user = await _userManager.FindByNameAsync(userDTO.Email);
            
            var isExist = _user != null;
            var isAuth = await _userManager.CheckPasswordAsync(_user, userDTO.Password);

            var isValid = isExist && isAuth;
            
            return isValid;
        }
    }
}
