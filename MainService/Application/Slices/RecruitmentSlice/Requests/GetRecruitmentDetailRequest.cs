using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Specifications;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class GetRecruitmentDetailRequest : IRequest<RecruitmentDTO>
{
    public Guid RecruitmentId { get; set; }

    public GetRecruitmentDetailRequest(Guid recruitmentId)
    {
        RecruitmentId = recruitmentId;
    }
}

public class GetRecruitmentDetailHandler : IRequestHandler<GetRecruitmentDetailRequest, RecruitmentDTO>
{
    private readonly IRepository<Recruitment> _recruitmentRepository;

    public GetRecruitmentDetailHandler(
        IRepository<Recruitment> recruitmentRepository)
    {
        _recruitmentRepository = recruitmentRepository;
    }

    public async Task<RecruitmentDTO> Handle(GetRecruitmentDetailRequest request, CancellationToken cancellationToken)
    {
        var recruitment = await _recruitmentRepository
            .FirstOrDefaultAsync(new RecruitmentByIdSpec(request.RecruitmentId), cancellationToken) ?? throw new NotFoundException("Recruitment not found");

        return recruitment.Adapt<RecruitmentDTO>();
    }
}
