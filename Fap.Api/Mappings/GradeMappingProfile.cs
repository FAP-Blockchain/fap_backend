using AutoMapper;
using Fap.Domain.DTOs.Grade;
using Fap.Domain.DTOs.GradeComponent;
using Fap.Domain.Entities;

namespace Fap.Api.Mappings
{
    /// <summary>
    /// AutoMapper profile for Grade entities
    /// </summary>
    public class GradeMappingProfile : Profile
    {
        public GradeMappingProfile()
        {
            // ======================================================================
            // GRADE MAPPINGS
            // ======================================================================

            CreateMap<Grade, GradeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src =>
                    src.Student != null ? src.Student.StudentCode : null))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.User != null
                        ? src.Student.User.FullName
                        : null))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.SubjectCode : null))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.SubjectName : null))
                .ForMember(dest => dest.GradeComponentId, opt => opt.MapFrom(src => src.GradeComponentId))
                .ForMember(dest => dest.ComponentName, opt => opt.MapFrom(src =>
                    src.GradeComponent != null ? src.GradeComponent.Name : null))
                .ForMember(dest => dest.ComponentWeight, opt => opt.MapFrom(src =>
                    src.GradeComponent != null ? src.GradeComponent.WeightPercent : 0))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.LetterGrade, opt => opt.MapFrom(src => src.LetterGrade))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

            CreateMap<Grade, GradeDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.LetterGrade, opt => opt.MapFrom(src => src.LetterGrade))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src =>
                    src.Student != null ? src.Student.StudentCode : null))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.User != null
                        ? src.Student.User.FullName
                        : null))
                .ForMember(dest => dest.StudentEmail, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.User != null
                        ? src.Student.User.Email
                        : null))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.SubjectCode : null))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.SubjectName : null))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.Credits : 0))
                .ForMember(dest => dest.GradeComponentId, opt => opt.MapFrom(src => src.GradeComponentId))
                .ForMember(dest => dest.ComponentName, opt => opt.MapFrom(src =>
                    src.GradeComponent != null ? src.GradeComponent.Name : null))
                .ForMember(dest => dest.ComponentWeight, opt => opt.MapFrom(src =>
                    src.GradeComponent != null ? src.GradeComponent.WeightPercent : 0))
                .ForMember(dest => dest.ClassName, opt => opt.Ignore())
                .ForMember(dest => dest.TeacherName, opt => opt.Ignore());

            // ======================================================================
            // GRADE COMPONENT MAPPINGS
            // ======================================================================

            CreateMap<GradeComponent, GradeComponentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.WeightPercent, opt => opt.MapFrom(src => src.WeightPercent))
                .ForMember(dest => dest.GradeCount, opt => opt.MapFrom(src =>
                    src.Grades != null ? src.Grades.Count : 0));
        }
    }
}
