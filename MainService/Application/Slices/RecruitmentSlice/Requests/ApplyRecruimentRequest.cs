using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.FileSlice.Interfaces;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Models;
using MediatR;
using File = MainService.Domain.Models.File;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class ApplyRecruimentRequest : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid RecruitmentId { get; set; }
    public IFormFile? CV { get; set; }
    public bool UsePreloadedCV { get; set; } = false;

    public ApplyRecruimentRequest(Guid userId, Guid recruitmentId, IFormFile? cV, bool userPreloadedCV)
    {
        UserId = userId;
        RecruitmentId = recruitmentId;
        CV = cV;
        UsePreloadedCV = userPreloadedCV;
    }
}

public class ApplyRecruitmentHandler : IRequestHandler<ApplyRecruimentRequest, Guid>
{
    private readonly IRepository<UserRecruitment> _userRecruitmentRepository;
    private readonly IRepository<File> _fileRepository;
    private readonly IUserService _userService;
    private readonly IStorageService _storageService;

    public ApplyRecruitmentHandler(
        IRepository<UserRecruitment> userRecruitmentRepository,
        IUserService userService,
        IStorageService storageService
        )
    {
        _userService = userService;
        _userRecruitmentRepository = userRecruitmentRepository;
        _storageService = storageService;
    }

    public async Task<Guid> Handle(ApplyRecruimentRequest request, CancellationToken cancellationToken)
    {
        var userRecruitment = new UserRecruitment(request.UserId, request.RecruitmentId);

        if (request.UsePreloadedCV)
        {
            var user = await _userService.GetUserDetail(request.UserId.ToString(), cancellationToken);
            if (user.CV != null)
            {
                userRecruitment.FileId = user.CV.FileId;
            }
            else
            {
                throw new BadRequestException("User havent uploaded any CV yet.");
            }
        }
        else
        {
            if (request.CV != null)
            {
                var file = new File();
                var result = await _storageService.UploadImageAsync(request.CV, file.Id.ToString());

                if (result != null)
                {
                    file.FileName = result.FileName;
                    file.FileUrl = result.FileUrl;
                    file.FileType = result.FileType;
                }

                await _fileRepository.AddAsync(file, cancellationToken);
                await _fileRepository.SaveChangesAsync(cancellationToken);

                userRecruitment.FileId = file.Id;
            }
            else
            {
                throw new BadRequestException("Please upload a file.");
            }
        }

        await _userRecruitmentRepository.AddAsync(userRecruitment, cancellationToken);
        await _userRecruitmentRepository.SaveChangesAsync(cancellationToken);

        return userRecruitment.Id;
    }
}