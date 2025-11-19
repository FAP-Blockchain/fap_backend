using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    /// <summary>
    /// Master data seeder - orchestrates all individual seeders
    /// ✅ Refactored into multiple small files for maintainability
    /// ✅ Updated to use SubjectOffering pattern for multi-semester support
    /// </summary>
    public static class DataSeeder
    {
        public static async Task SeedAsync(FapDbContext context)
        {
      // Quick check - if TimeSlots exist, assume data is already seeded
if (await context.TimeSlots.AnyAsync())
       {
   Console.WriteLine("==============================================");
     Console.WriteLine("✅ Seed data already exists. Skipping seeding.");
       Console.WriteLine("==============================================");
       return;
   }

    Console.WriteLine("==============================================");
       Console.WriteLine("🌱 Starting database seeding...");
Console.WriteLine("==============================================");

try
    {
     // Order is important! Follow dependency chain:
   // 1. Roles (no dependencies)
     await new RoleSeeder(context).SeedAsync();
    
      // 2. Users (depends on Roles)
   await new UserSeeder(context).SeedAsync();
        
   // 3. Teachers & Students (depends on Users)
    await new TeacherStudentSeeder(context).SeedAsync();
         
   // 4. Semesters (no dependencies)
 await new SemesterSeeder(context).SeedAsync();
      
     // 5. Subjects & SubjectOfferings (depends on Semesters)
  // ✅ NEW PATTERN: Subjects are master data, Offerings link to semesters
 await new SubjectOfferingSeeder(context).SeedAsync();
        
     // 6. TimeSlots (no dependencies)
    await new TimeSlotSeeder(context).SeedAsync();
        
    // 7. Classes (depends on SubjectOfferings and Teachers)
  // ✅ CHANGED: Classes now use SubjectOfferingId instead of SubjectId
 await new ClassSeeder(context).SeedAsync();

      // 8. Enrollments & ClassMembers (depends on Classes and Students)
    await new EnrollmentSeeder(context).SeedAsync();

         // Final save
    await context.SaveChangesAsync();

     Console.WriteLine("==============================================");
     Console.WriteLine("✅ Database seeding completed successfully!");
    Console.WriteLine("==============================================");
 Console.WriteLine();
     Console.WriteLine("📊 SEED DATA SUMMARY:");
    Console.WriteLine($"   • Roles: {await context.Roles.CountAsync()}");
    Console.WriteLine($"   • Users: {await context.Users.CountAsync()}");
Console.WriteLine($"   • Teachers: {await context.Teachers.CountAsync()}");
    Console.WriteLine($"• Students: {await context.Students.CountAsync()}");
   Console.WriteLine($"   • Semesters: {await context.Semesters.CountAsync()}");
      Console.WriteLine($"   • Subjects (Master): {await context.Subjects.CountAsync()}");
   Console.WriteLine($"   • SubjectOfferings: {await context.SubjectOfferings.CountAsync()} ✨ NEW!");
 Console.WriteLine($"   • TimeSlots: {await context.TimeSlots.CountAsync()}");
       Console.WriteLine($"   • Classes: {await context.Classes.CountAsync()}");
  Console.WriteLine($"   • Class Members: {await context.ClassMembers.CountAsync()}");
     Console.WriteLine($"   • Enrollments: {await context.Enrolls.CountAsync()}");
     Console.WriteLine("==============================================");
       Console.WriteLine();
Console.WriteLine("🎯 KEY IMPROVEMENTS:");
     Console.WriteLine("   ✅ Modular seeders - easy to maintain");
    Console.WriteLine("   ✅ SubjectOffering pattern - subjects can be offered in multiple semesters");
   Console.WriteLine("   ✅ No data duplication - subjects are master data");
   Console.WriteLine("   ✅ Sufficient test data for all APIs");
 Console.WriteLine("==============================================");
   }
  catch (Exception ex)
 {
   Console.WriteLine("==============================================");
  Console.WriteLine($"❌ ERROR during seeding: {ex.Message}");
   Console.WriteLine($"   Stack trace: {ex.StackTrace}");
 Console.WriteLine("==============================================");
   throw;
       }
     }
    }
}
