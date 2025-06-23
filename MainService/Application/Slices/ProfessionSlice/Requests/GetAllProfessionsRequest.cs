using MainService.Application.Interfaces;
using MainService.Application.Slices.ProfessionSlice.DTOs;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.ProfessionSlice.Requests;

public class GetAllProfessionsRequest : IRequest<ICollection<ProfessionDTO>>
{
}

public class GetAllProfessionsRequestHandler(IRepository<Profession> professionRepository) : IRequestHandler<GetAllProfessionsRequest, ICollection<ProfessionDTO>>
{
    public async Task<ICollection<ProfessionDTO>> Handle(GetAllProfessionsRequest request, CancellationToken cancellationToken)
    {
        return (await professionRepository.ListAsync(
            cancellationToken: cancellationToken
        )).Adapt<List<ProfessionDTO>>();
    }
}
