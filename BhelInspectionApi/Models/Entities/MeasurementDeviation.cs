using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhelInspectionApi.Models.Entities
{
    public class MeasurementDeviation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BladeMeasurementId { get; set; }

        [Required]
        [StringLength(10)]
        public string MeasurementType { get; set; } = string.Empty; // D1, D2, T1, etc.

        [Required]
        [Column(TypeName = "decimal(8,4)")]
        public decimal ActualValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,4)")]
        public decimal NominalValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,4)")]
        public decimal MinTolerance { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,4)")]
        public decimal MaxTolerance { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,6)")]
        public decimal DeviationValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,4)")]
        public decimal DeviationPercentage { get; set; }

        [Required]
        public bool IsWithinTolerance { get; set; }

        [Required]
        public bool IsCritical { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("BladeMeasurementId")]
        public virtual BladeMeasurement BladeMeasurement { get; set; } = null!;
    }
}