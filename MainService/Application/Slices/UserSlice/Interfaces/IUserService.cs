using Ardalis.Result;
using MainService.Application.Interfaces;
using MainService.Application.Slices.UserSlice.DTOs;

namespace MainService.Application.Slices.UserSlice.Interfaces;

public interface IUserService : IScopedService
{
    Task<string?> GetUserIdAsync(string userIdentifier, CancellationToken cancellationToken = default);
    Task<bool> CheckUserExistedAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);
    Task<UserDetailDTO> GetUserDetailAsync(string userId, CancellationToken cancellationToken = default);
    Task<ICollection<UserDetailDTO>> GetListUsersAsync(IEnumerable<Guid> listIds, CancellationToken cancellationToken = default);
    Task<ICollection<UserDetailDTO>> GetListUsersAsync(IEnumerable<string> listEmails, CancellationToken cancellationToken = default);
    Task<Result<string>> CreateUserAsync(CreateUserDTO payload, CancellationToken cancellationToken);
    Task<Result<string>> UpdateUserInfoAsync(UpdateUserInfoDTO payload, CancellationToken cancellationToken);
    Task<Result<string>> RegisterCompanyAsync(string userId, CancellationToken cancellationToken);
}