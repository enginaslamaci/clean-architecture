using AutoMapper;
using CleanArch.Application.Abstractions.Persistence.Services;
using CleanArch.Application.DTOs.User;
using CleanArch.Application.Exceptions;
using CleanArch.Domain.Common.Results;
using MediatR;

namespace CleanArch.Application.Features.User.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQueryRequest, Result<GetUserByIdQueryResponse>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public GetUserByIdQueryHandler(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<GetUserByIdQueryResponse>> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserById(request.Id);
            if (user != null)
            {
                var result = _mapper.Map<ApplicationUserDto>(user);
                return await Result<GetUserByIdQueryResponse>.SuccessAsync(new GetUserByIdQueryResponse() { User = result });
            }
            else
                throw new NotFoundException(request.Id);
        }
    }
}
