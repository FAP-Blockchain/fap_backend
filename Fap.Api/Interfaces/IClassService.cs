using Fap.Domain.DTOs.Class;
using Fap.Domain.DTOs.Common;

namespace Fap.Api.Interfaces
{
    public interface IClassService
    {
        Task<PagedResult<ClassDto>> GetClassesAsync(GetClassesRequest request);
        Task<ClassDetailDto?> GetClassByIdAsync(Guid id);
        Task<ClassResponse> CreateClassAsync(CreateClassRequest request);
        Task<ClassResponse> UpdateClassAsync(Guid id, UpdateClassRequest request);
        Task<ClassResponse> DeleteClassAsync(Guid id);
        Task<ClassRosterDto> GetClassRosterAsync(Guid id, ClassRosterRequest request);
    }
}
