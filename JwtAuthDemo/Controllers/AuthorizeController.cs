using JwtAuthDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthorizeController : Controller
    {

        public AuthorizeController(ILogger<AuthorizeController> logger, IOptions<JwtSettings> options)
        {
            _logger = logger;
            _jwtSettings = options.Value;
        }

        private readonly ILogger<AuthorizeController> _logger;
        private JwtSettings _jwtSettings { get; }

        //[HttpGet]
        //[HttpPost]
        public IActionResult Token(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            if(!(model.User == "jim" && model.Password == "123456"))
            {
                return BadRequest();
            }
            var claims = new System.Security.Claims.Claim[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "jim"),
                // Role 授权
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "user"),
                // Claim 授权
                //new System.Security.Claims.Claim("SuperAdminOnly", "true"),
            };
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(_jwtSettings.Issuser, 
                _jwtSettings.Audience, 
                claims, 
                DateTime.Now, 
                DateTime.Now.AddMinutes(30), 
                creds);

            return Ok(new { token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
