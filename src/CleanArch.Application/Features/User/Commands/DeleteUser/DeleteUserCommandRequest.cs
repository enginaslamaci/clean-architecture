using CleanArch.Domain.Common.Results.Abstracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandRequest : IRequest<IResult>
    {
        public string UserId { get; set; }
    }
}
