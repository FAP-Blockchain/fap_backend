using Fap.Domain.DTOs.Common;

namespace Fap.Domain.DTOs.Student
{
    public class GetStudentsRequest : PaginationRequest
    {
        public bool? IsGraduated { get; set; } // Filter by graduation status
        public bool? IsActive { get; set; } // Filter by active status
        public decimal? MinGPA { get; set; } // Filter by minimum GPA
        public decimal? MaxGPA { get; set; } // Filter by maximum GPA
    }
}
