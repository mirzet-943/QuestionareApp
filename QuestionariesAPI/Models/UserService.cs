using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuestionariesAppData;
using QuestionariesAppData.Models;
using QuestionariesAppData.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsAPI.Models
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> WriteToken(User user);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly IConfigurationSection _jwtSettings;
        private IRepository _repository;

        public UserService(IConfiguration configuration, IRepository repository)
        {
            _jwtSettings = configuration.GetSection("JwtSettings");
            _repository = repository;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = (await _repository.Find<User>(s => s.Username == username)).FirstOrDefault();

            if (user == null)
                return null;
            bool verified = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!verified)
                return null;
            return await WriteToken(user);
            
        }

        public async Task<User> WriteToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, user.UserID.ToString()),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.GetSection("validIssuer").Value,
                Audience = _jwtSettings.GetSection("validAudience").Value
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            await _repository.UpdateAsync<User>(user);
            return new User()
            {
                FullName = user.FullName,
                Role = user.Role,
                UserID = user.UserID,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                Token = user.Token
            };
        }
    }
}
