using EasyTourChoice.API.Application.DataAggregation;
using EasyTourChoice.API.Application.DataHandling;
using EasyTourChoice.API.Application.Profiles;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories;
using EasyTourChoice.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IAvalancheReportService, EAWSReportService>();
builder.Services.AddScoped<IWeatherForecastService, YRWeatherForecastService>();
// TODO: replace key strings by enums
builder.Services.AddKeyedScoped<ITravelPlanningService, TravelPlanningServiceOSRM>("OSRM");
builder.Services.AddKeyedScoped<ITravelPlanningService, TravelPlanningServiceTomTom>("TomTom");
builder.Services.AddScoped<IAvalancheRegionService, EAWSRegionService>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IAvalancheRegionsRepository, AvalancheRegionsRepository>();
builder.Services.AddScoped<IAvalancheReportsRepository, AvalancheReportsRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ITourDataRepository, TourDataRepository>();
builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
builder.Services.AddScoped<IAreaHandler, AreaHandler>();
builder.Services.AddScoped<ILocationHandler, LocationHandler>();
builder.Services.AddScoped<ITourDataHandler, TourDataHandler>();

builder.Services.AddAutoMapper(typeof(AreaProfile));
builder.Services.AddAutoMapper(typeof(AvalancheProblemProfile));
builder.Services.AddAutoMapper(typeof(AvalancheReportProfile));
builder.Services.AddAutoMapper(typeof(LocationProfile));
builder.Services.AddAutoMapper(typeof(WeatherForecastProfile));
builder.Services.AddAutoMapper(typeof(TourDataProfile));
builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<TourDataContext>(dbContextOptions =>
    dbContextOptions.UseSqlite(builder.Configuration["ETC_CONNECTIONSTRING"]));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TourDataContext>();
    await context.SeedAsync();
}

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
