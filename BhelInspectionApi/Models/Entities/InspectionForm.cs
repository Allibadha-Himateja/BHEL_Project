using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhelInspectionApi.Models.Entities
{
    public class InspectionForm
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FormId { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Revision { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string JobNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Customer { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Frame { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Component { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string PartNumber { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000)]
        public int Quantity { get; set; }

        [Required]
        [StringLength(100)]
        public string Operator { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "date")]
        public DateTime InspectionDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InstrumentId { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "date")]
        public DateTime CalibrationDueDate { get; set; }

        [Required]
        [StringLength(100)]
        public string InspectedBy { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ReviewedBy { get; set; } = string.Empty;

        [Required]
        public InspectionStatus Status { get; set; } = InspectionStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<BladeMeasurement> BladeMeasurements { get; set; } = new List<BladeMeasurement>();
        public virtual InspectionAnalysis? InspectionAnalysis { get; set; }
        public virtual ICollection<InspectionAuditLog> AuditLogs { get; set; } = new List<InspectionAuditLog>();
    }

    public enum InspectionStatus
    {
        Draft = 0,
        Submitted = 1,
        InReview = 2,
        Approved = 3,
        Rejected = 4,
        Completed = 5
    }
}