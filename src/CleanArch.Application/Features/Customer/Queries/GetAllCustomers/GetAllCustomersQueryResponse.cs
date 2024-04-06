using CleanArch.Application.DTOs.Customer;

namespace CleanArch.Application.Features.Customer.Queries.GetAllCustomers
{
    public class GetAllCustomersQueryResponse
    {
        public List<ListCustomerDto> Customers { get; set; }
    }
}
