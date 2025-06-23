using MainService.Application.Slices.ReviewSlices.DTOs;
using MainService.Application.Slices.ReviewSlices.Requests;
using MainService.Domain.Models;
using Mapster;

namespace MainService.Infrastructure.Mapping.Configuration;

public class ReviewMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Review, ReviewDTO>();

        config.NewConfig<UpdateReviewRequest, Review>()
            .IgnoreNullValues(true);
    }
}
