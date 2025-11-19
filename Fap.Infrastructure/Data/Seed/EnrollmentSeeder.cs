using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data.Seed
{
    public class EnrollmentSeeder : BaseSeeder
    {
        public EnrollmentSeeder(FapDbContext context) : base(context) { }

        public override async Task SeedAsync()
        {
            if (await _context.ClassMembers.AnyAsync())
            {
                Console.WriteLine("⏭️  Enrollments already exist. Skipping...");
                return;
            }

            await SeedClassMembersAsync();
            await SeedEnrollsAsync();
        }

        private async Task SeedClassMembersAsync()
        {
            // Enroll students into classes
            var classMembers = new List<ClassMember>
   {
     // SE101.01.S24 - 4 students
   new ClassMember
      {
         ClassId = ClassSeeder.SE101_01_Spring,
   StudentId = TeacherStudentSeeder.Student1Id,
    JoinedAt = new DateTime(2024, 1, 5)
       },
 new ClassMember
   {
    ClassId = ClassSeeder.SE101_01_Spring,
     StudentId = TeacherStudentSeeder.Student2Id,
     JoinedAt = new DateTime(2024, 1, 5)
      },
   new ClassMember
       {
     ClassId = ClassSeeder.SE101_01_Spring,
   StudentId = TeacherStudentSeeder.Student3Id,
   JoinedAt = new DateTime(2024, 1, 5)
   },
    new ClassMember
    {
 ClassId = ClassSeeder.SE101_01_Spring,
    StudentId = TeacherStudentSeeder.Student4Id,
  JoinedAt = new DateTime(2024, 1, 5)
     },

// SE101.02.S24 - 2 students
    new ClassMember
    {
     ClassId = ClassSeeder.SE101_02_Spring,
        StudentId = TeacherStudentSeeder.Student5Id,
 JoinedAt = new DateTime(2024, 1, 5)
      },
     new ClassMember
  {
   ClassId = ClassSeeder.SE101_02_Spring,
    StudentId = TeacherStudentSeeder.Student6Id,
    JoinedAt = new DateTime(2024, 1, 5)
     },

    // DB201.01.S24 - 3 students
   new ClassMember
 {
     ClassId = ClassSeeder.DB201_01_Spring,
  StudentId = TeacherStudentSeeder.Student1Id,
 JoinedAt = new DateTime(2024, 1, 6)
      },
     new ClassMember
    {
   ClassId = ClassSeeder.DB201_01_Spring,
   StudentId = TeacherStudentSeeder.Student2Id,
    JoinedAt = new DateTime(2024, 1, 6)
    },
       new ClassMember
     {
  ClassId = ClassSeeder.DB201_01_Spring,
   StudentId = TeacherStudentSeeder.Student3Id,
 JoinedAt = new DateTime(2024, 1, 6)
     },

// MATH101.01.S24 - 4 students
   new ClassMember
    {
  ClassId = ClassSeeder.MATH101_01_Spring,
   StudentId = TeacherStudentSeeder.Student1Id,
    JoinedAt = new DateTime(2024, 1, 7)
     },
new ClassMember
  {
ClassId = ClassSeeder.MATH101_01_Spring,
  StudentId = TeacherStudentSeeder.Student3Id,
 JoinedAt = new DateTime(2024, 1, 7)
  },
new ClassMember
  {
        ClassId = ClassSeeder.MATH101_01_Spring,
   StudentId = TeacherStudentSeeder.Student4Id,
  JoinedAt = new DateTime(2024, 1, 7)
},
     new ClassMember
{
  ClassId = ClassSeeder.MATH101_01_Spring,
StudentId = TeacherStudentSeeder.Student5Id,
    JoinedAt = new DateTime(2024, 1, 7)
  },

    // CS101.01.S24 - 3 students
        new ClassMember
    {
          ClassId = ClassSeeder.CS101_01_Spring,
    StudentId = TeacherStudentSeeder.Student2Id,
       JoinedAt = new DateTime(2024, 1, 8)
    },
 new ClassMember
{
     ClassId = ClassSeeder.CS101_01_Spring,
StudentId = TeacherStudentSeeder.Student4Id,
      JoinedAt = new DateTime(2024, 1, 8)
    },
new ClassMember
 {
    ClassId = ClassSeeder.CS101_01_Spring,
     StudentId = TeacherStudentSeeder.Student6Id,
JoinedAt = new DateTime(2024, 1, 8)
  }
         };

            await _context.ClassMembers.AddRangeAsync(classMembers);
            await SaveAsync("ClassMembers");

            Console.WriteLine($"📊 Enrolled {classMembers.Count} student-class relationships");
        }

        private async Task SeedEnrollsAsync()
        {
            // Create enrollment records for tracking
            var enrolls = new List<Enroll>
  {
       // Student 1 enrollments
    new Enroll
     {
     Id = Guid.NewGuid(),
 ClassId = ClassSeeder.SE101_01_Spring,
        StudentId = TeacherStudentSeeder.Student1Id,
 RegisteredAt = new DateTime(2024, 1, 5),
      IsApproved = true
    },
   new Enroll
     {
   Id = Guid.NewGuid(),
 ClassId = ClassSeeder.DB201_01_Spring,
 StudentId = TeacherStudentSeeder.Student1Id,
   RegisteredAt = new DateTime(2024, 1, 6),
        IsApproved = true
       },
      new Enroll
     {
       Id = Guid.NewGuid(),
   ClassId = ClassSeeder.MATH101_01_Spring,
     StudentId = TeacherStudentSeeder.Student1Id,
    RegisteredAt = new DateTime(2024, 1, 7),
      IsApproved = true
      },

    // Student 2 enrollments
    new Enroll
{
 Id = Guid.NewGuid(),
  ClassId = ClassSeeder.SE101_01_Spring,
    StudentId = TeacherStudentSeeder.Student2Id,
 RegisteredAt = new DateTime(2024, 1, 5),
IsApproved = true
      },
new Enroll
  {
       Id = Guid.NewGuid(),
   ClassId = ClassSeeder.DB201_01_Spring,
   StudentId = TeacherStudentSeeder.Student2Id,
  RegisteredAt = new DateTime(2024, 1, 6),
     IsApproved = true
       },
 new Enroll
     {
  Id = Guid.NewGuid(),
 ClassId = ClassSeeder.CS101_01_Spring,
       StudentId = TeacherStudentSeeder.Student2Id,
 RegisteredAt = new DateTime(2024, 1, 8),
 IsApproved = true
   }
    };

            await _context.Enrolls.AddRangeAsync(enrolls);
            await SaveAsync("Enrolls");

            Console.WriteLine($"📊 Created {enrolls.Count} enrollment records");
        }
    }
}
