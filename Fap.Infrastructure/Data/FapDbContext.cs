using Fap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Data
{
    public class FapDbContext : DbContext
    {
        public FapDbContext(DbContextOptions<FapDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassMember> ClassMembers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectOffering> SubjectOfferings { get; set; }  // ✅ NEW
        public DbSet<SubjectCriteria> SubjectCriteria { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeComponent> GradeComponents { get; set; }
        public DbSet<StudentRoadmap> StudentRoadmaps { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<CertificateTemplate> CertificateTemplates { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Otp> Otps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==================== DECIMAL PRECISION ====================
            // Grade.Score
            modelBuilder.Entity<Grade>()
                .Property(g => g.Score)
                .HasPrecision(4, 2); // 0.00 - 99.99

            // Student.GPA
            modelBuilder.Entity<Student>()
                .Property(s => s.GPA)
                .HasPrecision(4, 2); // 0.00 - 99.99

            // SubjectCriteria.MinScore
            modelBuilder.Entity<SubjectCriteria>()
                .Property(sc => sc.MinScore)
                .HasPrecision(4, 2); // 0.00 - 99.99

            // ==================== RELATIONSHIPS ====================

            // User <-> Student/Teacher (Restrict to prevent cascade path errors)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.User)
                .WithOne(u => u.Teacher)
                .HasForeignKey<Teacher>(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Role <-> Permission
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithOne(p => p.Role)
                .HasForeignKey(p => p.RoleId);

            // User <-> Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // ClassMember composite key
            modelBuilder.Entity<ClassMember>()
                .HasKey(cm => new { cm.ClassId, cm.StudentId });

            // ClassMember relationships (Restrict to prevent cascade path errors)
            modelBuilder.Entity<ClassMember>()
                .HasOne(cm => cm.Class)
                .WithMany(c => c.Members)
                .HasForeignKey(cm => cm.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassMember>()
                .HasOne(cm => cm.Student)
                .WithMany(s => s.ClassMembers)
                .HasForeignKey(cm => cm.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ NEW: SubjectOffering <-> Subject
            modelBuilder.Entity<SubjectOffering>()
                .HasOne(so => so.Subject)
                .WithMany(s => s.Offerings)
                .HasForeignKey(so => so.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ NEW: SubjectOffering <-> Semester
            modelBuilder.Entity<SubjectOffering>()
                .HasOne(so => so.Semester)
                .WithMany(sem => sem.SubjectOfferings)
                .HasForeignKey(so => so.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ NEW: SubjectOffering <-> Class
            modelBuilder.Entity<Class>()
                .HasOne(c => c.SubjectOffering)
                .WithMany(so => so.Classes)
                .HasForeignKey(c => c.SubjectOfferingId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ NEW: Unique constraint on SubjectOffering (Subject + Semester)
            modelBuilder.Entity<SubjectOffering>()
                .HasIndex(so => new { so.SubjectId, so.SemesterId })
                .IsUnique()
                .HasDatabaseName("UK_SubjectOffering_Subject_Semester");

            // Subject <-> SubjectCriteria
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.SubjectCriterias)
                .WithOne(sc => sc.Subject)
                .HasForeignKey(sc => sc.SubjectId);

            // Slot <-> Attendance (Restrict to prevent cascade path errors)
            modelBuilder.Entity<Slot>()
                .HasMany(s => s.Attendances)
                .WithOne(a => a.Slot)
                .HasForeignKey(a => a.SlotId)
                .OnDelete(DeleteBehavior.Restrict);

            // Subject <-> Grade
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Grades)
                .WithOne(g => g.Subject)
                .HasForeignKey(g => g.SubjectId);

            // Student <-> Grade
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Grades)
                .WithOne(g => g.Student)
                .HasForeignKey(g => g.StudentId);

            // Teacher <-> Class
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Classes)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherUserId);

            // Student <-> Enroll (Restrict to prevent cascade path errors)
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Enrolls)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Class <-> Enroll
            modelBuilder.Entity<Class>()
                .HasMany(c => c.Enrolls)
                .WithOne(e => e.Class)
                .HasForeignKey(e => e.ClassId);

            // User <-> RefreshToken
            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId);

            // TimeSlot <-> Slot
            modelBuilder.Entity<Slot>()
                .HasOne(s => s.TimeSlot)
                .WithMany(ts => ts.Slots)
                .HasForeignKey(s => s.TimeSlotId);

            // Class <-> Slot
            modelBuilder.Entity<Slot>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Slots)
                .HasForeignKey(s => s.ClassId);

            // Slot <-> SubstituteTeacher (Restrict to prevent cascade)
            modelBuilder.Entity<Slot>()
                .HasOne(s => s.SubstituteTeacher)
                .WithMany()
                .HasForeignKey(s => s.SubstituteTeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Credential <-> Student
            modelBuilder.Entity<Credential>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Credentials)
                .HasForeignKey(c => c.StudentId);

            // Credential <-> CertificateTemplate
            modelBuilder.Entity<Credential>()
                .HasOne(c => c.CertificateTemplate)
                .WithMany(ct => ct.Credentials)
                .HasForeignKey(c => c.CertificateTemplateId);

            // ActionLog <-> User
            modelBuilder.Entity<ActionLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.ActionLogs)
                .HasForeignKey(a => a.UserId);

            // Attendance <-> Student (Restrict to prevent cascade path errors)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Attendance <-> Slot (Restrict to prevent cascade path errors)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Slot)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.SlotId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enroll <-> Class (Restrict to prevent cascade path errors)
            modelBuilder.Entity<Enroll>()
                .HasOne(e => e.Class)
                .WithMany(c => c.Enrolls)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==================== STUDENT ROADMAP ====================
            // StudentRoadmap <-> Student (🔴 RESTRICT to prevent cascade cycle)
            modelBuilder.Entity<StudentRoadmap>()
                .HasOne(sr => sr.Student)
                .WithMany(s => s.Roadmaps)
                .HasForeignKey(sr => sr.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentRoadmap <-> Subject (🔴 RESTRICT to prevent cascade cycle)
            modelBuilder.Entity<StudentRoadmap>()
                .HasOne(sr => sr.Subject)
                .WithMany(s => s.Roadmaps)
                .HasForeignKey(sr => sr.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentRoadmap <-> Semester (🔴 RESTRICT to prevent cascade cycle)
            modelBuilder.Entity<StudentRoadmap>()
                .HasOne(sr => sr.Semester)
                .WithMany()
                .HasForeignKey(sr => sr.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ OTP Configuration
            modelBuilder.Entity<Otp>()
                .HasIndex(o => new { o.Email, o.Code, o.Purpose })
                .HasDatabaseName("IX_Otp_Email_Code_Purpose");

            modelBuilder.Entity<Otp>()
                .HasIndex(o => o.ExpiresAt)
                .HasDatabaseName("IX_Otp_ExpiresAt");
        }
    }
}