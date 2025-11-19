using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    /// <summary>
    /// Seeds Classes using SubjectOffering pattern
    /// ? Classes now link to SubjectOffering (not directly to Subject)
    /// </summary>
    public class ClassSeeder : BaseSeeder
    {
    // Class IDs
  public static readonly Guid SE101_01_Spring = Guid.Parse("40000000-0000-0000-0000-000000000001");
     public static readonly Guid SE101_02_Spring = Guid.Parse("40000000-0000-0000-0000-000000000002");
     public static readonly Guid SE101_01_Fall = Guid.Parse("40000000-0000-0000-0000-000000000003");
        public static readonly Guid DB201_01_Spring = Guid.Parse("40000000-0000-0000-0000-000000000004");
        public static readonly Guid DB201_02_Spring = Guid.Parse("40000000-0000-0000-0000-000000000005");
        public static readonly Guid WEB301_01_Summer = Guid.Parse("40000000-0000-0000-0000-000000000006");
        public static readonly Guid MATH101_01_Spring = Guid.Parse("40000000-0000-0000-0000-000000000007");
        public static readonly Guid CS101_01_Spring = Guid.Parse("40000000-0000-0000-0000-000000000008");

        public ClassSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
 {
            if (await _context.Classes.AnyAsync())
   {
       Console.WriteLine("??  Classes already exist. Skipping...");
                return;
      }

    var classes = new List<Class>
            {
    // ===== SE101 Spring 2024 - 2 classes =====
    new Class
      {
    Id = SE101_01_Spring,
     ClassCode = "SE101.01.S24",
SubjectOfferingId = SubjectOfferingSeeder.SE101_Spring2024, // ? USE OFFERING!
       TeacherUserId = TeacherStudentSeeder.Teacher1Id,
    MaxEnrollment = 40,
     IsActive = true,
          CreatedAt = DateTime.UtcNow,
  UpdatedAt = DateTime.UtcNow
  },
     new Class
     {
       Id = SE101_02_Spring,
         ClassCode = "SE101.02.S24",
       SubjectOfferingId = SubjectOfferingSeeder.SE101_Spring2024, // ? SAME OFFERING
TeacherUserId = TeacherStudentSeeder.Teacher2Id,
          MaxEnrollment = 40,
IsActive = true,
       CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
                },

  // ===== SE101 Fall 2024 - 1 class =====
           new Class
                {
          Id = SE101_01_Fall,
ClassCode = "SE101.01.F24",
  SubjectOfferingId = SubjectOfferingSeeder.SE101_Fall2024, // ? FALL OFFERING
      TeacherUserId = TeacherStudentSeeder.Teacher1Id,
  MaxEnrollment = 40,
     IsActive = true,
   CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
     },

         // ===== DB201 Spring 2024 - 2 classes =====
  new Class
     {
    Id = DB201_01_Spring,
    ClassCode = "DB201.01.S24",
  SubjectOfferingId = SubjectOfferingSeeder.DB201_Spring2024,
           TeacherUserId = TeacherStudentSeeder.Teacher2Id,
       MaxEnrollment = 40,
      IsActive = true,
   CreatedAt = DateTime.UtcNow,
     UpdatedAt = DateTime.UtcNow
  },
   new Class
     {
   Id = DB201_02_Spring,
   ClassCode = "DB201.02.S24",
     SubjectOfferingId = SubjectOfferingSeeder.DB201_Spring2024,
         TeacherUserId = TeacherStudentSeeder.Teacher3Id,
MaxEnrollment = 40,
                    IsActive = true,
        CreatedAt = DateTime.UtcNow,
           UpdatedAt = DateTime.UtcNow
             },

       // ===== WEB301 Summer 2024 - 1 class =====
   new Class
     {
  Id = WEB301_01_Summer,
            ClassCode = "WEB301.01.Su24",
          SubjectOfferingId = SubjectOfferingSeeder.WEB301_Summer2024,
         TeacherUserId = TeacherStudentSeeder.Teacher4Id,
             MaxEnrollment = 30,
    IsActive = true,
     CreatedAt = DateTime.UtcNow,
  UpdatedAt = DateTime.UtcNow
  },

           // ===== MATH101 Spring 2024 - 1 class =====
            new Class
     {
         Id = MATH101_01_Spring,
     ClassCode = "MATH101.01.S24",
       SubjectOfferingId = SubjectOfferingSeeder.MATH101_Spring2024,
 TeacherUserId = TeacherStudentSeeder.Teacher3Id,
           MaxEnrollment = 50,
          IsActive = true,
          CreatedAt = DateTime.UtcNow,
       UpdatedAt = DateTime.UtcNow
     },

// ===== CS101 Spring 2024 - 1 class =====
      new Class
  {
           Id = CS101_01_Spring,
ClassCode = "CS101.01.S24",
      SubjectOfferingId = SubjectOfferingSeeder.CS101_Spring2024,
  TeacherUserId = TeacherStudentSeeder.Teacher1Id,
    MaxEnrollment = 45,
        IsActive = true,
   CreatedAt = DateTime.UtcNow,
           UpdatedAt = DateTime.UtcNow
      }
   };

            await _context.Classes.AddRangeAsync(classes);
            await SaveAsync("Classes");

Console.WriteLine($"?? Created {classes.Count} classes linked to SubjectOfferings");
      Console.WriteLine("? Classes can now belong to semester-specific subject offerings!");
    }
    }
}
