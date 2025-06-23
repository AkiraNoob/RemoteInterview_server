using MainService.Application.Common.Models;
using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Specifications;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Enums;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class GetRecruitmentOfSpecificEmployerRequest : SimplePaginationFilter, IRequest<PaginationResponse<RecruitmentDTO>>
{
    public Guid EmployerId { get; set; }
    public RecruitmentStatusEnum? Status { get; set; }

}

public class GetRecruimentOfSpecificEmployerRequestHandler(IRepository<Recruitment> recruitmentRepository, IUserService userService) : IRequestHandler<GetRecruitmentOfSpecificEmployerRequest, PaginationResponse<RecruitmentDTO>>
{
    public async Task<PaginationResponse<RecruitmentDTO>> Handle(GetRecruitmentOfSpecificEmployerRequest request, CancellationToken cancellationToken)
    {
        var recruitments = await recruitmentRepository.ListAsync(new GetRecruitmentOfSpecificEmployerSpec(request), cancellationToken);

        List<RecruitmentDTO> response = new();

        foreach (var item in recruitments)
        {
            var recruitmentOwer = await userService.GetUserDetailAsync(item.CompanyId.ToString(), cancellationToken);
            RecruitmentDTO dto = item.Adapt<RecruitmentDTO>();

            dto.CompanyId = item.CompanyId.ToString();
            dto.CompanyName = recruitmentOwer.FullName;
            dto.CompanyAddress = recruitmentOwer.Address;
            dto.NumberOfApplicant = item.UserRecruitments.Count;

            response.Add(dto);
        }

        var count = await recruitmentRepository.CountAsync(new GetRecruitmentOfSpecificEmployerNoPaginationSpec(request), cancellationToken);

        return new PaginationResponse<RecruitmentDTO>(response, count, request.PageNumber, request.PageSize);
    }
}