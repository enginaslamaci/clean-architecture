using CleanArch.Application.Abstractions.Infrastructure.Services.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CleanArch.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string path, string fileName)
            => File.Delete($"{path}\\{fileName}");
        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }
        public bool HasFile(string path, string fileName)
            => File.Exists($"{path}\\{fileName}");
        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new List<(string fileName, string path)>();
            foreach (IFormFile file in files)
            {
                string fileName = string.Concat(Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
                using (var stream = File.Create(Path.Combine(uploadPath, fileName)))
                {
                    await file.CopyToAsync(stream);
                    await stream.FlushAsync();
                }
                datas.Add((fileName, $"{path}\\{fileName}"));

                //short way
                //file.CopyTo(new FileStream(Path.Combine(path, fileName), FileMode.Create));
            }
            return datas;
        }
    }
}
