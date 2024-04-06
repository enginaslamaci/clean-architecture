using CleanArch.Application.Exceptions;
using CleanArch.Application.Features.User.Commands.CreateUser;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Common.Results.Abstracts;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                user.UserName = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.Status = request.Status;

                //update user
                await _userManager.UpdateAsync(user);

                //update user roles (remove then recreate)
                var existRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, existRoles);
                await _userManager.AddToRolesAsync(user, request.Roles);

                return Result.Success("User updated");
            }
            else
                throw new NotFoundException(request.Email);
        }
    }
}
