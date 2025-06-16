using BhelInspectionApi.Models.Entities;

namespace BhelInspectionApi.Models.DTOs
{
    public class InspectionFormDto
    {
        public int Id { get; set; }
        public string FormId { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public string JobNumber { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public string Frame { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Operator { get; set; } = string.Empty;
        public DateTime InspectionDate { get; set; }
        public string InstrumentId { get; set; } = string.Empty;
        public DateTime CalibrationDueDate { get; set; }
        public string InspectedBy { get; set; } = string.Empty;
        public string ReviewedBy { get; set; } = string.Empty;
        public InspectionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<BladeMeasurementDto> BladeMeasurements { get; set; } = new();
        public InspectionAnalysisDto? InspectionAnalysis { get; set; }
    }

    public class CreateInspectionFormDto
    {
        public string FormId { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public string JobNumber { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public string Frame { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Operator { get; set; } = string.Empty;
        public DateTime InspectionDate { get; set; }
        public string InstrumentId { get; set; } = string.Empty;
        public DateTime CalibrationDueDate { get; set; }
        public string InspectedBy { get; set; } = string.Empty;
        public string ReviewedBy { get; set; } = string.Empty;
        public List<CreateBladeMeasurementDto> BladeMeasurements { get; set; } = new();
    }

    public class UpdateInspectionFormDto
    {
        public string? JobNumber { get; set; }
        public string? Customer { get; set; }
        public string? Frame { get; set; }
        public string? Component { get; set; }
        public string? PartNumber { get; set; }
        public int? Quantity { get; set; }
        public string? Operator { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? InstrumentId { get; set; }
        public DateTime? CalibrationDueDate { get; set; }
        public string? InspectedBy { get; set; }
        public string? ReviewedBy { get; set; }
        public InspectionStatus? Status { get; set; }
    }
}