using AutoMapper;
using BhelInspectionApi.Models.Entities;
using BhelInspectionApi.Models.DTOs;
using System.Text.Json;

namespace BhelInspectionApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // InspectionForm mappings
            CreateMap<InspectionForm, InspectionFormDto>();
            CreateMap<CreateInspectionFormDto, InspectionForm>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => InspectionStatus.Draft))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InspectionAnalysis, opt => opt.Ignore())
                .ForMember(dest => dest.AuditLogs, opt => opt.Ignore());

            CreateMap<UpdateInspectionFormDto, InspectionForm>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // BladeMeasurement mappings
            CreateMap<BladeMeasurement, BladeMeasurementDto>();
            CreateMap<CreateBladeMeasurementDto, BladeMeasurement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InspectionFormId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.InspectionForm, opt => opt.Ignore())
                .ForMember(dest => dest.MeasurementDeviations, opt => opt.Ignore());

            CreateMap<UpdateBladeMeasurementDto, BladeMeasurement>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // InspectionAnalysis mappings
            CreateMap<InspectionAnalysis, InspectionAnalysisDto>()
                .ForMember(dest => dest.Reasons, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.Reasons) ? new List<string>() : 
                    JsonSerializer.Deserialize<List<string>>(src.Reasons) ?? new List<string>()));

            CreateMap<InspectionAnalysisDto, InspectionAnalysis>()
                .ForMember(dest => dest.Reasons, opt => opt.MapFrom(src => JsonSerializer.Serialize(src.Reasons)))
                .ForMember(dest => dest.InspectionForm, opt => opt.Ignore());

            // MeasurementDeviation mappings
            CreateMap<MeasurementDeviation, MeasurementDeviationDto>();
            CreateMap<MeasurementDeviationDto, MeasurementDeviation>()
                .ForMember(dest => dest.BladeMeasurement, opt => opt.Ignore());
        }
    }
}