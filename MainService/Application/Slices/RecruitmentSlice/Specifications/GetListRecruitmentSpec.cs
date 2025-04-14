using Ardalis.Specification;
using MainService.Application.Slices.RecruitmentSlice.Requests;
using MainService.Domain.Models;
using TMS.Application.Common.Specification;

namespace MainService.Application.Slices.RecruitmentSlice.Specifications;

public class GetListRecruitmentSpec(GetListRecruitmentsRequest request) : EntitiesBySimplePaginationSpec<Recruitment>(request)
{
}