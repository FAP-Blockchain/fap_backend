using AutoMapper;
using Fap.Domain.DTOs.TimeSlot;
using Fap.Domain.Entities;

namespace Fap.Api.Mappings
{
    /// <summary>
    /// AutoMapper profile for TimeSlot entities
    /// </summary>
    public class TimeSlotMappingProfile : Profile
    {
        public TimeSlotMappingProfile()
        {
            // ======================================================================
            // TIMESLOT MAPPINGS
            // ======================================================================

            CreateMap<TimeSlot, TimeSlotDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.ToString(@"hh\:mm")))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.ToString(@"hh\:mm")))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src =>
                    (int)(src.EndTime - src.StartTime).TotalMinutes))
                .ForMember(dest => dest.TotalSlots, opt => opt.MapFrom(src =>
                    src.Slots != null ? src.Slots.Count : 0));
        }
    }
}
