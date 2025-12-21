using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories
{
    public class ActionLogRepository : GenericRepository<ActionLog>, IActionLogRepository
    {
        public ActionLogRepository(FapDbContext context) : base(context)
        {
        }

        public async Task<(List<ActionLog> Logs, int TotalCount)> GetPagedActionLogsAsync(
            int page,
            int pageSize,
            string? searchTerm,
            Guid? userId,
            Guid? credentialId,
            string? action,
            DateTime? from,
            DateTime? to,
            string? sortBy,
            string? sortOrder)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(x => x.User)
                .AsQueryable();

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId.Value);

            if (credentialId.HasValue)
                query = query.Where(x => x.CredentialId == credentialId.Value);

            if (!string.IsNullOrWhiteSpace(action))
                query = query.Where(x => x.Action == action);

            if (from.HasValue)
                query = query.Where(x => x.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(x => x.CreatedAt <= to.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(x =>
                    (x.Detail != null && x.Detail.Contains(term)) ||
                    x.Action.Contains(term) ||
                    x.User.FullName.Contains(term) ||
                    x.User.Email.Contains(term));
            }

            var totalCount = await query.CountAsync();

            query = ApplySorting(query, sortBy, sortOrder);

            var logs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (logs, totalCount);
        }

        private static IQueryable<ActionLog> ApplySorting(IQueryable<ActionLog> query, string? sortBy, string? sortOrder)
        {
            var isDescending = sortOrder?.Trim().ToLowerInvariant() == "desc";

            return sortBy?.Trim().ToLowerInvariant() switch
            {
                "action" => isDescending
                    ? query.OrderByDescending(x => x.Action).ThenByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.Action).ThenByDescending(x => x.CreatedAt),

                "createdat" => isDescending
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt),

                _ => query.OrderByDescending(x => x.CreatedAt)
            };
        }
    }
}
