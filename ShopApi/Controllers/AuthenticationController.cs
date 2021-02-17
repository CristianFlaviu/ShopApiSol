using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Dto;
using ShopApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager,
                                        SignInManager<IdentityUser> signInManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_userManager.FindByEmailAsync(loginDto.Email).Result is BaseUser user)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.ToList().SingleOrDefault(r => r.Email == loginDto.Email) as BaseUser;
                    var token = await GenerateJwtToken(loginDto.Email, appUser);

                    return Ok(new { token });
                }

                return BadRequest("Invalid login attempt!");
            }

            return NotFound("No such user could be found!");
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCaregiver([FromBody] UserRegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return BadRequest("Email Already Exists");
            }

            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return BadRequest("Username Already Exists");
            }

            if (await _roleManager.RoleExistsAsync(model.Role) == false)
            {
                return BadRequest("Invalid Role");
            }

            var user = new BasicUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Gender = model.Gender,
                Address = model.Address,
            };

            var resultAddUSer = await _userManager.CreateAsync(user, model.Password);

            if (resultAddUSer.Succeeded)
            {
                var resultAddUserToRole = await _userManager.AddToRoleAsync(user, model.Role);

                if (resultAddUserToRole.Succeeded == false)
                {
                    await _userManager.DeleteAsync(user);
                    return BadRequest(resultAddUserToRole.Errors);
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.Typ,model.Role),
                };

                await _userManager.AddClaimsAsync(user, claims);
                return Ok("Successfully Registered");
            }

            return BadRequest(resultAddUSer.Errors);
        }

        private async Task<object> GenerateJwtToken(string email, BaseUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, email),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task EnsureRolesAsync(string role)
        {
            var alreadyExists = await _roleManager.RoleExistsAsync(role);

            if (alreadyExists) return;

            await _roleManager.CreateAsync(new IdentityRole(role));
        }


    }
}
