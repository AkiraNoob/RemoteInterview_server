using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Specifications;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class GetApplyingListOfRecruitmentRequest : IRequest<ICollection<RecruitmentApplyingResultDTO>>
{
    public Guid RecruitmentId { get; set; }
    public GetApplyingListOfRecruitmentRequest(Guid recruitmentId)
    {
        RecruitmentId = recruitmentId;
    }
}

public class GetApplyingListOfRecruitmentHandler(IRepository<UserRecruitment> userRecruitmentRepository, IUserService userService)
    : IRequestHandler<GetApplyingListOfRecruitmentRequest, ICollection<RecruitmentApplyingResultDTO>>
{
    private readonly IUserService _userService = userService;
    private readonly IRepository<UserRecruitment> _userRecruitmentRepository = userRecruitmentRepository;

    public async Task<ICollection<RecruitmentApplyingResultDTO>> Handle(GetApplyingListOfRecruitmentRequest request, CancellationToken cancellationToken)
    {
        var applyingList = await _userRecruitmentRepository.ListAsync(new UserRecuitmentByRecruimentIdSpec(request.RecruitmentId.ToString()), cancellationToken);

        var userList = await _userService.GetListUsers(applyingList.ConvertAll(x => x.UserId).Distinct(), cancellationToken);

        var response = new List<RecruitmentApplyingResultDTO>();

        foreach (var item in applyingList)
        {
            var user = userList.Where(u => u.UserId == item.UserId.ToString()).FirstOrDefault();
            var result = item.Adapt<RecruitmentApplyingResultDTO>();
            result.User = user.Adapt<ShortenUserDetailDTO>();

            response.Add(result);
        }

        return response;
    }
}
    