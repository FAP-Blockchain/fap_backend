using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    /// <summary>
    /// Seeds GradeComponents (Quiz, Lab, Midterm, Final, etc.) for each subject
    /// </summary>
    public class GradeComponentSeeder : BaseSeeder
    {
        public GradeComponentSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
        {
            if (await _context.GradeComponents.AnyAsync())
            {
                Console.WriteLine("⏭️ Grade Components already exist. Skipping...");
                return;
            }

            // Get all subjects to create grade components for each
            var subjects = await _context.Subjects.ToListAsync();
            if (!subjects.Any())
            {
                Console.WriteLine("⚠️ No subjects found. Skipping Grade Components seeding.");
                return;
            }

            var components = new List<GradeComponent>();
            int componentIndex = 1;

            foreach (var subject in subjects)
            {
                // Create standard grade structure for each subject
                // You can customize this based on subject type (Lab-based vs Theory)
                
                if (subject.SubjectCode.Contains("LAB") || subject.SubjectCode.Contains("PRJ"))
                {
                    // Lab/Project subjects: More practical work
                    components.AddRange(new[]
                    {
                        new GradeComponent 
                        { 
                            Id = Guid.Parse($"50{componentIndex++:D6}-0000-0000-0000-000000000000"),
                            Name = "Lab Work",
                            WeightPercent = 30,
                            SubjectId = subject.Id
                        },
                        new GradeComponent 
                        { 
                            Id = Guid.Parse($"50{componentIndex++:D6}-0000-0000-0000-000000000000"),
                            Name = "Midterm Exam",
                            WeightPercent = 30,
                            SubjectId = subject.Id
                        },
                        new GradeComponent 
                        { 
                            Id = Guid.Parse($"50{componentIndex++:D6}-0000-0000-0000-000000000000"),
                            Name = "Final Exam",
                            WeightPercent = 40,
                            SubjectId = subject.Id
                        }
                    });
                }
                else
                {
                    // Theory subjects: Traditional structure
                    components.AddRange(new[]
                    {
                        new GradeComponent 
                        { 
                            Id = Guid.Parse($"50{componentIndex++:D6}-0000-0000-0000-000000000000"),
                            Name = "Assignment",
                            WeightPercent = 20,
                            SubjectId = subject.Id
                        },
                        new GradeComponent 
                        { 
                            Id = Guid.Parse($"50{componentIndex++:D6}-0000-0000-0000-000000000000"),
                            Name = "Midterm Exam",
                            WeightPercent = 30,
                            SubjectId = subject.Id
                        },
                        new GradeComponent 
                        { 
                            Id = Guid.Parse($"50{componentIndex++:D6}-0000-0000-0000-000000000000"),
                            Name = "Final Exam",
                            WeightPercent = 50,
                            SubjectId = subject.Id
                        }
                    });
                }
            }

            await _context.GradeComponents.AddRangeAsync(components);
            await SaveAsync("Grade Components");

            Console.WriteLine($"   ✅ Created {components.Count} grade components for {subjects.Count} subjects");
        }
    }
}
