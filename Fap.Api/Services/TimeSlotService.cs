using AutoMapper;
using Fap.Api.Interfaces;
using Fap.Domain.DTOs.TimeSlot;
using Fap.Domain.Entities;
using Fap.Domain.Repositories;

namespace Fap.Api.Services
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<TimeSlotService> _logger;

        public TimeSlotService(IUnitOfWork uow, IMapper mapper, ILogger<TimeSlotService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        // ========== GET ALL TIME SLOTS ==========
        public async Task<List<TimeSlotDto>> GetAllTimeSlotsAsync()
        {
            try
            {
                var timeSlots = await _uow.TimeSlots.GetAllWithSlotsAsync();
                return _mapper.Map<List<TimeSlotDto>>(timeSlots);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting time slots: {ex.Message}");
                throw;
            }
        }

        // ========== GET TIME SLOT BY ID ==========
        public async Task<TimeSlotDto?> GetTimeSlotByIdAsync(Guid id)
        {
            try
            {
                var timeSlot = await _uow.TimeSlots.GetByIdAsync(id);
                return timeSlot == null ? null : _mapper.Map<TimeSlotDto>(timeSlot);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting time slot {id}: {ex.Message}");
                throw;
            }
        }

        // ========== CREATE TIME SLOT ==========
        public async Task<TimeSlotResponse> CreateTimeSlotAsync(CreateTimeSlotRequest request)
        {
            var response = new TimeSlotResponse();

            try
            {
                // 1. Validate time slot name uniqueness
                var existingByName = await _uow.TimeSlots.GetByNameAsync(request.Name);
                if (existingByName != null)
                {
                    response.Errors.Add($"Time slot with name '{request.Name}' already exists");
                    response.Message = "Time slot creation failed";
                    return response;
                }

                // 2. Validate start time < end time
                if (request.StartTime >= request.EndTime)
                {
                    response.Errors.Add("Start time must be before end time");
                    response.Message = "Time slot creation failed";
                    return response;
                }

                // 3. Check for overlapping time slots
                var isOverlapping = await _uow.TimeSlots.IsTimeSlotOverlapping(request.StartTime, request.EndTime);
                if (isOverlapping)
                {
                    response.Errors.Add("Time slot overlaps with existing time slot");
                    response.Message = "Time slot creation failed";
                    return response;
                }

                // 4. Create new time slot
                var newTimeSlot = new TimeSlot
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime
                };

                await _uow.TimeSlots.AddAsync(newTimeSlot);
                await _uow.SaveChangesAsync();

                response.Success = true;
                response.Message = "Time slot created successfully";
                response.TimeSlotId = newTimeSlot.Id;
                _logger.LogInformation($"? Time slot {request.Name} created successfully");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error creating time slot: {ex.Message}");
                response.Errors.Add($"Internal error: {ex.Message}");
                response.Message = "Time slot creation failed";
                return response;
            }
        }

        // ========== UPDATE TIME SLOT ==========
        public async Task<TimeSlotResponse> UpdateTimeSlotAsync(Guid id, UpdateTimeSlotRequest request)
        {
            var response = new TimeSlotResponse
            {
                TimeSlotId = id
            };

            try
            {
                // 1. Check if time slot exists
                var existingTimeSlot = await _uow.TimeSlots.GetByIdAsync(id);
                if (existingTimeSlot == null)
                {
                    response.Errors.Add($"Time slot with ID '{id}' not found");
                    response.Message = "Time slot update failed";
                    return response;
                }

                // 2. Validate start time < end time
                if (request.StartTime >= request.EndTime)
                {
                    response.Errors.Add("Start time must be before end time");
                    response.Message = "Time slot update failed";
                    return response;
                }

                // 3. Check for overlapping (excluding current time slot)
                var isOverlapping = await _uow.TimeSlots.IsTimeSlotOverlapping(request.StartTime, request.EndTime, id);
                if (isOverlapping)
                {
                    response.Errors.Add("Time slot overlaps with existing time slot");
                    response.Message = "Time slot update failed";
                    return response;
                }

                // 4. Update time slot
                existingTimeSlot.Name = request.Name;
                existingTimeSlot.StartTime = request.StartTime;
                existingTimeSlot.EndTime = request.EndTime;

                _uow.TimeSlots.Update(existingTimeSlot);
                await _uow.SaveChangesAsync();

                response.Success = true;
                response.Message = "Time slot updated successfully";
                _logger.LogInformation($"? Time slot {id} updated successfully");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error updating time slot {id}: {ex.Message}");
                response.Errors.Add($"Internal error: {ex.Message}");
                response.Message = "Time slot update failed";
                return response;
            }
        }

        // ========== DELETE TIME SLOT ==========
        public async Task<TimeSlotResponse> DeleteTimeSlotAsync(Guid id)
        {
            var response = new TimeSlotResponse
            {
                TimeSlotId = id
            };

            try
            {
                var existingTimeSlot = await _uow.TimeSlots.GetByIdAsync(id);
                if (existingTimeSlot == null)
                {
                    response.Errors.Add($"Time slot with ID '{id}' not found");
                    response.Message = "Time slot deletion failed";
                    return response;
                }

                _uow.TimeSlots.Remove(existingTimeSlot);
                await _uow.SaveChangesAsync();

                response.Success = true;
                response.Message = "Time slot deleted successfully";
                _logger.LogInformation($"? Time slot {id} deleted successfully");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error deleting time slot {id}: {ex.Message}");
                response.Errors.Add($"Internal error: {ex.Message}");
                response.Message = "Time slot deletion failed";
                return response;
            }
        }
    }
}
