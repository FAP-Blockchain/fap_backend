using Fap.Domain.DTOs.Common;

namespace Fap.Domain.DTOs.Role
{
    public class GetPermissionsRequest : PaginationRequest
    {
        public Guid? RoleId { get; set; } // Filter by role ID
        public string? RoleName { get; set; } // Filter by role name
    }
}
