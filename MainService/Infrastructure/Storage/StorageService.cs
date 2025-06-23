using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MainService.Application.Exceptions;
using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Application.Slices.FileSlice.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;

namespace MainService.Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly Cloudinary _cloudinary;

    public StorageService(IOptions<CloudinarySettings> settings)
    {
        Log.Information(settings.Value.CloudName);

        var account = new Account(
           settings.Value.CloudName,
           settings.Value.ApiKey,
           settings.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
    }

    public async Task<UploadFileResultDTO> UploadFileAsync(IFormFile file, string publicId, CancellationToken cancellationToken = default)
    {
        if (file.Length <= 0)
        {
            throw new InternalServerException("File has been corrupted.");
        };

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
            FileName = file.FileName,
            FileUrl = uploadResult.SecureUrl.ToString(),
            FilePublicId = publicId,
            FileType = uploadResult.Type,
        };
    }
}
