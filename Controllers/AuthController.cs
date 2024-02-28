using Backend.Configuration;
using Backend.Models;
using Backend.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Backend.Repository.Auth;
using Backend.Models.Request;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IAuth _auth;
        public AuthController(IOptionsMonitor<JwtConfig> optionsMonitor,IAuth auth)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _auth = auth;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]Login login )
        {
            var flag = await _auth.Login(login.Email, login.Password);

            var jwttoken = GenerateJwtToken(login.Email);
            if (flag)
            {
                
                return Ok(new RegResponse
                {
                    Success = true,
                    Token = jwttoken

                });
            }
            else return BadRequest(new RegResponse { Success = false });
        }


        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] User user)
        {
            var  flag = await _auth.Registration(user);
            
                if (flag)
                {
                    var jwttoken = GenerateJwtToken(user.Email);
                    return Ok(new RegResponse
                    {
                        Success = true,
                        Token = jwttoken

                    });
                }
                else return BadRequest(new RegResponse { Success = false });
            
        }






        private string GenerateJwtToken(string email)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
               
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Sub,email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new 
                    SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
            

        }

        
    }
}

