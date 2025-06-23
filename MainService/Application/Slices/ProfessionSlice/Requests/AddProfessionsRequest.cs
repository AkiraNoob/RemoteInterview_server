using MainService.Application.Interfaces;
using MainService.Application.Slices.ProfessionSlice.DTOs;
using MainService.Domain.Models;
using MediatR;

namespace MainService.Application.Slices.ProfessionSlice.Requests;

public class AddProfessionsRequest : CreateProfessionsDTO, IRequest
{
}


public class AddProfessionRequestHandler(IRepository<Profession> professionRepository) : IRequestHandler<AddProfessionsRequest>
{
    public async Task Handle(AddProfessionsRequest request, CancellationToken cancellationToken)
    {
        List<Profession> professions = request.ProfessionNames.Select(p => new Profession(p)).ToList();

        await professionRepository.AddRangeAsync(professions, cancellationToken);
    }
}