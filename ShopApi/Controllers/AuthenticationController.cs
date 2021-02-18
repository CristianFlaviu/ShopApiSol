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
using MimeKit;
using ShopApi.Constants;
using ShopApi.Email;

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
        private readonly EmailSender _emailSender;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            EmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _emailSender = emailSender;

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
                    var appUser =
                        _userManager.Users.ToList().SingleOrDefault(r => r.Email == loginDto.Email) as BaseUser;


                    return Ok();
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

            var user2 = await _userManager.FindByEmailAsync(model.Email);
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return BadRequest("Email Already Exists");
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

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.Typ, model.Role),
                };
                await _userManager.AddClaimsAsync(user, claims);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var bodyMessage = String.Format(StringFormatTemplates.EmailMessageBody, model.FirstName + model.LastName, code);
                var mailSentResult = await _emailSender.SendMailAsync(bodyMessage, "Confirm Email ShopOnline", model.UserName, model.Email);

                return Ok("Successfully Registered");
            }

            return BadRequest(resultAddUSer.Errors);
        }


    }
}
