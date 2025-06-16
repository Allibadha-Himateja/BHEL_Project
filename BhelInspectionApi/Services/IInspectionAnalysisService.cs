using BhelInspectionApi.Models.Entities;
using BhelInspectionApi.Models.DTOs;

namespace BhelInspectionApi.Services
{
    public interface IInspectionAnalysisService
    {
        Task<InspectionAnalysisDto> AnalyzeInspectionAsync(int inspectionFormId);
        Task<InspectionAnalysisDto> RecalculateAnalysisAsync(int inspectionFormId);
        Task<List<MeasurementDeviationDto>> CalculateMeasurementDeviationsAsync(int bladeMeasurementId);
        Task<bool> ValidateTolerancesAsync(string formId, string revision);
    }
}