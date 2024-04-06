using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Abstractions.Infrastructure.Services
{
    public interface IImageService
    {
        MemoryStream ImageResize(byte[] imageBytes, int width, int height);
        IFormFile CreateFormFile(MemoryStream baseStream, long baseStreamOffset, long length, string name, string fileName, string contentType);
    }
}
