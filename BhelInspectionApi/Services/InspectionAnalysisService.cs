using Microsoft.EntityFrameworkCore;
using BhelInspectionApi.Data;
using BhelInspectionApi.Models.Entities;
using BhelInspectionApi.Models.DTOs;
using AutoMapper;
using System.Text.Json;

namespace BhelInspectionApi.Services
{
    public class InspectionAnalysisService : IInspectionAnalysisService
    {
        private readonly InspectionDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InspectionAnalysisService> _logger;

        public InspectionAnalysisService(
            InspectionDbContext context,
            IMapper mapper,
            ILogger<InspectionAnalysisService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<InspectionAnalysisDto> AnalyzeInspectionAsync(int inspectionFormId)
        {
            var inspectionForm = await _context.InspectionForms
                .Include(i => i.BladeMeasurements)
                .FirstOrDefaultAsync(i => i.Id == inspectionFormId);

            if (inspectionForm == null)
                throw new ArgumentException($"Inspection form with ID {inspectionFormId} not found.");

            var tolerances = await GetToleranceSpecificationsAsync(inspectionForm.FormId, inspectionForm.Revision);
            var analysis = await PerformAnalysisAsync(inspectionForm, tolerances);

            // Save or update analysis
            var existingAnalysis = await _context.InspectionAnalyses
                .FirstOrDefaultAsync(a => a.InspectionFormId == inspectionFormId);

            if (existingAnalysis != null)
            {
                UpdateAnalysisEntity(existingAnalysis, analysis);
                _context.InspectionAnalyses.Update(existingAnalysis);
            }
            else
            {
                var newAnalysis = CreateAnalysisEntity(analysis, inspectionFormId);
                _context.InspectionAnalyses.Add(newAnalysis);
            }

            await _context.SaveChangesAsync();

            return analysis;
        }

        public async Task<InspectionAnalysisDto> RecalculateAnalysisAsync(int inspectionFormId)
        {
            return await AnalyzeInspectionAsync(inspectionFormId);
        }

        public async Task<List<MeasurementDeviationDto>> CalculateMeasurementDeviationsAsync(int bladeMeasurementId)
        {
            var bladeMeasurement = await _context.BladeMeasurements
                .Include(b => b.InspectionForm)
                .FirstOrDefaultAsync(b => b.Id == bladeMeasurementId);

            if (bladeMeasurement == null)
                throw new ArgumentException($"Blade measurement with ID {bladeMeasurementId} not found.");

            var tolerances = await GetToleranceSpecificationsAsync(
                bladeMeasurement.InspectionForm.FormId,
                bladeMeasurement.InspectionForm.Revision);

            var deviations = CalculateDeviations(bladeMeasurement, tolerances);

            // Clear existing deviations
            var existingDeviations = await _context.MeasurementDeviations
                .Where(d => d.BladeMeasurementId == bladeMeasurementId)
                .ToListAsync();
            _context.MeasurementDeviations.RemoveRange(existingDeviations);

            // Add new deviations
            var deviationEntities = deviations.Select(d => new MeasurementDeviation
            {
                BladeMeasurementId = bladeMeasurementId,
                MeasurementType = d.MeasurementType,
                ActualValue = d.ActualValue,
                NominalValue = d.NominalValue,
                MinTolerance = d.MinTolerance,
                MaxTolerance = d.MaxTolerance,
                DeviationValue = d.DeviationValue,
                DeviationPercentage = d.DeviationPercentage,
                IsWithinTolerance = d.IsWithinTolerance,
                IsCritical = d.IsCritical
            }).ToList();

            _context.MeasurementDeviations.AddRange(deviationEntities);
            await _context.SaveChangesAsync();

            return deviations;
        }

        public async Task<bool> ValidateTolerancesAsync(string formId, string revision)
        {
            var tolerances = await _context.ToleranceSpecifications
                .Where(t => t.FormId == formId && t.Revision == revision && t.IsActive)
                .ToListAsync();

            return tolerances.Count >= 16; // Expecting 16 measurement types (D1-D8, T1-T5, E1-E3)
        }

        private async Task<List<ToleranceSpecification>> GetToleranceSpecificationsAsync(string formId, string revision)
        {
            return await _context.ToleranceSpecifications
                .Where(t => t.FormId == formId && t.Revision == revision && t.IsActive)
                .ToListAsync();
        }

        private async Task<InspectionAnalysisDto> PerformAnalysisAsync(
            InspectionForm inspectionForm,
            List<ToleranceSpecification> tolerances)
        {
            var analysis = new InspectionAnalysisDto
            {
                InspectionFormId = inspectionForm.Id,
                TotalMeasurements = 0,
                WithinTolerance = 0,
                OutsideTolerance = 0,
                CriticalDeviations = 0,
                Reasons = new List<string>()
            };

            var allDeviations = new List<decimal>();
            var criticalMeasurements = new List<string>();

            foreach (var bladeMeasurement in inspectionForm.BladeMeasurements)
            {
                var deviations = CalculateDeviations(bladeMeasurement, tolerances);
                
                foreach (var deviation in deviations)
                {
                    analysis.TotalMeasurements++;
                    allDeviations.Add(Math.Abs(deviation.DeviationValue));

                    if (deviation.IsWithinTolerance)
                    {
                        analysis.WithinTolerance++;
                    }
                    else
                    {
                        analysis.OutsideTolerance++;
                        
                        if (deviation.IsCritical)
                        {
                            analysis.CriticalDeviations++;
                            criticalMeasurements.Add($"{deviation.MeasurementType} on Blade {bladeMeasurement.BladeNumber}");
                        }
                    }
                }
            }

            // Calculate statistical measures
            if (allDeviations.Any())
            {
                analysis.AverageDeviation = allDeviations.Average();
                analysis.MaxDeviation = allDeviations.Max();
                analysis.StandardDeviation = CalculateStandardDeviation(allDeviations);
            }

            // Determine decision and confidence
            var decisionResult = DetermineDecision(analysis, criticalMeasurements);
            analysis.Decision = decisionResult.Decision;
            analysis.Confidence = decisionResult.Confidence;
            analysis.Summary = decisionResult.Summary;
            analysis.Reasons = decisionResult.Reasons;

            return analysis;
        }

        private List<MeasurementDeviationDto> CalculateDeviations(
            BladeMeasurement bladeMeasurement,
            List<ToleranceSpecification> tolerances)
        {
            var deviations = new List<MeasurementDeviationDto>();
            var measurementValues = GetMeasurementValues(bladeMeasurement);

            foreach (var (measurementType, value) in measurementValues)
            {
                if (!value.HasValue) continue;

                var tolerance = tolerances.FirstOrDefault(t => t.MeasurementType == measurementType);
                if (tolerance == null) continue;

                var deviationValue = value.Value - tolerance.NominalValue;
                var deviationPercentage = Math.Abs(deviationValue) / tolerance.NominalValue * 100;
                var isWithinTolerance = value.Value >= tolerance.MinTolerance && value.Value <= tolerance.MaxTolerance;
                var isCritical = deviationPercentage > 20 || 
                               (tolerance.RepairMinLimit.HasValue && value.Value < tolerance.RepairMinLimit) ||
                               (tolerance.RepairMaxLimit.HasValue && value.Value > tolerance.RepairMaxLimit);

                deviations.Add(new MeasurementDeviationDto
                {
                    BladeMeasurementId = bladeMeasurement.Id,
                    MeasurementType = measurementType,
                    ActualValue = value.Value,
                    NominalValue = tolerance.NominalValue,
                    MinTolerance = tolerance.MinTolerance,
                    MaxTolerance = tolerance.MaxTolerance,
                    DeviationValue = deviationValue,
                    DeviationPercentage = deviationPercentage,
                    IsWithinTolerance = isWithinTolerance,
                    IsCritical = isCritical
                });
            }

            return deviations;
        }

        private Dictionary<string, decimal?> GetMeasurementValues(BladeMeasurement measurement)
        {
            return new Dictionary<string, decimal?>
            {
                { "D1", measurement.D1 }, { "D2", measurement.D2 }, { "D3", measurement.D3 }, { "D4", measurement.D4 },
                { "D5", measurement.D5 }, { "D6", measurement.D6 }, { "D7", measurement.D7 }, { "D8", measurement.D8 },
                { "T1", measurement.T1 }, { "T2", measurement.T2 }, { "T3", measurement.T3 }, { "T4", measurement.T4 }, { "T5", measurement.T5 },
                { "E1", measurement.E1 }, { "E2", measurement.E2 }, { "E3", measurement.E3 }
            };
        }

        private (InspectionDecision Decision, int Confidence, string Summary, List<string> Reasons) DetermineDecision(
            InspectionAnalysisDto analysis, List<string> criticalMeasurements)
        {
            var reasons = new List<string>();
            
            if (analysis.CriticalDeviations > 0 || analysis.OutsideTolerance > 10)
            {
                reasons.Add($"Critical safety dimensions compromised ({analysis.CriticalDeviations} critical deviations)");
                reasons.AddRange(criticalMeasurements.Take(5));
                
                return (InspectionDecision.Reject, 95, 
                       $"Component should be REJECTED due to {analysis.CriticalDeviations} critical deviations and {analysis.OutsideTolerance} measurements outside tolerance.",
                       reasons);
            }
            
            if (analysis.OutsideTolerance > 3 && analysis.OutsideTolerance <= 10)
            {
                reasons.Add($"Significant wear detected ({analysis.OutsideTolerance} measurements outside tolerance)");
                reasons.Add("Replacement recommended for optimal performance");
                
                return (InspectionDecision.Replace, 85,
                       $"Component should be REPLACED due to {analysis.OutsideTolerance} measurements outside tolerance limits.",
                       reasons);
            }
            
            if (analysis.OutsideTolerance > 0 && analysis.OutsideTolerance <= 3)
            {
                reasons.Add($"Minor deviations within repairable limits ({analysis.OutsideTolerance} measurements)");
                reasons.Add("Component can be restored to acceptable condition");
                
                return (InspectionDecision.Repair, 80,
                       $"Component can be REPAIRED with {analysis.OutsideTolerance} minor deviations within repair limits.",
                       reasons);
            }
            
            reasons.Add("All dimensions within acceptable limits");
            reasons.Add("Component meets all quality standards");
            
            return (InspectionDecision.Okay, 95,
                   "All measurements within tolerance. Component is in excellent condition.",
                   reasons);
        }

        private decimal CalculateStandardDeviation(List<decimal> values)
        {
            if (values.Count <= 1) return 0;
            
            var average = values.Average();
            var sumOfSquares = values.Sum(x => (x - average) * (x - average));
            return (decimal)Math.Sqrt((double)(sumOfSquares / (values.Count - 1)));
        }

        private void UpdateAnalysisEntity(InspectionAnalysis entity, InspectionAnalysisDto dto)
        {
            entity.Decision = dto.Decision;
            entity.Confidence = dto.Confidence;
            entity.Summary = dto.Summary;
            entity.TotalMeasurements = dto.TotalMeasurements;
            entity.WithinTolerance = dto.WithinTolerance;
            entity.OutsideTolerance = dto.OutsideTolerance;
            entity.CriticalDeviations = dto.CriticalDeviations;
            entity.Reasons = JsonSerializer.Serialize(dto.Reasons);
            entity.AverageDeviation = dto.AverageDeviation;
            entity.MaxDeviation = dto.MaxDeviation;
            entity.StandardDeviation = dto.StandardDeviation;
        }

        private InspectionAnalysis CreateAnalysisEntity(InspectionAnalysisDto dto, int inspectionFormId)
        {
            return new InspectionAnalysis
            {
                InspectionFormId = inspectionFormId,
                Decision = dto.Decision,
                Confidence = dto.Confidence,
                Summary = dto.Summary,
                TotalMeasurements = dto.TotalMeasurements,
                WithinTolerance = dto.WithinTolerance,
                OutsideTolerance = dto.OutsideTolerance,
                CriticalDeviations = dto.CriticalDeviations,
                Reasons = JsonSerializer.Serialize(dto.Reasons),
                AverageDeviation = dto.AverageDeviation,
                MaxDeviation = dto.MaxDeviation,
                StandardDeviation = dto.StandardDeviation
            };
        }
    }
}