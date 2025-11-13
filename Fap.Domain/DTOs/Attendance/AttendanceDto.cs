using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Attendance;

public class AttendanceDto
{
    public Guid Id { get; set; }
    public Guid SlotId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentCode { get; set; }
    public string StudentName { get; set; }
    public Guid SubjectId { get; set; }
    public string SubjectName { get; set; }
    public DateTime Date { get; set; }
    public string TimeSlotName { get; set; }
    public string ClassCode { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
    public bool IsExcused { get; set; }
    public string? ExcuseReason { get; set; }
    public DateTime RecordedAt { get; set; }
}

public class AttendanceDetailDto : AttendanceDto
{
    public string StudentEmail { get; set; }
    public string TeacherName { get; set; }
    public string SemesterName { get; set; }
    public string SlotStatus { get; set; }
}

public class AttendanceStatisticsDto
{
    public Guid StudentId { get; set; }
    public string StudentCode { get; set; }
    public string StudentName { get; set; }
    public int TotalSlots { get; set; }
    public int PresentCount { get; set; }
    public int AbsentCount { get; set; }
    public int ExcusedCount { get; set; }
    public decimal AttendanceRate { get; set; }
    public List<AttendanceDto> AttendanceRecords { get; set; }
}

public class ClassAttendanceReportDto
{
    public Guid ClassId { get; set; }
    public string ClassCode { get; set; }
    public string SubjectName { get; set; }
    public string TeacherName { get; set; }
    public int TotalSlots { get; set; }
    public int TotalStudents { get; set; }
    public decimal AverageAttendanceRate { get; set; }
    public List<StudentAttendanceSummary> StudentSummaries { get; set; }
}

public class StudentAttendanceSummary
{
    public Guid StudentId { get; set; }
    public string StudentCode { get; set; }
    public string StudentName { get; set; }
    public int PresentCount { get; set; }
    public int AbsentCount { get; set; }
    public int ExcusedCount { get; set; }
    public decimal AttendanceRate { get; set; }
}

public class TakeAttendanceRequest
{
    [Required]
    public Guid SlotId { get; set; }
    [Required]
    public List<StudentAttendanceDto> Students { get; set; }
}

public class StudentAttendanceDto
{
    [Required]
    public Guid StudentId { get; set; }
    [Required]
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}

public class UpdateAttendanceRequest
{
    [Required]
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}

public class ExcuseAbsenceRequest
{
    [Required]
    [MinLength(10, ErrorMessage = "Excuse reason must be at least 10 characters")]
    [MaxLength(1000, ErrorMessage = "Excuse reason cannot exceed 1000 characters")]
    public string Reason { get; set; }
}

public class AttendanceFilterRequest
{
    public Guid? ClassId { get; set; }
    public Guid? StudentId { get; set; }
    public Guid? SubjectId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? IsPresent { get; set; }
    public bool? IsExcused { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
