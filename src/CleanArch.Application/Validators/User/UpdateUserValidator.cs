using CleanArch.Application.Features.User.Commands.UpdateUser;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.Application.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommandRequest>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UpdateUserValidator(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;

            RuleFor(p => p.FirstName)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Ad giriniz.")
                .MaximumLength(150)
                .MinimumLength(2);


            RuleFor(p => p.LastName)
           .NotNull()
                    .WithMessage("Soyad giriniz.")
                .MaximumLength(150)
                .MinimumLength(2);


            RuleFor(p => p.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email giriniz.");


            RuleFor(p => p.Status)
                .NotNull()
                .Must(r => Enum.IsDefined(typeof(StatusType), r));


            RuleFor(p => p.Roles)
           .NotNull()
                    .WithMessage("Role giriniz.")
                    .MustAsync(IsValidRole).WithMessage("Rol geçerli değil.");
        }



        private async Task<bool> IsValidRole(List<string> roleNames, CancellationToken arg2)
        {
            var roles = _roleManager.Roles.ToList();
            foreach (var item in roleNames)
            {
                if (!roles.Select(r => r.Name).Contains(item))
                    return false;
            }
            return true;
        }
    
    }
}
