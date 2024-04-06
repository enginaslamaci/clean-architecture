using CleanArch.API.Controllers.Base.v1;
using CleanArch.Application.Abstractions.Infrastructure.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers.v1
{
    [Authorize]
    public class ProductController : Basev1ApiController
    {
        private readonly IStorage _storage;
        private IWebHostEnvironment _hostingEnvironment;

        public ProductController(IStorage storage, IWebHostEnvironment hostingEnvironment)
        {
            _storage = storage;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload(IFormFileCollection files)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "productFiles");
            var response = await _storage.UploadAsync(path, files);
            return Ok(new List<object> { response.Select(r => new { r.fileName, r.pathOrContainerName }) });
        }
    }
}
