using System;

namespace Fap.Domain.DTOs.ActionLog
{
    public class ActionLogDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Detail { get; set; }

        public Guid UserId { get; set; }
        public string? UserFullName { get; set; }
        public string? UserEmail { get; set; }

        public Guid? CredentialId { get; set; }
    }
}
