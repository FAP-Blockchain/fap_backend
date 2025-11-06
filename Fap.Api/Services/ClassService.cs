using AutoMapper;
using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Class;
using Fap.Domain.DTOs.Common;
using Fap.Domain.Repositories;

namespace Fap.Api.Services
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ClassService> _logger;

        public ClassService(IUnitOfWork uow, IMapper mapper, ILogger<ClassService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        // ========== GET CLASSES WITH PAGINATION ==========
        public async Task<PagedResult<ClassDto>> GetClassesAsync(GetClassesRequest request)
        {
            try
            {
                var (classes, totalCount) = await _uow.Classes.GetPagedClassesAsync(
                    request.Page,
                    request.PageSize,
                    request.SubjectId,
                    request.TeacherId,
                    request.SemesterId,
                    request.ClassCode,
                    request.SortBy,
                    request.SortOrder
                );

                var classDtos = _mapper.Map<List<ClassDto>>(classes);

                return new PagedResult<ClassDto>(
                    classDtos,
                    totalCount,
                    request.Page,
                    request.PageSize
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting classes: {ex.Message}");
                throw;
            }
        }

        // ========== GET CLASS BY ID WITH DETAILS ==========
        public async Task<ClassDetailDto?> GetClassByIdAsync(Guid id)
        {
            try
            {
                var @class = await _uow.Classes.GetByIdWithDetailsAsync(id);
                if (@class == null)
                    return null;

                return _mapper.Map<ClassDetailDto>(@class);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting class {id}: {ex.Message}");
                throw;
            }
        }

        // ========== CREATE CLASS ==========
        public async Task<ClassResponse> CreateClassAsync(CreateClassRequest request)
        {
            var response = new ClassResponse
            {
                ClassCode = request.ClassCode
            };

            try
            {
                // 1. Validate ClassCode uniqueness
                var isUnique = await _uow.Classes.IsClassCodeUniqueAsync(request.ClassCode);
                if (!isUnique)
                {
                    response.Errors.Add($"ClassCode '{request.ClassCode}' already exists");
                    response.Message = "Class creation failed";
                    return response;
                }

                // 2. Validate Subject exists
                var subject = await _uow.Subjects.GetByIdAsync(request.SubjectId);
                if (subject == null)
                {
                    response.Errors.Add($"Subject with ID '{request.SubjectId}' not found");
                    response.Message = "Class creation failed";
                    return response;
                }

                // 3. Validate Teacher exists
                var teacher = await _uow.Teachers.GetByIdAsync(request.TeacherId);
                if (teacher == null)
                {
                    response.Errors.Add($"Teacher with ID '{request.TeacherId}' not found");
                    response.Message = "Class creation failed";
                    return response;
                }

                // 4. Create new Class
                var newClass = new Domain.Entities.Class
                {
                    Id = Guid.NewGuid(),
                    ClassCode = request.ClassCode,
                    SubjectId = request.SubjectId,
                    TeacherUserId = request.TeacherId
                };

                await _uow.Classes.AddAsync(newClass);
                await _uow.SaveChangesAsync();

                response.Success = true;
                response.Message = "Class created successfully";
                response.ClassId = newClass.Id;
                _logger.LogInformation($"? Class {request.ClassCode} created successfully");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error creating class: {ex.Message}");
                response.Errors.Add($"Internal error: {ex.Message}");
                response.Message = "Class creation failed";
                return response;
            }
        }

        // ========== UPDATE CLASS ==========
        public async Task<ClassResponse> UpdateClassAsync(Guid id, UpdateClassRequest request)
        {
            var response = new ClassResponse
            {
                ClassId = id,
                ClassCode = request.ClassCode
            };

            try
            {
                // 1. Check if class exists
                var existingClass = await _uow.Classes.GetByIdAsync(id);
                if (existingClass == null)
                {
                    response.Errors.Add($"Class with ID '{id}' not found");
                    response.Message = "Class update failed";
                    return response;
                }

                // 2. Validate ClassCode uniqueness (excluding current class)
                var isUnique = await _uow.Classes.IsClassCodeUniqueAsync(request.ClassCode, id);
                if (!isUnique)
                {
                    response.Errors.Add($"ClassCode '{request.ClassCode}' already exists");
                    response.Message = "Class update failed";
                    return response;
                }

                // 3. Validate Subject exists
                var subject = await _uow.Subjects.GetByIdAsync(request.SubjectId);
                if (subject == null)
                {
                    response.Errors.Add($"Subject with ID '{request.SubjectId}' not found");
                    response.Message = "Class update failed";
                    return response;
                }

                // 4. Validate Teacher exists
                var teacher = await _uow.Teachers.GetByIdAsync(request.TeacherId);
                if (teacher == null)
                {
                    response.Errors.Add($"Teacher with ID '{request.TeacherId}' not found");
                    response.Message = "Class update failed";
                    return response;
                }

                // 5. Update class
                existingClass.ClassCode = request.ClassCode;
                existingClass.SubjectId = request.SubjectId;
                existingClass.TeacherUserId = request.TeacherId;

                _uow.Classes.Update(existingClass);
                await _uow.SaveChangesAsync();

                response.Success = true;
                response.Message = "Class updated successfully";
                _logger.LogInformation($"? Class {id} updated successfully");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error updating class {id}: {ex.Message}");
                response.Errors.Add($"Internal error: {ex.Message}");
                response.Message = "Class update failed";
                return response;
            }
        }

        // ========== DELETE CLASS ==========
        public async Task<ClassResponse> DeleteClassAsync(Guid id)
        {
            var response = new ClassResponse
            {
                ClassId = id
            };

            try
            {
                var existingClass = await _uow.Classes.GetByIdAsync(id);
                if (existingClass == null)
                {
                    response.Errors.Add($"Class with ID '{id}' not found");
                    response.Message = "Class deletion failed";
                    return response;
                }

                response.ClassCode = existingClass.ClassCode;

                _uow.Classes.Remove(existingClass);
                await _uow.SaveChangesAsync();

                response.Success = true;
                response.Message = "Class deleted successfully";
                _logger.LogInformation($"? Class {id} deleted successfully");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error deleting class {id}: {ex.Message}");
                response.Errors.Add($"Internal error: {ex.Message}");
                response.Message = "Class deletion failed";
                return response;
            }
        }

        // ========== GET CLASS ROSTER ==========
        public async Task<ClassRosterDto> GetClassRosterAsync(Guid id, ClassRosterRequest request)
        {
            try
            {
                var classMembers = await _uow.Classes.GetClassRosterAsync(id);

                // Apply filtering
                var query = classMembers.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    query = query.Where(cm =>
                        cm.Student.StudentCode.Contains(request.SearchTerm) ||
                        cm.Student.User.FullName.Contains(request.SearchTerm) ||
                        cm.Student.User.Email.Contains(request.SearchTerm)
                    );
                }

                var totalCount = query.Count();

                // Apply pagination
                var paginatedMembers = query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var students = _mapper.Map<List<ClassStudentInfo>>(paginatedMembers);

                return new ClassRosterDto
                {
                    Students = students,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting class roster for {id}: {ex.Message}");
                throw;
            }
        }
    }
}
