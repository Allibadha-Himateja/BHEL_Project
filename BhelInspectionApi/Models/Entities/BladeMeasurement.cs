using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhelInspectionApi.Models.Entities
{
    public class BladeMeasurement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InspectionFormId { get; set; }

        [Required]
        [Range(1, 90)]
        public int BladeNumber { get; set; }

        [StringLength(50)]
        public string? PartNumber { get; set; }

        [StringLength(50)]
        public string? SerialNumber { get; set; }

        [Required]
        public PassFailStatus PassFail { get; set; }

        // Dimensional measurements (D1-D8)
        [Column(TypeName = "decimal(8,4)")]
        public decimal? D1 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? D2 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? D3 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? D4 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? D5 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? D6 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? D7 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? D8 { get; set; }

        // Thickness measurements (T1-T5)
        [Column(TypeName = "decimal(8,4)")]
        public decimal? T1 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? T2 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? T3 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? T4 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? T5 { get; set; }

        // Edge measurements (E1-E3)
        [Column(TypeName = "decimal(8,4)")]
        public decimal? E1 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? E2 { get; set; }

        [Column(TypeName = "decimal(8,4)")]
        public decimal? E3 { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("InspectionFormId")]
        public virtual InspectionForm InspectionForm { get; set; } = null!;

        public virtual ICollection<MeasurementDeviation> MeasurementDeviations { get; set; } = new List<MeasurementDeviation>();
    }

    public enum PassFailStatus
    {
        NotSet = 0,
        Pass = 1,
        Fail = 2
    }
}