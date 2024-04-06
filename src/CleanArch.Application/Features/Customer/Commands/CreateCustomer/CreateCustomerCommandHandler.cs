using CleanArch.Application.Abstractions.Persistence.Repositories;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Common.Results.Abstracts;
using MediatR;

namespace CleanArch.Application.Features.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommandRequest, IResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IResult> Handle(CreateCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            await _customerRepository.AddAsync(new Domain.Entities.Customer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                City = request.City,
                Country = request.Country,
                Phone = request.Phone
            });

            return await Result.SuccessAsync("Customer created");
        }
    }
}