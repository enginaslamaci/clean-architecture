using CleanArch.Domain.Common.Results.Abstracts;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Enums;
using MediatR;

namespace CleanArch.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommandRequest : IRequest<IResult>
    {
        //public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        public StatusType Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<string> Roles { get; set; }
    }
}
