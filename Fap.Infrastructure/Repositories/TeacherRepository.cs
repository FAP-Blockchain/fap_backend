using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(FapDbContext context) : base(context)
        {
        }

        public async Task<Teacher?> GetByTeacherCodeAsync(string teacherCode)
        {
            return await _dbSet
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TeacherCode == teacherCode);
        }

        public async Task<Teacher?> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }
    }
}