using Fap.Domain.DTOs.Common;

namespace Fap.Domain.DTOs.Teacher
{
    public class GetTeachersRequest : PaginationRequest
    {
        public string? Specialization { get; set; } // Filter by specialization
        public bool? IsActive { get; set; } // Filter by active status
    }
}
