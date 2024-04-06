using CleanArch.Application.DTOs.Account.Request;
using CleanArch.Application.DTOs.Account.Response;
using CleanArch.Domain.Common.Results;
using Refit;

namespace CleanArch.WebApp.Services
{
    public interface IAccountService
    {
        [Post("/authenticate")]
        Task<Result<AuthenticationResponse>> Authenticate([Body] AuthenticationRequest request);

        [Post("/google-login")]
        Task<Result<AuthenticationResponse>> GoogleLogin([Body] GoogleLoginDto request);
    }
}
