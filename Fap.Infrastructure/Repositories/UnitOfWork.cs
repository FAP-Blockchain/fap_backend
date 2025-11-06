using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;

namespace Fap.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FapDbContext _context;

        public IUserRepository Users { get; }
        public IRefreshTokenRepository RefreshTokens { get; }
        public IStudentRepository Students { get; }
        public ITeacherRepository Teachers { get; }
        public IRoleRepository Roles { get; }
        public IPermissionRepository Permissions { get; }
        public IOtpRepository Otps { get; }
        public IClassRepository Classes { get; }  // ✅ NEW
        public ISubjectRepository Subjects { get; }  // ✅ NEW
        public ITimeSlotRepository TimeSlots { get; }  // ✅ NEW

        public UnitOfWork(FapDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            RefreshTokens = new RefreshTokenRepository(context);
            Students = new StudentRepository(context);
            Teachers = new TeacherRepository(context);
            Roles = new RoleRepository(context);
            Permissions = new PermissionRepository(context);
            Otps = new OtpRepository(context);
            Classes = new ClassRepository(context);  // ✅ NEW
            Subjects = new SubjectRepository(context);  // ✅ NEW
            TimeSlots = new TimeSlotRepository(context);  // ✅ NEW
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
