namespace Fap.Domain.DTOs.Subject
{
    /// <summary>
    /// Basic subject DTO - Master data (no semester)
    /// </summary>
    public class SubjectDto
    {
        public Guid Id { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Credits { get; set; }
        public string? Category { get; set; }
        public string? Department { get; set; }
        public string? Prerequisites { get; set; }

        // ✅ CHANGED: Show total offerings instead of being tied to one semester
        public int TotalOfferings { get; set; }
    }

    /// <summary>
    /// Detailed subject DTO with offerings
    /// </summary>
    public class SubjectDetailDto
    {
        public Guid Id { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Credits { get; set; }
        public string? Category { get; set; }
        public string? Department { get; set; }
        public string? Prerequisites { get; set; }

        // ✅ NEW: Include all offerings (subject can be in multiple semesters)
        public List<SubjectOfferingDto> Offerings { get; set; } = new();

        // Statistics across all offerings
        public int TotalOfferings { get; set; }
        public int TotalClasses { get; set; }
        public int TotalStudentsEnrolled { get; set; }
    }

    /// <summary>
    /// Summary DTO for lists
    /// </summary>
    public class SubjectSummaryDto
    {
        public Guid Id { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int TotalClasses { get; set; }
    }

    public class ClassSummaryDto
    {
        public Guid Id { get; set; }
        public string ClassCode { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public int CurrentEnrollment { get; set; }
        public int MaxEnrollment { get; set; }
    }
}
