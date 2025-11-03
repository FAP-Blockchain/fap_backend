using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Fap.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FapDbContext>
{
    public FapDbContext CreateDbContext(string[] args)
    {
        // ✅ Trỏ đến thư mục Fap.Api để đọc appsettings.json
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Fap.Api");
        
        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        // ✅ Sửa key từ "Default" thành "DefaultConnection"
        var conn = config.GetConnectionString("DefaultConnection")
                 ?? "Server=localhost,1433;Database=FapDb;User Id=sa;Password=12345;TrustServerCertificate=True;Encrypt=False;";

        var options = new DbContextOptionsBuilder<FapDbContext>()
            .UseSqlServer(conn)
            .Options;

        return new FapDbContext(options);
    }
}
