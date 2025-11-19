using System;
using System.Collections.Generic;

namespace Fap.Domain.DTOs.Schedule
{
    // ==================== SCHEDULE ITEM ====================

    public class ScheduleItemDto
    {
        public Guid SlotId { get; set; }
        public Guid ClassId { get; set; }
        public string ClassCode { get; set; }
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public int Credits { get; set; }

        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }

        public Guid? TimeSlotId { get; set; }
        public string? TimeSlotName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        // Teacher info
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherCode { get; set; }

        // Substitute teacher (if any)
        public Guid? SubstituteTeacherId { get; set; }
        public string? SubstituteTeacherName { get; set; }
        public string? SubstitutionReason { get; set; }

        public string Status { get; set; }
        public string? Notes { get; set; }

        // Attendance info
        public bool HasAttendance { get; set; }
        public bool? IsPresent { get; set; } // For student schedule
        public int TotalStudents { get; set; } // For teacher schedule
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
    }

    // ==================== DAILY SCHEDULE ====================

    public class DailyScheduleDto
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }
        public List<ScheduleItemDto> Slots { get; set; } = new();
        public int TotalSlots { get; set; }
    }

    // ==================== WEEKLY SCHEDULE ====================

    public class WeeklyScheduleDto
    {
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string WeekLabel { get; set; } // e.g., "Week 1 - Jan 2024"
        public List<DailyScheduleDto> Days { get; set; } = new();
        public int TotalSlots { get; set; }
    }

    // ==================== SEMESTER SCHEDULE ====================

    public class SemesterScheduleDto
    {
        public Guid SemesterId { get; set; }
        public string SemesterName { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public DateTime SemesterEndDate { get; set; }
        public List<ClassScheduleSummary> Classes { get; set; } = new();
        public int TotalClasses { get; set; }
        public int TotalSlots { get; set; }
    }

    public class ClassScheduleSummary
    {
        public Guid ClassId { get; set; }
        public string ClassCode { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string TeacherName { get; set; }
        public int TotalSlots { get; set; }
        public int CompletedSlots { get; set; }
        public int UpcomingSlots { get; set; }
    }

    // ==================== SCHEDULE REQUESTS ====================

    public class GetScheduleRequest
    {
        public DateTime? Date { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? SemesterId { get; set; }
        public Guid? ClassId { get; set; }
        public string? Status { get; set; } // "Scheduled", "Completed", "Cancelled"
        public bool IncludeAttendance { get; set; } = true;
    }

    public class GetWeeklyScheduleRequest
    {
        public DateTime WeekStartDate { get; set; }
        public Guid? SemesterId { get; set; }
        public bool IncludeAttendance { get; set; } = true;
    }

    // ==================== SCHEDULE CONFLICTS ====================

    public class ScheduleConflictDto
    {
        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public List<ScheduleItemDto> ConflictingSlots { get; set; } = new();
        public string Reason { get; set; }
    }

    // ==================== SCHEDULE STATISTICS ====================

    public class ScheduleStatisticsDto
    {
        public int TotalSlots { get; set; }
        public int ScheduledSlots { get; set; }
        public int CompletedSlots { get; set; }
        public int CancelledSlots { get; set; }
        public int SlotsWithAttendance { get; set; }
        public int SlotsNeedingAttendance { get; set; }

        // For teachers
        public int TotalClasses { get; set; }
        public int TotalStudents { get; set; }

        // For students
        public decimal AttendanceRate { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
    }
}
