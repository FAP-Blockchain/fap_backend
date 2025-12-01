using System;
using System.Collections.Generic;
using System.Linq;
using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    public class EnrollmentSeeder : BaseSeeder
    {
        public EnrollmentSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
        {
            if (await _context.ClassMembers.AnyAsync() || await _context.Enrolls.AnyAsync())
            {
                Console.WriteLine("Enrollments already exist. Skipping seeding...");
                return;
            }

            await SeedClassMembersAsync();
            await SeedEnrollsAsync();
        }

        private async Task SeedClassMembersAsync()
        {
            var assignments = new List<(Guid ClassId, Guid StudentId, DateTime JoinedAt)>
            {
                (ClassSeeder.SE101_Winter2025_A, TeacherStudentSeeder.Student1Id, new DateTime(2025, 11, 10)),
                (ClassSeeder.SE101_Winter2025_A, TeacherStudentSeeder.Student2Id, new DateTime(2025, 11, 10)),
                (ClassSeeder.SE101_Winter2025_A, TeacherStudentSeeder.Student3Id, new DateTime(2025, 11, 11)),

                (ClassSeeder.CS101_Winter2025_A, TeacherStudentSeeder.Student1Id, new DateTime(2025, 11, 11)),
                (ClassSeeder.CS101_Winter2025_A, TeacherStudentSeeder.Student4Id, new DateTime(2025, 11, 12)),
                (ClassSeeder.CS101_Winter2025_A, TeacherStudentSeeder.Student2Id, new DateTime(2025, 11, 12)),

                (ClassSeeder.MATH101_Winter2025_A, TeacherStudentSeeder.Student1Id, new DateTime(2025, 11, 13)),
                (ClassSeeder.MATH101_Winter2025_A, TeacherStudentSeeder.Student3Id, new DateTime(2025, 11, 13)),
                (ClassSeeder.MATH101_Winter2025_A, TeacherStudentSeeder.Student5Id, new DateTime(2025, 11, 14)),
                (ClassSeeder.MATH101_Winter2025_A, TeacherStudentSeeder.Student6Id, new DateTime(2025, 11, 14)),

                (ClassSeeder.DB201_Winter2025_Evening, TeacherStudentSeeder.Student2Id, new DateTime(2025, 11, 15)),
                (ClassSeeder.DB201_Winter2025_Evening, TeacherStudentSeeder.Student5Id, new DateTime(2025, 11, 15)),

                (ClassSeeder.SE102_Spring2026_A, TeacherStudentSeeder.Student1Id, new DateTime(2026, 1, 15)),
                (ClassSeeder.SE102_Spring2026_A, TeacherStudentSeeder.Student2Id, new DateTime(2026, 1, 15)),
                (ClassSeeder.SE101_Spring2026_A, TeacherStudentSeeder.Student5Id, new DateTime(2026, 1, 16)),
                (ClassSeeder.SE101_Spring2026_A, TeacherStudentSeeder.Student6Id, new DateTime(2026, 1, 16)),
                (ClassSeeder.CS101_Spring2026_A, TeacherStudentSeeder.Student6Id, new DateTime(2026, 1, 17)),
                (ClassSeeder.MATH101_Spring2026_A, TeacherStudentSeeder.Student4Id, new DateTime(2026, 1, 17)),

                (ClassSeeder.DB201_Summer2026_A, TeacherStudentSeeder.Student3Id, new DateTime(2026, 5, 20)),
                (ClassSeeder.DB201_Summer2026_A, TeacherStudentSeeder.Student4Id, new DateTime(2026, 5, 20)),
                (ClassSeeder.WEB301_Summer2026_A, TeacherStudentSeeder.Student5Id, new DateTime(2026, 5, 21)),
                (ClassSeeder.CS201_Summer2026_A, TeacherStudentSeeder.Student1Id, new DateTime(2026, 5, 22)),
                (ClassSeeder.WEB301_Fall2026_A, TeacherStudentSeeder.Student6Id, new DateTime(2026, 8, 10)),
                (ClassSeeder.MATH201_Fall2026_A, TeacherStudentSeeder.Student1Id, new DateTime(2026, 8, 10)),
                (ClassSeeder.SE102_Fall2026_A, TeacherStudentSeeder.Student2Id, new DateTime(2026, 8, 11))
            };

            var classMembers = assignments.Select(a => new ClassMember
            {
                ClassId = a.ClassId,
                StudentId = a.StudentId,
                JoinedAt = a.JoinedAt
            }).ToList();

            await _context.ClassMembers.AddRangeAsync(classMembers);
            await SaveAsync("ClassMembers");

            Console.WriteLine($"Created {classMembers.Count} class membership records");
        }

        private async Task SeedEnrollsAsync()
        {
            var enrollmentDefinitions = new List<(Guid ClassId, Guid StudentId, DateTime RegisteredAt)>
            {
                (ClassSeeder.SE101_Winter2025_A, TeacherStudentSeeder.Student1Id, new DateTime(2025, 10, 20)),
                (ClassSeeder.CS101_Winter2025_A, TeacherStudentSeeder.Student1Id, new DateTime(2025, 10, 21)),
                (ClassSeeder.MATH101_Winter2025_A, TeacherStudentSeeder.Student1Id, new DateTime(2025, 10, 22)),
                (ClassSeeder.DB201_Winter2025_Evening, TeacherStudentSeeder.Student2Id, new DateTime(2025, 10, 25)),
                (ClassSeeder.MATH101_Winter2025_A, TeacherStudentSeeder.Student3Id, new DateTime(2025, 10, 23)),

                (ClassSeeder.SE102_Spring2026_A, TeacherStudentSeeder.Student1Id, new DateTime(2025, 12, 15)),
                (ClassSeeder.SE102_Spring2026_A, TeacherStudentSeeder.Student2Id, new DateTime(2025, 12, 15)),
                (ClassSeeder.SE101_Spring2026_A, TeacherStudentSeeder.Student5Id, new DateTime(2025, 12, 20)),
                (ClassSeeder.SE101_Spring2026_A, TeacherStudentSeeder.Student6Id, new DateTime(2025, 12, 20)),
                (ClassSeeder.CS101_Spring2026_A, TeacherStudentSeeder.Student6Id, new DateTime(2025, 12, 22)),

                (ClassSeeder.DB201_Summer2026_A, TeacherStudentSeeder.Student3Id, new DateTime(2026, 4, 10)),
                (ClassSeeder.CS201_Summer2026_A, TeacherStudentSeeder.Student1Id, new DateTime(2026, 4, 12)),
                (ClassSeeder.WEB301_Summer2026_A, TeacherStudentSeeder.Student5Id, new DateTime(2026, 4, 15)),
                (ClassSeeder.WEB301_Fall2026_A, TeacherStudentSeeder.Student6Id, new DateTime(2026, 7, 15)),
                (ClassSeeder.MATH201_Fall2026_A, TeacherStudentSeeder.Student1Id, new DateTime(2026, 7, 16)),
                (ClassSeeder.SE102_Fall2026_A, TeacherStudentSeeder.Student2Id, new DateTime(2026, 7, 16))
            };

            var enrolls = enrollmentDefinitions.Select(def => new Enroll
            {
                Id = Guid.NewGuid(),
                ClassId = def.ClassId,
                StudentId = def.StudentId,
                RegisteredAt = def.RegisteredAt,
                IsApproved = true
            }).ToList();

            await _context.Enrolls.AddRangeAsync(enrolls);
            await SaveAsync("Enrolls");

            Console.WriteLine($"Created {enrolls.Count} enrollment records");
        }
    }
}
