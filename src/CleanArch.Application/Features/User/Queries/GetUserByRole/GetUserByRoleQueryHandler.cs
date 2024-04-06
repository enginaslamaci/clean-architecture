using AutoMapper;
using CleanArch.Application.Abstractions.Persistence.Services;
using CleanArch.Application.DTOs.User;
using CleanArch.Application.Exceptions;
using CleanArch.Domain.Common.Results;
using MediatR;

namespace CleanArch.Application.Features.User.Queries.GetUserByRole
{
    public class GetUserByRoleQueryHandler : IRequestHandler<GetUserByRoleQueryRequest, Result<GetUserByRoleQueryResponse>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public GetUserByRoleQueryHandler(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<GetUserByRoleQueryResponse>> Handle(GetUserByRoleQueryRequest request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetPagedUserByRole(request.RoleName, request.Page, request.Size, request.SortOn, request.SortDir);
            var usersCount = _userService.GetAllQueryableUser().Count();
            if (users.Count > 0)
            {
                var result = await PaginatedResult<ApplicationUserDto>.SuccessAsync(_mapper.Map<IEnumerable<ApplicationUserDto>>(users), usersCount, request.Page, request.Size, request.SortOn, request.SortDir);
                return Result<GetUserByRoleQueryResponse>.Success(new GetUserByRoleQueryResponse() { Users = result });
            }
            else
                throw new NotFoundException();
        }
    }
}
