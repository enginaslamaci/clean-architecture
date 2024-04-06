using CleanArch.Application.Abstractions.Infrastructure.Services.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CleanArch.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : IAzureStorage
    {
        public AzureStorage(IConfiguration configuration)
        {
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {

        }
        public List<string> GetFiles(string containerName)
        {
            return new List<string>();
        }
        public bool HasFile(string containerName, string fileName)
        {
            return false;
        }
        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            return new List<(string fileName, string pathOrContainerName)>();
        }
    }
}
