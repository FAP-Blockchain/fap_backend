using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fap.Domain.Entities
{
    /// <summary>
    /// Represents a subject being offered in a specific semester
    /// Môn học được mở trong một kỳ cụ thể
    /// </summary>
    [Table("SubjectOfferings")]
    public class SubjectOffering
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Reference to the master subject
        /// </summary>
        [Required]
        public Guid SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; }

        /// <summary>
        /// Reference to the semester
        /// </summary>
        [Required]
        public Guid SemesterId { get; set; }

        [ForeignKey(nameof(SemesterId))]
        public virtual Semester Semester { get; set; }

        /// <summary>
        /// Maximum number of classes for this offering
        /// </summary>
        [Range(1, 50)]
        public int MaxClasses { get; set; } = 10;

        /// <summary>
        /// Is this offering currently accepting enrollments?
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Optional: Custom capacity for this semester
        /// Total students across all classes
        /// </summary>
        [Range(0, 10000)]
        public int? SemesterCapacity { get; set; }

        /// <summary>
        /// Registration start date for this offering
        /// </summary>
        public DateTime? RegistrationStartDate { get; set; }

        /// <summary>
        /// Registration end date for this offering
        /// </summary>
        public DateTime? RegistrationEndDate { get; set; }

        /// <summary>
        /// Notes or special instructions for this offering
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
