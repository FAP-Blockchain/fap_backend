using Fap.Api.Interfaces;
using Fap.Domain.DTOs.ActionLog;
using Fap.Domain.DTOs.Common;
using Fap.Domain.Repositories;

namespace Fap.Api.Services
{
    public class ActionLogService : IActionLogService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ActionLogService> _logger;

        public ActionLogService(IUnitOfWork uow, ILogger<ActionLogService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<PagedResult<ActionLogDto>> GetActionLogsAsync(GetActionLogsRequest request)
        {
            try
            {
                var (logs, totalCount) = await _uow.ActionLogs.GetPagedActionLogsAsync(
                    request.Page,
                    request.PageSize,
                    request.SearchTerm,
                    request.UserId,
                    request.CredentialId,
                    request.Action,
                    request.From,
                    request.To,
                    request.SortBy,
                    request.SortOrder);

                var items = logs.Select(x => new ActionLogDto
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Action = x.Action,
                    Detail = x.Detail,
                    UserId = x.UserId,
                    UserFullName = x.User?.FullName,
                    UserEmail = x.User?.Email,
                    CredentialId = x.CredentialId
                }).ToList();

                return new PagedResult<ActionLogDto>(items, totalCount, request.Page, request.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting action logs");
                throw;
            }
        }
    }
}
