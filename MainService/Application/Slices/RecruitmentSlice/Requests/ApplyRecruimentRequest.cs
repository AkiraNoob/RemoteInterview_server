using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.FileSlice.Interfaces;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Models;
using Mapster;
using MediatR;
using File = MainService.Domain.Models.File;

namespace MainService.Application.Slices.RecruitmentSlice.Requests;

public class ApplyRecruimentRequest : IRequest<Guid>
{
    public Guid RecruitmentId { get; set; }
    public IFormFile? CV { get; set; }
    public bool UsePreloadedCV { get; set; } = false;

    public ApplyRecruimentRequest(Guid recruitmentId, IFormFile? cV, bool userPreloadedCV)
    {
        RecruitmentId = recruitmentId;
        CV = cV;
        UsePreloadedCV = userPreloadedCV;
    }
}

public class ApplyRecruitmentHandler(
    IRepository<UserRecruitment> userRecruitmentRepository,
    ICurrentUser currentUser,
    IUserService userService,
    IStorageService storageService,
    IRepository<File> fileRepository) : IRequestHandler<ApplyRecruimentRequest, Guid>
{
    private readonly IRepository<UserRecruitment> _userRecruitmentRepository = userRecruitmentRepository;
    private readonly IRepository<File> _fileRepository = fileRepository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUserService _userService = userService;
    private readonly IStorageService _storageService = storageService;

    public async Task<Guid> Handle(ApplyRecruimentRequest request, CancellationToken cancellationToken)
    {
        var userRecruitment = new UserRecruitment(_currentUser.GetUserId(), request.RecruitmentId);

        if (request.UsePreloadedCV)
        {
            var user = await _userService.GetUserDetailAsync(_currentUser.GetUserId().ToString(), cancellationToken);
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
                var result = await _storageService.UploadFileAsync(request.CV, file.Id.ToString(), cancellationToken);

                result.Adapt(file);

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

        return userRecruitment.Id;
    }
}