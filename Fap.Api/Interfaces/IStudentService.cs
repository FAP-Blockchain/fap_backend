using Fap.Domain.DTOs.Common;
using Fap.Domain.DTOs.Student;

namespace Fap.Api.Interfaces
{
    public interface IStudentService
    {
        Task<PagedResult<StudentDto>> GetStudentsAsync(GetStudentsRequest request);
        Task<StudentDetailDto?> GetStudentByIdAsync(Guid id);
        Task<StudentDetailDto?> GetStudentByUserIdAsync(Guid userId);
    }
}
