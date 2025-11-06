using Fap.Domain.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Class
{
    public class GetClassesRequest : PaginationRequest
    {
        public string? SubjectId { get; set; }
        public string? TeacherId { get; set; }
        public string? SemesterId { get; set; }
        public string? ClassCode { get; set; }
    }
}
