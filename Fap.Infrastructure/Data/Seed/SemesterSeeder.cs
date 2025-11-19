using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    public class SemesterSeeder : BaseSeeder
    {
        public static readonly Guid Spring2024Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        public static readonly Guid Summer2024Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffffffffe");
        public static readonly Guid Fall2024Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffffffffd");
        public static readonly Guid Spring2025Id = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffffffffc");

        public SemesterSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
        {
            if (await _context.Semesters.AnyAsync())
            {
                Console.WriteLine("⏭️  Semesters already exist. Skipping...");
                return;
            }

            var semesters = new List<Semester>
            {
                new Semester
                {
                    Id = Spring2024Id,
                    Name = "Spring 2024",
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 5, 31),
                    IsActive = true,
                    IsClosed = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Semester
                {
                    Id = Summer2024Id,
                    Name = "Summer 2024",
                    StartDate = new DateTime(2024, 6, 1),
                    EndDate = new DateTime(2024, 8, 31),
                    IsActive = true,
                    IsClosed = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Semester
                {
                    Id = Fall2024Id,
                    Name = "Fall 2024",
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2024, 12, 31),
                    IsActive = true,
                    IsClosed = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Semester
                {
                    Id = Spring2025Id,
                    Name = "Spring 2025",
                    StartDate = new DateTime(2025, 1, 1),
                    EndDate = new DateTime(2025, 5, 31),
                    IsActive = false,
                    IsClosed = false,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await _context.Semesters.AddRangeAsync(semesters);
            await SaveAsync("Semesters");
        }
    }
}
