using System;

namespace Fap.Domain.DTOs.Class
{
    public class ClassDto
    {
        public Guid Id { get; set; }
        public string ClassCode { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public int Credits { get; set; }
        public string TeacherName { get; set; }
        public string TeacherCode { get; set; }
        public string SemesterName { get; set; }
        public int TotalStudents { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalSlots { get; set; }
    }

    public class ClassDetailDto
    {
        public Guid Id { get; set; }
        public string ClassCode { get; set; }
        
        // Subject Info
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public int Credits { get; set; }
        
        // Teacher Info
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherCode { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherPhone { get; set; }
        
        // Semester Info
        public string SemesterName { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public DateTime SemesterEndDate { get; set; }
        
        // Statistics
        public int TotalStudents { get; set; }
        public int TotalEnrollments { get; set; }
        public int ApprovedEnrollments { get; set; }
        public int PendingEnrollments { get; set; }
        public int TotalSlots { get; set; }
        public int CompletedSlots { get; set; }
        public int ScheduledSlots { get; set; }
        
        // Students in class
        public List<ClassStudentInfo> Students { get; set; } = new();
        
        // Enrollments
        public List<ClassEnrollmentInfo> Enrollments { get; set; } = new();
        
        // Schedule
        public List<ClassSlotInfo> Slots { get; set; } = new();
    }

    public class ClassStudentInfo
    {
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal GPA { get; set; }
        public DateTime JoinedAt { get; set; }
    }

    public class ClassEnrollmentInfo
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public DateTime RegisteredAt { get; set; }
        public bool IsApproved { get; set; }
    }

    public class ClassSlotInfo
    {
        public Guid SlotId { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlotName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public string? SubstituteTeacherName { get; set; }
        public int TotalAttendances { get; set; }
    }
}
