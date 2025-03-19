using System.Text;
using DotnetRpg;
using DotnetRpg.Data;
using DotnetRpg.Data.Seeding;
using DotnetRpg.Services.AuthService;
using DotnetRpg.Services.CharacterService;
using DotnetRpg.Services.DamageCalculator;
using DotnetRpg.Services.EnemyGeneratorService;
using DotnetRpg.Services.FightService;
using DotnetRpg.Services.InventoryService;
using DotnetRpg.Services.UserProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Swashbuckle.AspNetCore.Filters;

SelfLog.Enable(Console.Error);
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    var app = BuildApplication(args);

    ConfigureRequestPipeline(app);

    await SeedData(app);

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

return;

static async Task SeedData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedData();
}

static void ConfigureRequestPipeline(WebApplication app)
{
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(corsPolicyBuilder =>
            corsPolicyBuilder
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
        );
    }

    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();
}

static WebApplication BuildApplication(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var tokenSecret = builder.Configuration.GetSection("TokenSettings:Secret").Value
                      ?? throw new ArgumentException("Missing token secret");

    // Add Serilog
    builder.Host.UseSerilog(
        (_, services, configuration) =>
            configuration
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Infrastructure", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .WriteTo.Console()
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
    );

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddLogging(x => x.AddSerilog());
    builder.Services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(connectionString);
        options.EnableSensitiveDataLogging();
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition(
            "oauth2",
            new OpenApiSecurityScheme
            {
                Description = "Standard authorization header using the Bearer scheme",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            }
        );
        c.OperationFilter<SecurityRequirementsOperationFilter>();
    });
    builder.Services.AddAutoMapper(typeof(Program).Assembly);
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<ICharacterService, CharacterService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IFightService, FightService>();
    builder.Services.AddScoped<IEnemyGeneratorService, EnemyGeneratorService>();
    builder.Services.AddScoped<IInventoryService, InventoryService>();
    builder.Services.AddSingleton<IDamageCalculator, DamageCalculator>();

    builder.Services.AddScoped<DataSeeder>();

    builder.Services.AddScoped<IUserProvider, AuthenticatedUserProvider>();

    builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

    return builder.Build();
}