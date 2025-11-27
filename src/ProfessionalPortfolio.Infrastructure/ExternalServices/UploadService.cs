using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Settings;

namespace ProfessionalPortfolio.Infrastructure.ExternalServices
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;

        public UploadService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value ?? 
                throw new ArgumentNullException("CloudinarySettings");

            _cloudinary = new Cloudinary(new Account
            {
                ApiKey = settings.ApiKey,
                ApiSecret = settings.ApiSecret,
                Cloud = settings.CloudName
            });
        }

        public async Task<(bool Success, string Url, string PublicId)> UploadImageAsync(string fileName, string originalFileName, Stream stream)
        {
            var param = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                PublicId = $"images/{fileName}",
                UniqueFilename = true,
                UseFilename = false,
                Transformation = new Transformation()
            };

            var response = await _cloudinary.UploadAsync(param);
            return response != null && response.StatusCode == System.Net.HttpStatusCode.OK ? 
                (true, response.SecureUrl.ToString(), response.PublicId) : 
                (false, string.Empty, string.Empty);
        }

        public async Task<(bool Success, string Url, string PublicId)> UploadRawAsync(string fileName, string originalFileName, Stream stream)
        {
            var param = new RawUploadParams
            {
                File = new FileDescription(originalFileName, stream),
                PublicId = $"documents/{fileName}",
                UniqueFilename = true,
                UseFilename = false,
                AccessMode = "public"
            };

            var response = await _cloudinary.UploadAsync(param);
            return response != null && response.StatusCode == System.Net.HttpStatusCode.OK ?
                (true, response.SecureUrl.ToString(), response.PublicId) :
                (false, string.Empty, string.Empty);
        }

        public async Task<bool> DeleteAsync(string publicId, ResourceType type)
        {
            var deleteParam = new DeletionParams(publicId)
            {
                ResourceType = type
            };

            var response = await _cloudinary.DestroyAsync(deleteParam);
            return response != null &&
                response.StatusCode == System.Net.HttpStatusCode.OK &&
                response.Result.ToLower() == "ok";
        }
    }
}
