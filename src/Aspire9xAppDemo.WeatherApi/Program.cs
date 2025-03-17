using Aspire9xAppDemo.ServiceDefaults;
using Aspire9xAppDemo.WeatherApi.Models;
using Aspire9xAppDemo.WeatherApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors();

builder.AddSqlServerDbContext<WeatherDbContext>("sqldb");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.UseHttpsRedirection();

app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

var Summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

#region Dummy Data
app.MapGet("/weatherforecast", () =>
{
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Id = index,
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = RandomNumberGenerator.GetInt32(-20, 55),
        Summary = Summaries[RandomNumberGenerator.GetInt32(Summaries.Length)]
    })
     .ToArray();
})
.WithName("GetWeatherForecast")
.WithOpenApi();
#endregion

#region EF Core SQL Connection
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
    await context.Database.EnsureCreatedAsync();

    if (!context.WeatherForecasts.Any())
    {
        foreach (var index in Enumerable.Range(1, 5))
        {
            context.WeatherForecasts.Add(new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = RandomNumberGenerator.GetInt32(-20, 55),
                Summary = Summaries[RandomNumberGenerator.GetInt32(Summaries.Length)]
            });

            await context.SaveChangesAsync();
        }
    }
}

app.MapGet("/weatherforecastefsql", ([FromServices] WeatherDbContext context) =>
{
    return context.WeatherForecasts.ToArray();
});
#endregion

app.MapDefaultEndpoints();

await app.RunAsync().ConfigureAwait(true);
