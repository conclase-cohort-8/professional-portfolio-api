using Microsoft.EntityFrameworkCore;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Mapper;
using ProfessionalPortfolio.Application.Services;
using ProfessionalPortfolio.Application.Services.Interfaces;
using ProfessionalPortfolio.Infrastructure.Persistence;
using ProfessionalPortfolio.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add your services to the container.

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<SqlServerDbContext>(options => options.UseSqlServer(connectionString));
//
builder.Services.AddAutoMapper(m =>
{
}, typeof(MapperProfile));
builder.Services.AddScoped<InMemoryDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<ISkillRepository,  SkillRepository>();
// Other services here
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();

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
