using AutoMapper;
using Identity.API.Contracts.Requests;
using Identity.API.Contracts.Responses;
using Identity.API.Helpers;
using Identity.Domain.Models;
using Identity.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [Route("api/identities")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> logger;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly AppSettings appSettings;
        public IdentityController(ILogger<IdentityController> logger, IMapper mapper, IUserService userService, IOptions<AppSettings> appSettings)
        {
            this.logger = logger;
            this.userService = userService;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult<UserResponse>> Post([FromBody] UserCreateRequest userCreateRequest)
        {
            User entity = new User()
            {
                Email = userCreateRequest.Email
            };
            await userService.AddUser(entity, userCreateRequest.Password);
            UserResponse response = mapper.Map<UserResponse>(entity);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest loginRequest)
        {
            var user = await userService.Authenticate(loginRequest.Email, loginRequest.Password);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            LoginResponse loginResponse = new LoginResponse()
            {
                Id = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            };

            return Ok(loginResponse);
        }
    }
}
