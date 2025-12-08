using CloudinaryDotNet.Actions;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IUploadService
    {
        Task<bool> DeleteAsync(string publicId, ResourceType type);
        Task<(bool Success, string Url, string PublicId)> UploadImageAsync(string fileName, string originalFileName, Stream stream);
        Task<(bool Success, string Url, string PublicId)> UploadRawAsync(string fileName, string originalFileName, Stream stream);
    }
}
