using CleanArch.API.Controllers.Base.v1;
using CleanArch.Application.Abstractions.Persistence.Services;
using CleanArch.Application.DTOs.Account.Request;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers.v1
{
    public class AccountController : Basev1ApiController
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _authService.AuthenticateAsync(request, GenerateIPAddress()));
        }

        //[HttpPost("external-authenticate")]
        //public async Task<IActionResult> ExternalAuthenticateAsync([FromBody]ExternalAuthDto request)
        //{
        //    var result = await _authService.ExternalAuthenticate(request);
        //    return Ok(result);
        //}

        /// <summary>
        /// can use google id_token validation for client
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto request)
        {
            var result = await _authService.ValidateGoogleToken(request);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegistrationRequest request)
        {
            var origin = $"{this.Request.Scheme}://{this.Request.Host}"; // Request.Headers["origin"];
            return Ok(await _authService.RegisterAsync(request, origin));
        }

        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOutAsync(SignOutRequest request)
        {
            return Ok(await _authService.SignOutAsync(HttpContext.User, request, GenerateIPAddress()));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            return Ok(await _authService.RefreshTokenAsync(request.RefreshToken, GenerateIPAddress()));
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _authService.ConfirmEmailAsync(userId, code));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _authService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            return Ok(await _authService.ResetPassword(model));
        }

        #region Private Helpers

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        #endregion

    }
}
