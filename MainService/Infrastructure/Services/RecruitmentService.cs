using MainService.Application.Common.Models;
using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Application.Slices.RecruitmentSlice.Interfaces;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using MainService.Application.Slices.RecruitmentSlice.Specifications;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Enums;
using MainService.Domain.Models;
using MainService.Infrastructure.Persistence.Context;
using MainService.Infrastructure.Persistence.Extension;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace MainService.Infrastructure.Services;

public class RecruitmentService(ApplicationDbContext dbContext, ICurrentUser currentUser, IUserService userService) : IRecruitmentService
{
    private async Task<RecruitmentDTO> ConvertRecruimentToRecruitmentDTO(Recruitment recruitment, CancellationToken cancellationToken = default)
    {
        var recruitmentOwner = await userService.GetUserDetailAsync(recruitment.CompanyId.ToString(), cancellationToken);

        RecruitmentDTO dto = recruitment.Adapt<RecruitmentDTO>();

        dto.CompanyId = recruitment.CompanyId.ToString();
        dto.CompanyName = recruitmentOwner.FullName;
        dto.CompanyAddress = recruitmentOwner.Address;

        return dto;
    }

    private async Task<List<RecruitmentDTO>> ConvertRecruimentToRecruitmentDTO(ICollection<Recruitment> recruitments, CancellationToken cancellationToken = default)
    {
        List<RecruitmentDTO> recruitmentDTOs = new();
        foreach (var item in recruitments)
        {
            var recruitmentOwner = await userService.GetUserDetailAsync(item.CompanyId.ToString(), cancellationToken);

            RecruitmentDTO dto = item.Adapt<RecruitmentDTO>();

            dto.CompanyId = item.CompanyId.ToString();
            dto.CompanyName = recruitmentOwner.FullName;
            dto.CompanyAddress = recruitmentOwner.Address;

            recruitmentDTOs.Add(dto);
        }

        return recruitmentDTOs;
    }

    public async Task<RecruitmentDTO> CreateRecruitmentAsync(CreateRecruitmentRequest request, CancellationToken cancellationToken)
    {
        var recruitment = new Recruitment(
            currentUser.GetUserId(),
            request.Title,
            //request.Motivation,
            request.Description,
            request.Requirement,
            request.Welfare,
            request.Address,
            request.ProvinceId,
            request.DistrictId,
            request.MinExperience,
            request.MaxSalary,
            request.ProfessionId,
            request.ExpiredDate
        );

        await dbContext.Recruitment.AddAsync(recruitment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = await ConvertRecruimentToRecruitmentDTO(recruitment, cancellationToken);

        return response;
    }

    public async Task<PaginationResponse<RecruitmentDTO>> GetListRecruitmentsAsync(SearchRecruitmentsRequest request, CancellationToken cancellationToken)
    {
        var similarityThreshold = 0.3;

        var queryable = dbContext.Recruitment
                        .Include(x => x.Profession)
            .Include(x => x.UserRecruitments)
            .Where(u => PgTrgmDbFunctions.Similarity(u.Title, (request.Keyword ?? "")) > similarityThreshold)
            .AsQueryable();

        if (request.MinSalary != null)
        {
            queryable = queryable.Where(x => x.MaxSalary >= request.MinSalary);
        }

        if (request.MaxSalary != null)
        {
            queryable = queryable.Where(x => x.MaxSalary <= request.MaxSalary);
        }

        if (request.MinExperience != null)
        {
            queryable = queryable.Where(x => x.MinExperience >= request.MinExperience);
        }

        if (request.ProvinceId != null && request.DistrictId != null)
        {
            queryable = queryable.Where(x => x.ProvinceId == request.ProvinceId && x.DistrictId == request.DistrictId);
        }

        if (request.KeywordIds != null && request.KeywordIds.Count > 0)
        {
            queryable = queryable.Where(r => r.RecruitmentKeywords.Any(rt => request.KeywordIds.Contains(rt.KeywordId)));
        }

        var listRecruitments = await ConvertRecruimentToRecruitmentDTO(await queryable
                .OrderByDescending(u => PgTrgmDbFunctions.Similarity(u.Title, (request.Keyword ?? "")))
                .ThenByDescending(u => u.CreatedOn)
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync(cancellationToken), cancellationToken);

        var count = await queryable
            .CountAsync(cancellationToken);

        var response = new PaginationResponse<RecruitmentDTO>(listRecruitments, count, request.PageNumber, request.PageSize);

        return response;
    }

    public async Task<PaginationResponse<RecruitmentDTO>> GetNewestRecruitmentsAsync(GetNewestRecruitmentsRequest request, CancellationToken cancellationToken)
    {
        var queryable = dbContext.Recruitment
                        .Include(x => x.Profession)
            .Include(x => x.UserRecruitments)
            .AsQueryable();

        if (request.MinSalary != null)
        {
            queryable = queryable.Where(x => x.MaxSalary >= request.MinSalary);
        }

        if (request.MinExperience != null)
        {
            queryable = queryable.Where(x => x.MinExperience >= request.MinExperience);
        }

        if (request.ProvinceId != null)
        {
            queryable = queryable.Where(x => x.ProvinceId == request.ProvinceId);
        }

        if (request.TagId != null)
        {
            queryable = queryable.Where(r => r.RecruitmentKeywords.Any(rt => request.TagId == rt.KeywordId));
        }

        var listRecruitments = await ConvertRecruimentToRecruitmentDTO(await queryable
                .OrderByDescending(u => u.CreatedOn)
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync(cancellationToken), cancellationToken);

        var count = await queryable.CountAsync(cancellationToken);

        return new PaginationResponse<RecruitmentDTO>(listRecruitments, count, request.PageNumber, request.PageSize);
    }

    public async Task<RecruitmentDTO> GetRecruitmentByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var recruitment = await dbContext.Recruitment
            .Include(x => x.Profession)
            .Include(x => x.UserRecruitments)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new NotFoundException("Recruitment not found");

        return await ConvertRecruimentToRecruitmentDTO(recruitment, cancellationToken);
    }

