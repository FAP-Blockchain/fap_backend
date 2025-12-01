using System.Collections.Generic;
using System.Linq;
using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    public class SemesterSeeder : BaseSeeder
    {
        public static readonly Guid Spring2025Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffff0f001");
        public static readonly Guid Summer2025Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffff0f002");
        public static readonly Guid Fall2025Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffff0f003");
        public static readonly Guid Winter2025Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffff0f004");
        public static readonly Guid Spring2026Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffff0f005");
        public static readonly Guid Summer2026Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffff0f006");
        public static readonly Guid Fall2026Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffff0f007");

        private static readonly IReadOnlyList<SemesterTemplate> SemesterTemplates = new List<SemesterTemplate>
        {
            new SemesterTemplate(Spring2025Id, "Spring 2025", new DateTime(2025, 1, 6), new DateTime(2025, 4, 25)),
            new SemesterTemplate(Summer2025Id, "Summer 2025", new DateTime(2025, 5, 5), new DateTime(2025, 7, 25)),
            new SemesterTemplate(Fall2025Id, "Fall 2025", new DateTime(2025, 8, 4), new DateTime(2025, 10, 24)),
            new SemesterTemplate(Winter2025Id, "Winter 2025", new DateTime(2025, 11, 3), new DateTime(2026, 1, 23)),
            new SemesterTemplate(Spring2026Id, "Spring 2026", new DateTime(2026, 2, 2), new DateTime(2026, 5, 22)),
            new SemesterTemplate(Summer2026Id, "Summer 2026", new DateTime(2026, 5, 26), new DateTime(2026, 8, 14)),
            new SemesterTemplate(Fall2026Id, "Fall 2026", new DateTime(2026, 8, 24), new DateTime(2026, 11, 20))
        };

        public SemesterSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
        {
            if (await _context.Semesters.AnyAsync())
            {
                Console.WriteLine("Semesters already exist. Skipping seeding...");
                return;
            }

            var now = DateTime.UtcNow;
            var semesters = SemesterTemplates
                .Select(template => BuildSemester(template, now))
                .ToList();

            await _context.Semesters.AddRangeAsync(semesters);
            await SaveAsync("Semesters");
        }

        private static Semester BuildSemester(SemesterTemplate template, DateTime now)
        {
            var isCurrent = now >= template.StartDate && now <= template.EndDate;

            return new Semester
            {
                Id = template.Id,
                Name = template.Name,
                StartDate = template.StartDate,
                EndDate = template.EndDate,
                IsActive = isCurrent,
                IsClosed = now > template.EndDate,
                CreatedAt = template.StartDate.AddMonths(-2),
                UpdatedAt = template.StartDate.AddMonths(-2)
            };
        }

        private sealed record SemesterTemplate(Guid Id, string Name, DateTime StartDate, DateTime EndDate);
    }
}
