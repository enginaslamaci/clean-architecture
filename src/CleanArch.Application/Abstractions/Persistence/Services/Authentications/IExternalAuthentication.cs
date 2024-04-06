using CleanArch.Application.DTOs.Account.Request;
using CleanArch.Application.DTOs.Account.Response;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Common.Results.Abstracts;
using Microsoft.AspNetCore.Authentication;

namespace CleanArch.Application.Abstractions.Persistence.Services.Authentications
{
    public interface IExternalAuthentication
    {
        Task<Result<AuthenticationResponse>> ExternalAuthenticate(ExternalAuthDto request);
        Task<Result<AuthenticationResponse>> ValidateGoogleToken(GoogleLoginDto request);
    }
}
