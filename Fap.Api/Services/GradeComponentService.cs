using AutoMapper;
using Fap.Api.Interfaces;
using Fap.Domain.DTOs.GradeComponent;
using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fap.Api.Services
{
    public class GradeComponentService : IGradeComponentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GradeComponentService> _logger;

        public GradeComponentService(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<GradeComponentService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GradeComponentDto>> GetAllGradeComponentsAsync(Guid? subjectId = null)
        {
            try
            {
                var components = await _uow.GradeComponents.GetAllWithGradeCountAsync();
                
                // Filter by SubjectId if provided
                if (subjectId.HasValue)
                {
                    components = components.Where(gc => gc.SubjectId == subjectId.Value).ToList();
                }
                
                return components.Select(gc => new GradeComponentDto
                {
                    Id = gc.Id,
                    Name = gc.Name,
                    WeightPercent = gc.WeightPercent,
                    SubjectId = gc.SubjectId,
                    SubjectCode = gc.Subject?.SubjectCode ?? string.Empty,
                    SubjectName = gc.Subject?.SubjectName ?? string.Empty,
                    GradeCount = gc.Grades?.Count ?? 0
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all grade components");
                return new List<GradeComponentDto>();
            }
        }

        public async Task<GradeComponentDto?> GetGradeComponentByIdAsync(Guid id)
        {
            try
            {
                var component = await _uow.GradeComponents.GetByIdWithGradesAsync(id);
                if (component == null) return null;

                return new GradeComponentDto
                {
                    Id = component.Id,
                    Name = component.Name,
                    WeightPercent = component.WeightPercent,
                    SubjectId = component.SubjectId,
                    SubjectCode = component.Subject?.SubjectCode ?? string.Empty,
                    SubjectName = component.Subject?.SubjectName ?? string.Empty,
                    GradeCount = component.Grades?.Count ?? 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grade component {ComponentId}", id);
                return null;
            }
        }

        public async Task<GradeComponentResponse> CreateGradeComponentAsync(CreateGradeComponentRequest request)
        {
            var response = new GradeComponentResponse();

            try
            {
                // Validate SubjectId exists
                var subject = await _uow.Subjects.GetByIdAsync(request.SubjectId);
                if (subject == null)
                {
                    response.Errors.Add($"Subject with ID '{request.SubjectId}' not found");
                    response.Message = "Grade component creation failed";
                    return response;
                }

                // Check if component with same name already exists for this subject
                var existingComponents = await _uow.GradeComponents.GetAllWithGradeCountAsync();
                var duplicate = existingComponents.FirstOrDefault(gc => 
                    gc.SubjectId == request.SubjectId && 
                    gc.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
                    
                if (duplicate != null)
                {
                    response.Errors.Add($"Grade component '{request.Name}' already exists for subject '{subject.SubjectCode}'");
                    response.Message = "Grade component creation failed";
                    return response;
                }

                var newComponent = new GradeComponent
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    WeightPercent = request.WeightPercent,
                    SubjectId = request.SubjectId
                };

                await _uow.GradeComponents.AddAsync(newComponent);
                await _uow.SaveChangesAsync();

                response.Success = true;
                response.Message = "Grade component created successfully";
                response.GradeComponentId = newComponent.Id;

                _logger.LogInformation("Grade component created: {ComponentName} for Subject {SubjectId}", request.Name, request.SubjectId);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating grade component");
                response.Errors.Add($"Internal error: {ex.Message}");
                response.Message = "Grade component creation failed";
                return response;
            }
        }

        public async Task<GradeComponentResponse> UpdateGradeComponentAsync(
            Guid id,
            UpdateGradeComponentRequest request)
        {
            var response = new GradeComponentResponse();

            try
            {
                var component = await _uow.GradeComponents.GetByIdAsync(id);
                if (component == null)
                {
                    response.Errors.Add($"Grade component with ID '{id}' not found");
                    response.Message = "Grade component update failed";
                    return response;
                }

                // Validate SubjectId exists
                var subject = await _uow.Subjects.GetByIdAsync(request.SubjectId);
                if (subject == null)
                {
                    response.Errors.Add($"Subject with ID '{request.SubjectId}' not found");
                    response.Message = "Grade component update failed";
                    return response;
                }

                // Check if another component with same name exists for this subject
                var existingComponents = await _uow.GradeComponents.GetAllWithGradeCountAsync();
                var duplicate = existingComponents.FirstOrDefault(gc => 
                    gc.SubjectId == request.SubjectId && 
                    gc.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase) &&
                    gc.Id != id);
                    
                if (duplicate != null)
                {
                    response.Errors.Add($"Grade component '{request.Name}' already exists for subject '{subject.SubjectCode}'");
                    response.Message = "Grade component update failed";
                    return response;
                }

                component.Name = request.Name;
                component.WeightPercent = request.WeightPercent;
                component.SubjectId = request.SubjectId;

                _uow.GradeComponents.Update(component);
                await _uow.SaveChangesAsync();

                response.Success = true;
                response.Message = "Grade component updated successfully";
                response.GradeComponentId = component.Id;

                _logger.LogInformation("Grade component {ComponentId} updated", id);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grade component {ComponentId}", id);
                response.Errors.Add($"Internal error: {ex.Message}");
                response.Message = "Grade component update failed";
                return response;
            }
        }

        public async Task<GradeComponentResponse> DeleteGradeComponentAsync(Guid id)
        {
            var response = new GradeComponentResponse();

            try
            {
                var component = await _uow.GradeComponents.GetByIdAsync(id);
                if (component == null)
                {
                    response.Errors.Add($"Grade component with ID '{id}' not found");
                    response.Message = "Grade component deletion failed";
                    return response;
                }

                // Check if component is in use
                var isInUse = await _uow.GradeComponents.IsComponentInUseAsync(id);
                if (isInUse)
                {
                    response.Errors.Add("Cannot delete grade component that is currently in use");
                    response.Message = "Grade component deletion failed";
                    return response;
                }

                _uow.GradeComponents.Remove(component);
                await _uow.SaveChangesAsync();

                response.Success = true;
                response.Message = "Grade component deleted successfully";

                _logger.LogInformation("Grade component {ComponentId} deleted", id);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting grade component {ComponentId}", id);
                response.Errors.Add($"Internal error: {ex.Message}");
                response.Message = "Grade component deletion failed";
                return response;
            }
        }
    }
}
