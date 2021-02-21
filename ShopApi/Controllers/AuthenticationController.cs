using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ShopApi.Constants;
using ShopApi.Dto;
using ShopApi.Email;
using ShopApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Config;

namespace ShopApi.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailSender _emailSender;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationController(UserManager<IdentityUser> userManager,
                                        SignInManager<IdentityUser> signInManager,
                                        RoleManager<IdentityRole> roleManager,
                                        EmailSender emailSender,
                                        IOptions<JwtConfig> jwtConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _jwtConfig = jwtConfig.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {

            if (_userManager.FindByEmailAsync(loginDto.Email).Result is BaseUser user)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

                if (result.Succeeded)
                {
                    var loggedInUser = await _userManager.FindByEmailAsync(loginDto.Email);

                    var claims = await _userManager.GetClaimsAsync(loggedInUser);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtConfig.ExpireDays));


                    var token = new JwtSecurityToken(
                        _jwtConfig.Issuer,
                        _jwtConfig.Audience,
                        claims,
                        expires: expires,
                        signingCredentials: credentials
                    );

                    var finalToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { token = finalToken });
            }

            return BadRequest("Invalid login attempt!");
        }

            return NotFound("No such user could be found!");
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterCaregiver([FromBody] UserRegisterDto model)
    {

        //await   _roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user1 = await _userManager.FindByNameAsync(model.UserName);
        if (await _userManager.FindByEmailAsync(model.UserName) != null)
        {
            return BadRequest("Username Already Exists");

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
            var codeEncoded = code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            ;

            var bodyMessage = string.Format(StringFormatTemplates.EmailMessageBody, $"{model.LastName} {model.FirstName}", model.UserName, codeEncoded);
            var mailSentResult = await _emailSender.SendMailAsync(bodyMessage, "Confirm Email ShopOnline", model.UserName, model.Email);

            return Ok("Successfully Registered");
        }

        return BadRequest(resultAddUSer.Errors);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        var tokenDecodedByte = WebEncoders.Base64UrlDecode(token);
        var tokenDecoded = System.Text.Encoding.UTF8.GetString(tokenDecodedByte);

        var result = await _userManager.ConfirmEmailAsync(user, tokenDecoded);

        if (result.Succeeded)
        {
            return Ok($"Email Confirmed successfully for user {username}");
        }

        return BadRequest(result.Errors);

    }


}
}
