using System;

namespace Fap.Domain.DTOs.Slot
{
    public class SlotFilterRequest
    {
        public Guid? ClassId { get; set; }
        public Guid? TeacherId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Status { get; set; } // "Scheduled", "Completed", "Cancelled"
        public bool? HasAttendance { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
