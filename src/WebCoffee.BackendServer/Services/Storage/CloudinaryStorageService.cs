using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebCoffee.BackendServer.Helpers;

namespace WebCoffee.BackendServer.Services.Storage
{
    public class CloudinaryStorageService : IStorageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _projectName = "WebCoffee";

        public CloudinaryStorageService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string entityName)
        {
            if (file == null || file.Length == 0) return null;

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                // Cấu trúc Folder: "WebCoffee/SanPhams"
                Folder = $"{_projectName}/{entityName}",
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return false;

            // URL mẫu: https://res.cloudinary.com/cloud_name/image/upload/v1716480000/WebCoffee/SanPhams/xyz.jpg
            var uploadSegment = "/upload/";
            var uploadIndex = imageUrl.IndexOf(uploadSegment);
            if (uploadIndex == -1) return false;

            // Lấy phần phía sau chuỗi "/upload/" -> "v1716480000/WebCoffee/SanPhams/xyz.jpg"
            var pathAfterUpload = imageUrl.Substring(uploadIndex + uploadSegment.Length);

            var parts = pathAfterUpload.Split('/');

            // Nếu có version tag (Bắt đầu bằng chữ 'v' và theo sau là số), ta bỏ qua nó
            if (parts[0].StartsWith("v") && parts[0].Length > 1 && char.IsDigit(parts[0][1]))
            {
                pathAfterUpload = string.Join("/", parts.Skip(1)); // -> "WebCoffee/SanPhams/xyz.jpg"
            }

            // Cắt bỏ phần mở rộng file (.jpg, .png...) để lấy đúng chuẩn PublicId
            var lastDotIndex = pathAfterUpload.LastIndexOf('.');
            var publicId = lastDotIndex != -1 ? pathAfterUpload.Substring(0, lastDotIndex) : pathAfterUpload;

            // Xóa ảnh thông qua PublicId ("WebCoffee/SanPhams/xyz")
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok";
        }
    }
}