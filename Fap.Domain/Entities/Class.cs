using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fap.Domain.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key] public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string ClassCode { get; set; }

        [Required]
        public Guid SubjectOfferingId { get; set; }

        [ForeignKey(nameof(SubjectOfferingId))]
        public virtual SubjectOffering SubjectOffering { get; set; }


        [Required]
        public Guid TeacherUserId { get; set; }

        [ForeignKey(nameof(TeacherUserId))]
        public virtual Teacher Teacher { get; set; }

        [Range(1, 500)]
        public int MaxEnrollment { get; set; } = 40; 

        public bool IsActive { get; set; } = true;

        // On-chain class id (uint256 on chain); nullable for backward compatibility
        public long? OnChainClassId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<ClassMember> Members { get; set; } = new List<ClassMember>();
        public virtual ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();
        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
    }
}
