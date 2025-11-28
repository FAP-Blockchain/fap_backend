using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fap.Domain.DTOs.Slot;

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

    [Range(1, 500)]
    public int MaxEnrollment { get; set; } = 40;

        /// <summary>
        /// Optional initial slots that should be generated immediately after the class is created.
        /// </summary>
        public List<CreateClassSlotRequest> InitialSlots { get; set; } = new();
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

        [Range(1, 500)]
        public int MaxEnrollment { get; set; } = 40;

        /// <summary>
        /// Optional slots to append to the class schedule after the class details are updated.
        /// </summary>
        public List<CreateClassSlotRequest> AdditionalSlots { get; set; } = new();
    }

    public class ClassResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? ClassId { get; set; }
        public string ClassCode { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<Guid> CreatedSlotIds { get; set; } = new();
        public List<string> SlotErrors { get; set; } = new();
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
