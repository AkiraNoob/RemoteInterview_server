using MainService.Application.Interfaces;
using MainService.Application.Slices.FileSlice.DTOs;

namespace MainService.Application.Slices.FileSlice.Interfaces;

public interface IStorageService : IScopedService
{
    Task<UploadFileResultDTO?> UploadImageAsync(IFormFile file, string publicId);
}
