using CleanArch.Application.Abstractions.Persistence.Services.Authentications;
using CleanArch.Application.DTOs.Account.Request;
using CleanArch.Domain.Common.Results;
using System.Security.Claims;

namespace CleanArch.Application.Abstractions.Persistence.Services
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
        Task<Result<string>> RegisterAsync(RegistrationRequest request, string origin);
        Task<Result<string>> ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Result<string>> ResetPassword(ResetPasswordRequest model);
        Task<Result<string>> ConfirmEmailAsync(string userId, string code);
        Task<Result<int>> SignOutAsync(ClaimsPrincipal principal, SignOutRequest request, string ip);
        Task<Result<bool>> RevokeToken(string token, string ipAddress);
    }
}
