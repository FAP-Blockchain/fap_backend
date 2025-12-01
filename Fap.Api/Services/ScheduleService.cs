using AutoMapper;
using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Schedule;
using Fap.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Fap.Api.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ScheduleService> _logger;

        public ScheduleService(IUnitOfWork uow, IMapper mapper, ILogger<ScheduleService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        // ==================== HELPER METHODS ====================

        public async Task<Guid?> GetTeacherIdByUserIdAsync(Guid userId)
        {
            var teachers = await _uow.Teachers.FindAsync(t => t.UserId == userId);
            return teachers.FirstOrDefault()?.Id;
        }

        public async Task<Guid?> GetStudentIdByUserIdAsync(Guid userId)
        {
            var students = await _uow.Students.FindAsync(s => s.UserId == userId);
            return students.FirstOrDefault()?.Id;
        }

        // ==================== TEACHER SCHEDULE ====================

        public async Task<List<ScheduleItemDto>> GetTeacherScheduleAsync(Guid teacherId, GetScheduleRequest request)
        {
            try
            {
                // Get teacher to validate
                var teacher = await _uow.Teachers.GetByIdAsync(teacherId);
                if (teacher == null)
                {
                    throw new InvalidOperationException($"Teacher with ID {teacherId} not found");
                }

                // Get slots for teacher
                var slots = await _uow.Slots.GetByTeacherIdAsync(teacherId);

                // Apply filters
                var query = slots.AsQueryable();

                if (request.Date.HasValue)
                {
                    query = query.Where(s => s.Date.Date == request.Date.Value.Date);
                }

                if (request.FromDate.HasValue)
                {
                    query = query.Where(s => s.Date >= request.FromDate.Value);
                }

                if (request.ToDate.HasValue)
                {
                    query = query.Where(s => s.Date <= request.ToDate.Value);
                }

                if (request.SemesterId.HasValue)
                {
                    query = query.Where(s => s.Class.SubjectOffering.SemesterId == request.SemesterId.Value);
                }

                if (request.ClassId.HasValue)
                {
                    query = query.Where(s => s.ClassId == request.ClassId.Value);
                }

                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(s => s.Status == request.Status);
                }

                var filteredSlots = query.OrderBy(s => s.Date).ThenBy(s => s.TimeSlot.StartTime).ToList();

                // Map to DTOs
                var scheduleItems = filteredSlots.Select(slot => MapToScheduleItemDto(slot, null, request.IncludeAttendance)).ToList();

                return scheduleItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting teacher schedule: {ex.Message}");
                throw;
            }
        }

        public async Task<WeeklyScheduleDto> GetTeacherWeeklyScheduleAsync(Guid teacherId, GetWeeklyScheduleRequest request)
        {
            try
            {
                var weekStart = request.WeekStartDate.Date;
                var weekEnd = weekStart.AddDays(6);

                var scheduleRequest = new GetScheduleRequest
                {
                    FromDate = weekStart,
                    ToDate = weekEnd,
                    SemesterId = request.SemesterId,
                    IncludeAttendance = request.IncludeAttendance
                };

                var slots = await GetTeacherScheduleAsync(teacherId, scheduleRequest);

                return BuildWeeklySchedule(weekStart, weekEnd, slots);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting teacher weekly schedule: {ex.Message}");
                throw;
            }
        }

        public async Task<DailyScheduleDto> GetTeacherDailyScheduleAsync(Guid teacherId, DateTime date, bool includeAttendance = true)
        {
            try
            {
                var scheduleRequest = new GetScheduleRequest
                {
                    Date = date,
                    IncludeAttendance = includeAttendance
                };

                var slots = await GetTeacherScheduleAsync(teacherId, scheduleRequest);

                return new DailyScheduleDto
                {
                    Date = date.Date,
                    DayOfWeek = date.ToString("dddd", CultureInfo.InvariantCulture),
                    Slots = slots,
                    TotalSlots = slots.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting teacher daily schedule: {ex.Message}");
                throw;
            }
        }

        public async Task<SemesterScheduleDto> GetTeacherSemesterScheduleAsync(Guid teacherId, Guid semesterId)
        {
            try
            {
                var semester = await _uow.Semesters.GetByIdAsync(semesterId);
                if (semester == null)
                {
                    throw new InvalidOperationException($"Semester with ID {semesterId} not found");
                }

                var teacher = await _uow.Teachers.GetByIdAsync(teacherId);
                if (teacher == null)
                {
                    throw new InvalidOperationException($"Teacher with ID {teacherId} not found");
                }

                // Get all classes taught by teacher in this semester
                var allClasses = await _uow.Classes.GetAllWithDetailsAsync();
                var teacherClasses = allClasses
                    .Where(c => c.TeacherUserId == teacherId && c.SubjectOffering.SemesterId == semesterId)
                    .ToList();

                var classSummaries = new List<ClassScheduleSummary>();

                foreach (var classEntity in teacherClasses)
                {
                    var slots = classEntity.Slots?.ToList() ?? new List<Domain.Entities.Slot>();

                    classSummaries.Add(new ClassScheduleSummary
                    {
                        ClassId = classEntity.Id,
                        ClassCode = classEntity.ClassCode,
                        SubjectName = classEntity.SubjectOffering?.Subject?.SubjectName ?? "Unknown",
                        SubjectCode = classEntity.SubjectOffering?.Subject?.SubjectCode ?? "Unknown",
                        TeacherName = classEntity.Teacher?.User?.FullName ?? "Unknown",
                        TotalSlots = slots.Count,
                        CompletedSlots = slots.Count(s => s.Status == "Completed"),
                        UpcomingSlots = slots.Count(s => s.Status == "Scheduled" && s.Date >= DateTime.UtcNow.Date)
                    });
                }

                var totalSlots = classSummaries.Sum(c => c.TotalSlots);

                return new SemesterScheduleDto
                {
                    SemesterId = semesterId,
                    SemesterName = semester.Name,
                    SemesterStartDate = semester.StartDate,
                    SemesterEndDate = semester.EndDate,
                    Classes = classSummaries,
                    TotalClasses = classSummaries.Count,
                    TotalSlots = totalSlots
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting teacher semester schedule: {ex.Message}");
                throw;
            }
        }

        // ==================== STUDENT SCHEDULE ====================

        public async Task<List<ScheduleItemDto>> GetStudentScheduleAsync(Guid studentId, GetScheduleRequest request)
        {
            try
            {
                // Get student to validate
                var student = await _uow.Students.GetByIdAsync(studentId);
                if (student == null)
                {
                    throw new InvalidOperationException($"Student with ID {studentId} not found");
                }

                // Get all classes the student is enrolled in
                var classMembers = await _uow.Classes.GetAllWithDetailsAsync();
                var studentClasses = classMembers
   .Where(c => c.Members != null && c.Members.Any(m => m.StudentId == studentId))
   .ToList();

                var studentClassIds = studentClasses.Select(c => c.Id).ToList();

                // Get all slots for these classes with navigation properties loaded
                var slots = await _uow.Slots.GetByClassIdsAsync(studentClassIds);

                // Apply filters
                var query = slots.AsQueryable();

                if (request.Date.HasValue)
                {
                    query = query.Where(s => s.Date.Date == request.Date.Value.Date);
                }

                if (request.FromDate.HasValue)
                {
                    query = query.Where(s => s.Date >= request.FromDate.Value);
                }

                if (request.ToDate.HasValue)
                {
                    query = query.Where(s => s.Date <= request.ToDate.Value);
                }

                if (request.SemesterId.HasValue)
                {
                    query = query.Where(s => s.Class.SubjectOffering.SemesterId == request.SemesterId.Value);
                }

                if (request.ClassId.HasValue)
                {
                    query = query.Where(s => s.ClassId == request.ClassId.Value);
                }

                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(s => s.Status == request.Status);
                }

                var filteredSlots = query.OrderBy(s => s.Date).ThenBy(s => s.TimeSlot.StartTime).ToList();

                // Map to DTOs with student attendance info
                var scheduleItems = filteredSlots.Select(slot => MapToScheduleItemDto(slot, studentId, request.IncludeAttendance)).ToList();

                return scheduleItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting student schedule: {ex.Message}");
                throw;
            }
        }

        public async Task<WeeklyScheduleDto> GetStudentWeeklyScheduleAsync(Guid studentId, GetWeeklyScheduleRequest request)
        {
            try
            {
                var weekStart = request.WeekStartDate.Date;
                var weekEnd = weekStart.AddDays(6);

                var scheduleRequest = new GetScheduleRequest
                {
                    FromDate = weekStart,
                    ToDate = weekEnd,
                    SemesterId = request.SemesterId,
                    IncludeAttendance = request.IncludeAttendance
                };

                var slots = await GetStudentScheduleAsync(studentId, scheduleRequest);

                return BuildWeeklySchedule(weekStart, weekEnd, slots);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting student weekly schedule: {ex.Message}");
                throw;
            }
        }

        public async Task<DailyScheduleDto> GetStudentDailyScheduleAsync(Guid studentId, DateTime date, bool includeAttendance = true)
        {
            try
            {
                var scheduleRequest = new GetScheduleRequest
                {
                    Date = date,
                    IncludeAttendance = includeAttendance
                };

                var slots = await GetStudentScheduleAsync(studentId, scheduleRequest);

                return new DailyScheduleDto
                {
                    Date = date.Date,
                    DayOfWeek = date.ToString("dddd", CultureInfo.InvariantCulture),
                    Slots = slots,
                    TotalSlots = slots.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting student daily schedule: {ex.Message}");
                throw;
            }
        }

        public async Task<SemesterScheduleDto> GetStudentSemesterScheduleAsync(Guid studentId, Guid semesterId)
        {
            try
            {
                var semester = await _uow.Semesters.GetByIdAsync(semesterId);
                if (semester == null)
                {
                    throw new InvalidOperationException($"Semester with ID {semesterId} not found");
                }

                var student = await _uow.Students.GetByIdAsync(studentId);
                if (student == null)
                {
                    throw new InvalidOperationException($"Student with ID {studentId} not found");
                }

                // Get all classes student is enrolled in for this semester
                var allClasses = await _uow.Classes.GetAllWithDetailsAsync();
                var studentClasses = allClasses
                        .Where(c => c.SubjectOffering.SemesterId == semesterId &&
                        c.Members != null && c.Members.Any(m => m.StudentId == studentId))
                        .ToList();

                var classSummaries = new List<ClassScheduleSummary>();

                foreach (var classEntity in studentClasses)
                {
                    var slots = classEntity.Slots?.ToList() ?? new List<Domain.Entities.Slot>();

                    classSummaries.Add(new ClassScheduleSummary
                    {
                        ClassId = classEntity.Id,
                        ClassCode = classEntity.ClassCode,
                        SubjectName = classEntity.SubjectOffering?.Subject?.SubjectName ?? "Unknown",
                        SubjectCode = classEntity.SubjectOffering?.Subject?.SubjectCode ?? "Unknown",
                        TeacherName = classEntity.Teacher?.User?.FullName ?? "Unknown",
                        TotalSlots = slots.Count,
                        CompletedSlots = slots.Count(s => s.Status == "Completed"),
                        UpcomingSlots = slots.Count(s => s.Status == "Scheduled" && s.Date >= DateTime.UtcNow.Date)
                    });
                }

                var totalSlots = classSummaries.Sum(c => c.TotalSlots);

                return new SemesterScheduleDto
                {
                    SemesterId = semesterId,
                    SemesterName = semester.Name,
                    SemesterStartDate = semester.StartDate,
                    SemesterEndDate = semester.EndDate,
                    Classes = classSummaries,
                    TotalClasses = classSummaries.Count,
                    TotalSlots = totalSlots
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting student semester schedule: {ex.Message}");
                throw;
            }
        }

        // ==================== CURRENT USER SCHEDULE ====================

        public async Task<List<ScheduleItemDto>> GetMyScheduleAsync(Guid userId, GetScheduleRequest request)
        {
            try
            {
                // Check if user is a teacher or student
                var user = await _uow.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }

                // Try to get as teacher first
                var teachers = await _uow.Teachers.FindAsync(t => t.UserId == userId);
                var teacher = teachers.FirstOrDefault();

                if (teacher != null)
                {
                    return await GetTeacherScheduleAsync(teacher.Id, request);
                }

                // Try to get as student
                var students = await _uow.Students.FindAsync(s => s.UserId == userId);
                var student = students.FirstOrDefault();

                if (student != null)
                {
                    return await GetStudentScheduleAsync(student.Id, request);
                }

                throw new InvalidOperationException("User is neither a teacher nor a student");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting my schedule: {ex.Message}");
                throw;
            }
        }

        public async Task<WeeklyScheduleDto> GetMyWeeklyScheduleAsync(Guid userId, GetWeeklyScheduleRequest request)
        {
            try
            {
                var user = await _uow.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }

                var teachers = await _uow.Teachers.FindAsync(t => t.UserId == userId);
                var teacher = teachers.FirstOrDefault();

                if (teacher != null)
                {
                    return await GetTeacherWeeklyScheduleAsync(teacher.Id, request);
                }

                var students = await _uow.Students.FindAsync(s => s.UserId == userId);
                var student = students.FirstOrDefault();

                if (student != null)
                {
                    return await GetStudentWeeklyScheduleAsync(student.Id, request);
                }

                throw new InvalidOperationException("User is neither a teacher nor a student");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting my weekly schedule: {ex.Message}");
                throw;
            }
        }

        public async Task<DailyScheduleDto> GetMyDailyScheduleAsync(Guid userId, DateTime date, bool includeAttendance = true)
        {
            try
            {
                var user = await _uow.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }

                var teachers = await _uow.Teachers.FindAsync(t => t.UserId == userId);
                var teacher = teachers.FirstOrDefault();

                if (teacher != null)
                {
                    return await GetTeacherDailyScheduleAsync(teacher.Id, date, includeAttendance);
                }

                var students = await _uow.Students.FindAsync(s => s.UserId == userId);
                var student = students.FirstOrDefault();

                if (student != null)
                {
                    return await GetStudentDailyScheduleAsync(student.Id, date, includeAttendance);
                }

                throw new InvalidOperationException("User is neither a teacher nor a student");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting my daily schedule: {ex.Message}");
                throw;
            }
        }

        // ==================== SCHEDULE UTILITIES ====================

        public async Task<List<ScheduleConflictDto>> CheckScheduleConflictsAsync(Guid userId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var request = new GetScheduleRequest
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    IncludeAttendance = false
                };

                var schedule = await GetMyScheduleAsync(userId, request);

                var conflicts = new List<ScheduleConflictDto>();

                // Group by date
                var slotsByDate = schedule.GroupBy(s => s.Date.Date);

                foreach (var dateGroup in slotsByDate)
                {
                    var slotsWithTime = dateGroup.Where(s => s.StartTime.HasValue && s.EndTime.HasValue).ToList();

                    // Check for overlapping time slots
                    for (int i = 0; i < slotsWithTime.Count; i++)
                    {
                        for (int j = i + 1; j < slotsWithTime.Count; j++)
                        {
                            var slot1 = slotsWithTime[i];
                            var slot2 = slotsWithTime[j];

                            // Check if time ranges overlap
                            if (slot1.StartTime < slot2.EndTime && slot2.StartTime < slot1.EndTime)
                            {
                                conflicts.Add(new ScheduleConflictDto
                                {
                                    Date = dateGroup.Key,
                                    StartTime = slot1.StartTime < slot2.StartTime ? slot1.StartTime : slot2.StartTime,
                                    EndTime = slot1.EndTime > slot2.EndTime ? slot1.EndTime : slot2.EndTime,
                                    ConflictingSlots = new List<ScheduleItemDto> { slot1, slot2 },
                                    Reason = "Time slot overlap"
                                });
                            }
                        }
                    }
                }

                return conflicts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking schedule conflicts: {ex.Message}");
                throw;
            }
        }

        public async Task<ScheduleStatisticsDto> GetScheduleStatisticsAsync(Guid userId, Guid? semesterId = null)
        {
            try
            {
                var user = await _uow.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }

                var request = new GetScheduleRequest
                {
                    SemesterId = semesterId,
                    IncludeAttendance = true
                };

                var schedule = await GetMyScheduleAsync(userId, request);

                var statistics = new ScheduleStatisticsDto
                {
                    TotalSlots = schedule.Count,
                    ScheduledSlots = schedule.Count(s => s.Status == "Scheduled"),
                    CompletedSlots = schedule.Count(s => s.Status == "Completed"),
                    CancelledSlots = schedule.Count(s => s.Status == "Cancelled"),
                    SlotsWithAttendance = schedule.Count(s => s.HasAttendance),
                    SlotsNeedingAttendance = schedule.Count(s => s.Status == "Scheduled" && !s.HasAttendance && s.Date < DateTime.UtcNow.Date)
                };

                // Check if student
                var students = await _uow.Students.FindAsync(s => s.UserId == userId);
                var student = students.FirstOrDefault();

                if (student != null)
                {
                    var attendedSlots = schedule.Where(s => s.IsPresent.HasValue).ToList();
                    statistics.TotalPresent = attendedSlots.Count(s => s.IsPresent == true);
                    statistics.TotalAbsent = attendedSlots.Count(s => s.IsPresent == false);

                    if (attendedSlots.Any())
                    {
                        statistics.AttendanceRate = (decimal)statistics.TotalPresent / attendedSlots.Count * 100;
                    }
                }
                else
                {
                    // Check if teacher
                    var teachers = await _uow.Teachers.FindAsync(t => t.UserId == userId);
                    var teacher = teachers.FirstOrDefault();

                    if (teacher != null)
                    {
                        var allClasses = await _uow.Classes.GetAllWithDetailsAsync();
                        var teacherClasses = allClasses.Where(c => c.TeacherUserId == teacher.Id).ToList();

                        statistics.TotalClasses = teacherClasses.Count;
                        statistics.TotalStudents = teacherClasses.Sum(c => c.Members?.Count ?? 0);
                    }
                }

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting schedule statistics: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER METHODS ====================

        private ScheduleItemDto MapToScheduleItemDto(Domain.Entities.Slot slot, Guid? studentId, bool includeAttendance)
        {
            var dto = new ScheduleItemDto
            {
                SlotId = slot.Id,
                ClassId = slot.ClassId,
                ClassCode = slot.Class?.ClassCode ?? "Unknown",
                SubjectId = slot.Class?.SubjectOffering?.SubjectId ?? Guid.Empty,
                SubjectName = slot.Class?.SubjectOffering?.Subject?.SubjectName ?? "Unknown",
                SubjectCode = slot.Class?.SubjectOffering?.Subject?.SubjectCode ?? "Unknown",
                Credits = slot.Class?.SubjectOffering?.Subject?.Credits ?? 0,
                Date = slot.Date,
                DayOfWeek = slot.Date.ToString("dddd", CultureInfo.InvariantCulture),
                TimeSlotId = slot.TimeSlotId,
                TimeSlotName = slot.TimeSlot?.Name,
                StartTime = slot.TimeSlot?.StartTime,
                EndTime = slot.TimeSlot?.EndTime,
                TeacherId = slot.Class?.TeacherUserId ?? Guid.Empty,
                TeacherName = slot.Class?.Teacher?.User?.FullName ?? "Unknown",
                TeacherCode = slot.Class?.Teacher?.TeacherCode ?? "Unknown",
                SubstituteTeacherId = slot.SubstituteTeacherId,
                SubstituteTeacherName = slot.SubstituteTeacher?.User?.FullName,
                SubstitutionReason = slot.SubstitutionReason,
                Status = slot.Status,
                Notes = slot.Notes,
                HasAttendance = slot.Attendances?.Any() ?? false,
                TotalStudents = slot.Class?.Members?.Count ?? 0,
                PresentCount = slot.Attendances?.Count(a => a.IsPresent) ?? 0,
                AbsentCount = slot.Attendances?.Count(a => !a.IsPresent) ?? 0
            };

            // If student ID is provided, include their attendance status
            if (studentId.HasValue && includeAttendance && slot.Attendances != null)
            {
                var attendance = slot.Attendances.FirstOrDefault(a => a.StudentId == studentId.Value);
                if (attendance != null)
                {
                    dto.IsPresent = attendance.IsPresent;
                }
            }

            return dto;
        }

        private WeeklyScheduleDto BuildWeeklySchedule(DateTime weekStart, DateTime weekEnd, List<ScheduleItemDto> slots)
        {
            var weeklySchedule = new WeeklyScheduleDto
            {
                WeekStartDate = weekStart,
                WeekEndDate = weekEnd,
                WeekLabel = $"Week of {weekStart:MMM dd} - {weekEnd:MMM dd, yyyy}",
                Days = new List<DailyScheduleDto>(),
                TotalSlots = slots.Count
            };

            // Create daily schedules for each day of the week
            for (int i = 0; i < 7; i++)
            {
                var currentDate = weekStart.AddDays(i);
                var dailySlots = slots.Where(s => s.Date.Date == currentDate.Date).ToList();

                weeklySchedule.Days.Add(new DailyScheduleDto
                {
                    Date = currentDate,
                    DayOfWeek = currentDate.ToString("dddd", CultureInfo.InvariantCulture),
                    Slots = dailySlots,
                    TotalSlots = dailySlots.Count
                });
            }

            return weeklySchedule;
        }
    }
}
