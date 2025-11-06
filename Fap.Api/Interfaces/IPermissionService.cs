using Fap.Domain.DTOs.Role;
using Fap.Domain.DTOs.Common;

namespace Fap.Api.Interfaces
{
    public interface IPermissionService
    {
        Task<List<PermissionDto>> GetAllPermissionsAsync();
        Task<PagedResult<PermissionDto>> GetPermissionsAsync(GetPermissionsRequest request);
        Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(Guid roleId);
    }
}
