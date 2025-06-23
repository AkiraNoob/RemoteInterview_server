using MainService.Application.Slices.UserSlice.DTOs;

namespace MainService.Application.Slices.ReviewSlices.DTOs;

public class ReviewDTO
{
    public double Rating { get; set; }
    public string Comment { get; set; }
    public ShortenUserDetailDTO? Reviewer { get; set; }
    public bool IsOwner { get; set; }
}
