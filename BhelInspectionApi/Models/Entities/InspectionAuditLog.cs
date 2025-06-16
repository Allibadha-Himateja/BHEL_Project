using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhelInspectionApi.Models.Entities
{
    public class InspectionAuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InspectionFormId { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PerformedBy { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string? Details { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? OldValues { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? NewValues { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("InspectionFormId")]
        public virtual InspectionForm InspectionForm { get; set; } = null!;
    }
}