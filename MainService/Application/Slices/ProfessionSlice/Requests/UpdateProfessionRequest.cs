using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.ProfessionSlice.DTOs;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.ProfessionSlice.Requests;

public class UpdateProfessionRequest : UpdateProfessionDTO, IRequest
{
    public Guid Id { get; set; }
    public UpdateProfessionRequest(Guid professionId, UpdateProfessionDTO dto)
    {
        Id = professionId;
        dto.Adapt(this);
    }
}

public class UpdateProfessionRequestHandler(IRepository<Profession> professionRepository) : IRequestHandler<UpdateProfessionRequest>
{
    public async Task Handle(UpdateProfessionRequest request, CancellationToken cancellationToken)
    {
        var profession = await professionRepository.GetByIdAsync(
            request.Id,
            cancellationToken: cancellationToken
        ) ?? throw new NotFoundException("Profession not found");

        request.Adapt(profession);

        await professionRepository.UpdateAsync(
            profession,
            cancellationToken: cancellationToken
        );
    }
}