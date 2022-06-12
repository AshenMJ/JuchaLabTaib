using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }


        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] AuthorizationDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid client request");
            }

            if (this.authorizationService.Login(dto) >= 0)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dto.Login),
                    new Claim(ClaimTypes.Role, dto.Login.Contains("admin") ? "admin":"user")
                };
                var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("passworD@123"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:12124",
                    audience: "http://localhost:12124",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                    );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }

        }
  


    }
}
