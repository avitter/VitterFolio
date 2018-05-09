using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vitter.Core.Security;

namespace VitterFolio.Api.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;
        //private readonly IUserManager _userManager;

        public TokenController(IConfiguration configuration)
        {
            _config = configuration;
            //_userManager = userManager;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User user)
        {
            //var user = _userManager.FindByEmail(model.UserName);

            if (user != null)
            {
                var checkPwd = true;
                if (checkPwd)
                {
                    var claims = new[]
                    {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Birthdate, "12/05/1979"),
                            new Claim(JwtRegisteredClaimNames.Jti,"5")
                        };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }

            return BadRequest("Could not create token");
        }
    }
}
