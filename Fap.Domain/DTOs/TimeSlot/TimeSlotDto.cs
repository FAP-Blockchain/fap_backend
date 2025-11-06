using System;
using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.TimeSlot
{
    public class TimeSlotDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int DurationMinutes { get; set; }
        public int TotalSlots { get; set; }
    }

    public class CreateTimeSlotRequest
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }

    public class UpdateTimeSlotRequest
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }

    public class TimeSlotResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? TimeSlotId { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
