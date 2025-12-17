using backend.Context;
using backend.Services;
using backend.Services.MatchEventValidation;
using backend.Services.MatchValidation;
using backend.Services.PlayerService;
using backend.Services.TeamPlayerValidation;
using backend.Services.TeamsValidation;
using backend.Services.TeamValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var serverVersion = new MySqlServerVersion(new Version(10, 4, 32));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FootballDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnectionString"), serverVersion));
builder.Services.AddScoped<ICommonValidationService, CommonValidationService>();
builder.Services.AddScoped<ITeamValidationService, TeamValidationService>();
builder.Services.AddScoped<IMatchValidationService, MatchValidationService>();
builder.Services.AddScoped<IMatchEventValidationService, MatchEventValidationService>();
builder.Services.AddScoped<ITeamPlayerValidationService, TeamPlayerValidationService>();
builder.Services.AddScoped<IPlayerRegistration, PlayerRegistration>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
