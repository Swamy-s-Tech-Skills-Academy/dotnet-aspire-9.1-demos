using AspireApp.ApiService.Models;
using MySqlConnector;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AspireApp.ApiService.Services;

public class WeatherForecastService(MySqlDataSource dataSource)
{
    private readonly List<WeatherForecast> weatherForecasts = [];

    public IEnumerable<WeatherForecast> GetWeatherForecasts()
    {
        using var connection = dataSource.CreateConnection();
        connection.Open();

        var command = new MySqlCommand(@"SELECT Date, TemperatureC, Summary FROM WeatherForecasts", connection);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            weatherForecasts.Add(new WeatherForecast
            {
                Date = DateOnly.FromDateTime(reader.GetDateTime(0)),
                TemperatureC = reader.GetInt32(1),
                Summary = reader.GetString(2)
            });
        }

        return weatherForecasts;
    }

}
