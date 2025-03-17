using Aspire9xAppDemo.ServiceDefaults;
using AspireApp.ApiService.Models;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors();

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
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = RandomNumberGenerator.GetInt32(-20, 55),
        Summary = Summaries[RandomNumberGenerator.GetInt32(Summaries.Length)]
    })
     .ToArray();
})
.WithName("GetWeatherForecast")
.WithOpenApi();
#endregion

app.MapDefaultEndpoints();

await app.RunAsync().ConfigureAwait(true);
