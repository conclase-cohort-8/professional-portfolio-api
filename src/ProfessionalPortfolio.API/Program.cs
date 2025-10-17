using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Infrastructure.Persistence;
using ProfessionalPortfolio.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add your services to the container.
builder.Services.AddScoped<InMemoryDbContext>();
builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();
// Other services here
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
