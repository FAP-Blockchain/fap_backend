using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface IActionLogRepository : IGenericRepository<ActionLog>
    {
        Task<(List<ActionLog> Logs, int TotalCount)> GetPagedActionLogsAsync(
            int page,
            int pageSize,
            string? searchTerm,
            Guid? userId,
            Guid? credentialId,
            string? action,
            DateTime? from,
            DateTime? to,
            string? sortBy,
            string? sortOrder);
    }
}
