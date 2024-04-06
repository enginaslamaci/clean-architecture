using CleanArch.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Features.User.Queries.GetUserById
{
    public class GetUserByIdQueryRequest : IRequest<Result<GetUserByIdQueryResponse>>
    {
        public string Id { get; set; }
    }
}
