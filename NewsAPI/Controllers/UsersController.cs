using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsAPI.Models;
using NewsAPI.Models.Pagging;
using NewsAppData;
using NewsAppData.ViewModels;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IRepository _repository;
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, IRepository repository, IUserService userService)
        {
            _logger = logger;
            _repository = repository;
            _userService = userService;
        }


        [HttpGet, Authorize(Policy = "AdminOnly")]
        public async Task<PagedList<UserVM>> Get([FromQuery] int PageNumber, string searchTerm)
        {
            if (searchTerm == null)
                searchTerm = "";
            var result =  await _repository.GetAllAsync<User>(new Models.Parameters.PageParameters() { PageNumber = PageNumber }, s => s.Role == "Writer" && (s.Username.Contains(searchTerm) || s.FullName.Contains(searchTerm)), s => s.CreatedAt);
            var resultVM = new PagedList<UserVM>(result.Items.Select(s => new UserVM()
            {
                FullName = s.FullName,
                Role = s.Role,  
                UserID = s.UserID,
                Username = s.Username,
                CreatedAt = s.CreatedAt
            }).ToList(), result.TotalCount, result.CurrentPage, result.PageSize);

            return resultVM;
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

        [HttpPost,Route("register"), Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid object");

            if ((await _repository.Find<User>(s => s.Equals(user))).FirstOrDefault() == null)
            {
                user.Role = "Writer";
                user.CreatedAt = DateTime.Now;
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _repository.CreateAsync<User>(user);
                return Ok();
            }
            return StatusCode(403, "User already exists");
        }


        [HttpPut, Route("edit/{userid}"), Authorize(Policy = "Writer")]
        public async Task<IActionResult> UpdateUser([FromRoute] int userid,[FromBody] User modifiedUser)
        {
            var claimId = User.Claims.Where(s => s.Type == ClaimTypes.Name).FirstOrDefault();
            if (!ModelState.IsValid)
                return BadRequest("Invalid object");
            var user = await _repository.SelectById<User>(userid);
            
            if (user == null)
            {
                return BadRequest("User does not exist");
            }
            if (modifiedUser.Password.Length < 6)
                return BadRequest("Password invalid");
            if (user.UserID != Convert.ToInt32(claimId.Value) && User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, "Writer"))
                return Unauthorized();
            user.Password = BCrypt.Net.BCrypt.HashPassword(modifiedUser.Password);
            user.FullName = modifiedUser.FullName;
            user.Username = modifiedUser.Username;
            await _repository.UpdateAsync<User>(user);
            return Ok();
        }


        [HttpDelete, Route("delete/{userid}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] int userid)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid object");
            var user = await _repository.SelectById<User>(userid);
            if (user == null)
            {
                return BadRequest("User does not exist");
            }
            await _repository.DeleteAsync<User>(user);
            return Ok();
        }


    }
}
