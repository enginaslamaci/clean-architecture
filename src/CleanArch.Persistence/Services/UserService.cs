using CleanArch.Application.Abstractions.Persistence.Services;
using CleanArch.Application.DTOs.Account.Request;
using CleanArch.Application.DTOs.Account.Response;
using CleanArch.Application.Exceptions;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Enums;
using CleanArch.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace CleanArch.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        #region Get
        public IQueryable<ApplicationUser> GetAllQueryableUser()
        {
            return _userManager.Users;
        }
        public async Task<List<ApplicationUser>> GetPagedUser(int pageNumber, int pageSize, string sortOn, string sortDir)
        {
            var query = _userManager.Users;

            if (!string.IsNullOrEmpty(sortOn))
            {
                if (sortDir == "Desc")
                    query = query.OrderByDescending(r => EF.Property<ApplicationUser>(r, sortOn));
                else
                    query = query.OrderBy(r => EF.Property<ApplicationUser>(r, sortOn));
            }

            return await query
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .AsNoTracking()
                 .ToListAsync();
        }
        public async Task<List<ApplicationUser>> GetPagedUserByRole(string role, int pageNumber, int pageSize, string sortOn, string sortDir)
        {
            var query = _userManager.Users.Include(u => u.Roles).ThenInclude(ur => ur.Role).AsQueryable();

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(r => r.Roles.Any(s => s.Role.Name == role));
            }

            if (!string.IsNullOrEmpty(sortOn))
            {
                if (sortDir == "Desc")
                    query = query.OrderByDescending(r => EF.Property<ApplicationUser>(r, sortOn));
                else
                    query = query.OrderBy(r => EF.Property<ApplicationUser>(r, sortOn));
            }

            return await query
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .AsNoTracking()
                 .ToListAsync();
        }
        public async Task<ApplicationUser> GetUserById(string Id)
        {
            return _userManager.Users.FirstOrDefault(u => u.Id == Id);
        }
        public async Task<string[]> GetRolesByUserId(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            return new string[] { };
        }
        public async Task<Result<UserDetailsResponse>> GetUserProfile(ClaimsPrincipal principal)
        {
            // Get user claims
            var user = await _userManager.GetUserAsync(principal);

            // If we have no user...
            if (user == null)
                // Return error
                throw new Exception("User not found");

            // Return token to user
            return await Result<UserDetailsResponse>.SuccessAsync(new UserDetailsResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber
            });
        }

        #endregion

        public async Task<Result<string>> UpdateUserProfile(ClaimsPrincipal principal, UpdateUserProfileRequest model)
        {
            #region Get User

            // Get the current user
            var user = await _userManager.GetUserAsync(principal);

            // If we have no user...
            if (user == null)
                throw new Exception("User not found");

            #endregion

            #region Update Profile

            // If we have a first name...
            if (!string.IsNullOrEmpty(model.FirstName))
                // Update the profile details
                user.FirstName = model.FirstName;

            // If we have a last name...
            if (!string.IsNullOrEmpty(model.LastName))
                // Update the profile details
                user.LastName = model.LastName;

            // If we have a email...
            if (!string.IsNullOrEmpty(model.Email) &&
                // And it is not the same...
                !string.Equals(model.Email.Replace(" ", ""), user.NormalizedEmail))
            {
                // Update the email
                user.Email = model.Email;

                // Set username as email
                user.UserName = model.Email;

                // Un-verify the email
                user.EmailConfirmed = false;

                // Flag we have changed email
            }

            #endregion

            #region Save Profile

            // Attempt to commit changes to data store
            var result = await _userManager.UpdateAsync(user);

            // If successful, send out email verification
            //if (result.Succeeded && emailChanged)
            //    // Send email verification
            //    await SendUserEmailVerificationAsync(user);

            #endregion

            #region Respond

            if (result.Succeeded)
                // Return successful response
                return await Result<string>.SuccessAsync("Successfully updated");
            // Otherwise if it failed...
            else
                // Return the failed response
                return await Result<string>.FailAsync(string.Join(",", result.Errors.Select(x => x.Description)));

            #endregion
        }
        public async Task<Result<string>> ChangeUserPassword(ClaimsPrincipal principal, ChangeUserPasswordRequest model)
        {
            #region Get User

            // Get the current user
            var user = await _userManager.GetUserAsync(principal);

            // If we have no user...
            if (user == null)
                throw new Exception("User not found");

            #endregion

            #region Update Password

            if (model.CurrentPassword is null)
            {
                throw new ArgumentNullException(nameof(model.CurrentPassword));
            }

            if (model.NewPassword is null)
            {
                throw new ArgumentNullException(nameof(model.NewPassword));
            }

            // Attempt to commit changes to data store
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            #endregion

            #region Respond

            // If we were successful...
            if (result.Succeeded)
                // Return successful response
                return await Result<string>.SuccessAsync("Successfully updated");
            // Otherwise if it failed...
            else
                // Return the failed response
                return await Result<string>.FailAsync(string.Join(",", result.Errors.Select(r => r.Description)));

            #endregion
        }
    }
}