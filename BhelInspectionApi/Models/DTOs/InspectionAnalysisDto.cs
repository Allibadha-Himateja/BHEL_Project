using BhelInspectionApi.Models.Entities;

namespace BhelInspectionApi.Models.DTOs
{
    public class InspectionAnalysisDto
    {
        public int Id { get; set; }
        public int InspectionFormId { get; set; }
        public InspectionDecision Decision { get; set; }
        public int Confidence { get; set; }
        public string Summary { get; set; } = string.Empty;
        public int TotalMeasurements { get; set; }
        public int WithinTolerance { get; set; }
        public int OutsideTolerance { get; set; }
        public int CriticalDeviations { get; set; }
        public List<string> Reasons { get; set; } = new();
        public decimal? AverageDeviation { get; set; }
        public decimal? MaxDeviation { get; set; }
        public decimal? StandardDeviation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MeasurementDeviationDto
    {
        public int Id { get; set; }
        public int BladeMeasurementId { get; set; }
        public string MeasurementType { get; set; } = string.Empty;
        public decimal ActualValue { get; set; }
        public decimal NominalValue { get; set; }
        public decimal MinTolerance { get; set; }
        public decimal MaxTolerance { get; set; }
        public decimal DeviationValue { get; set; }
        public decimal DeviationPercentage { get; set; }
        public bool IsWithinTolerance { get; set; }
        public bool IsCritical { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}