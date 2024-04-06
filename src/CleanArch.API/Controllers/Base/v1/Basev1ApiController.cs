using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CleanArch.API.Controllers.Base.v1
{
    [ApiController]
    [Route("api/v1/[controller]")] //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [EnableRateLimiting("Fixed")]
    public class Basev1ApiController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
