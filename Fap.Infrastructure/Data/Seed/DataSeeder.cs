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
            var classId = Guid.Parse("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1");
            var studentEntityId = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccce");
            var teacherEntityId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbe");
            var gradeComponentId = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1");
            var gradeId = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2");
            var certificateTemplateId = Guid.Parse("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3");
            var credentialId = Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4");
            var enrollId = Guid.Parse("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5");

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
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddRangeAsync(admin, teacher, student);

            // ========== SEMESTER & SUBJECT ==========
            var semester = new Semester
            {
                Id = semesterId,
                Name = "Fall 2025",
                StartDate = new DateTime(2025, 9, 1),
                EndDate = new DateTime(2025, 12, 31)
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

            // ========== TEACHER & STUDENT ENTITY ==========
            var teacherEntity = new Teacher
            {
                Id = teacherEntityId,
                UserId = teacherUserId,
                TeacherCode = "GV001",
                HireDate = new DateTime(2020, 1, 1),
                Specialization = "Blockchain",
                PhoneNumber = "0901234567"
            };

            var studentEntity = new Student
            {
                Id = studentEntityId,
                UserId = studentUserId,
                StudentCode = "SV001",
                EnrollmentDate = new DateTime(2025, 9, 1),
                GPA = 8.5m
            };

            await context.Teachers.AddAsync(teacherEntity);
            await context.Students.AddAsync(studentEntity);

            // ========== CLASS ==========
            var classEntity = new Class
            {
                Id = classId,
                ClassCode = "BC101A",
                SubjectId = subjectId,
                TeacherUserId = teacherEntityId
            };
            await context.Classes.AddAsync(classEntity);

            // ========== ENROLL ==========
            var enroll = new Enroll
            {
                Id = enrollId,
                StudentId = studentEntityId,
                ClassId = classId,
                RegisteredAt = DateTime.UtcNow,
                IsApproved = true
            };
            await context.Enrolls.AddAsync(enroll);

            // ========== CLASS MEMBER ==========
            var classMember = new ClassMember
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                StudentId = studentEntityId,
                JoinedAt = DateTime.UtcNow
            };
            await context.ClassMembers.AddAsync(classMember);

            // ========== GRADE COMPONENT ==========
            var gradeComponent = new GradeComponent
            {
                Id = gradeComponentId,
                Name = "Final Exam",
                WeightPercent = 70
            };
            await context.GradeComponents.AddAsync(gradeComponent);

            // ========== GRADE ==========
            var grade = new Grade
            {
                Id = gradeId,
                StudentId = studentEntityId,
                SubjectId = subjectId,
                GradeComponentId = gradeComponentId,
                Score = 9.0m,
                LetterGrade = "A",
                UpdatedAt = DateTime.UtcNow
            };
            await context.Grades.AddAsync(grade);

            // ========== CERTIFICATE TEMPLATE ==========
            var certificateTemplate = new CertificateTemplate
            {
                Id = certificateTemplateId,
                Name = "Blockchain Certificate",
                Description = "Certificate for Blockchain Fundamentals",
                TemplateFileUrl = "https://example.com/cert-template.pdf"
            };
            await context.CertificateTemplates.AddAsync(certificateTemplate);

            // ========== CREDENTIAL ==========
            var credential = new Credential
            {
                Id = credentialId,
                CredentialId = "BC101-SV001-2025",
                IPFSHash = "Qm1234567890abcdef",
                FileUrl = "https://example.com/cert.pdf",
                IssuedDate = DateTime.UtcNow,
                IsRevoked = false,
                StudentId = studentEntityId,
                CertificateTemplateId = certificateTemplateId
            };
            await context.Credentials.AddAsync(credential);

            await context.SaveChangesAsync();
        }
    }
}
