using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    /// <summary>
    /// Master data seeder - orchestrates all individual seeders.
    /// Refactored into multiple small files for maintainability.
    /// Updated to use SubjectOffering pattern for multi-semester support.
    /// Adds missing seeders so every core table has sample data.
    /// </summary>
    public static class DataSeeder
    {
        public static async Task SeedAsync(FapDbContext context)
        {
            // Force update of Credential Templates (Temporary fix for demo)
            await new CredentialSeeder(context).SeedAsync();

            // Quick check - if TimeSlots exist, assume data is already seeded
            if (await context.TimeSlots.AnyAsync())
            {
                Console.WriteLine("==============================================");
                Console.WriteLine("Seed data already exists. Skipping seeding.");
                Console.WriteLine("==============================================");
                return;
            }

            Console.WriteLine("==============================================");
            Console.WriteLine("Starting database seeding...");
            Console.WriteLine("==============================================");

            try
            {
                // Order is important! Follow dependency chain:
                // 1. Roles (no dependencies)
                await new RoleSeeder(context).SeedAsync();

                // 2. Permissions (depends on Roles)
                await new PermissionSeeder(context).SeedAsync();

                // 3. Users (depends on Roles)
                await new UserSeeder(context).SeedAsync();

                // 4. Curriculums (no dependencies)
                await new CurriculumSeeder(context).SeedAsync();

                // 5. Teachers & Students (depends on Users + Curriculums)
                await new TeacherStudentSeeder(context).SeedAsync();

                // 6. Semesters (no dependencies)
                await new SemesterSeeder(context).SeedAsync();

                // 7. Subjects & SubjectOfferings (depends on Semesters)
                await new SubjectOfferingSeeder(context).SeedAsync();

                // 7b. Specializations (depends on Subjects and Teachers)
                await new SpecializationSeeder(context).SeedAsync();

                // 8. Curriculum Subjects (depends on Curriculums & Subjects)
                await new CurriculumSubjectSeeder(context).SeedAsync();

                // 9. Subject Criteria (depends on Subjects)
                await new SubjectCriteriaSeeder(context).SeedAsync();

                // 10. TimeSlots (no dependencies)
                await new TimeSlotSeeder(context).SeedAsync();

                // 11. Classes (depends on SubjectOfferings and Teachers)
                await new ClassSeeder(context).SeedAsync();

                // 12. Enrollments & ClassMembers (depends on Classes and Students)
                await new EnrollmentSeeder(context).SeedAsync();

                // 13. Slots (depends on Classes and TimeSlots)
                await new SlotSeeder(context).SeedAsync();

                // 14. Attendance (depends on Slots and Students)
                await new AttendanceSeeder(context).SeedAsync();

                // 15. Grade Components (no dependencies)
                await new GradeComponentSeeder(context).SeedAsync();

                // 16. Grades (depends on Students, Subjects, GradeComponents)
                await new GradeSeeder(context).SeedAsync();

                // 17. Certificate Templates (no dependencies)
                await new CertificateTemplateSeeder(context).SeedAsync();

                // 18. Credentials (depends on Students and CertificateTemplates)
                await new CredentialSeeder(context).SeedAsync();

                // 19. Student Roadmaps (depends on Students, Subjects, Semesters)
                await new StudentRoadmapSeeder(context).SeedAsync();

                // 20. Refresh Tokens (depends on Users)
                await new RefreshTokenSeeder(context).SeedAsync();

                // 21. OTPs (depends on Users)
                await new OtpSeeder(context).SeedAsync();

                // 22. Action Logs (depends on Users, Credentials, etc.)
                await new ActionLogSeeder(context).SeedAsync();

                // Final save
                await context.SaveChangesAsync();

                Console.WriteLine("==============================================");
                Console.WriteLine("Database seeding completed successfully.");
                Console.WriteLine("==============================================");
                Console.WriteLine();
                Console.WriteLine("Seed data summary:");
                Console.WriteLine($"   • Roles: {await context.Roles.CountAsync()}");
                Console.WriteLine($"   • Permissions: {await context.Permissions.CountAsync()}");
                Console.WriteLine($"   • Users: {await context.Users.CountAsync()}");
                Console.WriteLine($"   • Curriculums: {await context.Curriculums.CountAsync()}");
                Console.WriteLine($"   • Curriculum Subjects: {await context.CurriculumSubjects.CountAsync()}");
                Console.WriteLine($"   • Teachers: {await context.Teachers.CountAsync()}");
                Console.WriteLine($"   • Students: {await context.Students.CountAsync()}");
                Console.WriteLine($"   • Semesters: {await context.Semesters.CountAsync()}");
                Console.WriteLine($"   • Subjects (Master): {await context.Subjects.CountAsync()}");
                Console.WriteLine($"   • SubjectOfferings: {await context.SubjectOfferings.CountAsync()}");
                Console.WriteLine($"   • SubjectCriteria: {await context.SubjectCriteria.CountAsync()}");
                Console.WriteLine($"   • TimeSlots: {await context.TimeSlots.CountAsync()}");
                Console.WriteLine($"   • Classes: {await context.Classes.CountAsync()}");
                Console.WriteLine($"   • Class Members: {await context.ClassMembers.CountAsync()}");
                Console.WriteLine($"   • Enrollments: {await context.Enrolls.CountAsync()}");
                Console.WriteLine($"   • Slots: {await context.Slots.CountAsync()}");
                Console.WriteLine($"   • Attendances: {await context.Attendances.CountAsync()}");
                Console.WriteLine($"   • Grade Components: {await context.GradeComponents.CountAsync()}");
                Console.WriteLine($"   • Grades: {await context.Grades.CountAsync()}");
                Console.WriteLine($"   • Certificate Templates: {await context.CertificateTemplates.CountAsync()}");
                Console.WriteLine($"   • Credentials: {await context.Credentials.CountAsync()}");
                Console.WriteLine($"   • Student Roadmaps: {await context.StudentRoadmaps.CountAsync()}");
                Console.WriteLine($"   • Refresh Tokens: {await context.RefreshTokens.CountAsync()}");
                Console.WriteLine($"   • OTPs: {await context.Otps.CountAsync()}");
                Console.WriteLine($"   • Action Logs: {await context.ActionLogs.CountAsync()}");
                Console.WriteLine("==============================================");
                Console.WriteLine();
                Console.WriteLine("Key highlights:");
                Console.WriteLine("   • All 25 database tables seeded");
                Console.WriteLine("   • Modular seeders for easy maintenance");
                Console.WriteLine("   • SubjectOffering pattern enables multi-semester support");
                Console.WriteLine("   • No data duplication thanks to normalized design");
                Console.WriteLine("   • Comprehensive test data covering attendance, grades, credentials, roadmaps");
                Console.WriteLine("   • Role-based permissions and security flows");
                Console.WriteLine("   • OTP/refresh token flows and audit trail coverage");
                Console.WriteLine("   • Edge cases like revoked credentials and substitute teachers included");
                Console.WriteLine("==============================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("==============================================");
                Console.WriteLine($"Error during seeding: {ex.Message}");
                Console.WriteLine($"   Stack trace: {ex.StackTrace}");
                Console.WriteLine("==============================================");
                throw;
            }
        }
    }
}
