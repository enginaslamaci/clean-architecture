using CleanArch.Domain.Common.Results;
using MediatR;

namespace CleanArch.Application.Features.User.Queries.GetAllUsers
{
    public class GetAllUsersQueryRequest : IRequest<Result<GetAllUsersQueryResponse>>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
