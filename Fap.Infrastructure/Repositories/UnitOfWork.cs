using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fap.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FapDbContext _context;

        public IUserRepository Users { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        public UnitOfWork(FapDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            RefreshTokens = new RefreshTokenRepository(context);
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
