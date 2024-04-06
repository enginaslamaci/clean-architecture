using CleanArch.Application.DTOs.Account.Response;
using CleanArch.Domain.Common.Results;
using CleanArch.WebApp.Extensions;
using CleanArch.WebApp.Services;
using CleanArch.WebApp.Services.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger, IAuthenticationSchemeProvider schemeProvider)
        {
            _logger = logger;
            _accountService = accountService;
            _schemeProvider = schemeProvider;
        }

        [HttpGet("login")]
        public async Task<ActionResult> Login()
        {
            var authenticationSchemes = await _schemeProvider.GetAllSchemesAsync();
            return View(authenticationSchemes);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(string userName, string password)
        {
            try
            {
                //var response = await _accountService.Authenticate(new() { Email = "basicuser@gmail.com", Password = "123456" });
                var response = await _accountService.Authenticate(new() { Email = userName, Password = password });
                if (response.Succeeded)
                {
                    SessionUser user = new SessionUser();
                    user.ID = response.Data.Id;
                    user.Token = response.Data.JWToken;
                    user.Role = response.Data.Roles.FirstOrDefault() ?? "";
                    SessionHelper.CurrentUser = user;
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Refit.ApiException ex)
            {
                var error = await ex.ErrorResult();
                _logger.LogError(error);
                this.ErrorNotify(error);
            }
            return View();
        }


        [HttpGet("external-login")]
        public async Task<IActionResult> ExternalLogin(string scheme = "")
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(ExternalLoginCallback), new { scheme = scheme })
            };
            return Challenge(props, scheme);
        }

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string scheme)
        {
            var result = await HttpContext.AuthenticateAsync(scheme);
            if (result?.Succeeded != true)
            {
                this.ErrorNotify("External authentication error");
                return RedirectToAction("login");
            }

            var idToken = result?.Properties?.GetTokenValue("id_token");
            var provider = result?.Ticket?.AuthenticationScheme;

            var response = new Result<AuthenticationResponse>();
            if (scheme == GoogleDefaults.AuthenticationScheme)
                response = await _accountService.GoogleLogin(new() { IdToken = idToken, Provider = provider });
           
            try
            {
                if (response.Succeeded)
                {
                    SessionUser user = new SessionUser();
                    user.ID = response.Data.Id;
                    user.Token = response.Data.JWToken;
                    user.Role = response.Data.Roles.FirstOrDefault() ?? "";
                    SessionHelper.CurrentUser = user;
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Refit.ApiException ex)
            {
                var error = await ex.ErrorResult();
                _logger.LogError(error);
                this.ErrorNotify(error);
            }

            _logger.LogError("Login faied");
            return RedirectToAction("login");
        }


        [HttpGet("unuthorized")]
        public async Task<ActionResult> Unauthorized()
        {
            return View();
        }

    }
}
