using Ardalis.Specification;
using MainService.Application.Common.Models;
using MainService.Application.Interfaces;
using MainService.Application.Slices.FileSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Specifications;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Enums;
using MainService.Domain.Models;
using Mapster;
using MediatR;
using File = MainService.Domain.Models.File;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class GetApplyingListOfRecruitmentRequest : SimplePaginationFilter, IRequest<PaginationResponse<RecruitmentApplyingResultDTO>>
{
    public Guid RecruitmentId { get; set; }
    public UserRecruitmentStatusEnum? Status { get; set; } = null;
}

public class GetApplyingListOfRecruitmentRequestHandler(IRepository<UserRecruitment> userRecruitmentRepository, IUserService userService)
    : IRequestHandler<GetApplyingListOfRecruitmentRequest, PaginationResponse<RecruitmentApplyingResultDTO>>
{
    private readonly IUserService _userService = userService;
    private readonly IRepository<UserRecruitment> _userRecruitmentRepository = userRecruitmentRepository;

    public async Task<PaginationResponse<RecruitmentApplyingResultDTO>> Handle(GetApplyingListOfRecruitmentRequest request, CancellationToken cancellationToken)
    {
        var applyingList = await _userRecruitmentRepository.ListAsync(new GetUserRecuitmentByRecruimentIdSpec(request), cancellationToken);
        var count = await _userRecruitmentRepository.CountAsync(new GetUserRecuitmentByRecruimentIdSpec(request), cancellationToken);

        var userList = await _userService.GetListUsersAsync(applyingList.Select(x => x.UserId).Distinct().ToList(), cancellationToken);

        var applyingListResult = applyingList.Adapt<List<RecruitmentApplyingResultDTO>>().Select(x =>
        {
            var user = userList.Where(u => u.Id == x.UserId.ToString()).FirstOrDefault();
            x.User = user.Adapt<ShortenUserDetailDTO>();
            return x;
        }).ToList();

        return new PaginationResponse<RecruitmentApplyingResultDTO>(applyingListResult, count, request.PageNumber, request.PageSize);
    }
}