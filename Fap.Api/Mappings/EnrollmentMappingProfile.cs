using AutoMapper;
using Fap.Domain.DTOs.Enrollment;
using Fap.Domain.Entities;

namespace Fap.Api.Mappings
{
    /// <summary>
    /// AutoMapper profile for Enrollment entities using SubjectOffering data via Class navigation
    /// </summary>
    public class EnrollmentMappingProfile : Profile
    {
        public EnrollmentMappingProfile()
        {
            // ======================================================================
            // ENROLLMENT MAPPINGS
            // ======================================================================

            CreateMap<Enroll, EnrollmentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
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
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.ClassId))
                .ForMember(dest => dest.ClassCode, opt => opt.MapFrom(src =>
                    src.Class != null ? src.Class.ClassCode : null))
                // Subject info via SubjectOffering
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Subject != null
                        ? src.Class.SubjectOffering.Subject.SubjectName
                        : null))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Subject != null
                        ? src.Class.SubjectOffering.Subject.SubjectCode
                        : null))
                .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(src => src.RegisteredAt))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved));

            CreateMap<Enroll, EnrollmentDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(src => src.RegisteredAt))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
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
                .ForMember(dest => dest.StudentPhone, opt => opt.Ignore())
                .ForMember(dest => dest.StudentGPA, opt => opt.MapFrom(src =>
                    src.Student != null ? src.Student.GPA : 0m))
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.ClassId))
                .ForMember(dest => dest.ClassCode, opt => opt.MapFrom(src =>
                    src.Class != null ? src.Class.ClassCode : null))
                // Subject info via SubjectOffering
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null
                        ? src.Class.SubjectOffering.SubjectId
                        : Guid.Empty))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Subject != null
                        ? src.Class.SubjectOffering.Subject.SubjectCode
                        : null))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Subject != null
                        ? src.Class.SubjectOffering.Subject.SubjectName
                        : null))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Subject != null
                        ? src.Class.SubjectOffering.Subject.Credits
                        : 0))
                // Teacher info
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src =>
                    src.Class != null ? src.Class.TeacherUserId : Guid.Empty))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.Teacher != null && src.Class.Teacher.User != null
                        ? src.Class.Teacher.User.FullName
                        : null))
                .ForMember(dest => dest.TeacherEmail, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.Teacher != null && src.Class.Teacher.User != null
                        ? src.Class.Teacher.User.Email
                        : null))
                // Semester info via SubjectOffering
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Semester != null
                        ? src.Class.SubjectOffering.Semester.Name
                        : null))
                .ForMember(dest => dest.SemesterStartDate, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Semester != null
                        ? src.Class.SubjectOffering.Semester.StartDate
                        : DateTime.MinValue))
                .ForMember(dest => dest.SemesterEndDate, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Semester != null
                        ? src.Class.SubjectOffering.Semester.EndDate
                        : DateTime.MinValue));

            CreateMap<Enroll, StudentEnrollmentHistoryDto>()
                .ForMember(dest => dest.EnrollmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.ClassId))
                .ForMember(dest => dest.ClassCode, opt => opt.MapFrom(src =>
                    src.Class != null ? src.Class.ClassCode : null))
                // Subject info via SubjectOffering
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Subject != null
                        ? src.Class.SubjectOffering.Subject.SubjectCode
                        : null))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Subject != null
                        ? src.Class.SubjectOffering.Subject.SubjectName
                        : null))
                .ForMember(dest => dest.Credits, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Subject != null
                        ? src.Class.SubjectOffering.Subject.Credits
                        : 0))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.Teacher != null && src.Class.Teacher.User != null
                        ? src.Class.Teacher.User.FullName
                        : null))
                // Semester info via SubjectOffering
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Semester != null
                        ? src.Class.SubjectOffering.Semester.Name
                        : null))
                .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(src => src.RegisteredAt))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
                .ForMember(dest => dest.SemesterStartDate, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Semester != null
                        ? src.Class.SubjectOffering.Semester.StartDate
                        : DateTime.MinValue))
                .ForMember(dest => dest.SemesterEndDate, opt => opt.MapFrom(src =>
                    src.Class != null && src.Class.SubjectOffering != null && src.Class.SubjectOffering.Semester != null
                        ? src.Class.SubjectOffering.Semester.EndDate
                        : DateTime.MinValue));
        }
    }
}
