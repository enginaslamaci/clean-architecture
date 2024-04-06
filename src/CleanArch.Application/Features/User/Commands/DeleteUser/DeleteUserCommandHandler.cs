using CleanArch.Application.Exceptions;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Common.Results.Abstracts;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommandRequest, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user != null)
            {
                //delete user
                user.Status = StatusType.Deleted;
                await _userManager.UpdateAsync(user);

                return Result.Success("User deleted");
            }
            else
                throw new NotFoundException(request.UserId);

        }
    }
}
