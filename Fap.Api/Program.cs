using Fap.Api.Filters;
using Fap.Api.Mappings;
using Fap.Api.Services;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Fap.Infrastructure.Data.Seed;
using Fap.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==================================================
// 🔹 CONTROLLERS & SWAGGER
// ==================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Swagger cấu hình JWT (paste token không cần chữ Bearer)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FAP API",
        Version = "v1",
        Description = "University Academic & Student Management on Blockchain"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập **JWT token** vào đây."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    c.OperationFilter<SwaggerAuthorizeOperationFilter>();
});

// ==================================================
// 🔹 DATABASE & REPOSITORIES
// ==================================================
builder.Services.AddDbContext<FapDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Repository pattern
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ==================================================
// 🔹 SERVICES & AUTOMAPPER
// ==================================================
builder.Services.AddScoped<AuthService>();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AutoMapperProfile)));

// ==================================================
// 🔹 JWT AUTHENTICATION
// ==================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

// ==================================================
// 🔹 BUILD APP
// ==================================================
var app = builder.Build();

// Apply migrations & seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FapDbContext>();
    await db.Database.MigrateAsync();
    await DataSeeder.SeedAsync(db);
}

// ==================================================
// 🔹 MIDDLEWARE PIPELINE
// ==================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
