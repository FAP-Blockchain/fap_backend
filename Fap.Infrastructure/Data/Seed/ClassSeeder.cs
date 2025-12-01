using System;
using System.Collections.Generic;
using System.Linq;
using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    /// <summary>
    /// Seeds Classes using SubjectOffering pattern with fixed, conflict-free schedules.
    /// </summary>
    public class ClassSeeder : BaseSeeder
    {
        public static readonly Guid SE101_Winter2025_A = Guid.Parse("40000000-0000-0000-0000-000000000101");
        public static readonly Guid SE101_Spring2026_A = Guid.Parse("40000000-0000-0000-0000-000000000102");
        public static readonly Guid SE101_Fall2026_A = Guid.Parse("40000000-0000-0000-0000-000000000103");
        public static readonly Guid SE102_Spring2026_A = Guid.Parse("40000000-0000-0000-0000-000000000104");
        public static readonly Guid SE102_Fall2026_A = Guid.Parse("40000000-0000-0000-0000-000000000105");
        public static readonly Guid DB201_Winter2025_Evening = Guid.Parse("40000000-0000-0000-0000-000000000106");
        public static readonly Guid DB201_Summer2026_A = Guid.Parse("40000000-0000-0000-0000-000000000107");
        public static readonly Guid WEB301_Summer2026_A = Guid.Parse("40000000-0000-0000-0000-000000000108");
        public static readonly Guid WEB301_Fall2026_A = Guid.Parse("40000000-0000-0000-0000-000000000109");
        public static readonly Guid MATH101_Winter2025_A = Guid.Parse("40000000-0000-0000-0000-00000000010a");
        public static readonly Guid MATH101_Spring2026_A = Guid.Parse("40000000-0000-0000-0000-00000000010b");
        public static readonly Guid MATH201_Fall2026_A = Guid.Parse("40000000-0000-0000-0000-00000000010c");
        public static readonly Guid CS101_Winter2025_A = Guid.Parse("40000000-0000-0000-0000-00000000010d");
        public static readonly Guid CS101_Spring2026_A = Guid.Parse("40000000-0000-0000-0000-00000000010e");
        public static readonly Guid CS201_Summer2026_A = Guid.Parse("40000000-0000-0000-0000-00000000010f");

        private static readonly IReadOnlyList<ClassDefinition> ClassDefinitions = new List<ClassDefinition>
        {
            new ClassDefinition(SE101_Winter2025_A, "SE101.W25.A", SubjectOfferingSeeder.SE101_Winter2025, TeacherStudentSeeder.Teacher1Id, 42),
            new ClassDefinition(SE101_Spring2026_A, "SE101.SP26.A", SubjectOfferingSeeder.SE101_Spring2026, TeacherStudentSeeder.Teacher1Id, 48),
            new ClassDefinition(SE101_Fall2026_A, "SE101.F26.A", SubjectOfferingSeeder.SE101_Fall2026, TeacherStudentSeeder.Teacher2Id, 48),

            new ClassDefinition(SE102_Spring2026_A, "SE102.SP26.A", SubjectOfferingSeeder.SE102_Spring2026, TeacherStudentSeeder.Teacher2Id, 36),
            new ClassDefinition(SE102_Fall2026_A, "SE102.F26.A", SubjectOfferingSeeder.SE102_Fall2026, TeacherStudentSeeder.Teacher1Id, 36),

            new ClassDefinition(DB201_Winter2025_Evening, "DB201.W25.E", SubjectOfferingSeeder.DB201_Winter2025, TeacherStudentSeeder.Teacher2Id, 32),
            new ClassDefinition(DB201_Summer2026_A, "DB201.SU26.A", SubjectOfferingSeeder.DB201_Summer2026, TeacherStudentSeeder.Teacher3Id, 40),

            new ClassDefinition(WEB301_Summer2026_A, "WEB301.SU26.A", SubjectOfferingSeeder.WEB301_Summer2026, TeacherStudentSeeder.Teacher4Id, 30),
            new ClassDefinition(WEB301_Fall2026_A, "WEB301.F26.A", SubjectOfferingSeeder.WEB301_Fall2026, TeacherStudentSeeder.Teacher4Id, 28),

            new ClassDefinition(MATH101_Winter2025_A, "MATH101.W25.A", SubjectOfferingSeeder.MATH101_Winter2025, TeacherStudentSeeder.Teacher3Id, 50),
            new ClassDefinition(MATH101_Spring2026_A, "MATH101.SP26.A", SubjectOfferingSeeder.MATH101_Spring2026, TeacherStudentSeeder.Teacher3Id, 50),
            new ClassDefinition(MATH201_Fall2026_A, "MATH201.F26.A", SubjectOfferingSeeder.MATH201_Fall2026, TeacherStudentSeeder.Teacher3Id, 45),

            new ClassDefinition(CS101_Winter2025_A, "CS101.W25.A", SubjectOfferingSeeder.CS101_Winter2025, TeacherStudentSeeder.Teacher2Id, 45),
            new ClassDefinition(CS101_Spring2026_A, "CS101.SP26.A", SubjectOfferingSeeder.CS101_Spring2026, TeacherStudentSeeder.Teacher2Id, 45),
            new ClassDefinition(CS201_Summer2026_A, "CS201.SU26.A", SubjectOfferingSeeder.CS201_Summer2026, TeacherStudentSeeder.Teacher1Id, 35)
        };

        public ClassSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
        {
            if (await _context.Classes.AnyAsync())
            {
                Console.WriteLine("Classes already exist. Skipping seeding...");
                return;
            }

            var timestamp = DateTime.UtcNow;
            var classes = ClassDefinitions
                .Select(def => new Class
                {
                    Id = def.Id,
                    ClassCode = def.ClassCode,
                    SubjectOfferingId = def.SubjectOfferingId,
                    TeacherUserId = def.TeacherUserId,
                    MaxEnrollment = def.MaxEnrollment,
                    IsActive = true,
                    CreatedAt = timestamp,
                    UpdatedAt = timestamp
                })
                .ToList();

            await _context.Classes.AddRangeAsync(classes);
            await SaveAsync("Classes");

            Console.WriteLine($"Created {classes.Count} classes linked to semester-specific offerings");
        }

        private sealed record ClassDefinition(Guid Id, string ClassCode, Guid SubjectOfferingId, Guid TeacherUserId, int MaxEnrollment);
    }
}
