using AutoMapper;
using Fap.Domain.DTOs.Semester;
using Fap.Domain.Entities;

namespace Fap.Api.Mappings
{
    /// <summary>
    /// AutoMapper profile for Semester entities
    /// ✅ UPDATED: Uses SubjectOfferings instead of direct Subjects
    /// </summary>
    public class SemesterMappingProfile : Profile
    {
        public SemesterMappingProfile()
        {
            // ======================================================================
            // SEMESTER MAPPINGS
            // ======================================================================

            CreateMap<Semester, SemesterDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                // ✅ CHANGED: Count offerings, not subjects
                .ForMember(dest => dest.TotalSubjects, opt => opt.MapFrom(src =>
                    src.SubjectOfferings != null ? src.SubjectOfferings.Count : 0))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsClosed, opt => opt.MapFrom(src => src.IsClosed));

            CreateMap<Semester, SemesterDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsClosed, opt => opt.MapFrom(src => src.IsClosed))
                // ✅ CHANGED: Statistics from offerings
                .ForMember(dest => dest.TotalSubjects, opt => opt.MapFrom(src =>
                    src.SubjectOfferings != null ? src.SubjectOfferings.Count : 0))
                .ForMember(dest => dest.TotalClasses, opt => opt.MapFrom(src =>
                    src.SubjectOfferings != null
                        ? src.SubjectOfferings.Sum(so => so.Classes != null ? so.Classes.Count : 0)
                        : 0))
                .ForMember(dest => dest.TotalStudentsEnrolled, opt => opt.MapFrom(src =>
                    src.SubjectOfferings != null
                        ? src.SubjectOfferings
                            .SelectMany(so => so.Classes ?? new List<Class>())
                            .SelectMany(c => c.Members ?? new List<ClassMember>())
                            .Select(m => m.StudentId)
                            .Distinct()
                            .Count()
                        : 0))
                // ✅ CHANGED: Map offerings instead of subjects
                .ForMember(dest => dest.SubjectOfferings, opt => opt.MapFrom(src => src.SubjectOfferings));
        }
    }
}
