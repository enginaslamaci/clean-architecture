using CleanArch.API.Controllers.Base.v1;
using CleanArch.Application.Abstractions.Infrastructure.Services.Cache.Redis;
using CleanArch.Application.Abstractions.Infrastructure.Services.Storage;
using CleanArch.Application.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace CleanArch.API.Controllers.v1
{
    [Authorize]
    public class ProductController : Basev1ApiController
    {
        private readonly IStorage _storage;
        private readonly IRedisService _redisService;
        private IWebHostEnvironment _hostingEnvironment;

        public ProductController(IStorage storage, IWebHostEnvironment hostingEnvironment, IRedisService redisService)
        {
            _storage = storage;
            _hostingEnvironment = hostingEnvironment;
            _redisService = redisService;
        }


        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload(IFormFileCollection files)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "productFiles");
            var response = await _storage.UploadAsync(path, files);
            return Ok(new List<object> { response.Select(r => new { r.fileName, r.pathOrContainerName }) });
        }


        [HttpPost]
        public async Task SetProduct()
        {
            List<ListProductDto> products = new List<ListProductDto>()
            {
                new ListProductDto() { Id = 1, ProductName = "pencil", Package = 1000, IsDiscontinued = true, UnitPrice = 100 },
                new ListProductDto() { Id = 2, ProductName = "eraser", Package = 1500, IsDiscontinued = false, UnitPrice = 150 },
                new ListProductDto() { Id = 3, ProductName = "notebook", Package = 2000, IsDiscontinued = true, UnitPrice = 200 }
            };

            var entries = products.Select(r => new HashEntry(r.Id, JsonSerializer.Serialize(r))).ToArray();

            await _redisService.SetHashAsync("products", entries);
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            if (await _redisService.KeyExists("products"))
            {
                var result = await _redisService.GetHashAsync("products");
                return Ok(result.ToStringDictionary());
            }
            return NotFound();
        }



        [HttpDelete]
        public async Task ClearCache(string key)
        {
            await _redisService.Clear(key);
        }

    }
}
