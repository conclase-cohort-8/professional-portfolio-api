using Mailjet.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProfessionalPortfolio.API.Extensions;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Mapper;
using ProfessionalPortfolio.Application.Services;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Settings;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.ExternalServices;
using ProfessionalPortfolio.Infrastructure.Persistence;
using ProfessionalPortfolio.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        new JsonFormatter(),
        "logs/logs-.json", 
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information
        )
    .CreateLogger();

builder.Host.UseSerilog();
// Add your services to the container.

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<SqlServerDbContext>(options => options.UseSqlServer(connectionString));
// Configure Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequiredLength = 8;
    opt.Password.RequireDigit = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireLowercase = true;

    opt.User.RequireUniqueEmail = true;

    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    opt.Lockout.MaxFailedAccessAttempts = 3;

    opt.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<SqlServerDbContext>()
.AddDefaultTokenProviders();
//
builder.Services.AddAutoMapper(m =>
{
}, typeof(MapperProfile));
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
// Configure Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new UrlSegmentApiVersionReader());
});

builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
//Configure HttpContext
builder.Services.AddHttpContextAccessor();
// Other services here
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

//Mail config
builder.Services.Configure<MailKitSettings>(builder.Configuration.GetSection("MailKitSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

//Mailjet config
var mailJetSection = builder.Configuration.GetSection("MailJet");
builder.Services.Configure<MailJetSettings>(mailJetSection);
var mailJetSettings = mailJetSection.Get<MailJetSettings>() ?? 
    throw new ArgumentNullException("MailJetSettings");
builder.Services.AddHttpClient<IMailjetClient, MailjetClient>(opt =>
{
    opt.UseBasicAuthentication(mailJetSettings.ApiKey, mailJetSettings.ApiSecret);
});

//Cloudinary config
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IUploadService, UploadService>();
//Add Authentication configuration
//JWT: header: type: JWT, alg: HMAC256, payload: userId, email, roles, signature
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtOptions>(jwtSection);
var jwtSettings = jwtSection.Get<JwtOptions>() ??
    throw new ArgumentNullException("JwtSettings");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    option.IncludeXmlComments(xmlPath);

    option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authentication",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });

    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Portfolio API",
        Version = "v1",
        Description = "Portfolio API v1.0",
        Contact = new OpenApiContact
        {
            Name = "Conclase Cohort 8",
            Email = "info@email.com"
        }
    });
    option.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Portfolio API",
        Version = "v2",
        Description = "Portfolio API v2.0",
        Contact = new OpenApiContact
        {
            Name = "Conclase Cohort 8",
            Email = "info@email.com"
        }
    });
});

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.UseGlobalExceptionHandler(logger);
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms.";
    options.EnrichDiagnosticContext = (dCtx, httpCtx) =>
    {
        dCtx.Set("RequestHost", httpCtx.Request.Host.Value);
        dCtx.Set("Scheme", httpCtx.Request.Scheme);
        dCtx.Set("UserAgent", httpCtx.Response.Headers["User-Agent"].ToString());
        dCtx.Set("ClientIP", httpCtx.Connection.RemoteIpAddress?.ToString());
        dCtx.Set("Endpoint", httpCtx.GetEndpoint()?.DisplayName);
    };
});

await app.SeedAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        opt.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
