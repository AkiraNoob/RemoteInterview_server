using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MainService.Application.Exceptions;
using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Application.Slices.FileSlice.Interfaces;
using Mapster;
using Microsoft.Extensions.Options;
using Serilog;
using File = MainService.Domain.Models.File;

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

    public async Task<File> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file.Length <= 0)
        {
            throw new InternalServerException("File has been corrupted.");
        };

        var entity = new File();

        await using var stream = file.OpenReadStream();

        var uploadParams = new RawUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            PublicId = entity.Id.ToString(),
            Overwrite = true,
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            throw new Exception(uploadResult.Error.Message);
        }

        entity.FileName = file.FileName; ;
        entity.FileUrl = uploadResult.SecureUrl.ToString();
        entity.FileType = uploadResult.Type;

        return entity;
    }
}
