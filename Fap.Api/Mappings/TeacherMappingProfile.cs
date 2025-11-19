using AutoMapper;
using Fap.Domain.DTOs.Teacher;
using Fap.Domain.Entities;

namespace Fap.Api.Mappings
{
    /// <summary>
    /// AutoMapper profile for Teacher entities
    /// </summary>
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile()
        {
            // ======================================================================
            // TEACHER MAPPINGS
            // ======================================================================

            CreateMap<Teacher, TeacherDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TeacherCode, opt => opt.MapFrom(src => src.TeacherCode))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.HireDate))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User.IsActive))
                .ForMember(dest => dest.TotalClasses, opt => opt.MapFrom(src => src.Classes != null ? src.Classes.Count : 0));

            CreateMap<Teacher, TeacherDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TeacherCode, opt => opt.MapFrom(src => src.TeacherCode))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.HireDate))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User.IsActive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.User.CreatedAt))
                .ForMember(dest => dest.TotalClasses, opt => opt.MapFrom(src => src.Classes != null ? src.Classes.Count : 0))
                .ForMember(dest => dest.TotalStudents, opt => opt.MapFrom(src =>
                    src.Classes != null
                    ? src.Classes.Sum(c => c.Members != null ? c.Members.Count : 0)
                    : 0))
                .ForMember(dest => dest.Classes, opt => opt.MapFrom(src => src.Classes));
        }
    }
}
