using BhelInspectionApi.Models.Entities;

namespace BhelInspectionApi.Models.DTOs
{
    public class BladeMeasurementDto
    {
        public int Id { get; set; }
        public int InspectionFormId { get; set; }
        public int BladeNumber { get; set; }
        public string? PartNumber { get; set; }
        public string? SerialNumber { get; set; }
        public PassFailStatus PassFail { get; set; }
        public decimal? D1 { get; set; }
        public decimal? D2 { get; set; }
        public decimal? D3 { get; set; }
        public decimal? D4 { get; set; }
        public decimal? D5 { get; set; }
        public decimal? D6 { get; set; }
        public decimal? D7 { get; set; }
        public decimal? D8 { get; set; }
        public decimal? T1 { get; set; }
        public decimal? T2 { get; set; }
        public decimal? T3 { get; set; }
        public decimal? T4 { get; set; }
        public decimal? T5 { get; set; }
        public decimal? E1 { get; set; }
        public decimal? E2 { get; set; }
        public decimal? E3 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<MeasurementDeviationDto> MeasurementDeviations { get; set; } = new();
    }

    public class CreateBladeMeasurementDto
    {
        public int BladeNumber { get; set; }
        public string? PartNumber { get; set; }
        public string? SerialNumber { get; set; }
        public PassFailStatus PassFail { get; set; }
        public decimal? D1 { get; set; }
        public decimal? D2 { get; set; }
        public decimal? D3 { get; set; }
        public decimal? D4 { get; set; }
        public decimal? D5 { get; set; }
        public decimal? D6 { get; set; }
        public decimal? D7 { get; set; }
        public decimal? D8 { get; set; }
        public decimal? T1 { get; set; }
        public decimal? T2 { get; set; }
        public decimal? T3 { get; set; }
        public decimal? T4 { get; set; }
        public decimal? T5 { get; set; }
        public decimal? E1 { get; set; }
        public decimal? E2 { get; set; }
        public decimal? E3 { get; set; }
    }

    public class UpdateBladeMeasurementDto
    {
        public string? PartNumber { get; set; }
        public string? SerialNumber { get; set; }
        public PassFailStatus? PassFail { get; set; }
        public decimal? D1 { get; set; }
        public decimal? D2 { get; set; }
        public decimal? D3 { get; set; }
        public decimal? D4 { get; set; }
        public decimal? D5 { get; set; }
        public decimal? D6 { get; set; }
        public decimal? D7 { get; set; }
        public decimal? D8 { get; set; }
        public decimal? T1 { get; set; }
        public decimal? T2 { get; set; }
        public decimal? T3 { get; set; }
        public decimal? T4 { get; set; }
        public decimal? T5 { get; set; }
        public decimal? E1 { get; set; }
        public decimal? E2 { get; set; }
        public decimal? E3 { get; set; }
    }
}