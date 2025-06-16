using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhelInspectionApi.Models.Entities
{
    public class ToleranceSpecification
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
        [StringLength(10)]
        public string MeasurementType { get; set; } = string.Empty; // D1, D2, T1, etc.

        [Required]
        [Column(TypeName = "decimal(8,4)")]
        public decimal NominalValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,4)")]
        public decimal MinTolerance { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,4)")]
        public decimal MaxTolerance { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? RepairMinLimit { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? RepairMaxLimit { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}