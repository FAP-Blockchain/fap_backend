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

            foreach (var subject in subjects)
            {
                // Create standard grade structure for each subject
                
                if (subject.SubjectCode.Contains("LAB") || subject.SubjectCode.Contains("PRJ"))
                {
                    // Lab/Project subjects: More practical work
                    // Lab Exercises (30%) -> Lab 1, Lab 2
                    var labParent = new GradeComponent
                    {
                        Id = Guid.NewGuid(),
                        Name = "Lab Exercises",
                        WeightPercent = 30,
                        SubjectId = subject.Id
                    };
                    components.Add(labParent);

                    components.AddRange(CreateChildComponents(labParent, subject.Id, "Lab 1", "Lab 2"));

                    // Midterm (30%)
                    components.Add(new GradeComponent
                    {
                        Id = Guid.NewGuid(),
                        Name = "Midterm Exam",
                        WeightPercent = 30,
                        SubjectId = subject.Id
                    });

                    // Final (40%)
                    components.Add(new GradeComponent
                    {
                        Id = Guid.NewGuid(),
                        Name = "Final Exam",
                        WeightPercent = 40,
                        SubjectId = subject.Id
                    });
                }
                else
                {
                    // Theory subjects: Traditional structure
                    // Assignments (20%) -> Ass 1, Ass 2
                    var assignParent = new GradeComponent
                    {
                        Id = Guid.NewGuid(),
                        Name = "Assignments",
                        WeightPercent = 20,
                        SubjectId = subject.Id
                    };
                    components.Add(assignParent);

                    components.AddRange(CreateChildComponents(assignParent, subject.Id, "Assignment 1", "Assignment 2"));

                    // Progress Tests (30%) -> PT 1, PT 2
                    var ptParent = new GradeComponent
                    {
                        Id = Guid.NewGuid(),
                        Name = "Progress Tests",
                        WeightPercent = 30,
                        SubjectId = subject.Id
                    };
                    components.Add(ptParent);

                    components.AddRange(CreateChildComponents(ptParent, subject.Id, "Progress Test 1", "Progress Test 2"));

                    // Final Exam (50%)
                    components.Add(new GradeComponent
                    {
                        Id = Guid.NewGuid(),
                        Name = "Final Exam",
                        WeightPercent = 50,
                        SubjectId = subject.Id
                    });
                }
            }

            await _context.GradeComponents.AddRangeAsync(components);
            await SaveAsync("Grade Components");

            Console.WriteLine($"   ✅ Created {components.Count} grade components for {subjects.Count} subjects");
        }
        private IEnumerable<GradeComponent> CreateChildComponents(GradeComponent parent, Guid subjectId, params string[] childNames)
        {
            if (childNames is null || childNames.Length == 0)
            {
                yield break;
            }

            var baseWeight = parent.WeightPercent / childNames.Length;
            var remainder = parent.WeightPercent % childNames.Length;

            for (var index = 0; index < childNames.Length; index++)
            {
                var weight = baseWeight + (index < remainder ? 1 : 0);

                yield return new GradeComponent
                {
                    Id = Guid.NewGuid(),
                    Name = childNames[index],
                    WeightPercent = weight,
                    SubjectId = subjectId,
                    ParentId = parent.Id
                };
            }
        }
    }
}
