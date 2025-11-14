using Fap.Domain.DTOs.Common;
using Fap.Domain.DTOs.Teacher;

namespace Fap.Api.Interfaces
{
    public interface ITeacherService
    {
        Task<List<TeacherDto>> GetAllTeachersAsync();
        Task<PagedResult<TeacherDto>> GetTeachersAsync(GetTeachersRequest request);
        Task<TeacherDetailDto?> GetTeacherByIdAsync(Guid id);
        Task<TeacherDetailDto?> GetTeacherByUserIdAsync(Guid userId);
    }
}
