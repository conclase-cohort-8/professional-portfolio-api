using Mailjet.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Services;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Application.Settings;
using ProfessionalPortfolio.Domain.Entities;
using ProfessionalPortfolio.Infrastructure.ExternalServices;
using ProfessionalPortfolio.Infrastructure.Persistence;
using ProfessionalPortfolio.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add your services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<SqlServerDbContext>(options => options.UseSqlServer(connectionString));
//
builder.Services.AddScoped<InMemoryDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<ISkillRepository,  SkillRepository>();
// Other services here
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IEmailService, EmailService>();

//services.Configure<MailSettings>(configuration.GetSection("DryMailClient"));
//services.AddHttpClient<IMailjetClient, MailjetClient>(sp =>
//{
//    sp.UseBasicAuthentication(settings.ApiKey, settings.ApiSecret);
//});

//Add email settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
//Add Authentication configuration
//JWT: header: type: JWT, alg: HMAC256, payload: userId, email, roles, signature
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtOptions>(jwtSection);
var jwtSettings = jwtSection.Get<JwtOptions>() ??
    throw new ArgumentNullException("JwtSettings");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
