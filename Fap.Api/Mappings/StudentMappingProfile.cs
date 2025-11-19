using AutoMapper;
using Fap.Domain.DTOs.Student;
using Fap.Domain.Entities;

namespace Fap.Api.Mappings
{
    /// <summary>
    /// AutoMapper profile for Student entities
    /// </summary>
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            // ======================================================================
            // STUDENT MAPPINGS
            // ======================================================================

            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.StudentCode))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrollmentDate))
                .ForMember(dest => dest.GPA, opt => opt.MapFrom(src => src.GPA))
                .ForMember(dest => dest.IsGraduated, opt => opt.MapFrom(src => src.IsGraduated))
                .ForMember(dest => dest.GraduationDate, opt => opt.MapFrom(src => src.GraduationDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User.IsActive))
                .ForMember(dest => dest.TotalEnrollments, opt => opt.MapFrom(src => src.Enrolls != null ? src.Enrolls.Count : 0))
                .ForMember(dest => dest.TotalClasses, opt => opt.MapFrom(src => src.ClassMembers != null ? src.ClassMembers.Count : 0));

            CreateMap<Student, StudentDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.StudentCode))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrollmentDate))
                .ForMember(dest => dest.GPA, opt => opt.MapFrom(src => src.GPA))
                .ForMember(dest => dest.IsGraduated, opt => opt.MapFrom(src => src.IsGraduated))
                .ForMember(dest => dest.GraduationDate, opt => opt.MapFrom(src => src.GraduationDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User.IsActive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.User.CreatedAt))
                .ForMember(dest => dest.TotalEnrollments, opt => opt.MapFrom(src => src.Enrolls != null ? src.Enrolls.Count : 0))
                .ForMember(dest => dest.ApprovedEnrollments, opt => opt.MapFrom(src => src.Enrolls != null ? src.Enrolls.Count(e => e.IsApproved) : 0))
                .ForMember(dest => dest.PendingEnrollments, opt => opt.MapFrom(src => src.Enrolls != null ? src.Enrolls.Count(e => !e.IsApproved) : 0))
                .ForMember(dest => dest.TotalClasses, opt => opt.MapFrom(src => src.ClassMembers != null ? src.ClassMembers.Count : 0))
                .ForMember(dest => dest.TotalGrades, opt => opt.MapFrom(src => src.Grades != null ? src.Grades.Count : 0))
                .ForMember(dest => dest.TotalAttendances, opt => opt.MapFrom(src => src.Attendances != null ? src.Attendances.Count : 0))
                .ForMember(dest => dest.Enrollments, opt => opt.MapFrom(src => src.Enrolls))
                .ForMember(dest => dest.CurrentClasses, opt => opt.MapFrom(src => src.ClassMembers));
        }
    }
}
