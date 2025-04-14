namespace MainService.Application.Slices.FileSlice.DTOs;

public class UploadFileResultDTO
{
    public string FileUrl { get; set; }
    public string FilePublicId { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
}
