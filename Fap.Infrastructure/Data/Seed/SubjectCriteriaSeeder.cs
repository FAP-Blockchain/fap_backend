using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fap.Infrastructure.Data.Seed
{
    /// <summary>
    /// Seeds SubjectCriteria - requirements for passing each subject
    /// </summary>
    public class SubjectCriteriaSeeder : BaseSeeder
    {
        private static readonly string[] SoftwareEngineeringPrefixes = { "PRF", "CEA", "PRO", "CSD", "DBI", "PRJ", "SWP", "SWT", "SEP" };
        private static readonly string[] DatabasePrefixes = { "DBI" };
        private static readonly string[] WebPrefixes = { "PRJ", "WDU" };
        private static readonly string[] MathPrefixes = { "MAE", "MAD", "MAS" };
        private static readonly string[] ComputerSciencePrefixes = { "CSI", "CSD", "PRF", "PRO" };
        private static readonly string[] DesignPrefixes = { "DRP", "DTG", "DRS", "VCM", "TPG", "DGP", "ANS", "ANC", "GRP" };

        public SubjectCriteriaSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
        {
            if (await _context.SubjectCriteria.AnyAsync())
            {
                Console.WriteLine("Subject criteria already exist. Skipping seeding...");
                return;
            }

            var criteria = new List<SubjectCriteria>();

            // Get all subjects
            var subjects = await _context.Subjects.ToListAsync();

            if (!subjects.Any())
            {
                Console.WriteLine("No subjects found. Skipping subject criteria seeding...");
                return;
            }

            foreach (var subject in subjects)
            {
                // ==================== MANDATORY CRITERIA ====================

                // 1. Attendance Requirement (Mandatory for all subjects)
                criteria.Add(new SubjectCriteria
                {
                    Id = Guid.NewGuid(),
                    SubjectId = subject.Id,
                    Name = "Minimum Attendance Requirement",
                    Description = "Student must attend at least 75% of all class sessions to be eligible for final exam",
                    MinScore = 75.0m, // 75% attendance
                    IsMandatory = true,
                    CreatedAt = DateTime.UtcNow
                });

                // 2. Average Grade Requirement (Mandatory)
                criteria.Add(new SubjectCriteria
                {
                    Id = Guid.NewGuid(),
                    SubjectId = subject.Id,
                    Name = "Minimum Average Grade",
                    Description = "Overall average grade must be at least 5.0 to pass the subject",
                    MinScore = 5.0m,
                    IsMandatory = true,
                    CreatedAt = DateTime.UtcNow
                });

                // 3. No Component Below Minimum (Mandatory)
                criteria.Add(new SubjectCriteria
                {
                    Id = Guid.NewGuid(),
                    SubjectId = subject.Id,
                    Name = "No Failing Component Scores",
                    Description = "No individual grade component (quiz, midterm, final, etc.) can be below 3.0",
                    MinScore = 3.0m,
                    IsMandatory = true,
                    CreatedAt = DateTime.UtcNow
                });

                // ==================== SUBJECT-SPECIFIC CRITERIA ====================

                var code = subject.SubjectCode ?? string.Empty;

                // For Software Engineering subjects
                if (HasPrefix(code, SoftwareEngineeringPrefixes))
                {
                    // Project requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Project Completion",
                        Description = "Must complete and present a software project with minimum score of 5.0",
                        MinScore = 5.0m,
                        IsMandatory = true,
                        CreatedAt = DateTime.UtcNow
                    });

                    // Lab work requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Lab Work Submission",
                        Description = "Must submit at least 80% of lab assignments",
                        MinScore = 80.0m, // 80% submission rate
                        IsMandatory = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                // For Database subjects
                if (HasPrefix(code, DatabasePrefixes))
                {
                    // Practical exam requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Database Practical Exam",
                        Description = "Must achieve at least 5.0 in database practical exam",
                        MinScore = 5.0m,
                        IsMandatory = true,
                        CreatedAt = DateTime.UtcNow
                    });

                    // SQL assignment requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "SQL Assignment Completion",
                        Description = "Must complete all SQL assignments with average score >= 6.0",
                        MinScore = 6.0m,
                        IsMandatory = false, // Not mandatory but recommended
                        CreatedAt = DateTime.UtcNow
                    });
                }

                // For Web Development subjects
                if (HasPrefix(code, WebPrefixes))
                {
                    // Final project requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Web Application Project",
                        Description = "Must develop and deploy a complete web application with minimum score of 6.0",
                        MinScore = 6.0m,
                        IsMandatory = true,
                        CreatedAt = DateTime.UtcNow
                    });

                    // Presentation requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Project Presentation",
                        Description = "Must present the web project to class with minimum score of 5.0",
                        MinScore = 5.0m,
                        IsMandatory = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                // For Math subjects
                if (HasPrefix(code, MathPrefixes))
                {
                    // Midterm requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Midterm Exam Minimum",
                        Description = "Must achieve at least 4.0 in midterm exam to be eligible for final",
                        MinScore = 4.0m,
                        IsMandatory = true,
                        CreatedAt = DateTime.UtcNow
                    });

                    // Final exam requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Final Exam Minimum",
                        Description = "Must achieve at least 4.0 in final exam to pass the subject",
                        MinScore = 4.0m,
                        IsMandatory = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                // For Computer Science subjects
                if (HasPrefix(code, ComputerSciencePrefixes))
                {
                    // Programming assignment requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Programming Assignments",
                        Description = "Must submit at least 85% of programming assignments with average >= 5.5",
                        MinScore = 5.5m,
                        IsMandatory = true,
                        CreatedAt = DateTime.UtcNow
                    });

                    // Coding challenge requirement
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Coding Challenge Participation",
                        Description = "Must participate in at least 2 coding challenges",
                        MinScore = 2.0m, // Number of participations
                        IsMandatory = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                // For Graphic Design subjects
                if (HasPrefix(code, DesignPrefixes))
                {
                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Portfolio Review",
                        Description = "Must submit a portfolio piece approved by the studio mentor",
                        MinScore = 6.0m,
                        IsMandatory = true,
                        CreatedAt = DateTime.UtcNow
                    });

                    criteria.Add(new SubjectCriteria
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = subject.Id,
                        Name = "Studio Critique Participation",
                        Description = "Participate in at least two critique sessions during the course",
                        MinScore = 2.0m,
                        IsMandatory = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SubjectCriteria.AddRangeAsync(criteria);
            await SaveAsync("Subject Criteria");

            Console.WriteLine($"   Created {criteria.Count} subject criteria:");
            Console.WriteLine($"      • Mandatory criteria: {criteria.Count(c => c.IsMandatory)}");
            Console.WriteLine($"      • Recommended criteria: {criteria.Count(c => !c.IsMandatory)}");
            Console.WriteLine($"      • Average per subject: {(criteria.Count / subjects.Count):F1}");
        }

        private static bool HasPrefix(string subjectCode, params string[] prefixes)
        {
            if (string.IsNullOrWhiteSpace(subjectCode) || prefixes == null || prefixes.Length == 0)
            {
                return false;
            }

            return prefixes.Any(p => subjectCode.StartsWith(p, StringComparison.OrdinalIgnoreCase));
        }
    }
}
