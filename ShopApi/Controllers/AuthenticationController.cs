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
    [Route("auth")]
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
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {

            if (_userManager.FindByNameAsync(loginDto.Username).Result is BaseUser user)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

                if (result.Succeeded)
                {
                    var loggedInUser = await _userManager.FindByNameAsync(loginDto.Username);

                    var claims = await _userManager.GetClaimsAsync(loggedInUser);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddSeconds(Convert.ToDouble(20));


                    var token = new JwtSecurityToken(
                        _jwtConfig.Issuer,
                        _jwtConfig.Audience,
                        claims,
                        expires: expires,
                        signingCredentials: credentials
                    );

                    var finalToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { access_token = finalToken, refresh_token = "ceva", some = "some" });
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

            string defaultRole = "defaultRole";

            if ((!await _roleManager.RoleExistsAsync(defaultRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(defaultRole));

            }
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return BadRequest("Email Already Exists");
            }

            var user = new BaseUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var resultAddUSer = await _userManager.CreateAsync(user, model.Password);

            if (resultAddUSer.Succeeded)
            {
                var resultAddUserToRole = await _userManager.AddToRoleAsync(user, defaultRole);

                if (resultAddUserToRole.Succeeded == false)
                {
                    await _userManager.DeleteAsync(user);
                    return BadRequest(resultAddUserToRole.Errors);
                }

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.Typ, defaultRole),
                };
                await _userManager.AddClaimsAsync(user, claims);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var codeEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                ;

                var bodyMessage = string.Format(StringFormatTemplates.EmailMessageBody, $"{model.LastName} {model.FirstName}", model.Email, codeEncoded);
                var mailSentResult = await _emailSender.SendMailAsync(bodyMessage, "Confirm Email ShopOnline", model.Email, model.Email);

                return Ok(new { messae = "Succesfully Registered" });
            }

            return BadRequest(resultAddUSer.Errors);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmationEmailDto modelDto)
        {
            var user = await _userManager.FindByEmailAsync(modelDto.Email);
            var tokenDecodedByte = WebEncoders.Base64UrlDecode(modelDto.Token);
            var tokenDecoded = System.Text.Encoding.UTF8.GetString(tokenDecodedByte);

            var result = await _userManager.ConfirmEmailAsync(user, tokenDecoded);

            if (result.Succeeded)
            {
                return Ok(new { message = $"Email Confirmed successfully" });
            }

            return BadRequest(result.Errors);

        }

        [HttpGet("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {

           await _emailSender.SendMailAsync("default message","I just wanna say hi", "Ioana", "buda_ioana2002@yahoo.com");
            return Ok();

            //var user = await _userManager.FindByIdAsync(id);

            //if (user != null)
            //{
            //    await _userManager.DeleteAsync(user);

            //    return Ok();
            //}

            //return BadRequest(new { message = "bad user Id" });
        }



    }
}
