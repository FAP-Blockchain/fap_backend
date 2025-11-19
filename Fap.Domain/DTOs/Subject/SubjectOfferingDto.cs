namespace Fap.Domain.DTOs.Subject
{
    /// <summary>
    /// DTO for SubjectOffering - represents a subject offered in a specific semester
    /// </summary>
    public class SubjectOfferingDto
    {
        public Guid Id { get; set; }

        // Subject info
        public Guid SubjectId { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int Credits { get; set; }

        // Semester info
        public Guid SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;

        // Offering configuration
        public int MaxClasses { get; set; }
        public int? SemesterCapacity { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }

        // Statistics
        public int TotalClasses { get; set; }
        public int TotalStudents { get; set; }
    }
}
