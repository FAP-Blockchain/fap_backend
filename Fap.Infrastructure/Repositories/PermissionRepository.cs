using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fap.Infrastructure.Repositories
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(FapDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Permission>> GetByRoleIdAsync(Guid roleId)
        {
            return await _dbSet
                .Where(p => p.RoleId == roleId)
                .OrderBy(p => p.Code)
                .ToListAsync();
        }

        public async Task<Permission?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Code.ToLower() == code.ToLower());
        }

        public async Task DeleteByRoleIdAsync(Guid roleId)
        {
            var permissions = await _dbSet
                .Where(p => p.RoleId == roleId)
                .ToListAsync();

            _dbSet.RemoveRange(permissions);
        }

        public async Task<(List<Permission> Permissions, int TotalCount)> GetPagedPermissionsAsync(
            int page,
            int pageSize,
            string? searchTerm,
            Guid? roleId,
            string? roleName,
            string? sortBy,
            string? sortOrder)
        {
            var query = _dbSet
                .Include(p => p.Role)
                .AsQueryable();

            // 1. Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p =>
                    p.Code.Contains(searchTerm) ||
                    (p.Description != null && p.Description.Contains(searchTerm)) ||
                    p.Role.Name.Contains(searchTerm)
                );
            }

            if (roleId.HasValue)
            {
                query = query.Where(p => p.RoleId == roleId.Value);
            }

            if (!string.IsNullOrWhiteSpace(roleName))
            {
                query = query.Where(p => p.Role.Name.Contains(roleName));
            }

            // 2. Get total count
            var totalCount = await query.CountAsync();

            // 3. Apply sorting
            query = sortBy?.ToLower() switch
            {
                "code" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Code)
                    : query.OrderBy(p => p.Code),
                "description" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Description ?? "")
                    : query.OrderBy(p => p.Description ?? ""),
                "rolename" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(p => p.Role.Name)
                    : query.OrderBy(p => p.Role.Name),
                _ => query.OrderBy(p => p.Code) // Default sort
            };

            // 4. Apply pagination
            var permissions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (permissions, totalCount);
        }
    }
}
