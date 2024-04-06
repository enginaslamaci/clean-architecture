using CleanArch.Domain.Common.Results.Abstracts;
using MediatR;

namespace CleanArch.Application.Features.User.Commands.ChangePassword
{
    public class ChangePasswordRequest : IRequest<IResult>
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
