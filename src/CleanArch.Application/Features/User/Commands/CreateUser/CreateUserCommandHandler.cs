using CleanArch.Application.Exceptions;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Common.Results.Abstracts;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;

namespace CleanArch.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Status = StatusType.Active
                };

                //create user
                await _userManager.CreateAsync(user,"pass123!");

                //create user roles
                await _userManager.AddToRolesAsync(user, request.Roles);

                return Result.Success("User created");
            }
            else
                throw new BadRequestException("User already exists");
        }
    }
}
