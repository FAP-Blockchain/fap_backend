using System;
using System.Collections.Generic;
using System.Linq;
using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    /// <summary>
    /// Seeds Subjects (master data) and SubjectOfferings (semester-specific)
    /// SubjectOffering pattern allows subjects to be offered in multiple semesters
    /// </summary>
    public class SubjectOfferingSeeder : BaseSeeder
    {
        // Subject IDs (Master Data)
        public static readonly Guid SE101Id = Guid.Parse("10000000-0000-0000-0000-000000000001");
        public static readonly Guid SE102Id = Guid.Parse("10000000-0000-0000-0000-000000000002");
        public static readonly Guid DB201Id = Guid.Parse("10000000-0000-0000-0000-000000000003");
        public static readonly Guid WEB301Id = Guid.Parse("10000000-0000-0000-0000-000000000004");
        public static readonly Guid MATH101Id = Guid.Parse("10000000-0000-0000-0000-000000000005");
        public static readonly Guid MATH201Id = Guid.Parse("10000000-0000-0000-0000-000000000006");
        public static readonly Guid CS101Id = Guid.Parse("10000000-0000-0000-0000-000000000007");
        public static readonly Guid CS201Id = Guid.Parse("10000000-0000-0000-0000-000000000008");

    // SubjectOffering IDs (Semester-specific offerings)
    public static readonly Guid SE101_Winter2025 = Guid.Parse("20000000-0000-0000-0000-000000000101");
    public static readonly Guid SE101_Spring2026 = Guid.Parse("20000000-0000-0000-0000-000000000102");
    public static readonly Guid SE101_Fall2026 = Guid.Parse("20000000-0000-0000-0000-000000000103");
    public static readonly Guid SE102_Spring2026 = Guid.Parse("20000000-0000-0000-0000-000000000104");
    public static readonly Guid SE102_Fall2026 = Guid.Parse("20000000-0000-0000-0000-000000000105");
    public static readonly Guid DB201_Winter2025 = Guid.Parse("20000000-0000-0000-0000-000000000106");
    public static readonly Guid DB201_Summer2026 = Guid.Parse("20000000-0000-0000-0000-000000000107");
    public static readonly Guid WEB301_Summer2026 = Guid.Parse("20000000-0000-0000-0000-000000000108");
    public static readonly Guid WEB301_Fall2026 = Guid.Parse("20000000-0000-0000-0000-000000000109");
    public static readonly Guid MATH101_Winter2025 = Guid.Parse("20000000-0000-0000-0000-00000000010a");
    public static readonly Guid MATH101_Spring2026 = Guid.Parse("20000000-0000-0000-0000-00000000010b");
    public static readonly Guid MATH201_Fall2026 = Guid.Parse("20000000-0000-0000-0000-00000000010c");
    public static readonly Guid CS101_Winter2025 = Guid.Parse("20000000-0000-0000-0000-00000000010d");
    public static readonly Guid CS101_Spring2026 = Guid.Parse("20000000-0000-0000-0000-00000000010e");
    public static readonly Guid CS201_Summer2026 = Guid.Parse("20000000-0000-0000-0000-00000000010f");

        public SubjectOfferingSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
        {
            var subjectsExist = await _context.Subjects.AnyAsync();
            var offeringsExist = await _context.SubjectOfferings.AnyAsync();

            if (subjectsExist && offeringsExist)
            {
                Console.WriteLine("Subjects and offerings already exist. Skipping seeding...");
                return;
            }

            if (!subjectsExist)
            {
                await SeedSubjectsAsync();
            }

            if (!offeringsExist)
            {
                await SeedSubjectOfferingsAsync();
            }
        }

        private async Task SeedSubjectsAsync()
        {
            var subjects = new List<Subject>
            {
                // ===== SOFTWARE ENGINEERING =====
                new Subject
                {
                    Id = SE101Id,
                    SubjectCode = "SE101",
                    SubjectName = "Introduction to Software Engineering",
                    Description = "Fundamentals of software development lifecycle, methodologies, and best practices",
                    Credits = 3,
                    Category = "Core",
                    Department = "Software Engineering",
                    Prerequisites = null,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Subject
                {
                    Id = SE102Id,
                    SubjectCode = "SE102",
                    SubjectName = "Advanced Software Engineering",
                    Description = "Design patterns, architecture, testing strategies, and agile methodologies",
                    Credits = 4,
                    Category = "Core",
                    Department = "Software Engineering",
                    Prerequisites = "SE101",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                // ===== DATABASE =====
                new Subject
                {
                    Id = DB201Id,
                    SubjectCode = "DB201",
                    SubjectName = "Database Design and Implementation",
                    Description = "Relational database design, SQL, normalization, and database management",
                    Credits = 4,
                    Category = "Core",
                    Department = "Computer Science",
                    Prerequisites = "CS101",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                // ===== WEB DEVELOPMENT =====
                new Subject
                {
                    Id = WEB301Id,
                    SubjectCode = "WEB301",
                    SubjectName = "Modern Web Development",
                    Description = "Full-stack web development with React, Node.js, and RESTful APIs",
                    Credits = 4,
                    Category = "Elective",
                    Department = "Software Engineering",
                    Prerequisites = "SE101,DB201",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                // ===== MATHEMATICS =====
                new Subject
                {
                    Id = MATH101Id,
                    SubjectCode = "MATH101",
                    SubjectName = "Calculus I",
                    Description = "Differential and integral calculus, limits, derivatives, and applications",
                    Credits = 4,
                    Category = "General",
                    Department = "Mathematics",
                    Prerequisites = null,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Subject
                {
                    Id = MATH201Id,
                    SubjectCode = "MATH201",
                    SubjectName = "Discrete Mathematics",
                    Description = "Logic, sets, relations, graph theory, and combinatorics for computer science",
                    Credits = 3,
                    Category = "Core",
                    Department = "Mathematics",
                    Prerequisites = "MATH101",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                // ===== COMPUTER SCIENCE =====
                new Subject
                {
                    Id = CS101Id,
                    SubjectCode = "CS101",
                    SubjectName = "Programming Fundamentals",
                    Description = "Introduction to programming with C#, algorithms, and problem solving",
                    Credits = 4,
                    Category = "Core",
                    Department = "Computer Science",
                    Prerequisites = null,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Subject
                {
                    Id = CS201Id,
                    SubjectCode = "CS201",
                    SubjectName = "Data Structures and Algorithms",
                    Description = "Arrays, linked lists, trees, graphs, sorting, searching, and complexity analysis",
                    Credits = 4,
                    Category = "Core",
                    Department = "Computer Science",
                    Prerequisites = "CS101",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await _context.Subjects.AddRangeAsync(subjects);
            await SaveAsync("Subjects (Master Data)");
        }

        private async Task SeedSubjectOfferingsAsync()
        {
            var semesters = await _context.Semesters.ToDictionaryAsync(s => s.Id);

            var offerings = new List<SubjectOffering>
            {
                CreateOffering(SE101_Winter2025, SE101Id, SemesterSeeder.Winter2025Id, semesters, maxClasses: 4, semesterCapacity: 160, notes: "Bridge run before Spring term"),
                CreateOffering(SE101_Spring2026, SE101Id, SemesterSeeder.Spring2026Id, semesters, maxClasses: 6, semesterCapacity: 240, notes: "High demand for freshmen"),
                CreateOffering(SE101_Fall2026, SE101Id, SemesterSeeder.Fall2026Id, semesters, maxClasses: 6, semesterCapacity: 240, notes: "Repeat offering for late entrants"),

                CreateOffering(SE102_Spring2026, SE102Id, SemesterSeeder.Spring2026Id, semesters, maxClasses: 3, semesterCapacity: 120),
                CreateOffering(SE102_Fall2026, SE102Id, SemesterSeeder.Fall2026Id, semesters, maxClasses: 3, semesterCapacity: 120, notes: "Project-focused cohort"),

                CreateOffering(DB201_Winter2025, DB201Id, SemesterSeeder.Winter2025Id, semesters, maxClasses: 2, semesterCapacity: 80, notes: "Evening intensive"),
                CreateOffering(DB201_Summer2026, DB201Id, SemesterSeeder.Summer2026Id, semesters, maxClasses: 4, semesterCapacity: 160),

                CreateOffering(WEB301_Summer2026, WEB301Id, SemesterSeeder.Summer2026Id, semesters, maxClasses: 3, semesterCapacity: 90, notes: "Bootcamp format"),
                CreateOffering(WEB301_Fall2026, WEB301Id, SemesterSeeder.Fall2026Id, semesters, maxClasses: 2, semesterCapacity: 60, notes: "Capstone studio"),

                CreateOffering(MATH101_Winter2025, MATH101Id, SemesterSeeder.Winter2025Id, semesters, maxClasses: 5, semesterCapacity: 200, notes: "Foundation course"),
                CreateOffering(MATH101_Spring2026, MATH101Id, SemesterSeeder.Spring2026Id, semesters, maxClasses: 6, semesterCapacity: 240),
                CreateOffering(MATH201_Fall2026, MATH201Id, SemesterSeeder.Fall2026Id, semesters, maxClasses: 4, semesterCapacity: 160),

                CreateOffering(CS101_Winter2025, CS101Id, SemesterSeeder.Winter2025Id, semesters, maxClasses: 6, semesterCapacity: 240),
                CreateOffering(CS101_Spring2026, CS101Id, SemesterSeeder.Spring2026Id, semesters, maxClasses: 6, semesterCapacity: 240),
                CreateOffering(CS201_Summer2026, CS201Id, SemesterSeeder.Summer2026Id, semesters, maxClasses: 3, semesterCapacity: 120, notes: "Accelerated DSA cohort")
            };

            await _context.SubjectOfferings.AddRangeAsync(offerings);
            await SaveAsync("SubjectOfferings");

            Console.WriteLine($"Created {offerings.Count} subject offerings across {offerings.Select(o => o.SemesterId).Distinct().Count()} semesters");
        }

        private static SubjectOffering CreateOffering(
            Guid offeringId,
            Guid subjectId,
            Guid semesterId,
            IReadOnlyDictionary<Guid, Semester> semesters,
            int maxClasses,
            int semesterCapacity,
            string? notes = null)
        {
            if (!semesters.TryGetValue(semesterId, out var semester))
            {
                throw new InvalidOperationException($"Semester {semesterId} has not been seeded yet.");
            }

            var registrationStart = semester.StartDate.AddDays(-35);
            var registrationEnd = semester.StartDate.AddDays(7);

            return new SubjectOffering
            {
                Id = offeringId,
                SubjectId = subjectId,
                SemesterId = semesterId,
                MaxClasses = maxClasses,
                SemesterCapacity = semesterCapacity,
                RegistrationStartDate = registrationStart,
                RegistrationEndDate = registrationEnd,
                IsActive = true,
                Notes = notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
