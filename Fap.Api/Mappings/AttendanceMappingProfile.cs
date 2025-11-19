using AutoMapper;
using Fap.Domain.DTOs.Attendance;
using Fap.Domain.Entities;

namespace Fap.Api.Mappings
{
    /// <summary>
    /// AutoMapper profile for Attendance entities
    /// </summary>
    public class AttendanceMappingProfile : Profile
    {
        public AttendanceMappingProfile()
        {
            // ======================================================================
            // ATTENDANCE MAPPINGS
            // ======================================================================

            CreateMap<Attendance, AttendanceDto>()
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src =>
                    src.Student != null ? src.Student.StudentCode : null))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.User != null
                        ? src.Student.User.FullName
                        : null))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src =>
                    src.Subject != null ? src.Subject.SubjectName : null))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src =>
                    src.Slot != null ? src.Slot.Date : DateTime.MinValue))
                .ForMember(dest => dest.TimeSlotName, opt => opt.MapFrom(src =>
                    src.Slot != null && src.Slot.TimeSlot != null
                        ? src.Slot.TimeSlot.Name
                        : null))
                .ForMember(dest => dest.ClassCode, opt => opt.MapFrom(src =>
                    src.Slot != null && src.Slot.Class != null
                        ? src.Slot.Class.ClassCode
                        : null));

            CreateMap<Attendance, AttendanceDetailDto>()
                .IncludeBase<Attendance, AttendanceDto>()
                .ForMember(dest => dest.StudentEmail, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.User != null
                        ? src.Student.User.Email
                        : null))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src =>
                    src.Slot != null && src.Slot.Class != null && src.Slot.Class.Teacher != null && src.Slot.Class.Teacher.User != null
                        ? src.Slot.Class.Teacher.User.FullName
                        : null))
                // ✅ CHANGED: Get semester via SubjectOffering
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src =>
                    src.Slot != null && src.Slot.Class != null && src.Slot.Class.SubjectOffering != null && src.Slot.Class.SubjectOffering.Semester != null
                        ? src.Slot.Class.SubjectOffering.Semester.Name
                        : null))
                .ForMember(dest => dest.SlotStatus, opt => opt.MapFrom(src =>
                    src.Slot != null ? src.Slot.Status : null));
        }
    }
}
