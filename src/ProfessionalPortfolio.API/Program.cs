using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
// COnfigure AutoMapper
builder.Services.AddAutoMapper(m => {}, typeof(MapperProfile));
//Configure API Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new UrlSegmentApiVersionReader());
});

//Api Explorer config
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddScoped<InMemoryDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<ISkillRepository,  SkillRepository>();
// Other services here
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<ISkillService, SkillService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Portfolio API",
        Version = "v1",
        Description = "Portfolio API v1.0"
    });
    opt.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Portfolio API",
        Version = "v2",
        Description = "Portfolio API v2.0"
    });
});

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