    public async Task<PaginationResponse<RecruitmentDTO>> GetRecruitmentOfSpecificEmployerAsync(GetRecruitmentOfSpecificEmployerRequest request, CancellationToken cancellationToken)
    {
        var queryable = dbContext.Recruitment
                        .Include(x => x.Profession)
            .Include(x => x.UserRecruitments)
            .Where(x => x.CompanyId == request.EmployerId)
            .Where(x => request.Status == RecruitmentStatusEnum.Open ? x.ExpiredDate > DateTime.UtcNow : x.ExpiredDate <= DateTime.UtcNow)
            .AsQueryable();

        var recruitments = await ConvertRecruimentToRecruitmentDTO(await queryable
            .ToListAsync(cancellationToken), cancellationToken);

        var count = await queryable.CountAsync(cancellationToken);

        return new PaginationResponse<RecruitmentDTO>(recruitments, count, request.PageNumber, request.PageSize);
    }

    public async Task<PaginationResponse<RecruitmentDTO>> GetSuggestRecruitmentsAsync(GetSuggestRecruitmentsRequest request, CancellationToken cancellationToken)
    {
        var currentJob = await dbContext.Recruitment
            .FirstOrDefaultAsync(j => j.Id == request.RecruitmentId, cancellationToken) ?? throw new NotFoundException("Recruitment not found.");

        var queryable = dbContext.Recruitment
                        .Include(x => x.Profession)
            .Include(x => x.UserRecruitments)
            .Where(j => j.Id != request.RecruitmentId)
            .Select(j => new
            {
                Job = j,
                Score =
                    (j.ProfessionId == currentJob.ProfessionId ? 5 : 0) +
                    (j.ProvinceId == currentJob.ProvinceId ? j.DistrictId == currentJob.DistrictId ? 5 : 4 : 0) +
                    (Math.Abs(j.MaxSalary - currentJob.MaxSalary) <= 2000000 ? 3 : 0) +
                    (j.MinExperience <= currentJob.MinExperience ? 2 : 0)
            })
            .AsQueryable();

        var result = await queryable
            .OrderByDescending(x => x.Score)
            .ThenByDescending(x => x.Job.CreatedOn)
            .Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .Select(x => x.Job)
            .ToListAsync(cancellationToken);

        var count = await queryable.CountAsync(cancellationToken);

        return new PaginationResponse<RecruitmentDTO>(
            (await ConvertRecruimentToRecruitmentDTO(result, cancellationToken)),
            count,
            request.PageNumber,
            request.PageSize);
    }

    public async Task<RecruitmentDTO> UpdateRecruitmentAsync(UpdateRecruitmentRequest request, CancellationToken cancellationToken)
    {
        var recruitment = await dbContext.Recruitment
            .FirstOrDefaultAsync(x => x.Id == request.RecruitmentId, cancellationToken) ?? throw new NotFoundException("Recruitment not found");

        request.Adapt(recruitment);

        dbContext.Recruitment.Update(recruitment);

        await dbContext.SaveChangesAsync(cancellationToken);

        return await ConvertRecruimentToRecruitmentDTO(recruitment, cancellationToken);
    }
}
