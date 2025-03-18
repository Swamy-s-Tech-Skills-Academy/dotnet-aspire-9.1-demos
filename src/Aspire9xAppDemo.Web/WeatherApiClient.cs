namespace Aspire9xAppDemo.Web;

internal sealed class WeatherApiClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        var forecasts = new List<WeatherForecast>(maxItems);

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>(
            "/weatherforecast", cancellationToken).ConfigureAwait(false))
        {
            if (forecasts.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null) // Explicit null check
            {
                forecasts.Add(forecast);
            }
        }

        return forecasts.ToArray();
    }
}

internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
