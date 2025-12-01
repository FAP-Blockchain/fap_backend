using Fap.Domain.DTOs.Subject;

namespace Fap.Domain.DTOs.Semester
{
    public class SemesterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSubjects { get; set; }
        public bool IsActive { get; set; }
        public bool IsClosed { get; set; }
    }

    public class SemesterDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsClosed { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalClasses { get; set; }
        public int TotalStudentsEnrolled { get; set; }

        // Use SubjectOfferings instead of Subjects
        public List<SubjectOfferingDto> SubjectOfferings { get; set; } = new();
    }
}
