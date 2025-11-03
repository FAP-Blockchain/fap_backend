using System;
using System.Threading.Tasks;

namespace Fap.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }
        IRoleRepository Roles { get; }
        IOtpRepository Otps { get; }  // ✅ NEW
        
        Task<int> SaveChangesAsync();
    }
}
