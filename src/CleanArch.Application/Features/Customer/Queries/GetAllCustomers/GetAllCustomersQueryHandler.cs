using AutoMapper;
using CleanArch.Application.Abstractions.Persistence.Repositories;
using CleanArch.Application.DTOs.Customer;
using CleanArch.Domain.Common.Results;
using MediatR;

namespace CleanArch.Application.Features.Customer.Queries.GetAllCustomers
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQueryRequest, Result<GetAllCustomersQueryResponse>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetAllCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<Result<GetAllCustomersQueryResponse>> Handle(GetAllCustomersQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _customerRepository.GetPagedResponseAsync(request.Page, request.Size);
            var result = _mapper.Map<List<ListCustomerDto>>(data);
            return await Result<GetAllCustomersQueryResponse>.SuccessAsync(new GetAllCustomersQueryResponse() { Customers = result });
        }
    }
}
