namespace MainService.Application.Slices.ProfessionSlice.DTOs;

public class CreateProfessionsDTO
{
    public ICollection<string> ProfessionNames { get; set; } = new List<string>();
}
