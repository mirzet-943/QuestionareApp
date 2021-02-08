using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsAppData;
using NewsAppData.Models;
using NewsAppData.ViewModels;
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
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private IRepository _repository;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IRepository repository)
        {
            _appSettings = appSettings.Value;
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
            await Task.Run(async () =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserID.ToString()),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);
                await _repository.UpdateAsync<User>(user);
            });
            return new User()
            {
                FullName  = user.FullName,
                Role = user.Role,
                UserID = user.UserID,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                Token = user.Token
            };
        }

    }
}
