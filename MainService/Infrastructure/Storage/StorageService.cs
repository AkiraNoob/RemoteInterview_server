using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Application.Slices.FileSlice.Interfaces;
using Microsoft.Extensions.Options;

namespace MainService.Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly Cloudinary _cloudinary;

    public StorageService(IOptions<CloudinarySetting> settings)
    {
        var account = new Account(
           settings.Value.CloudName,
           settings.Value.ApiKey,
           settings.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
    }

    public async Task<UploadFileResultDTO?> UploadImageAsync(IFormFile file, string publicId)
    {
        if (file.Length <= 0) return null;

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            PublicId = publicId,
            Overwrite = true
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            throw new Exception(uploadResult.Error.Message);
        }

        return new UploadFileResultDTO
        {
            FileName = uploadResult.DisplayName,
            FileUrl = uploadResult.SecureUrl.ToString(),
            FilePublicId = publicId,
            FileType = uploadResult.Type,
        };
    }
}
