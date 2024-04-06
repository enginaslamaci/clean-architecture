using CleanArch.Application.Abstractions.Persistence.Services;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Enums;
using MediatR;

namespace CleanArch.Application.Features.User.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, Result<GetAllUsersQueryResponse>>
    {
        private readonly IUserService _userService;

        public GetAllUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<GetAllUsersQueryResponse>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetPagedUser(request.Page, request.Size, "", "");
            users = users.Where(x => x.Status == StatusType.Active).ToList();
            return await Result<GetAllUsersQueryResponse>.SuccessAsync(new GetAllUsersQueryResponse() { Users = users });
        }
    }
}
