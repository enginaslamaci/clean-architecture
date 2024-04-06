using CleanArch.Application.Features.User.Commands.ChangePassword;
using FluentValidation;

namespace CleanArch.Application.Validators.User
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {

            RuleFor(p => p.Email)
                  .NotEmpty()
                  .NotNull()
                      .WithMessage("Email giriniz.");


            RuleFor(p => p.CurrentPassword)
               .NotEmpty()
               .NotNull();


            RuleFor(p => p.NewPassword)
                  .NotEmpty()
                  .NotNull()
                  .WithMessage("Yeni şifre giriniz.")
                  .MaximumLength(30)
                  .MinimumLength(5);
                //.Equal(p => p.PasswordConfirmation);
        }
    }
}
