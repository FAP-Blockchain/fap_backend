using Fap.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fap.Infrastructure.Data.Seed
{
    /// <summary>
    /// Base class for all seeders with common utilities
    /// </summary>
    public abstract class BaseSeeder
    {
        protected readonly FapDbContext _context;
        protected readonly PasswordHasher<User> _hasher;

        protected BaseSeeder(FapDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
        }

        /// <summary>
        /// Execute the seeding logic
        /// </summary>
        public abstract Task SeedAsync();

        /// <summary>
        /// Generate consistent password hash
        /// </summary>
        protected string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        /// <summary>
        /// Save changes with error handling
        /// </summary>
        protected async Task SaveAsync(string entityName)
        {
            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ {entityName} seeded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error seeding {entityName}: {ex.Message}");
                throw;
            }
        }
    }
}
