using AutoMapper;
using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Common;
using Fap.Domain.DTOs.Role;
using Fap.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fap.Api.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(IUnitOfWork uow, IMapper mapper, ILogger<PermissionService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        // ========== GET ALL PERMISSIONS ==========
        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissions = await _uow.Permissions.GetAllAsync();

            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                Code = p.Code,
                Description = p.Description
            }).OrderBy(p => p.Code).ToList();
        }

        // ========== GET PERMISSIONS WITH PAGINATION ==========
        public async Task<PagedResult<PermissionDto>> GetPermissionsAsync(GetPermissionsRequest request)
        {
            try
            {
                var (permissions, totalCount) = await _uow.Permissions.GetPagedPermissionsAsync(
                    request.Page,
                    request.PageSize,
                    request.SearchTerm,
                    request.RoleId,
                    request.RoleName,
                    request.SortBy,
                    request.SortOrder
                );

                var permissionDtos = permissions.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Description = p.Description
                }).ToList();

                return new PagedResult<PermissionDto>(
                    permissionDtos,
                    totalCount,
                    request.Page,
                    request.PageSize
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting permissions: {ex.Message}");
                throw;
            }
        }

        // ========== GET PERMISSIONS BY ROLE ==========
        public async Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            try
            {
                var permissions = await _uow.Permissions.GetByRoleIdAsync(roleId);

                return permissions.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Description = p.Description
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting permissions for role {roleId}: {ex.Message}");
                throw;
            }
        }
    }
}
