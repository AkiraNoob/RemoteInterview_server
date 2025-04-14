using MainService.Domain.Contracts;

namespace MainService.Domain.Models;

public class File : AuditableEntity, IAggregateRoot
{
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public string FileType { get; set; }
}
