using Microsoft.AspNetCore.Http;

namespace WebCoffee.BackendServer.Services.Storage
{
    public interface IStorageService
    {
        Task<bool> DeleteImageAsync(string imageUrl);
        Task<string> UploadImageAsync(IFormFile file, string entityName);
    }
}