using File = MainService.Domain.Models.File;

namespace MainService.Application.Slices.FileSlice.Interfaces;

public interface IStorageService
{
    Task<File> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
}
