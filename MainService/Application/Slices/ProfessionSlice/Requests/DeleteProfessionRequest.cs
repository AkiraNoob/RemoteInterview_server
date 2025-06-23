using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Domain.Models;
using MediatR;

namespace MainService.Application.Slices.ProfessionSlice.Requests;

public class DeleteProfessionRequest : IRequest
{
    public Guid ProfessionId { get; set; }
    public DeleteProfessionRequest(Guid professionId)
    {
        ProfessionId = professionId;
    }
}


public class DeleteProfessionRequestHandler(IRepository<Profession> professionRepository) : IRequestHandler<DeleteProfessionRequest>
{
    public async Task Handle(DeleteProfessionRequest request, CancellationToken cancellationToken)
    {
        var profession = await professionRepository.GetByIdAsync(
            request.ProfessionId,
            cancellationToken: cancellationToken
        ) ?? throw new NotFoundException("Profession not found");

        await professionRepository.DeleteAsync(
            profession,
            cancellationToken: cancellationToken
        );
    }
}