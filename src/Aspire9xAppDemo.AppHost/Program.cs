var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Aspire9xAppDemo_WeatherApi>("aspire9xappdemo-weatherapi");

builder.Build().Run();
