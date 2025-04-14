namespace MainService.Application.Slices.FileSlice.DTOs
{
    public class FileDTO
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }

        public FileDTO(Guid fileId, string fileName, string fileUrl, string fileType)
        {
            FileId = fileId;
            FileName = fileName;
            FileUrl = fileUrl;
            FileType = fileType;
        }
    }
}
