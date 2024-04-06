using CleanArch.Application.Abstractions.Persistence.Repositories;
using CleanArch.Application.Features.User.Commands.CreateUser;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.Application.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommandRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public CreateUserValidator(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
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
                .WithMessage("Email giriniz.")
                .MustAsync(IsUniqueEmail).WithMessage("Email ile kayıtlı kullanıcı zaten mevcut.");


            RuleFor(p => p.Roles)
           .NotNull()
                    .WithMessage("Role giriniz.")
                    .MustAsync(IsValidRole).WithMessage("Rol geçerli değil.");
        }

        private async Task<bool> IsUniqueEmail(string email, CancellationToken arg2)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null ? true : false;
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
