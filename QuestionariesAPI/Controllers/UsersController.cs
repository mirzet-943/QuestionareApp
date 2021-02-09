using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsAPI.JWTFeatures;
using NewsAPI.JWTFeatures.Entities;
using NewsAPI.Models;
using NewsAPI.Models.Pagging;
using QuestionariesAppData;
using QuestionariesAppData.ViewModels;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IRepository _repository;
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly JwtHandler _jwtHandler;

        public UsersController(ILogger<UsersController> logger, IRepository repository, IUserService userService, JwtHandler jwtHandler)
        {
            _logger = logger;
            _repository = repository;
            _userService = userService;
            _jwtHandler = jwtHandler;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel userParam)
        {
            var user = await _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return Unauthorized("Username or password is incorrect");

            return Ok(user);
        }

        [HttpPost, Route("register"), Authorize(Policy = "Admin")]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid object");

            if ((await _repository.Find<User>(s => s.Equals(user))).FirstOrDefault() == null)
            {
                user.Role = "Candidate";
                user.CreatedAt = DateTime.Now;
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _repository.CreateAsync<User>(user);
                return Ok();
            }
            return StatusCode(403, "User already exists");
        }

        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuth externalAuth)
        {
            var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            var user = new User { Email = payload.Email, FullName = payload.Name, Role = "Admin", Username = payload.Email, CreatedAt = DateTime.Now };
            if ((await _repository.Find<User>(s => s.Username == user.Username)).FirstOrDefault() == null)
            {
                if (string.IsNullOrEmpty(externalAuth.Password))
                {
                    return Ok(new
                    {
                        isAuthSuccess = true,
                        isPasswordSetupNeeded = true
                    });
                }
                user.Password = externalAuth.Password;
                await _repository.CreateAsync<User>(user);
            }
            var userToken = await _userService.WriteToken(user);
            return Ok(new
            {
                user = new User
                {
                    Username = userToken.Username,
                    FullName = userToken.FullName,
                    Role = userToken.Role,
                    Token = userToken.Token
                },
                isAuthSuccess = true,
                isPasswordSetupNeeded = false
            });
        }
    }
}
