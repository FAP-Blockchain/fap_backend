using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(FapDbContext context) : base(context)
        {
        }

        public async Task<Student?> GetByStudentCodeAsync(string studentCode)
        {
            return await _dbSet
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentCode == studentCode);
        }

        public async Task<Student?> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}