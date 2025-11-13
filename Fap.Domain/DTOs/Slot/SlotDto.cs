using System;

namespace Fap.Domain.DTOs.Slot
{
    public class SlotDto
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string ClassCode { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public DateTime Date { get; set; }
        public Guid? TimeSlotId { get; set; }
        public string? TimeSlotName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public Guid? SubstituteTeacherId { get; set; }
        public string? SubstituteTeacherName { get; set; }
        public string? SubstitutionReason { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
        public bool HasAttendance { get; set; }
        public int TotalAttendances { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
