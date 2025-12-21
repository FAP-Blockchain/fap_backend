using System;
using Fap.Domain.DTOs.Common;

namespace Fap.Domain.DTOs.ActionLog
{
    public class GetActionLogsRequest : PaginationRequest
    {
        public Guid? UserId { get; set; }
        public Guid? CredentialId { get; set; }
        public string? Action { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
