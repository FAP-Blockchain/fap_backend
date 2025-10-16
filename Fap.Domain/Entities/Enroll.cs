using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fap.Domain.Entities
{
    [Table("Enrolls")]
    public class Enroll
    {
        [Key] public Guid Id { get; set; }

        [Required] public Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public Student Student { get; set; }

        [Required] public Guid ClassId { get; set; }
        [ForeignKey(nameof(ClassId))] public Class Class { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = true;
    }
}
