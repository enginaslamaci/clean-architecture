using CleanArch.Domain.Common.Results;
using MediatR;

namespace CleanArch.Application.Features.Customer.Queries.GetAllCustomers
{
    public class GetAllCustomersQueryRequest : IRequest<Result<GetAllCustomersQueryResponse>>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}