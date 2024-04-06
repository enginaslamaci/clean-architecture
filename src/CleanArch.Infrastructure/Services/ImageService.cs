using CleanArch.Application.Abstractions.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace CleanArch.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        public MemoryStream ImageResize(byte[] imageBytes, int width, int height)
        {
            using (Image image = Image.Load(imageBytes))
            {
                var stream = new MemoryStream(imageBytes);
                image.Mutate(x => x
                     .Resize(width, height));
                image.Save(stream, new JpegEncoder()); // Automatic encoder selected based on extension.
                return stream;
            }
        }

        public IFormFile CreateFormFile(MemoryStream baseStream, long baseStreamOffset, long length, string name, string fileName, string contentType)
        {
            return new FormFile(baseStream, baseStreamOffset, length, name, fileName) { Headers = new HeaderDictionary(), ContentType = contentType };
        }
    }
}
