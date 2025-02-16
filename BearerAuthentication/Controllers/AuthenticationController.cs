using BearerAuthentication.Data;
using BearerAuthentication.DTOs.RequestDtos;
using BearerAuthentication.JwtOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BearerAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly Jwt _jwtOptions;
        private readonly DBcontext _dbcontext;
        public AuthenticationController(DBcontext dbcontext, Jwt jwtOptions)
        {
            _dbcontext = dbcontext;
            _jwtOptions = jwtOptions;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegesterAsync([FromBody] RegesterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var existingUser = await _dbcontext.Users
                .Where(user => user.Username == dto.Username || user.Email == dto.Email)
                .ToListAsync();

            if (existingUser.Any())
            {
                if (existingUser.Any(user => user.Username == dto.Username))
                {
                    return BadRequest("Username already exists.");
                }

                if (existingUser.Any(user => user.Email == dto.Email))
                {
                    return BadRequest("Email already registered.");
                }
            }

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                Name = dto.Name,
            };
            await _dbcontext.Users.AddAsync(newUser);
            await _dbcontext.SaveChangesAsync();

            var toketHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audiense,
               SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),SecurityAlgorithms.HmacSha256),


                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, newUser.Username),
                    new Claim(ClaimTypes.Email, newUser.Email),
                    new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.Lifetime)
            };
            var securityKey = toketHandler.CreateToken(tokenDescriptor);
            var accessToken = toketHandler.WriteToken(securityKey);

            return Ok(accessToken);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }
            var user = await _dbcontext.Users
                .Where(user => user.Username == dto.Username && user.Password == dto.Password)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Invalid credentials.");
            }

            
           

            return Ok();
            
        }

    }
}
