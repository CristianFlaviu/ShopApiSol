using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core;
using ShopApi.Core.Email;
using ShopApi.Dto;
using System.Threading.Tasks;

namespace ShopApi.Authentication
{
    [AllowAnonymous]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailSender _emailSender;
        private readonly AuthenticationService _authenticationService;


        public AuthenticationController(UserManager<IdentityUser> userManager,
                                         RoleManager<IdentityRole> roleManager,
                                        EmailSender emailSender,
                                         AuthenticationService authenticationService
                                     )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            var response = await _authenticationService.Login(loginDto.Username, loginDto.Password);

            if (response.AccessToken != null)
            {
                return Ok(new { access_token = response.AccessToken, refresh_token = response.RefreshToken });
            }

            return Unauthorized(response.ErrorMessage);
        }

        [HttpPost("register")]
        public async Task<CommandResult<bool>> RegisterCaregiver([FromBody] UserRegisterDto model)
        {
            return await _authenticationService.Register(model);
        }

        [HttpPost("confirm-email")]
        public async Task<CommandResult<bool>> ConfirmEmail([FromBody] ConfirmationEmailDto modelDto)
        {
            return await _authenticationService.ConfirmEmail(modelDto);
        }
    }
}
