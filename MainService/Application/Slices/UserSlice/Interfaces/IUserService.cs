using MainService.Application.Interfaces;
using MainService.Application.Slices.UserSlice.DTOs;

namespace MainService.Application.Slices.UserSlice.Interfaces;

public interface IUserService : IScopedService
{
    Task<string?> GetUserId(string userIdentifier, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);
    Task<UserDetailDTO> GetUserDetail(string userId, CancellationToken cancellationToken = default);
    Task<ICollection<UserDetailDTO>> GetListUsers(IEnumerable<Guid> listIds, CancellationToken cancellationToken = default);
    Task<ICollection<UserDetailDTO>> GetListUsers(IEnumerable<string> listEmails, CancellationToken cancellationToken = default);
}