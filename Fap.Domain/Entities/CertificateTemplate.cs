using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fap.Domain.Entities
{
    [Table("CertificateTemplates")]
    public class CertificateTemplate
    {
        [Key] public Guid Id { get; set; }

        [Required, MaxLength(120)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

    [MaxLength(200)]
    public string? TemplateFileUrl { get; set; } // IPFS/Cloud template

    // Template type
    [Required, MaxLength(50)]
    public string TemplateType { get; set; } = "SubjectCompletion"; // "SubjectCompletion", "SemesterCompletion", "RoadmapCompletion"

    // Template design
    public string? TemplateContent { get; set; } // HTML template with placeholders

        [MaxLength(500)]
        public string? HeaderImagePath { get; set; }

        [MaxLength(500)]
        public string? FooterImagePath { get; set; }

        [MaxLength(500)]
        public string? BackgroundImagePath { get; set; }

        [MaxLength(500)]
        public string? LogoImagePath { get; set; }

    [MaxLength(500)]
    public string? SignatureImagePath { get; set; }

    // Template variables (JSON)
    public string? TemplateVariables { get; set; } // JSON: {"StudentName": "{{StudentName}}", ...}

    // Template settings
    [MaxLength(20)]
    public string PageSize { get; set; } = "A4"; // A4, Letter

    [MaxLength(20)]
    public string Orientation { get; set; } = "Landscape"; // Landscape, Portrait

    public string? CustomStyles { get; set; } // CSS styles

    // Sample/demo flag
    public bool IsSample { get; set; } = false; // Pre-built sample templates

    public bool IsActive { get; set; } = true;
    public bool IsDefault { get; set; } = false;

    // Version control
    public int Version { get; set; } = 1;

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public virtual ICollection<Credential> Credentials { get; set; } = new List<Credential>();
    }
}
