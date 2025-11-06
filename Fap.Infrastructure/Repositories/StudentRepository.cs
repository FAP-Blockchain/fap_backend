using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<Student?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(s => s.User)
                .Include(s => s.Enrolls)
                    .ThenInclude(e => e.Class)
                        .ThenInclude(c => c.Subject)
                .Include(s => s.Enrolls)
                    .ThenInclude(e => e.Class)
                        .ThenInclude(c => c.Teacher)
                            .ThenInclude(t => t.User)
                .Include(s => s.ClassMembers)
                    .ThenInclude(cm => cm.Class)
                        .ThenInclude(c => c.Subject)
                .Include(s => s.ClassMembers)
                    .ThenInclude(cm => cm.Class)
                        .ThenInclude(c => c.Teacher)
                            .ThenInclude(t => t.User)
                .Include(s => s.Grades)
                .Include(s => s.Attendances)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetAllWithUsersAsync()
        {
            return await _dbSet
                .Include(s => s.User)
                .Include(s => s.Enrolls)
                .Include(s => s.ClassMembers)
                .OrderBy(s => s.StudentCode)
                .ToListAsync();
        }

        public async Task<(List<Student> Students, int TotalCount)> GetPagedStudentsAsync(
            int page,
            int pageSize,
            string? searchTerm,
            bool? isGraduated,
            bool? isActive,
            decimal? minGPA,
            decimal? maxGPA,
            string? sortBy,
            string? sortOrder)
        {
            var query = _dbSet
                .Include(s => s.User)
                .Include(s => s.Enrolls)
                .Include(s => s.ClassMembers)
                .AsQueryable();

            // 1. Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s =>
                    s.StudentCode.Contains(searchTerm) ||
                    (s.User != null && s.User.FullName.Contains(searchTerm)) ||
                    (s.User != null && s.User.Email.Contains(searchTerm))
                );
            }

            if (isGraduated.HasValue)
            {
                query = query.Where(s => s.IsGraduated == isGraduated.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(s => s.User != null && s.User.IsActive == isActive.Value);
            }

            if (minGPA.HasValue)
            {
                query = query.Where(s => s.GPA >= minGPA.Value);
            }

            if (maxGPA.HasValue)
            {
                query = query.Where(s => s.GPA <= maxGPA.Value);
            }

            // 2. Get total count
            var totalCount = await query.CountAsync();

            // 3. Apply sorting
            query = sortBy?.ToLower() switch
            {
                "studentcode" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.StudentCode)
                    : query.OrderBy(s => s.StudentCode),
                "fullname" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.User != null ? s.User.FullName : "")
                    : query.OrderBy(s => s.User != null ? s.User.FullName : ""),
                "email" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.User != null ? s.User.Email : "")
                    : query.OrderBy(s => s.User != null ? s.User.Email : ""),
                "enrollmentdate" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.EnrollmentDate)
                    : query.OrderBy(s => s.EnrollmentDate),
                "gpa" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.GPA)
                    : query.OrderBy(s => s.GPA),
                "graduationdate" => sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(s => s.GraduationDate ?? DateTime.MaxValue)
                    : query.OrderBy(s => s.GraduationDate ?? DateTime.MaxValue),
                _ => query.OrderBy(s => s.StudentCode) // Default sort
            };

            // 4. Apply pagination
            var students = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (students, totalCount);
        }
    }
}