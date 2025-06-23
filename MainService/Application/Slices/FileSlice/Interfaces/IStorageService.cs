using MainService.Application.Interfaces;
using MainService.Application.Slices.FileSlice.DTOs;

namespace MainService.Application.Slices.FileSlice.Interfaces;

public interface IStorageService
{
    Task<UploadFileResultDTO> UploadFileAsync(IFormFile file, string publicId, CancellationToken cancellationToken = default);
}
