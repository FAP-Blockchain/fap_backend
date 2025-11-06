using Fap.Domain.DTOs.Role;

namespace Fap.Api.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(Guid roleId);
        Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);
        Task<RoleResponse> UpdateRoleAsync(Guid roleId, UpdateRoleRequest request);
        Task<AssignPermissionsResponse> AssignPermissionsAsync(Guid roleId, AssignPermissionsRequest request);
        Task<RoleResponse> DeleteRoleAsync(Guid roleId);
        Task<List<PermissionDto>> GetAllPermissionsAsync();
        Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(Guid roleId);
        Task<RoleResponse> RemovePermissionAsync(Guid roleId, Guid permissionId);
    }
}
