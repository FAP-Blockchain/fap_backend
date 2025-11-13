using Fap.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(FapDbContext context)
        {
            // TEMPORARY: Comment out to force re-seed
            // if (await context.Users.AnyAsync()) return;

            // Check if TimeSlots exist instead (new data we added)
            if (await context.TimeSlots.AnyAsync())
            {
                Console.WriteLine("Seed data already exists. Skipping seeding.");
                return;
            }

            var hasher = new PasswordHasher<User>();

            // ========== GUID cố định ==========
            var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var teacherRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var studentRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            var adminUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var teacher1UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var teacher2UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbe");
            var teacher3UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd");
            var student1UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
            var student2UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccf");
            var student3UserId = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccce");

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

            var teacher1 = new User
            {
                Id = teacher1UserId,
                FullName = "Nguyễn Văn Giáo Viên",
                Email = "teacher1@fap.edu.vn",
                PasswordHash = hasher.HashPassword(null, "123456"),
                IsActive = true,
                RoleId = teacherRoleId,
                CreatedAt = DateTime.UtcNow
            };

            var teacher2 = new User
            {
                Id = teacher2UserId,
                FullName = "Trần Thị Hồng",
                Email = "teacher2@fap.edu.vn",
                PasswordHash = hasher.HashPassword(null, "123456"),
                IsActive = true,
                RoleId = teacherRoleId,
                CreatedAt = DateTime.UtcNow
            };

            var teacher3 = new User
            {
                Id = teacher3UserId,
                FullName = "Lê Văn Toán",
                Email = "teacher3@fap.edu.vn",
                PasswordHash = hasher.HashPassword(null, "123456"),
                IsActive = true,
                RoleId = teacherRoleId,
                CreatedAt = DateTime.UtcNow
            };

            var student1 = new User
            {
                Id = student1UserId,
                FullName = "Nguyễn Văn An",
                Email = "student1@fap.edu.vn",
                PasswordHash = hasher.HashPassword(null, "123456"),
                IsActive = true,
                RoleId = studentRoleId,
                CreatedAt = DateTime.UtcNow
            };

            var student2 = new User
            {
                Id = student2UserId,
                FullName = "Phạm Thị Bình",
                Email = "student2@fap.edu.vn",
                PasswordHash = hasher.HashPassword(null, "123456"),
                IsActive = true,
                RoleId = studentRoleId,
                CreatedAt = DateTime.UtcNow
            };

            var student3 = new User
            {
                Id = student3UserId,
                FullName = "Hoàng Văn Châu",
                Email = "student3@fap.edu.vn",
                PasswordHash = hasher.HashPassword(null, "123456"),
                IsActive = true,
                RoleId = studentRoleId,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddRangeAsync(admin, teacher1, teacher2, teacher3, student1, student2, student3);

            // ========== SEMESTERS ==========
            var semester1 = new Semester
            {
                Id = Guid.Parse("d1111111-1111-1111-1111-111111111111"),
                Name = "Spring 2024",
                StartDate = new DateTime(2024, 1, 15),
                EndDate = new DateTime(2024, 5, 15),
                IsClosed = true  // Closed semester for testing
            };

            var semester2 = new Semester
            {
                Id = Guid.Parse("d2222222-2222-2222-2222-222222222222"),
                Name = "Summer 2024",
                StartDate = new DateTime(2024, 6, 1),
                EndDate = new DateTime(2024, 8, 31),
                IsClosed = false
            };

            var semester3 = new Semester
            {
                Id = Guid.Parse("d3333333-3333-3333-3333-333333333333"),
                Name = "Fall 2024",
                StartDate = new DateTime(2024, 9, 1),
                EndDate = new DateTime(2024, 12, 31),
                IsClosed = false
            };

            var semester4 = new Semester
            {
                Id = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
                Name = "Spring 2025",
                StartDate = new DateTime(2025, 1, 15),
                EndDate = new DateTime(2025, 5, 15),
                IsClosed = false
            };

            var semester5 = new Semester
            {
                Id = Guid.Parse("d5555555-5555-5555-5555-555555555555"),
                Name = "Fall 2025",
                StartDate = new DateTime(2025, 9, 1),
                EndDate = new DateTime(2025, 12, 31),
                IsClosed = false
            };

            await context.Semesters.AddRangeAsync(semester1, semester2, semester3, semester4, semester5);

            // ========== TEACHERS ==========
            var teacher1Entity = new Teacher
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbba"),
                UserId = teacher1UserId,
                TeacherCode = "GV001",
                HireDate = new DateTime(2020, 1, 1),
                Specialization = "Blockchain & Cryptography",
                PhoneNumber = "0901234567"
            };

            var teacher2Entity = new Teacher
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb9"),
                UserId = teacher2UserId,
                TeacherCode = "GV002",
                HireDate = new DateTime(2019, 6, 1),
                Specialization = "Software Engineering",
                PhoneNumber = "0901234568"
            };

            var teacher3Entity = new Teacher
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb8"),
                UserId = teacher3UserId,
                TeacherCode = "GV003",
                HireDate = new DateTime(2021, 3, 15),
                Specialization = "Database & Data Science",
                PhoneNumber = "0901234569"
            };

            await context.Teachers.AddRangeAsync(teacher1Entity, teacher2Entity, teacher3Entity);

            // ========== STUDENTS ==========
            var student1Entity = new Student
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccb"),
                UserId = student1UserId,
                StudentCode = "SV001",
                EnrollmentDate = new DateTime(2023, 9, 1),
                GPA = 8.5m
            };

            var student2Entity = new Student
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccca"),
                UserId = student2UserId,
                StudentCode = "SV002",
                EnrollmentDate = new DateTime(2023, 9, 1),
                GPA = 7.8m
            };

            var student3Entity = new Student
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc9"),
                UserId = student3UserId,
                StudentCode = "SV003",
                EnrollmentDate = new DateTime(2024, 1, 15),
                GPA = 9.2m
            };

            await context.Students.AddRangeAsync(student1Entity, student2Entity, student3Entity);

            // ========== SUBJECTS ==========
            // Spring 2024 (Closed) Subjects
            var subject1 = new Subject
            {
                Id = Guid.Parse("e1111111-1111-1111-1111-111111111111"),
                SubjectCode = "BC101",
                SubjectName = "Blockchain Fundamentals",
                Credits = 3,
                SemesterId = semester1.Id
            };

            var subject2 = new Subject
            {
                Id = Guid.Parse("e2222222-2222-2222-2222-222222222222"),
                SubjectCode = "SE201",
                SubjectName = "Software Engineering",
                Credits = 4,
                SemesterId = semester1.Id
            };

            // Summer 2024 Subjects
            var subject3 = new Subject
            {
                Id = Guid.Parse("e3333333-3333-3333-3333-333333333333"),
                SubjectCode = "DB301",
                SubjectName = "Database Management Systems",
                Credits = 3,
                SemesterId = semester2.Id
            };

            var subject4 = new Subject
            {
                Id = Guid.Parse("e4444444-4444-4444-4444-444444444444"),
                SubjectCode = "BC202",
                SubjectName = "Smart Contract Development",
                Credits = 4,
                SemesterId = semester2.Id
            };

            // Fall 2024 Subjects
            var subject5 = new Subject
            {
                Id = Guid.Parse("e5555555-5555-5555-5555-555555555555"),
                SubjectCode = "BC303",
                SubjectName = "Blockchain Security",
                Credits = 3,
                SemesterId = semester3.Id
            };

            var subject6 = new Subject
            {
                Id = Guid.Parse("e6666666-6666-6666-6666-666666666666"),
                SubjectCode = "AI401",
                SubjectName = "Artificial Intelligence",
                Credits = 4,
                SemesterId = semester3.Id
            };

            var subject7 = new Subject
            {
                Id = Guid.Parse("e7777777-7777-7777-7777-777777777777"),
                SubjectCode = "NW201",
                SubjectName = "Computer Networks",
                Credits = 3,
                SemesterId = semester3.Id
            };

            // Spring 2025 Subjects
            var subject8 = new Subject
            {
                Id = Guid.Parse("e8888888-8888-8888-8888-888888888888"),
                SubjectCode = "DS501",
                SubjectName = "Data Structures & Algorithms",
                Credits = 4,
                SemesterId = semester4.Id
            };

            var subject9 = new Subject
            {
                Id = Guid.Parse("e9999999-9999-9999-9999-999999999999"),
                SubjectCode = "WEB301",
                SubjectName = "Web Development",
                Credits = 3,
                SemesterId = semester4.Id
            };

            // Fall 2025 Subjects
            var subject10 = new Subject
            {
                Id = Guid.Parse("ea111111-1111-1111-1111-111111111111"),
                SubjectCode = "ML501",
                SubjectName = "Machine Learning",
                Credits = 4,
                SemesterId = semester5.Id
            };

            await context.Subjects.AddRangeAsync(
                subject1, subject2, subject3, subject4, subject5,
                subject6, subject7, subject8, subject9, subject10
            );

            // ========== CLASSES ==========
            // Classes for Spring 2024 (with students enrolled)
            var class1 = new Class
            {
                Id = Guid.Parse("f1111111-1111-1111-1111-111111111111"),
                ClassCode = "BC101-A",
                SubjectId = subject1.Id,
                TeacherUserId = teacher1Entity.Id
            };

            var class2 = new Class
            {
                Id = Guid.Parse("f2222222-2222-2222-2222-222222222222"),
                ClassCode = "SE201-A",
                SubjectId = subject2.Id,
                TeacherUserId = teacher2Entity.Id
            };

            // Classes for Summer 2024
            var class3 = new Class
            {
                Id = Guid.Parse("f3333333-3333-3333-3333-333333333333"),
                ClassCode = "DB301-A",
                SubjectId = subject3.Id,
                TeacherUserId = teacher3Entity.Id
            };

            var class4 = new Class
            {
                Id = Guid.Parse("f4444444-4444-4444-4444-444444444444"),
                ClassCode = "BC202-A",
                SubjectId = subject4.Id,
                TeacherUserId = teacher1Entity.Id
            };

            var class5 = new Class
            {
                Id = Guid.Parse("f5555555-5555-5555-5555-555555555555"),
                ClassCode = "BC202-B",
                SubjectId = subject4.Id,
                TeacherUserId = teacher1Entity.Id
            };

            // Classes for Fall 2024
            var class6 = new Class
            {
                Id = Guid.Parse("f6666666-6666-6666-6666-666666666666"),
                ClassCode = "BC303-A",
                SubjectId = subject5.Id,
                TeacherUserId = teacher1Entity.Id
            };

            var class7 = new Class
            {
                Id = Guid.Parse("f7777777-7777-7777-7777-777777777777"),
                ClassCode = "AI401-A",
                SubjectId = subject6.Id,
                TeacherUserId = teacher2Entity.Id
            };

            await context.Classes.AddRangeAsync(class1, class2, class3, class4, class5, class6, class7);

            // ========== CLASS MEMBERS ==========
            var classMembers = new List<ClassMember>
            {
                // Class 1 (BC101-A) - 3 students
                new ClassMember { Id = Guid.NewGuid(), ClassId = class1.Id, StudentId = student1Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-90) },
                new ClassMember { Id = Guid.NewGuid(), ClassId = class1.Id, StudentId = student2Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-89) },
                new ClassMember { Id = Guid.NewGuid(), ClassId = class1.Id, StudentId = student3Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-88) },

                // Class 2 (SE201-A) - 2 students
                new ClassMember { Id = Guid.NewGuid(), ClassId = class2.Id, StudentId = student1Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-85) },
                new ClassMember { Id = Guid.NewGuid(), ClassId = class2.Id, StudentId = student3Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-84) },

                // Class 3 (DB301-A) - 3 students
                new ClassMember { Id = Guid.NewGuid(), ClassId = class3.Id, StudentId = student1Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-60) },
                new ClassMember { Id = Guid.NewGuid(), ClassId = class3.Id, StudentId = student2Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-59) },
                new ClassMember { Id = Guid.NewGuid(), ClassId = class3.Id, StudentId = student3Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-58) },

                // Class 4 (BC202-A) - 2 students
                new ClassMember { Id = Guid.NewGuid(), ClassId = class4.Id, StudentId = student1Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-55) },
                new ClassMember { Id = Guid.NewGuid(), ClassId = class4.Id, StudentId = student2Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-54) },

                // Class 5 (BC202-B) - 1 student
                new ClassMember { Id = Guid.NewGuid(), ClassId = class5.Id, StudentId = student3Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-53) },

                // Class 6 (BC303-A) - 2 students
                new ClassMember { Id = Guid.NewGuid(), ClassId = class6.Id, StudentId = student1Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-30) },
                new ClassMember { Id = Guid.NewGuid(), ClassId = class6.Id, StudentId = student3Entity.Id, JoinedAt = DateTime.UtcNow.AddDays(-29) }
            };

            await context.ClassMembers.AddRangeAsync(classMembers);

            // ========== ENROLLMENTS ==========
            var enrollments = new List<Enroll>
            {
                new Enroll { Id = Guid.NewGuid(), StudentId = student1Entity.Id, ClassId = class1.Id, RegisteredAt = DateTime.UtcNow.AddDays(-91), IsApproved = true },
                new Enroll { Id = Guid.NewGuid(), StudentId = student2Entity.Id, ClassId = class1.Id, RegisteredAt = DateTime.UtcNow.AddDays(-90), IsApproved = true },
                new Enroll { Id = Guid.NewGuid(), StudentId = student3Entity.Id, ClassId = class1.Id, RegisteredAt = DateTime.UtcNow.AddDays(-89), IsApproved = true },
                new Enroll { Id = Guid.NewGuid(), StudentId = student1Entity.Id, ClassId = class2.Id, RegisteredAt = DateTime.UtcNow.AddDays(-86), IsApproved = true },
                new Enroll { Id = Guid.NewGuid(), StudentId = student3Entity.Id, ClassId = class2.Id, RegisteredAt = DateTime.UtcNow.AddDays(-85), IsApproved = true },
                new Enroll { Id = Guid.NewGuid(), StudentId = student2Entity.Id, ClassId = class3.Id, RegisteredAt = DateTime.UtcNow.AddDays(-61), IsApproved = false }, // Pending
                new Enroll { Id = Guid.NewGuid(), StudentId = student1Entity.Id, ClassId = class6.Id, RegisteredAt = DateTime.UtcNow.AddDays(-31), IsApproved = false } // Pending
            };

            await context.Enrolls.AddRangeAsync(enrollments);

            // ========== GRADE COMPONENTS ==========
            var gradeComponents = new List<GradeComponent>
            {
                new GradeComponent { Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"), Name = "Midterm Exam", WeightPercent = 30 },
                new GradeComponent { Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"), Name = "Final Exam", WeightPercent = 70 },
                new GradeComponent { Id = Guid.Parse("a3333333-3333-3333-3333-333333333333"), Name = "Assignment", WeightPercent = 20 },
                new GradeComponent { Id = Guid.Parse("a4444444-4444-4444-4444-444444444444"), Name = "Project", WeightPercent = 30 }
            };

            await context.GradeComponents.AddRangeAsync(gradeComponents);

            // ========== GRADES ==========
            var grades = new List<Grade>
            {
                // ========== BC101-A (Class 1) - Blockchain Fundamentals ==========
                // Student 1 (SV001) - Complete grades with all components
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[0].Id, Score = 8.5m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[1].Id, Score = 9.0m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[2].Id, Score = 8.0m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[3].Id, Score = 8.7m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },

                // Student 2 (SV002) - Partial grades (missing some components)
                new Grade { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[0].Id, Score = 7.5m, LetterGrade = "B", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[1].Id, Score = 8.0m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[2].Id, Score = 7.0m, LetterGrade = "B", UpdatedAt = DateTime.UtcNow },

                // Student 3 (SV003) - Excellent grades
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[0].Id, Score = 9.5m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[1].Id, Score = 9.8m, LetterGrade = "A+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[2].Id, Score = 9.2m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, GradeComponentId = gradeComponents[3].Id, Score = 9.6m, LetterGrade = "A+", UpdatedAt = DateTime.UtcNow },

                // ========== SE201-A (Class 2) - Software Engineering ==========
                // Student 1 (SV001) - Good grades
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject2.Id, GradeComponentId = gradeComponents[0].Id, Score = 8.0m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject2.Id, GradeComponentId = gradeComponents[1].Id, Score = 8.5m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject2.Id, GradeComponentId = gradeComponents[3].Id, Score = 9.0m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },

                // Student 3 (SV003) - Excellent performance
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject2.Id, GradeComponentId = gradeComponents[0].Id, Score = 9.0m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject2.Id, GradeComponentId = gradeComponents[1].Id, Score = 9.3m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject2.Id, GradeComponentId = gradeComponents[2].Id, Score = 8.8m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject2.Id, GradeComponentId = gradeComponents[3].Id, Score = 9.5m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },

                // ========== DB301-A (Class 3) - Database Management Systems ==========
                // Student 1 (SV001) - Average grades
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject3.Id, GradeComponentId = gradeComponents[0].Id, Score = 7.5m, LetterGrade = "B", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject3.Id, GradeComponentId = gradeComponents[1].Id, Score = 8.0m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject3.Id, GradeComponentId = gradeComponents[2].Id, Score = 7.8m, LetterGrade = "B", UpdatedAt = DateTime.UtcNow },

                // Student 2 (SV002) - Good grades
                new Grade { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject3.Id, GradeComponentId = gradeComponents[0].Id, Score = 8.2m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject3.Id, GradeComponentId = gradeComponents[1].Id, Score = 8.5m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },

                // Student 3 (SV003) - Excellent grades
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject3.Id, GradeComponentId = gradeComponents[0].Id, Score = 9.0m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject3.Id, GradeComponentId = gradeComponents[1].Id, Score = 9.5m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject3.Id, GradeComponentId = gradeComponents[2].Id, Score = 9.2m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject3.Id, GradeComponentId = gradeComponents[3].Id, Score = 9.8m, LetterGrade = "A+", UpdatedAt = DateTime.UtcNow },

                // ========== BC202-A (Class 4) - Smart Contract Development ==========
                // Student 1 (SV001) - In progress (only midterm)
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject4.Id, GradeComponentId = gradeComponents[0].Id, Score = 8.3m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject4.Id, GradeComponentId = gradeComponents[2].Id, Score = 8.0m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },

                // Student 2 (SV002) - In progress
                new Grade { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject4.Id, GradeComponentId = gradeComponents[0].Id, Score = 7.8m, LetterGrade = "B", UpdatedAt = DateTime.UtcNow },

                // ========== BC202-B (Class 5) - Smart Contract Development (Another section) ==========
                // Student 3 (SV003) - Complete grades
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject4.Id, GradeComponentId = gradeComponents[0].Id, Score = 9.2m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject4.Id, GradeComponentId = gradeComponents[1].Id, Score = 9.5m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject4.Id, GradeComponentId = gradeComponents[2].Id, Score = 9.0m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject4.Id, GradeComponentId = gradeComponents[3].Id, Score = 9.7m, LetterGrade = "A+", UpdatedAt = DateTime.UtcNow },

                // ========== BC303-A (Class 6) - Blockchain Security ==========
                // Student 1 (SV001) - Just started (only assignment)
                new Grade { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject5.Id, GradeComponentId = gradeComponents[2].Id, Score = 8.5m, LetterGrade = "B+", UpdatedAt = DateTime.UtcNow },

                // Student 3 (SV003) - Early stage
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject5.Id, GradeComponentId = gradeComponents[2].Id, Score = 9.0m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow },
                new Grade { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject5.Id, GradeComponentId = gradeComponents[3].Id, Score = 9.3m, LetterGrade = "A", UpdatedAt = DateTime.UtcNow }
            };

            await context.Grades.AddRangeAsync(grades);

            // ========== CERTIFICATE TEMPLATE ==========
            var certificateTemplate = new CertificateTemplate
            {
                Id = Guid.Parse("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
                Name = "Blockchain Certificate",
                Description = "Certificate for Blockchain Fundamentals",
                TemplateFileUrl = "https://example.com/cert-template.pdf"
            };
            await context.CertificateTemplates.AddAsync(certificateTemplate);

            // ========== CREDENTIALS ==========
            var credentials = new List<Credential>
            {
                new Credential
                {
                    Id = Guid.NewGuid(),
                    CredentialId = "BC101-SV001-2024",
                    IPFSHash = "Qm1234567890abcdef",
                    FileUrl = "https://example.com/cert-sv001.pdf",
                    IssuedDate = DateTime.UtcNow.AddDays(-30),
                    IsRevoked = false,
                    StudentId = student1Entity.Id,
                    CertificateTemplateId = certificateTemplate.Id
                },
                new Credential
                {
                    Id = Guid.NewGuid(),
                    CredentialId = "BC101-SV003-2024",
                    IPFSHash = "Qmabcdefghijk12345",
                    FileUrl = "https://example.com/cert-sv003.pdf",
                    IssuedDate = DateTime.UtcNow.AddDays(-25),
                    IsRevoked = false,
                    StudentId = student3Entity.Id,
                    CertificateTemplateId = certificateTemplate.Id
                }
            };

            await context.Credentials.AddRangeAsync(credentials);

            // ========== TIME SLOTS ==========
            var timeSlot1 = new TimeSlot
            {
                Id = Guid.Parse("01111111-1111-1111-1111-111111111111"),
                Name = "Slot 1",
                StartTime = new TimeSpan(7, 30, 0),  // 07:30
                EndTime = new TimeSpan(9, 15, 0)     // 09:15
            };

            var timeSlot2 = new TimeSlot
            {
                Id = Guid.Parse("02222222-2222-2222-2222-222222222222"),
                Name = "Slot 2",
                StartTime = new TimeSpan(9, 30, 0),  // 09:30
                EndTime = new TimeSpan(11, 15, 0)    // 11:15
            };

            var timeSlot3 = new TimeSlot
            {
                Id = Guid.Parse("03333333-3333-3333-3333-333333333333"),
                Name = "Slot 3",
                StartTime = new TimeSpan(12, 30, 0), // 12:30
                EndTime = new TimeSpan(14, 15, 0)    // 14:15
            };

            var timeSlot4 = new TimeSlot
            { 
                Id = Guid.Parse("04444444-4444-4444-4444-444444444444"),
                Name = "Slot 4",
                StartTime = new TimeSpan(14, 30, 0), // 14:30
                EndTime = new TimeSpan(16, 15, 0)    // 16:15
            };

            var timeSlot5 = new TimeSlot
            {
                Id = Guid.Parse("05555555-5555-5555-5555-555555555555"),
                Name = "Slot 5",
                StartTime = new TimeSpan(16, 30, 0), // 16:30
                EndTime = new TimeSpan(18, 15, 0) // 18:15
            };

            await context.TimeSlots.AddRangeAsync(timeSlot1, timeSlot2, timeSlot3, timeSlot4, timeSlot5);

            // ========== SLOTS (Class Sessions) ==========
            var slots = new List<Slot>();

            // ========== Class 1: BC101-A (Blockchain Fundamentals) - Completed course ==========
            // Week 1
            slots.Add(new Slot { Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"), ClassId = class1.Id, TimeSlotId = timeSlot1.Id, Date = new DateTime(2024, 1, 15), Status = "Completed", SubstituteTeacherId = null });
            slots.Add(new Slot { Id = Guid.Parse("a1111111-1111-1111-1111-111111111112"), ClassId = class1.Id, TimeSlotId = timeSlot2.Id, Date = new DateTime(2024, 1, 17), Status = "Completed", SubstituteTeacherId = null });
            slots.Add(new Slot { Id = Guid.Parse("a1111111-1111-1111-1111-111111111113"), ClassId = class1.Id, TimeSlotId = timeSlot1.Id, Date = new DateTime(2024, 1, 19), Status = "Completed", SubstituteTeacherId = teacher2Entity.Id, SubstitutionReason = "Giáo viên chính bận công tác" }); // Teacher 2 substitutes for Teacher 1
  
            // Week 2
            slots.Add(new Slot { Id = Guid.Parse("a1111111-1111-1111-1111-111111111114"), ClassId = class1.Id, TimeSlotId = timeSlot1.Id, Date = new DateTime(2024, 1, 22), Status = "Completed", SubstituteTeacherId = null });
            slots.Add(new Slot { Id = Guid.Parse("a1111111-1111-1111-1111-111111111115"), ClassId = class1.Id, TimeSlotId = timeSlot2.Id, Date = new DateTime(2024, 1, 24), Status = "Completed", SubstituteTeacherId = null });

            // Week 3
            slots.Add(new Slot { Id = Guid.Parse("a1111111-1111-1111-1111-111111111116"), ClassId = class1.Id, TimeSlotId = timeSlot1.Id, Date = new DateTime(2024, 1, 29), Status = "Completed", SubstituteTeacherId = null });
            slots.Add(new Slot { Id = Guid.Parse("a1111111-1111-1111-1111-111111111117"), ClassId = class1.Id, TimeSlotId = timeSlot2.Id, Date = new DateTime(2024, 1, 31), Status = "Completed", SubstituteTeacherId = null });

            // ========== Class 4: BC202-A (Smart Contract Development) - Active course ==========
            // Recent slots
            slots.Add(new Slot { Id = Guid.Parse("a4444444-4444-4444-4444-444444444441"), ClassId = class4.Id, TimeSlotId = timeSlot3.Id, Date = DateTime.UtcNow.AddDays(-7), Status = "Completed", SubstituteTeacherId = null });
            slots.Add(new Slot { Id = Guid.Parse("a4444444-4444-4444-4444-444444444442"), ClassId = class4.Id, TimeSlotId = timeSlot3.Id, Date = DateTime.UtcNow.AddDays(-5), Status = "Completed", SubstituteTeacherId = null });
            slots.Add(new Slot { Id = Guid.Parse("a4444444-4444-4444-4444-444444444443"), ClassId = class4.Id, TimeSlotId = timeSlot3.Id, Date = DateTime.UtcNow.AddDays(-3), Status = "Completed", SubstituteTeacherId = teacher3Entity.Id, SubstitutionReason = "Giáo viên chính nghỉ ốm" }); // Teacher 3 substitutes for Teacher 1
            slots.Add(new Slot { Id = Guid.Parse("a4444444-4444-4444-4444-444444444444"), ClassId = class4.Id, TimeSlotId = timeSlot3.Id, Date = DateTime.UtcNow.AddDays(-1), Status = "Completed", SubstituteTeacherId = null });
          
            // Upcoming slots
            slots.Add(new Slot { Id = Guid.Parse("a4444444-4444-4444-4444-444444444445"), ClassId = class4.Id, TimeSlotId = timeSlot3.Id, Date = DateTime.UtcNow.AddDays(1), Status = "Scheduled", SubstituteTeacherId = null });
            slots.Add(new Slot { Id = Guid.Parse("a4444444-4444-4444-4444-444444444446"), ClassId = class4.Id, TimeSlotId = timeSlot3.Id, Date = DateTime.UtcNow.AddDays(3), Status = "Scheduled", SubstituteTeacherId = null });

            // ========== Class 6: BC303-A (Blockchain Security) - New course ==========
            slots.Add(new Slot { Id = Guid.Parse("a6666666-6666-6666-6666-666666666661"), ClassId = class6.Id, TimeSlotId = timeSlot4.Id, Date = DateTime.UtcNow.AddDays(-2), Status = "Completed", SubstituteTeacherId = null });
            slots.Add(new Slot { Id = Guid.Parse("a6666666-6666-6666-6666-666666666662"), ClassId = class6.Id, TimeSlotId = timeSlot4.Id, Date = DateTime.UtcNow.AddDays(2), Status = "Scheduled", SubstituteTeacherId = null });

            await context.Slots.AddRangeAsync(slots);

            // ========== ATTENDANCES ==========
            var attendances = new List<Attendance>();

            // ========== Class 1 (BC101-A) - Completed Course Attendance ==========
            // Slot 1 (2024-01-15) - All present
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, SlotId = slots[0].Id, IsPresent = true, Notes = "On time", RecordedAt = new DateTime(2024, 1, 15, 8, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, SlotId = slots[0].Id, IsPresent = true, Notes = "On time", RecordedAt = new DateTime(2024, 1, 15, 8, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, SlotId = slots[0].Id, IsPresent = true, Notes = "On time", RecordedAt = new DateTime(2024, 1, 15, 8, 0, 0) });

            // Slot 2 (2024-01-17) - SV002 absent without excuse
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, SlotId = slots[1].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 17, 10, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, SlotId = slots[1].Id, IsPresent = false, Notes = "Không có mặt", IsExcused = false, RecordedAt = new DateTime(2024, 1, 17, 10, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, SlotId = slots[1].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 17, 10, 0, 0) });

            // Slot 3 (2024-01-19) - SV002 absent with excuse
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, SlotId = slots[2].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 19, 8, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, SlotId = slots[2].Id, IsPresent = false, Notes = "Vắng có phép", IsExcused = true, ExcuseReason = "Em bị ốm có giấy bác sĩ xác nhận", RecordedAt = new DateTime(2024, 1, 19, 8, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, SlotId = slots[2].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 19, 8, 0, 0) });

            // Slot 4 (2024-01-22) - All present
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, SlotId = slots[3].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 22, 8, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, SlotId = slots[3].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 22, 8, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, SlotId = slots[3].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 22, 8, 0, 0) });

            // Slot 5 (2024-01-24) - SV001 late
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, SlotId = slots[4].Id, IsPresent = true, Notes = "Đến muộn 15 phút", RecordedAt = new DateTime(2024, 1, 24, 10, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, SlotId = slots[4].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 24, 10, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, SlotId = slots[4].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 24, 10, 0, 0) });

            // Slot 6 (2024-01-29) - All present
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, SlotId = slots[5].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 29, 8, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, SlotId = slots[5].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 29, 8, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, SlotId = slots[5].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 29, 8, 0, 0) });

            // Slot 7 (2024-01-31) - SV001 absent without excuse
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject1.Id, SlotId = slots[6].Id, IsPresent = false, Notes = "Vắng không lý do", IsExcused = false, RecordedAt = new DateTime(2024, 1, 31, 10, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject1.Id, SlotId = slots[6].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 31, 10, 0, 0) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student3Entity.Id, SubjectId = subject1.Id, SlotId = slots[6].Id, IsPresent = true, Notes = null, RecordedAt = new DateTime(2024, 1, 31, 10, 0, 0) });

            // ========== Class 4 (BC202-A) - Recent Attendance Records ==========
            // Slot -7 days ago - All present
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject4.Id, SlotId = slots[7].Id, IsPresent = true, Notes = null, RecordedAt = DateTime.UtcNow.AddDays(-7) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject4.Id, SlotId = slots[7].Id, IsPresent = true, Notes = null, RecordedAt = DateTime.UtcNow.AddDays(-7) });

            // Slot -5 days ago - SV002 absent without excuse
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject4.Id, SlotId = slots[8].Id, IsPresent = true, Notes = null, RecordedAt = DateTime.UtcNow.AddDays(-5) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject4.Id, SlotId = slots[8].Id, IsPresent = false, Notes = "Vắng mặt", IsExcused = false, RecordedAt = DateTime.UtcNow.AddDays(-5) });

            // Slot -3 days ago - All present
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject4.Id, SlotId = slots[9].Id, IsPresent = true, Notes = null, RecordedAt = DateTime.UtcNow.AddDays(-3) });
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student2Entity.Id, SubjectId = subject4.Id, SlotId = slots[9].Id, IsPresent = true, Notes = null, RecordedAt = DateTime.UtcNow.AddDays(-3) });

            // Slot -1 day ago - SV001 late, SV002 absent with excuse
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject4.Id, SlotId = slots[10].Id, IsPresent = true, Notes = "Đến muộn 10 phút", RecordedAt = DateTime.UtcNow.AddDays(-1) });
            attendances.Add(new Attendance { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), StudentId = student2Entity.Id, SubjectId = subject4.Id, SlotId = slots[10].Id, IsPresent = false, Notes = "Vắng có xin phép", IsExcused = true, ExcuseReason = "Em có việc gia đình đột xuất, em xin phép nghỉ buổi học này", RecordedAt = DateTime.UtcNow.AddDays(-1) });

            // ========== Class 6 (BC303-A) - New Course Attendance ==========
            // Slot -2 days ago - First class, SV003 absent
            attendances.Add(new Attendance { Id = Guid.NewGuid(), StudentId = student1Entity.Id, SubjectId = subject5.Id, SlotId = slots[13].Id, IsPresent = true, Notes = "First class", RecordedAt = DateTime.UtcNow.AddDays(-2) });
            attendances.Add(new Attendance { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), StudentId = student3Entity.Id, SubjectId = subject5.Id, SlotId = slots[13].Id, IsPresent = false, Notes = "Vắng buổi đầu", IsExcused = false, RecordedAt = DateTime.UtcNow.AddDays(-2) });

            await context.Attendances.AddRangeAsync(attendances);

            await context.SaveChangesAsync();
        }
    }
}
