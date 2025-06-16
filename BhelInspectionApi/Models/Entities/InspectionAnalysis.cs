using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhelInspectionApi.Models.Entities
{
    public class InspectionAnalysis
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InspectionFormId { get; set; }

        [Required]
        public InspectionDecision Decision { get; set; }

        [Required]
        [Range(0, 100)]
        public int Confidence { get; set; }

        [Required]
        [StringLength(1000)]
        public string Summary { get; set; } = string.Empty;

        [Required]
        public int TotalMeasurements { get; set; }

        [Required]
        public int WithinTolerance { get; set; }

        [Required]
        public int OutsideTolerance { get; set; }

        [Required]
        public int CriticalDeviations { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Reasons { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,6)")]
        public decimal? AverageDeviation { get; set; }

        [Column(TypeName = "decimal(10,6)")]
        public decimal? MaxDeviation { get; set; }

        [Column(TypeName = "decimal(10,6)")]
        public decimal? StandardDeviation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("InspectionFormId")]
        public virtual InspectionForm InspectionForm { get; set; } = null!;
    }

    public enum InspectionDecision
    {
        NotSet = 0,
        Okay = 1,
        Repair = 2,
        Replace = 3,
        Reject = 4
    }
}