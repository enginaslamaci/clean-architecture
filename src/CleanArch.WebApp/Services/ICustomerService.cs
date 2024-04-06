using CleanArch.Application.Features.Customer.Commands.CreateCustomer;
using CleanArch.Application.Features.Customer.Queries.GetAllCustomers;
using CleanArch.Domain.Common.Results;
using Refit;

namespace CleanArch.WebApp.Services
{
    [Headers("accept: application/json", "Authorization: Bearer")]
    public interface ICustomerService
    {
        [Get("")]
        Task<Result<GetAllCustomersQueryResponse>> GetCustomers([Query] GetAllCustomersQueryRequest request);

        [Post("")]
        Task<IResult> CreateCustomer([Body] CreateCustomerCommandRequest request);
    }
}
