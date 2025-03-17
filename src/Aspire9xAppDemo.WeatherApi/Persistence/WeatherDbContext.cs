using Aspire9xAppDemo.WeatherApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Aspire9xAppDemo.WeatherApi.Persistence;

public class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();
}
