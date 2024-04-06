using CleanArch.Application.DTOs.Account.Request;
using CleanArch.Application.DTOs.Account.Response;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Entities.Identity;
using System.Security.Claims;

namespace CleanArch.Application.Abstractions.Persistence.Services
{
    public interface IUserService
    {
        IQueryable<ApplicationUser> GetAllQueryableUser();
        Task<List<ApplicationUser>> GetPagedUser(int pageNumber, int pageSize, string sortOn, string sortDir);
        Task<List<ApplicationUser>> GetPagedUserByRole(string role, int pageNumber, int pageSize, string sortOn, string sortDir);
        Task<ApplicationUser> GetUserById(string Id);
        Task<string[]> GetRolesByUserId(string userId);

        Task<Result<UserDetailsResponse>> GetUserProfile(ClaimsPrincipal principal);
        Task<Result<string>> UpdateUserProfile(ClaimsPrincipal principal, UpdateUserProfileRequest model);
        Task<Result<string>> ChangeUserPassword(ClaimsPrincipal principal, ChangeUserPasswordRequest model);
     
    }
}