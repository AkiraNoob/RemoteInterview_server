using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Domain.Models;
using MediatR;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class RespondRecruitmentApplicationRequest : RespondRecruitmentApplicationDTO, IRequest<string>
{
    public Guid UserRecruitmentId { get; set; }
    public RespondRecruitmentApplicationRequest(Guid userRecruitmentId, RespondRecruitmentApplicationDTO dto)
    {
        UserRecruitmentId = userRecruitmentId;
        Status = dto.Status;
    }
}

public class ResponseRecruitmentApplicationRequestHandler(IRepository<UserRecruitment> userRecruitmentRepository) : IRequestHandler<RespondRecruitmentApplicationRequest, string>
{
    public async Task<string> Handle(RespondRecruitmentApplicationRequest request, CancellationToken cancellationToken)
    {
        var userRecruitment = await userRecruitmentRepository.GetByIdAsync(request.UserRecruitmentId, cancellationToken)
            ?? throw new NotFoundException("User recruitment not found.");

        userRecruitment.Status = request.Status;
        
        await userRecruitmentRepository.UpdateAsync(userRecruitment, cancellationToken);

        return userRecruitment.Id.ToString();
    }
}