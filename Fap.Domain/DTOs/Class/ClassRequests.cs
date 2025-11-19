using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Class
{
    public class CreateClassRequest
    {
        [Required, MaxLength(50)]
        public string ClassCode { get; set; }

        // ? Only SubjectOfferingId - it contains Subject info via navigation
        [Required]
        public Guid SubjectOfferingId { get; set; }

        [Required]
        public Guid TeacherId { get; set; }
    }

    public class UpdateClassRequest
    {
        [Required, MaxLength(50)]
        public string ClassCode { get; set; }

        // ? Only SubjectOfferingId - it contains Subject info via navigation
        [Required]
        public Guid SubjectOfferingId { get; set; }

        [Required]
        public Guid TeacherId { get; set; }
    }

    public class ClassResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? ClassId { get; set; }
        public string ClassCode { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class ClassRosterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SearchTerm { get; set; }
        public bool? IsApproved { get; set; }
    }

    public class ClassRosterDto
    {
        public List<ClassStudentInfo> Students { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }

    // ==================== ASSIGN STUDENTS TO CLASS ====================

    public class AssignStudentsRequest
    {
        [Required]
        public List<Guid> StudentIds { get; set; } = new();
    }

    public class AssignStudentsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int TotalAssigned { get; set; }
        public int TotalFailed { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<AssignedStudentInfo> AssignedStudents { get; set; } = new();
    }

    public class RemoveStudentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }
}
