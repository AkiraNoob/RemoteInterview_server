using MainService.Application.Common.Models;
using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Requests;

namespace MainService.Application.Slices.RecruitmentSlice.Interfaces;

public interface IRecruitmentService : IScopedService
{
    Task<PaginationResponse<RecruitmentDTO>> GetNewestRecruitmentsAsync(GetNewestRecruitmentsRequest request, CancellationToken cancellationToken);
    Task<PaginationResponse<RecruitmentDTO>> GetListRecruitmentsAsync(SearchRecruitmentsRequest request, CancellationToken cancellationToken);
    Task<PaginationResponse<RecruitmentDTO>> GetSuggestRecruitmentsAsync(GetSuggestRecruitmentsRequest request, CancellationToken cancellationToken);
    Task<PaginationResponse<RecruitmentDTO>> GetRecruitmentOfSpecificEmployerAsync(GetRecruitmentOfSpecificEmployerRequest request, CancellationToken cancellationToken);
    Task<RecruitmentDTO> GetRecruitmentByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<RecruitmentDTO> CreateRecruitmentAsync(CreateRecruitmentRequest request, CancellationToken cancellationToken);
    Task<RecruitmentDTO> UpdateRecruitmentAsync(UpdateRecruitmentRequest request, CancellationToken cancellationToken);
}
