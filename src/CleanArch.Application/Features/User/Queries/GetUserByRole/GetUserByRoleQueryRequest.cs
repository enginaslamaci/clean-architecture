using CleanArch.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Features.User.Queries.GetUserByRole
{
    public class GetUserByRoleQueryRequest : IRequest<Result<GetUserByRoleQueryResponse>>
    {
        public string RoleName { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? SortOn { get; set; }
        public string? SortDir { get; set; }
    }
}
