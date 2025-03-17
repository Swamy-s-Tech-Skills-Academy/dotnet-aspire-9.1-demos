using AspireApp.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireApp.ApiService.Persistence;

public class WeatherDbPSqlContext(DbContextOptions<WeatherDbPSqlContext> options) : DbContext(options)
{
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();
}
