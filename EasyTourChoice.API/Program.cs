using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Profiles;
using EasyTourChoice.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<ITourDataRepository, TourDataRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
// TODO: replace key strings by enums
builder.Services.AddKeyedScoped<ITravelPlanningService, OSRMTravelPlanningService>("OSRM");
builder.Services.AddKeyedScoped<ITravelPlanningService, TomTomTravelPlanningService>("TomTom");

builder.Services.AddAutoMapper(typeof(TourDataProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<TourDataContext>(dbContextOptions => 
    dbContextOptions.UseSqlite(builder.Configuration["ETC_CONNECTIONSTRING"]));

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
