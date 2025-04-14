using MainService.Application.Interfaces;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.RecruitmentSlice.DTOs;

public class CreateRecruitmentRequest : IRequest<RecruitmentDTO>
{
    public string Title { get; set; }
    public string Motivation { get; set; }
    public string Description { get; set; }
    public string Requirement { get; set; }
    public string Welfare { get; set; }
    public string Address { get; set; }
    public int MinExperience { get; set; }
    public long MaxSalary { get; set; }
    public int ProvinceId { get; set; }
    public int DistrictId { get; set; }
}

public class CreateRecruitmentHandler : IRequestHandler<CreateRecruitmentRequest, RecruitmentDTO>
{
    private readonly IRepository<Recruitment> _recruitmentRepository;
    private readonly ICurrentUser _currentUser;

    public CreateRecruitmentHandler(
        IRepository<Recruitment> recruitmentRepository,
        ICurrentUser currentUser)
    {
        _currentUser = currentUser;
        _recruitmentRepository = recruitmentRepository;
    }

    public async Task<RecruitmentDTO> Handle(CreateRecruitmentRequest request, CancellationToken cancellationToken)
    {
        var recruitment = new Recruitment(_currentUser.GetUserId(),
            request.Title,
            request.Motivation,
            request.Description,
            request.Requirement,
            request.Welfare,
            request.Address,
            request.ProvinceId,
            request.DistrictId,
            request.MinExperience,
            request.MaxSalary);

        await _recruitmentRepository.AddAsync(recruitment, cancellationToken);

        return recruitment.Adapt<RecruitmentDTO>();
    }
}
