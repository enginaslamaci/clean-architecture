using CleanArch.Domain.Entities.Identity;

namespace CleanArch.Application.Abstractions.Infrastructure.Services.Token
{
    public interface ITokenHandler
    {
        Task<string> CreateAccessToken(ApplicationUser appUser);
        Task<RefreshToken> CreateRefreshToken(string ipAddress);
    }
}
