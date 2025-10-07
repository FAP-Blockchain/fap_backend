using Fap.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(FapDbContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var hasher = new PasswordHasher<User>();

            // ========== GUID cố định ==========
            var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var teacherRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var studentRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            var adminUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var teacherUserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var studentUserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

            var semesterId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
            var subjectId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");

            // ========== ROLES ==========
            var adminRole = new Role { Id = adminRoleId, Name = "Admin" };
            var teacherRole = new Role { Id = teacherRoleId, Name = "Teacher" };
            var studentRole = new Role { Id = studentRoleId, Name = "Student" };
            await context.Roles.AddRangeAsync(adminRole, teacherRole, studentRole);

            // ========== USERS ==========
            var admin = new User
            {
                Id = adminUserId,
                FullName = "Administrator",
                Email = "admin@fap.edu.vn",
                PasswordHash = hasher.HashPassword(null, "123456"),
                IsActive = true,
                RoleId = adminRoleId,
                CreatedAt = DateTime.UtcNow
            };

            var teacher = new User
            {
                Id = teacherUserId,
                FullName = "Nguyễn Văn Giáo Viên",
                Email = "teacher@fap.edu.vn",
                PasswordHash = hasher.HashPassword(null, "123456"),
                IsActive = true,
                RoleId = teacherRoleId,
                CreatedAt = DateTime.UtcNow
            };

            var student = new User
            {
                Id = studentUserId,
                FullName = "Nguyễn Văn Sinh Viên",
                Email = "student@fap.edu.vn",
                PasswordHash = hasher.HashPassword(null, "123456"),
                IsActive = true,
                RoleId = studentRoleId,
                CreatedAt = DateTime.UtcNow,
                StudentCode = "SV001"
            };

            await context.Users.AddRangeAsync(admin, teacher, student);

            // ========== SEMESTER & SUBJECT ==========
            var semester = new Semester
            {
                Id = semesterId,
                Name = "Fall 2025",
                StartDate = new DateTime(2025, 9, 1),
                EndDate = new DateTime(2025, 12, 31),
                RegistrationStart = new DateTime(2025, 8, 15),
                RegistrationEnd = new DateTime(2025, 8, 30)
            };

            var subject = new Subject
            {
                Id = subjectId,
                SubjectCode = "BC101",
                SubjectName = "Blockchain Fundamentals",
                Credits = 3,
                SemesterId = semesterId
            };

            await context.Semesters.AddAsync(semester);
            await context.Subjects.AddAsync(subject);

            await context.SaveChangesAsync();
        }
    }
}
