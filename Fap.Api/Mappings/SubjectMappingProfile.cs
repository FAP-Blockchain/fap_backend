using AutoMapper;
using Fap.Domain.DTOs.Subject;
using Fap.Domain.Entities;

namespace Fap.Api.Mappings
{
    /// <summary>
    /// AutoMapper profile for Subject entities
    /// ✅ UPDATED: Uses SubjectOffering pattern
    /// </summary>
    public class SubjectMappingProfile : Profile
    {
        public SubjectMappingProfile()
        {
            // ======================================================================
            // SUBJECT MAPPINGS - Master Data (No Semester)
            // ======================================================================

            CreateMap<Subject, SubjectDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.SubjectCode))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.SubjectName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src => src.Credits))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Prerequisites, opt => opt.MapFrom(src => src.Prerequisites))
                // ✅ CHANGED: Total offerings instead of classes
                .ForMember(dest => dest.TotalOfferings, opt => opt.MapFrom(src =>
                    src.Offerings != null ? src.Offerings.Count : 0));

            CreateMap<Subject, SubjectDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.SubjectCode))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.SubjectName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src => src.Credits))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Prerequisites, opt => opt.MapFrom(src => src.Prerequisites))
                // ✅ NEW: Include offerings
                .ForMember(dest => dest.Offerings, opt => opt.MapFrom(src => src.Offerings))
                .ForMember(dest => dest.TotalOfferings, opt => opt.MapFrom(src =>
                    src.Offerings != null ? src.Offerings.Count : 0))
                // ✅ CHANGED: Calculate total classes across all offerings
                .ForMember(dest => dest.TotalClasses, opt => opt.MapFrom(src =>
                    src.Offerings != null
                        ? src.Offerings.Sum(o => o.Classes != null ? o.Classes.Count : 0)
                        : 0))
                .ForMember(dest => dest.TotalStudentsEnrolled, opt => opt.MapFrom(src =>
                    src.Offerings != null
                        ? src.Offerings
                            .SelectMany(o => o.Classes ?? new List<Class>())
                            .SelectMany(c => c.Members ?? new List<ClassMember>())
                            .Select(m => m.StudentId)
                            .Distinct()
                            .Count()
                        : 0));

            CreateMap<Subject, SubjectSummaryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.SubjectCode))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.SubjectName))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src => src.Credits))
                // ✅ CHANGED: Total classes across all offerings
                .ForMember(dest => dest.TotalClasses, opt => opt.MapFrom(src =>
                    src.Offerings != null
                        ? src.Offerings.Sum(o => o.Classes != null ? o.Classes.Count : 0)
                        : 0));

            // ======================================================================
            // SUBJECT OFFERING MAPPINGS
            // ======================================================================

            CreateMap<SubjectOffering, SubjectOfferingDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.SubjectCode : null))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.SubjectName : null))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.Credits : 0))
                .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src => src.SemesterId))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src =>
                    src.Semester != null ? src.Semester.Name : null))
                .ForMember(dest => dest.MaxClasses, opt => opt.MapFrom(src => src.MaxClasses))
                .ForMember(dest => dest.SemesterCapacity, opt => opt.MapFrom(src => src.SemesterCapacity))
                .ForMember(dest => dest.RegistrationStartDate, opt => opt.MapFrom(src => src.RegistrationStartDate))
                .ForMember(dest => dest.RegistrationEndDate, opt => opt.MapFrom(src => src.RegistrationEndDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.TotalClasses, opt => opt.MapFrom(src =>
                    src.Classes != null ? src.Classes.Count : 0))
                .ForMember(dest => dest.TotalStudents, opt => opt.MapFrom(src =>
                    src.Classes != null
                        ? src.Classes.SelectMany(c => c.Members ?? new List<ClassMember>())
                            .Select(m => m.StudentId)
                            .Distinct()
                            .Count()
                        : 0));
        }
    }
}
