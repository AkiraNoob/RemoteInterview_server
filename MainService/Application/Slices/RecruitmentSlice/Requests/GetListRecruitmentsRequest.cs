using MainService.Application.Common.Models;
using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Specifications;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class GetListRecruitmentsRequest : SimplePaginationFilter, IRequest<PaginationResponse<RecruitmentDTO>>
{
    public ICollection<Guid>? TagIds { get; set; }
    public long? MinSalary {  get; set; }
    public int? MinExperience { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }

    public GetListRecruitmentsRequest(ICollection<Guid>? tagIds, long? minSalary, int? minExperience, int? provinceId, int? districtId)
    {
        TagIds = tagIds;
        MinSalary = minSalary;
        MinExperience = minExperience;
        ProvinceId = provinceId;
        DistrictId = districtId;
    }
}

public class GetListRecruitmentHandler(
    IRepository<Recruitment> recruitmentRepository) : IRequestHandler<GetListRecruitmentsRequest, PaginationResponse<RecruitmentDTO>>
{
    private readonly IRepository<Recruitment> _recruitmentRepository = recruitmentRepository;
    public async Task<PaginationResponse<RecruitmentDTO>> Handle(GetListRecruitmentsRequest request, CancellationToken cancellationToken)
    {
        var listRecruitments = (await _recruitmentRepository.ListAsync(new GetListRecruitmentSpec(request), cancellationToken)).Adapt<List<RecruitmentDTO>>();
        var count = await _recruitmentRepository.CountAsync(cancellationToken);

        var response = new PaginationResponse<RecruitmentDTO>(listRecruitments, count, request.PageNumber, request.PageSize);

        return response;
    }
}
