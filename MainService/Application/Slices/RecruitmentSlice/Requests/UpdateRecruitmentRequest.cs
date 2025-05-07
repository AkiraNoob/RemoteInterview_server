using MainService.Application.Exceptions;
using MainService.Application.Helpers;
using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using MainService.Application.Slices.RecruitmentSlice.Specifications;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.RecruitmentSlice.DTOs;

public class UpdateRecruitmentRequest : UpdateRecruitmentDTO, IRequest<RecruitmentDTO>
{
    public Guid RecruitmentId { get; set; }

    public UpdateRecruitmentRequest(Guid recruitmentId, UpdateRecruitmentDTO dto)
    {
        RecruitmentId = recruitmentId;
        Title = dto.Title;
        Motivation = dto.Motivation;
        Description = dto.Description;
        Requirement = dto.Requirement;
        Welfare = dto.Welfare;
        Address = dto.Address;
        MinExperience = dto.MinExperience;
        Salary = dto.Salary;
        ProvinceId = dto.ProvinceId;
        DistrictId = dto.DistrictId;
    }
}


public class UpdateRecruitmentHandler : IRequestHandler<UpdateRecruitmentRequest, RecruitmentDTO>
{
    private readonly IRepository<Recruitment> _recruitmentRepository;

    public UpdateRecruitmentHandler(
        IRepository<Recruitment> recruitmentRepository)
    {
        _recruitmentRepository = recruitmentRepository;
    }

    public async Task<RecruitmentDTO> Handle(UpdateRecruitmentRequest request, CancellationToken cancellationToken)
    {
        var recruitment = await _recruitmentRepository
            .FirstOrDefaultAsync(new GetRecruitmentByIdSpec(request.RecruitmentId), cancellationToken) ?? throw new NotFoundException("Recruitment not found");

        recruitment.Update(request);

        await _recruitmentRepository.UpdateAsync(recruitment, cancellationToken);
        return recruitment.Adapt<RecruitmentDTO>();
    }
}
