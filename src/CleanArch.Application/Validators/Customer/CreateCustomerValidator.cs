using CleanArch.Application.Abstractions.Persistence.Repositories;
using CleanArch.Application.Features.Customer.Commands.CreateCustomer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Validators.Customer
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommandRequest>
    {
        private readonly ICustomerRepository _customerRepository;
        public CreateCustomerValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(p => p.FirstName)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Müşteri adı giriniz.")
                .MaximumLength(150)
                .MinimumLength(2);


            RuleFor(p => p.LastName)
             .NotEmpty()
             .NotNull();


            RuleFor(p => p.Phone)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} alanı zorunludur.")
                .MustAsync(IsUniquePhoneNumber).WithMessage("{PropertyName} alanı zaten mevcut.");
        }

        private async Task<bool> IsUniquePhoneNumber(string phone, CancellationToken arg2)
        {
            var phoneNumberCount = await _customerRepository.CountAsync(x => x.Phone == phone);
            return phoneNumberCount == 0 ? true : false;
        }

    }
}
