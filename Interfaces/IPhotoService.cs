using CloudinaryDotNet.Actions;

namespace RunWebAppGroup.Interfaces
{
    public interface IPhotoService
    {

        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
