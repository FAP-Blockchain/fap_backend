using Fap.Domain.DTOs.ActionLog;
using Fap.Domain.DTOs.Common;

namespace Fap.Api.Interfaces
{
    public interface IActionLogService
    {
        Task<PagedResult<ActionLogDto>> GetActionLogsAsync(GetActionLogsRequest request);
    }
}
