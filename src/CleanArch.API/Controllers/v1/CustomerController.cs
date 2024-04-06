using CleanArch.API.Controllers.Base.v1;
using CleanArch.Application.Enums;
using CleanArch.Application.Exceptions;
using CleanArch.Application.Features.Customer.Commands.CreateCustomer;
using CleanArch.Application.Features.Customer.Queries.GetAllCustomers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers.v1
{
    [Authorize(Roles = nameof(Roles.Basic))]
    public class CustomerController : Basev1ApiController
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }


        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Policy = nameof(Policies.CustomerRead))]
        public async Task<IActionResult> GetAllCustomers([FromQuery] GetAllCustomersQueryRequest query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }


        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Policy = nameof(Policies.CustomerCreate))]
        public async Task<IActionResult> CreateCustomer(CreateCustomerCommandRequest command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
