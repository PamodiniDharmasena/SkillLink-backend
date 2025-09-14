using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkillLink.ApplicationServices.AuthModule;
using SkillLink.BusinessObjects.AuthModule;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkillLink.Controllers.AuthModule
{
    [ApiController]
    [Route("api/v1.0/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AuthApplicationService _authApplicationService;


        public AuthController(IConfiguration configuration, AuthApplicationService authApplicationService)
        {
            _configuration = configuration;
            _authApplicationService = authApplicationService;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {

            if (loginModel == null || string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Invalid login request.");
            }

            LoginResult loginResult = await _authApplicationService.CheckUserAuth(loginModel);

            if (loginResult.IsLoggedIn)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Convert.FromBase64String(_configuration["Jwt:Key"]);
                var signingKey = new SymmetricSecurityKey(key);
                var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, loginModel.Email),
                        new Claim("UserID", loginResult.UserId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = signingCredentials,
                    Issuer = _configuration["Jwt:Issuer"]
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }
    }
}
