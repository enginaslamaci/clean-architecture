using CleanArch.Application.DTOs.Account.Request;
using CleanArch.Application.DTOs.Account.Response;
using CleanArch.Domain.Common.Results;

namespace CleanArch.Application.Abstractions.Persistence.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Result<AuthenticationResponse>> RefreshTokenAsync(string token, string ipAddress);
    }
}
