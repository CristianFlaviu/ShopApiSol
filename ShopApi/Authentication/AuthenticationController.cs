using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopApi.Core;
using ShopApi.Dto;
using System;
using System.Threading.Tasks;

namespace ShopApi.Authentication
{
    [AllowAnonymous]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {

        private readonly AuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;


        public AuthenticationController(AuthenticationService authenticationService,
                                         ILogger<AuthenticationController> logger
                                     )
        {
            _authenticationService = authenticationService;
            _logger = logger;
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
        public async Task<CommandResult<bool>> ConfirmEmail([FromBody] ConfirmationEmail modelDto)
        {
            return await _authenticationService.ConfirmEmail(modelDto);
        }
    }
}
