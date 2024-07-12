using System.Text;
using DotnetRpg.Data;
using DotnetRpg.Services.AuthService;
using DotnetRpg.Services.CharacterService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using DotnetRpg.Services.FightService;
using DotnetRpg.Services.EnemyGeneratorService;
using DotnetRpg;
using DotnetRpg.Data.Seeding;
using DotnetRpg.Services.InventoryService;
using DotnetRpg.Services.UserProvider;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;

SelfLog.Enable(Console.Error);
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
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

    builder.Services.AddScoped<IUserProvider, AuthenticatedUserProvider>();

    builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
    
    builder.Services.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("DefaultLogger"));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(
            corsPolicyBuilder =>
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

    var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
    optionsBuilder.UseSqlServer(connectionString);
    optionsBuilder.EnableSensitiveDataLogging();

    var context = new DataContext(optionsBuilder.Options, new MockUserProvider());
    
    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

    await DataSeeder.SeedData(context, loggerFactory.CreateLogger(nameof(DataSeeder)));

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