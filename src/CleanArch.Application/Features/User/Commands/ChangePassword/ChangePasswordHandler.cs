using CleanArch.Application.Exceptions;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Common.Results.Abstracts;
using CleanArch.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.Application.Features.User.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordRequest, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                IdentityResult result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                if (result.Succeeded)
                    return Result.Success("User password changed");
                else
                    throw new ApiException("Password cannot changed");
            }
            else
                throw new NotFoundException(request.Email);
        }
    }
}
