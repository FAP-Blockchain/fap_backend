using Fap.Api.Interfaces;
using Fap.Api.Services;
using Fap.Domain.DTOs.Role;
using Fap.Domain.DTOs.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]  // ? Ch? Admin m?i có quy?n xem permissions
    public class PermissionsController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(IRoleService roleService, IPermissionService permissionService, ILogger<PermissionsController> logger)
        {
            _roleService = roleService;
            _permissionService = permissionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPermissions([FromQuery] GetPermissionsRequest request)
        {
            

            try
            {
                var results = await _permissionService.GetPermissionsAsync(request);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting permissions: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving permissions" });
            }
        }

        
        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRoleId(Guid roleId)
        {

            try
            {
                var role = await _roleService.GetRoleByIdAsync(roleId);

                if (role == null)
                    return NotFound(new { message = $"Role with ID '{roleId}' not found" });

                var permissions = await _roleService.GetPermissionsByRoleIdAsync(roleId);

                return Ok(new
                {
                    success = true,
                    roleId = role.Id,
                    roleName = role.Name,
                    permissions = permissions,
                    count = permissions.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting permissions {roleId}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving permissions" });
            }
        }

    }
}
